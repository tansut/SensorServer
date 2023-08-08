using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Configuration
{
    public sealed class SensorServerConfigurationSection : IConfigurationSectionHandler
    {
        private static string defaultMetadataProvider;
        private static ProviderSettingsCollection metadataProviders;
        private static ProviderSettingsCollection analyseProviders;


        private static SensorServerConfigurationSection currrentInstance = null;

        private static object initLock = new object();
        private static bool isInited = false;


        static SensorServerConfigurationSection()
        {

        }

        public SensorServerConfigurationSection()
        {
        }


        #region Properties
        public ProviderSettingsCollection MetadataProviders
        {
            get
            {
                return metadataProviders;
            }
            private set
            {
                metadataProviders = value;
            }
        }

        public ProviderSettingsCollection AnalyseProviders
        {
            get
            {
                return analyseProviders;
            }
            private set
            {
                analyseProviders = value;
            }
        }

        public string DefaultMetadataProvider
        {
            get
            {
                return defaultMetadataProvider;
            }
            set
            {
                defaultMetadataProvider = value;
            }
        }
        #endregion

        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            if (!isInited)
            {
                lock (initLock)
                {
                    if (!isInited)
                    {
                        currrentInstance = new SensorServerConfigurationSection();
                        var doc = XDocument.Parse(section.OuterXml, LoadOptions.None);
                        XElement mdp = null;
                        XElement root = null;
                        XElement ap = null;
                        XElement sc = null;
                        try
                        {
                            root = doc.Descendants("KalitteSensorServer").Single();
                            mdp = doc.Descendants("metadataProviders").Single();
                            ap = doc.Descendants("analyseProviders").SingleOrDefault();
                            sc = doc.Descendants("serverConfiguration").SingleOrDefault();
                        }
                        catch (InvalidOperationException ee)
                        {
                            throw new ConfigurationErrorsException("Configuration section define error, Please check the configuration file", ee, section);
                        }

                        if (mdp != null) currrentInstance.MetadataProviders = XmlHelper.GetProviderSettings(mdp);
                        if (ap != null) currrentInstance.AnalyseProviders = XmlHelper.GetProviderSettings(ap);

                        if (root != null)
                        {
                            var dmp = root.Attributes("defaultMetadataProvider").SingleOrDefault();
                            if (dmp != null) currrentInstance.DefaultMetadataProvider = dmp.Value;
                            else
                            {
                                if (currrentInstance.MetadataProviders.Count > 0)
                                    currrentInstance.DefaultMetadataProvider = currrentInstance.MetadataProviders[0].Name;
                                else throw new ConfigurationErrorsException("metadataProviders must contain at least one provider", section);
                            }
                        }

                        ServerConfiguration.Init(sc);
                        isInited = true;
                    }
                }
            }

            return currrentInstance;
        }

        #endregion

        

        public static void Init()
        {
            currrentInstance = ConfigurationManager.GetSection("KalitteSensorServer") as SensorServerConfigurationSection;

        }
    }
}
