using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public class ExtendedMetadata
    {
        private Dictionary<PropertyKey, ExtendedPropertyMetadata> metaData;

        public ExtendedMetadata(Dictionary<PropertyKey, ExtendedPropertyMetadata> metaData)
        {
            this.metaData = metaData;
        }

        public Dictionary<PropertyKey, ExtendedPropertyMetadata> PropertyMetadata
        {
            get
            {
                return this.metaData;
            }
        }
    }
}
