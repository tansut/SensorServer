using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public class DispatcherMetadata
    {
        private Dictionary<PropertyKey, DispatcherPropertyMetadata> metaData;

        public DispatcherMetadata(Dictionary<PropertyKey, DispatcherPropertyMetadata> metaData)
        {
            this.metaData = metaData;
        }

        public Dictionary<PropertyKey, DispatcherPropertyMetadata> DispatcherPropertyMetadata
        {
            get
            {
                return this.metaData;
            }
        }
    }
}
