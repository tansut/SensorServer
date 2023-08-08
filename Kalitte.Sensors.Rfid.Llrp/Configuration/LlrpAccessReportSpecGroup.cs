using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Rfid.Llrp.Core;
using Kalitte.Sensors.Rfid.Llrp.Helpers;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpAccessReportSpecGroup
    {
        // Fields
        public const string Trigger = "Trigger";
        internal static readonly PropertyKey TriggerKey = new PropertyKey("LLRP Access Report Spec", "Trigger");
        internal static readonly DevicePropertyMetadata TriggerMetadata = new DevicePropertyMetadata(typeof(string), LlrpResources.AccessTriggerDescription, SensorPropertyRelation.Device, AccessReportTrigger.OnEndOfAccessSpec.ToString(), true, false, false, false, Util.GetNames(typeof(AccessReportTrigger)));
    }

 

 

}
