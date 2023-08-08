using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public class HostingConfiguration
    {
        public bool IISHosted { get; set; }

        public HostingConfiguration()
        {
            IISHosted = false;
        }
    }
}
