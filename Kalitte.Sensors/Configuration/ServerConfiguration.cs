using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Utilities;
using System.IO;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Security;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public sealed class ServerConfiguration
    {
        private static ServerConfiguration current;
        private static ServerConfigurationMetadata metadata;
        private static object initLock = new object();
        private static bool isInited = false;
        public LogConfiguration LogConfiguration { get; private set; }
        public ServerAnalyseConfiguration WatchConfiguration { get; private set; }
        public ServiceConfiguration ServiceConfiguration { get; private set; }
        public HostingConfiguration HostingConfiguration { get; private set; }

        public int QueTimeout { get; private set; }
        public int MethodCallTimeout { get; private set; }
        public int EventPipeTimeout { get; private set; }
        public int LastEventsSize { get; private set; }

        
        


        public static void Init(ServerConfiguration currentInstance)
        {
            if (!isInited)
            {
                lock (initLock)
                {
                    if (!isInited)
                    {
                        current = currentInstance;
                        isInited = true;
                    }
                }
            }
        }

        public static void Init(XElement element)
        {
            if (!isInited)
            {
                lock (initLock)
                {
                    if (!isInited)
                    {
                        current = new ServerConfiguration();
                        current.LogConfiguration = new LogConfiguration();
                        current.ServiceConfiguration = new ServiceConfiguration();
                        current.HostingConfiguration = new HostingConfiguration();
                        current.QueTimeout = 500;
                        current.MethodCallTimeout = 60000;
                        current.EventPipeTimeout = 120000;
                        current.LastEventsSize = 32;
                        XElement lc = null;
                        XElement sac = null;
                        XElement service = null;
                        XElement hosting = null;
                        if (element != null)
                        {
                            try
                            {
                                lc = element.Descendants("logConfiguration").SingleOrDefault();
                                sac = element.Descendants("serverAnalyseConfiguration").SingleOrDefault();
                                service = element.Descendants("serviceConfiguration").SingleOrDefault();
                                hosting = element.Descendants("hostingConfiguration").SingleOrDefault();

                            }
                            catch (InvalidOperationException ee)
                            {
                                throw new ConfigurationErrorsException("Configuration section error, please check configuration file settings.", ee);
                            }

                            XmlHelper.SetObjectFromXml<LogConfiguration>(lc, current.LogConfiguration);
                            XmlHelper.SetObjectFromXml<ServerConfiguration>(element, current);
                            XmlHelper.SetObjectFromXml<ServiceConfiguration>(service, current.ServiceConfiguration);
                            XmlHelper.SetObjectFromXml<HostingConfiguration>(hosting, current.HostingConfiguration);

                        }
                        var saConf = new ServerAnalyseConfiguration(sac);
                        current.WatchConfiguration = saConf;                       
                        isInited = true;
                    }
                }
            }
        }

        static ServerConfiguration()
        {

        }


        public static string GetItemLogPath(string folder, string itemName)
        {
            var fullPath = Path.Combine(Current.LogConfiguration.BaseDirectory, folder);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            var fileName = SensorCommon.RemoveNonCharsAndDigits(itemName);
            fullPath = Path.Combine(fullPath, fileName);
            return string.Format("{0}.log", fullPath);
        }


        public static string GetItemLogPath(ProcessingItem itemType, string itemName)
        {
            if (itemType == ProcessingItem.Server)
                return GetItemLogPath("", Current.LogConfiguration.ServerLogFile);
            else return GetItemLogPath(itemType.ToString(), itemName);
        }

        public static ServerConfiguration Current
        {
            get
            {
                return current;
            }
        }


        

    }
}
