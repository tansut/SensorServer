using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public class ServiceConfiguration
    {
        public static readonly int DefaultManagementServicePort = 8746;
        public static readonly int DefaultSensorCommandServicePort = DefaultManagementServicePort;

        public bool EnableManagementService { get; set; }
        public bool EnableSensorCommandService { get; set; }
        public int ManagementServicePort { get; set; }
        public int SensorCommandServicePort { get; set; }
        public bool UseDefaultWcfSettings { get; set; }
        public string EndpointAddressPrefix { get; set; }

        public ServiceConfiguration()
        {
            EnableManagementService = true;
            EnableSensorCommandService = true;
            UseDefaultWcfSettings = true;
            ManagementServicePort = DefaultManagementServicePort;
            SensorCommandServicePort = ManagementServicePort;
            EndpointAddressPrefix = "";
        }
    }
}
