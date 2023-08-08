using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing;
using System.IO;
using System.Reflection;
using Kalitte.Sensors.Configuration;
using System.Security.Policy;

namespace Kalitte.Sensors.Utilities
{
    public static class MarshalHelper
    {
        public static  AppDomain CreateAppDomanin(string friendlyName)
        {
            AppDomainSetup ads = new AppDomainSetup();
            ads.ApplicationBase =
                AppDomain.CurrentDomain.BaseDirectory;
            string binPath = Path.Combine(ads.ApplicationBase, "bin");
            if (Directory.Exists(binPath))
                ads.ApplicationBase = binPath;
            Evidence baseEvidence = AppDomain.CurrentDomain.Evidence; 
            Evidence evidence = new Evidence(baseEvidence);
            ads.DisallowBindingRedirects = false;
            ads.DisallowCodeDownload = true;
            ads.ConfigurationFile =
                AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            AppDomain ad = AppDomain.CreateDomain(friendlyName, evidence, ads);
            //ad.TypeResolve += new ResolveEventHandler(ad_TypeResolve);
            return ad;
        }


        public static T GetMetadataOfItem<T>(string type, ProcessingItem itemType)
        {
            VirtualMarshal marshal = new VirtualMarshal(type);
            T result = marshal.GetStaticMethodResult<T>("GetMetadataOfItem", false, new object[] { itemType });
            marshal.Close();
            return result;

        }


        public static T GetMetadata<T>(string type, params object [] parameters)
        {
            VirtualMarshal marshal = new VirtualMarshal(type);
            T result = marshal.GetStaticMethodResult<T>("GetMetadata", false, parameters);
            marshal.Close();
            return result;

        }
    }
}
