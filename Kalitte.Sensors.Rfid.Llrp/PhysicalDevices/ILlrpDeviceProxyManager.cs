using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Rfid.Llrp.PhysicalDevices
{
    internal interface ILlrpDeviceProxyManager : IDisposable
    {
        // Events
        event EventHandler<DiscoveryEventArgs> Discovery;
        event EventHandler<NotificationEventArgs> Notification;

        // Methods
        LlrpDeviceProxy GetProxy(ConnectionInformation connectionInformation);
    }

 

 

}
