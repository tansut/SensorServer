using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using Kalitte.Sensors.SetupConfiguration.Core.Exception;
using System.IO;

namespace Kalitte.Sensors.SetupConfiguration.Helpers
{
    public enum IISState
    {
        Installed, InstalledAndRunning, NotInstalled, WWWPublishNotInstalled, AspNet4NotRegistered
    }
    public static class IISHelper
    {

        public static readonly string IISDisplayName = "Internet Information Services";
        public static readonly string IISServiceDisplayName = "World Wide Web Publishing Service";
        public static readonly string IISServiceName = "W3SVC";
        public static readonly string AspNet4DisplayName = "ASP.NET 4.0";
        public static readonly string MetaBasePath = "IIS://localhost/W3SVC";
        private static readonly string Framework = "4.0.30319";
        private static readonly string ApplicationPool = "SensorApplicationPool";


        public static IISState GetIISState()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp");
            if (key != null)
            {
                if (!AspNet4Registered()) return IISState.AspNet4NotRegistered;
                var service = ServiceHelper.GetService(IISServiceName);
                if (service != null)
                {
                    if (service.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                        return IISState.InstalledAndRunning;
                    else return IISState.Installed;
                }
                else return IISState.WWWPublishNotInstalled;
            }
            else return IISState.NotInstalled;
        }

        private static bool AspNet4Registered()
        {
            bool aspNet4Registered = false;
            var aspNetKeys = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\ASP.NET").GetSubKeyNames();
            foreach (var item in aspNetKeys)
            {
                var version = Registry.LocalMachine.OpenSubKey(string.Format(@"Software\Microsoft\ASP.NET\{0}", item)).GetValue("AssemblyVersion");
                if (version != null && (string)version == "4.0.0.0")
                {
                    aspNet4Registered = true;
                    break;
                }
            }
            return aspNet4Registered;
        }

        public static void StartIIS()
        {
            ServiceHelper.StartService(IISServiceName);
        }


        public static bool CreateWebSiteOnIIS(string siteName, string appPoolName, string physicalPath)
        {
            try
            {
                int siteId = GetUniqueSiteId();
                if (!CheckApplicationPoolNameInUse(MetaBasePath, appPoolName))
                    CreateAppPool(MetaBasePath, appPoolName);
                DirectoryEntry objDirEntry = new DirectoryEntry(MetaBasePath);
                string className = objDirEntry.SchemaClassName;
                if (!className.EndsWith("Service")) throw new UserException("Invalid configuration variables");
                object[] hosts = new object[1];
                hosts[0] = ":80:localhost";
                DirectoryEntry newSite = objDirEntry.Children.Add(Convert.ToString(siteId), (className.Replace("Service", "Server")));
                newSite.Properties["ServerComment"][0] = siteName;
                newSite.Properties["ServerBindings"].Value = hosts;
                newSite.Invoke("Put", "ServerAutoStart", 1);
                newSite.Invoke("Put", "ServerSize", 1);
                newSite.CommitChanges();
                DirectoryEntry newSiteVDir = newSite.Children.Add("Root", "IIsWebVirtualDir");
                newSiteVDir.Properties["Path"][0] = Path.Combine(physicalPath, @"\Management");
                newSiteVDir.Properties["EnableDefaultDoc"][0] = true;
                newSiteVDir.Properties["DefaultDoc"].Value = "default.aspx";
                newSiteVDir.Properties["AppIsolated"][0] = 2;
                newSiteVDir.Properties["AccessRead"][0] = true;
                newSiteVDir.Properties["AccessWrite"][0] = false;
                newSiteVDir.Properties["AccessScript"][0] = true;
                newSiteVDir.Properties["AccessFlags"].Value = 513;
                newSiteVDir.Properties["AppRoot"][0] = @"/LM/W3SVC/" + Convert.ToString(siteId) + "/Root";
                newSiteVDir.Properties["AppPoolId"].Value = appPoolName;
                newSiteVDir.Properties["AuthNTLM"][0] = true;
                newSiteVDir.Properties["AuthAnonymous"][0] = true;
                newSiteVDir.CommitChanges();
                Regex versionRegex = new Regex(@"(?<=\\v)\d{1}\.\d{1}\.\d{1,5}(?=\\)");
                PropertyValueCollection lstScriptMaps = newSiteVDir.Properties["ScriptMaps"];
                System.Collections.ArrayList arrScriptMaps = new System.Collections.ArrayList();
                foreach (string scriptMap in lstScriptMaps)
                {
                    if (scriptMap.Contains("Framework"))
                    {
                        arrScriptMaps.Add(versionRegex.Replace(scriptMap, Framework));
                    }
                    else
                    {
                        arrScriptMaps.Add(scriptMap);
                    }
                }
                newSiteVDir.Properties["ScriptMaps"].Value = arrScriptMaps.ToArray();
                newSiteVDir.CommitChanges();
                SetApplicationPool(MetaBasePath, appPoolName);
                return true;
            }
            catch (Exception ex)
            {
                throw new UserException(ex.Message, ex);
            }
        }

        private static void SetApplicationPool(string metabasePath, string appPoolName)
        {
            DirectoryEntry vDir = new DirectoryEntry(metabasePath);
            string className = vDir.SchemaClassName.ToString();
            if (className.EndsWith("VirtualDir"))
            {
                object[] param = { 0, appPoolName, true };
                vDir.Invoke("AppCreate3", param);
                vDir.Properties["AppIsolated"][0] = "2";
            }
        }

        private static void CreateAppPool(string metabasePath, string appPoolName)
        {
            var path = string.Format("{0}/AppPools", metabasePath);
            DirectoryEntry newpool;
            DirectoryEntry apppools = new DirectoryEntry(path);
            newpool = apppools.Children.Add(appPoolName, "IIsApplicationPool");
            newpool.InvokeSet("ManagedPipelineMode", new Object[] { 0 });
            newpool.CommitChanges();
        }

        public static bool CheckWebSiteNameInUse(string metabasePath, string webSiteName)
        {
            bool result = false;
            DirectoryEntry objDirEntry = new DirectoryEntry(metabasePath);
            string className = objDirEntry.SchemaClassName;
            if (!className.EndsWith("Service")) throw new UserException("Invalid configuration variables");
            else
            {
                foreach (DirectoryEntry item in objDirEntry.Children)
                {
                    if (item.SchemaClassName == "IIsWebServer")
                    {
                        if (item.Properties.Contains("ServerComment"))
                        {
                            if (item.Properties["ServerComment"].Value.Equals(webSiteName))
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static bool CheckApplicationPoolNameInUse(string metabasePath, string appPoolName)
        {
            bool result = false;
            var path = string.Format("{0}/AppPools", metabasePath);
            DirectoryEntry apppools = new DirectoryEntry(path);
            foreach (DirectoryEntry item in apppools.Children)
            {
                if (item.SchemaClassName == "IIsApplicationPool")
                {
                    if (item.Name == appPoolName)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private static int GetUniqueSiteId()
        {
            int siteId = 1;
            DirectoryEntry objDirEntry = new DirectoryEntry(MetaBasePath);
            foreach (DirectoryEntry e in objDirEntry.Children)
            {
                if (e.SchemaClassName == "IIsWebServer")
                {
                    int id = Convert.ToInt32(e.Name);
                    if (id >= siteId)
                        siteId = id + 1;
                }
            }
            return siteId;
        }


    }
}
