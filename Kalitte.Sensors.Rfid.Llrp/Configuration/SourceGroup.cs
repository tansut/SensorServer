using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Rfid.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class SourceGroup
    {
        // Fields
        internal static readonly PropertyKey EnabledKey = new PropertyKey("Source", "Enabled");
        internal static readonly DevicePropertyMetadata EnabledMetadata = SourcePropertyGroup.EnabledMetadata;
        public const string LlrpSourceId = "Id";
        internal static readonly PropertyKey LlrpSourceIdKey = new PropertyKey("Source", "Id");
        internal static readonly DevicePropertyMetadata LlrpSourceIdMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.LlrpSourceIdDescription, SensorPropertyRelation.Source, null, false, false, true, false);
        internal static readonly PropertyKey PortInputValueKey = new PropertyKey("Source", "Port Input Value");
        internal static readonly DevicePropertyMetadata PortInputValueMetadata = new DevicePropertyMetadata(typeof(byte[]), LlrpResources.PortInputValueDescription, SensorPropertyRelation.Source, null, false, false, true, false);
        internal static readonly PropertyKey PortOutputValueKey = new PropertyKey("Source", "Port Output Value");
        internal static readonly DevicePropertyMetadata PortOutputValueMetadata = new DevicePropertyMetadata(typeof(byte[]), LlrpResources.PortOutputValueDescription, SensorPropertyRelation.Source, null, true, false, true, false);
        internal static readonly PropertyKey SourceTypeKey = new PropertyKey("Source", "Source Type");
        internal static readonly DevicePropertyMetadata SourceTypeMetadata = SourcePropertyGroup.SourceTypeMetadata;
        internal static readonly PropertyKey SystemEnabledKey = new PropertyKey("Source", "System Enabled");
        internal static readonly DevicePropertyMetadata SystemEnabledMetadata = SourcePropertyGroup.SystemEnabledMetadata;
    }

 

 

}
