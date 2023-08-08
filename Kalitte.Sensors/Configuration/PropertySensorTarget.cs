using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Configuration
{
    [Serializable, Flags]
    public enum SensorPropertyRelation
    {
        Device = 1,
        DeviceAndSource = 0x11,
        Source = 0x10
    }

 

}
