using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Rfid.Llrp.Core;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Rfid.Llrp;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Rfid.Llrp.Utilities;
using Kalitte.Sensors.Rfid.Configuration;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
public sealed class NotificationGroup
{
    // Fields
    internal static readonly PropertyKey EventModeKey = new PropertyKey("Notification", "Event Mode");
    internal static readonly DevicePropertyMetadata EventModeMetadata = new DevicePropertyMetadata(NotificationPropertyGroup.EventModeMetadata.Type, NotificationPropertyGroup.EventModeMetadata.Description, NotificationPropertyGroup.EventModeMetadata.PropertyTargets, NotificationPropertyGroup.EventModeMetadata.DefaultValue, true, false, false, false);
    public const string InventoryAccessSpec = "Inventory Access Specification";
    internal static readonly PropertyKey InventoryAccessSpecKey = new PropertyKey("Notification", "Inventory Access Specification");
    internal static readonly DevicePropertyMetadata InventoryAccessSpecMetadata = new DevicePropertyMetadata(typeof(string), LlrpResources.InventoryAccessSpecDescription, SensorPropertyRelation.Device, null, true, false, false, false);
    public const string InventoryROSpec = "Inventory RO Specification";
    internal static readonly PropertyKey InventoryROSpecKey = new PropertyKey("Notification", "Inventory RO Specification");
    internal static readonly DevicePropertyMetadata InventoryROSpecMetadata = new DevicePropertyMetadata(typeof(string), LlrpResources.InventoryROSpecDescription, SensorPropertyRelation.Device, LlrpSerializationHelper.SerializeToXmlDataContract(CreateDefaultROSpec(), false), true, false, false, false);

    // Methods
    private NotificationGroup()
    {
    }

    private static ROSpec CreateDefaultROSpec()
    {
        Collection<ushort> antennaIds = new Collection<ushort>();
        antennaIds.Add(0);
        Collection<InventoryParameterSpec> inventoryParameterSpecs = new Collection<InventoryParameterSpec>();
        InventoryParameterSpec item = new InventoryParameterSpec(AirProtocolId.EpcClass1Gen2, null, null);
        inventoryParameterSpecs.Add(item);
        AISpec spec2 = new AISpec(antennaIds, new AISpecStopTrigger(AISpecStopTriggerType.Null, uint.MaxValue, null, null), inventoryParameterSpecs, null);
        Collection<AISpec> aiSpec = new Collection<AISpec>();
        aiSpec.Add(spec2);
        Collection<AirProtocolSpecificEpcMemorySelectorParameter> memorySelector = new Collection<AirProtocolSpecificEpcMemorySelectorParameter>();
        memorySelector.Add(new C1G2EpcMemorySelector(true, true));

        return new ROSpec(IdGenerator.GenerateROSpecIdForProvider(), 0, new ROBoundarySpec(new ROSpecStartTrigger(ROSpecStartTriggerType.Immediate, null, null), new ROSpecStopTrigger()), aiSpec, null, null, new ROReportSpec(ROReportTrigger.NTagReportDataOrROSpecEnd, 1, new TagReportContentSelector(true, true, true, true, true, true, true, true, true, true, memorySelector), null));
    }
}

 

 

}
