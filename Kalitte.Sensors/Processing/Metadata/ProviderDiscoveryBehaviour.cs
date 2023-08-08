using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Processing.Metadata
{

    [Serializable]
    public enum DiscoverySensorMatchType
    {
        SensorName,
        SensorId
    }

    [Serializable]
    public class ProviderDiscoveryBehaviour
    {


        public DiscoverySensorMatchType SensorMatch { get; set; }
        public bool CreateSensorIfNoMatch { get; set; }

        public ProviderDiscoveryBehaviour()
        {
            this.SensorMatch = DiscoverySensorMatchType.SensorId;
            CreateSensorIfNoMatch = true;
        }
    }
}
