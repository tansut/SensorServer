using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpTroubleshootGroup
    {
        // Fields
        public const string CleanupSpecs = "Cleanup specs";
        internal static readonly PropertyKey CleanupSpecsKey = new PropertyKey("Llrp Troubleshoot", "Cleanup specs");
        internal static readonly DevicePropertyMetadata CleanupSpecsMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.CleanupSpecsDescription, SensorPropertyRelation.Device, false, true, false, true, false);
        public const string ResetToFactoryDefault = "Reset to Factory default";
        internal static readonly PropertyKey ResetToFactoryDefaultKey = new PropertyKey("Llrp Troubleshoot", "Reset to Factory default");
        internal static readonly DevicePropertyMetadata ResetToFactoryDefaultMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.ResetToFactoryDefaultDescription, SensorPropertyRelation.Device, false, true, false, true, false);
    }

 

 

}
