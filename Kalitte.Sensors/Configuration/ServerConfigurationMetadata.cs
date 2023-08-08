using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public class ServerConfigurationMetadata
    {
        private Dictionary<PropertyKey, ServerConfigurationPropertyMetadata> metaData;

        public ServerConfigurationMetadata(Dictionary<PropertyKey, ServerConfigurationPropertyMetadata> metaData)
        {
            this.metaData = metaData;
        }

        public Dictionary<PropertyKey, ServerConfigurationPropertyMetadata> ConfigurationMetadata
        {
            get
            {
                return this.metaData;
            }
        }
    }
}
