using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpConfigurationStateGroup
    {
        // Fields
        public const string State = "State";
        internal static readonly PropertyKey StateKey = new PropertyKey("LLRP Configuration State", "State");
        internal static readonly DevicePropertyMetadata StateMetadata = new DevicePropertyMetadata(typeof(uint), LlrpResources.StateDescription, SensorPropertyRelation.Device, (uint)0, false, false, true, false);
    }

 

 

}
