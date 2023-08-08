using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpC1G2CapabilitiesGroup
    {
        // Fields
        public const string MaximumSelectFiltersPerQuery = "Maximum Select Filters per Query";
        internal static readonly PropertyKey MaximumSelectFiltersPerQueryKey = new PropertyKey("LLRP C1G2 Capabilities", "Maximum Select Filters per Query");
        internal static readonly DevicePropertyMetadata MaximumSelectFiltersPerQueryMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.MaximumNumSelectFiltersPerQuery, SensorPropertyRelation.Device, null, false, false, true, false);
        public const string SupportsBlockErase = "Supports Block Erase";
        internal static readonly PropertyKey SupportsBlockEraseKey = new PropertyKey("LLRP C1G2 Capabilities", "Supports Block Erase");
        internal static readonly DevicePropertyMetadata SupportsBlockEraseMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.SupportsBlockEraseDescription, SensorPropertyRelation.Device, true, false, false, true, false);
        public const string SupportsBlockWrite = "Supports Block Write";
        internal static readonly PropertyKey SupportsBlockWriteKey = new PropertyKey("LLRP C1G2 Capabilities", "Supports Block Write");
        internal static readonly DevicePropertyMetadata SupportsBlockWriteMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.SupportsBlockWriteDescription, SensorPropertyRelation.Device, true, false, false, true, false);
    }

 

 

    
}
