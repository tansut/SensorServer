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

    public sealed class LlrpC1G2InventoryCommandGroup
    {
        // Fields
        public const string Filters = "Filters";
        internal static readonly PropertyKey FiltersKey = new PropertyKey("LLRP C1G2 Inventory command", "Filters");
        internal static readonly DevicePropertyMetadata FiltersMetadata = new DevicePropertyMetadata(typeof(string), LlrpResources.C1G2FiltersDescription, SensorPropertyRelation.Source, null, true, false, false, false);
        public const string ModeIndex = "Mode Index";
        internal static readonly PropertyKey ModeIndexKey = new PropertyKey("LLRP C1G2 Inventory command", "Mode Index");
        internal static readonly DevicePropertyMetadata ModeIndexMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.ModeIndexDescription, SensorPropertyRelation.Source, null, true, false, false, false);
        public const string Session = "Session";
        internal static readonly PropertyKey SessionKey = new PropertyKey("LLRP C1G2 Inventory command", "Session");
        internal static readonly DevicePropertyMetadata SessionMetadata = new DevicePropertyMetadata(typeof(string), LlrpResources.SessionDescription, SensorPropertyRelation.Source, TagSession.S0.ToString(), true, false, false, false, Util.GetNames(typeof(TagSession)));
        public const string TagInventoryStateAware = "Tag Inventory State Aware";
        internal static readonly PropertyKey TagInventoryStateAwareKey = new PropertyKey("LLRP C1G2 Inventory command", "Tag Inventory State Aware");
        internal static readonly DevicePropertyMetadata TagInventoryStateAwareMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.TagInventoryStateAwareDescription, SensorPropertyRelation.Source, false, true, false, false, false);
        public const string TagInventoryStateAwareSingulationActionSLFlag = "Tag Inventory State Aware Singulation Action SL Flag";
        internal static readonly PropertyKey TagInventoryStateAwareSingulationActionSLFlagKey = new PropertyKey("LLRP C1G2 Inventory command", "Tag Inventory State Aware Singulation Action SL Flag");
        internal static readonly DevicePropertyMetadata TagInventoryStateAwareSingulationActionSLFlagMetadata = new DevicePropertyMetadata(typeof(string), LlrpResources.TagInventoryStateAwareSingulationActionSLFlagDescription, SensorPropertyRelation.Source, TagSLState.Assert.ToString(), true, false, false, false, Util.GetNames(typeof(TagSLState)));
        public const string TagInventoryStateAwareSingulationActionState = "Tag Inventory State Aware Singulation Action State";
        internal static readonly PropertyKey TagInventoryStateAwareSingulationActionStateKey = new PropertyKey("LLRP C1G2 Inventory command", "Tag Inventory State Aware Singulation Action State");
        internal static readonly DevicePropertyMetadata TagInventoryStateAwareSingulationActionStateMetadata = new DevicePropertyMetadata(typeof(string), LlrpResources.TagInventoryStateAwareSingulationActionStateDescription, SensorPropertyRelation.Source, TagInventoryState.StateA.ToString(), true, false, false, false, Util.GetNames(typeof(TagInventoryState)));
        public const string TagPopulation = "Tag Population";
        internal static readonly PropertyKey TagPopulationKey = new PropertyKey("LLRP C1G2 Inventory command", "Tag Population");
        internal static readonly DevicePropertyMetadata TagPopulationMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.TagPopulationDescription, SensorPropertyRelation.Source, null, true, false, false, false);
        public const string TagTransitTime = "Tag Transit Time";
        internal static readonly PropertyKey TagTransitTimeKey = new PropertyKey("LLRP C1G2 Inventory command", "Tag Transit Time");
        internal static readonly DevicePropertyMetadata TagTransitTimeMetadata = new DevicePropertyMetadata(typeof(uint), LlrpResources.TagTransitTimeDescription, SensorPropertyRelation.Source, null, true, false, false, false);
        public const string Tari = "Tari";
        internal static readonly PropertyKey TariKey = new PropertyKey("LLRP C1G2 Inventory command", "Tari");
        internal static readonly DevicePropertyMetadata TariMetadata = new DevicePropertyMetadata(typeof(short), LlrpResources.TariDescription, SensorPropertyRelation.Source, null, true, false, false, false);
    }

 

 

}
