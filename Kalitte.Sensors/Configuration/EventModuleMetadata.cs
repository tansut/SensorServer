using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public class EventModuleMetadata
    {
        private Dictionary<PropertyKey, EventModulePropertyMetadata> metaData;

        public EventModuleMetadata(Dictionary<PropertyKey, EventModulePropertyMetadata> metaData)
        {
            this.metaData = metaData;
        }

        public Dictionary<PropertyKey, EventModulePropertyMetadata> ModulePropertyMetadata
        {
            get
            {
                return this.metaData;
            }
        }

    }
}
