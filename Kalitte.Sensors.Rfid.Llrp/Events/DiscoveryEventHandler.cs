namespace Kalitte.Sensors.Rfid.Llrp.Events
{
    
    using System;
    using System.Runtime.CompilerServices;
    using Kalitte.Sensors.SensorDevices;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;

    internal delegate void DiscoveryEventHandler(DiscoveryEventArgs args, LlrpDeviceProxy device);
}
