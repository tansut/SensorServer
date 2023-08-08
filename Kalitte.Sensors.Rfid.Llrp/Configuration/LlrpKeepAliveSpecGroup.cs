using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpKeepAliveSpecGroup
    {
        // Fields
        public const string SendKeepAlive = "Send KeepAlive";
        internal static readonly PropertyKey SendKeepaliveKey = new PropertyKey("LLRP KeepAlive", "Send KeepAlive");
        internal static readonly DevicePropertyMetadata SendKeepaliveMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.SendKeepaliveDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string TimeInterval = "Time Interval";
        internal static readonly PropertyKey TimeIntervalKey = new PropertyKey("LLRP KeepAlive", "Time Interval");
        internal static readonly DevicePropertyMetadata TimeIntervalMetatdata = new DevicePropertyMetadata(typeof(uint), LlrpResources.TimeIntervalDescription, SensorPropertyRelation.Device, (uint)0, true, false, false, false);
    }

 

 

}
