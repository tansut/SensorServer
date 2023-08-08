using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpEventsAndReportGroup
    {
        // Fields
        public const string HoldsEventsAndReport = "Holds Events and Report";
        internal static readonly PropertyKey HoldsEventsAndReportKey = new PropertyKey("LLRP Events And Reports", "Holds Events and Report");
        internal static readonly DevicePropertyMetadata HoldsEventsAndReportMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.HoldsEventsAndReportDescription, SensorPropertyRelation.Device, true, true, false, false, false);
    }

 

 

}
