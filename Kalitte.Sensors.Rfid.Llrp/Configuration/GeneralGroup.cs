using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Rfid.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    internal sealed class GeneralGroup
    {
        // Fields
        internal static readonly PropertyKey FirmwareVersionKey = new PropertyKey("General", "Firmware version");
        internal static readonly DevicePropertyMetadata FirmwareVersionMetadata = GeneralPropertyGroup.FirmwareVersionMetadata;
        internal static readonly PropertyKey NameKey = new PropertyKey("General", "Name");
        internal static readonly DevicePropertyMetadata NameMetadata = new DevicePropertyMetadata(GeneralPropertyGroup.NameMetadata.Type, GeneralPropertyGroup.NameMetadata.Description, SensorPropertyRelation.Source, GeneralPropertyGroup.NameMetadata.DefaultValue, false, false, true, false);
        internal static readonly PropertyKey RegulatoryRegionKey = new PropertyKey("General", "Regulatory region");
        internal static readonly DevicePropertyMetadata RegulatoryRegionMetadata = new DevicePropertyMetadata(GeneralPropertyGroup.RegulatoryRegionMetadata.Type, GeneralPropertyGroup.RegulatoryRegionMetadata.Description, GeneralPropertyGroup.RegulatoryRegionMetadata.PropertyTargets, GeneralPropertyGroup.RegulatoryRegionMetadata.DefaultValue, false, GeneralPropertyGroup.RegulatoryRegionMetadata.IsMandatory, GeneralPropertyGroup.RegulatoryRegionMetadata.IsPersistent, GeneralPropertyGroup.RegulatoryRegionMetadata.RequiresRestart);
        internal static readonly PropertyKey VendorKey = new PropertyKey("General", "Vendor");
        internal static readonly DevicePropertyMetadata VendorMetadata = GeneralPropertyGroup.VendorMetadata;
    }

 

 

}
