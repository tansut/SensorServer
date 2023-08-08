using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public class LogicalSensorMetadata
    {
        private Dictionary<PropertyKey, LogicalSensorPropertyMetadata> metaData;

        public LogicalSensorMetadata(Dictionary<PropertyKey, LogicalSensorPropertyMetadata> metaData)
        {
            this.metaData = metaData;
        }

        public Dictionary<PropertyKey, LogicalSensorPropertyMetadata> LogicalSensorPropertyMetadata
        {
            get
            {
                return this.metaData;
            }
        }
    }
}
