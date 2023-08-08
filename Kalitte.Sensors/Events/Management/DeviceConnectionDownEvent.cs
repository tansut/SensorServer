using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Events.Management
{
    [Serializable]
    public sealed class DeviceConnectionDownEvent : DeviceManagementEvent
    {
        // Methods
        public DeviceConnectionDownEvent(string description)
            : base(EventLevel.Error, EventType.DeviceConnectionClosed, description)
        {
        }
    }

 

}
