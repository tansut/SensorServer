using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Kalitte.Sensors.Configuration
{
    public sealed class SensorConfigurationSection : ConfigurationSection
    {
        private static readonly ConfigurationProperty _defaultPersistanceProvider;
        private static ConfigurationPropertyCollection _properties;
        private static readonly ConfigurationProperty knownTypesProviders;

        static SensorConfigurationSection()
        {
            knownTypesProviders = new ConfigurationProperty("knownTypesProviders", typeof(ProviderSettingsCollection), null, ConfigurationPropertyOptions.None);
            _properties = new ConfigurationPropertyCollection();
            _properties.Add(knownTypesProviders);
        }

        public SensorConfigurationSection()
        {
        }

        [ConfigurationProperty("knownTypesProviders")]
        public ProviderSettingsCollection KnownTypesProviders
        {
            get
            {
                return (ProviderSettingsCollection)base[knownTypesProviders];
            }
        }
    }
}
