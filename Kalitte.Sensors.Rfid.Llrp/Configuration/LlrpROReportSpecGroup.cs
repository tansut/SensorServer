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
    public sealed class LlrpROReportSpecGroup
    {
        // Fields
        public const string C1G2EnableCrc = "C1G2 Enable CRC";
        internal static readonly PropertyKey C1G2EnableCrcKey = new PropertyKey("LLRP RO Report Specification", "C1G2 Enable CRC");
        internal static readonly DevicePropertyMetadata C1G2EnableCrcMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.C1G2EnableCrcDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string C1G2EnablePC = "C1G2 Enable PC";
        internal static readonly PropertyKey C1G2EnablePCKey = new PropertyKey("LLRP RO Report Specification", "C1G2 Enable PC");
        internal static readonly DevicePropertyMetadata C1G2EnablePCMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.C1G2EnablePCDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string EnableAccessSpecId = "Enable Access Spec Id";
        internal static readonly PropertyKey EnableAccessSpecIdKey = new PropertyKey("LLRP RO Report Specification", "Enable Access Spec Id");
        internal static readonly DevicePropertyMetadata EnableAccessSpecIdMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.EnableAccessSpecIdDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string EnableAntennaId = "Enable Antenna Id";
        internal static readonly PropertyKey EnableAntennaIdKey = new PropertyKey("LLRP RO Report Specification", "Enable Antenna Id");
        internal static readonly DevicePropertyMetadata EnableAntennaIdMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.EnableAntennaIdDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string EnableChannelIndex = "Enable Channel Index";
        internal static readonly PropertyKey EnableChannelIndexKey = new PropertyKey("LLRP RO Report Specification", "Enable Channel Index");
        internal static readonly DevicePropertyMetadata EnableChannelIndexMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.EnableChannelIndexDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string EnableFirstSeenTimestamp = "Enable First Seen Timestamp";
        internal static readonly PropertyKey EnableFirstSeenTimestampKey = new PropertyKey("LLRP RO Report Specification", "Enable First Seen Timestamp");
        internal static readonly DevicePropertyMetadata EnableFirstSeenTimestampMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.EnableFirstSeenTimestampDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string EnableInventoryParameterSpecId = "Enable Inventory Parameter Spec Id";
        internal static readonly PropertyKey EnableInventoryParameterSpecIdKey = new PropertyKey("LLRP RO Report Specification", "Enable Inventory Parameter Spec Id");
        internal static readonly DevicePropertyMetadata EnableInventoryParameterSpecIdMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.EnableInventoryParameterSpecIdDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string EnableLastSeenTimestamp = "Enable Last Seen Timestamp";
        internal static readonly PropertyKey EnableLastSeenTimestampKey = new PropertyKey("LLRP RO Report Specification", "Enable Last Seen Timestamp");
        internal static readonly DevicePropertyMetadata EnableLastSeenTimestampMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.EnableLastSeenTimestampDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string EnablePeakRssi = "Enable Peak Rssi";
        internal static readonly PropertyKey EnablePeakRssiKey = new PropertyKey("LLRP RO Report Specification", "Enable Peak Rssi");
        internal static readonly DevicePropertyMetadata EnablePeakRssiMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.EnablePeakRssiDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string EnableROSpecId = "Enable RO Spec Id";
        internal static readonly PropertyKey EnableROSpecIdKey = new PropertyKey("LLRP RO Report Specification", "Enable RO Spec Id");
        internal static readonly DevicePropertyMetadata EnableROSpecIdMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.EnableROSpecIdDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string EnableSpecIndex = "Enable Spec Index";
        internal static readonly PropertyKey EnableSpecIndexKey = new PropertyKey("LLRP RO Report Specification", "Enable Spec Index");
        internal static readonly DevicePropertyMetadata EnableSpecIndexMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.EnableSpecIndexDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string EnableTagSeenCount = "Enable Tag Seen Count";
        internal static readonly PropertyKey EnableTagSeenCountKey = new PropertyKey("LLRP RO Report Specification", "Enable Tag Seen Count");
        internal static readonly DevicePropertyMetadata EnableTagSeenCountMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.EnableTagSeenCountDescription, SensorPropertyRelation.Device, false, true, false, false, false);
        public const string NumberOfTagReportData = "Number of Tag Report Data";
        internal static readonly PropertyKey NumberOfTagReportDataKey = new PropertyKey("LLRP RO Report Specification", "Number of Tag Report Data");
        internal static readonly DevicePropertyMetadata NumberOfTagReportDataMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.NumberOfTagReportDataDescription, SensorPropertyRelation.Device, (ushort)0, true, false, false, false);
        public const string Trigger = "Trigger";
        internal static readonly PropertyKey TriggerKey = new PropertyKey("LLRP RO Report Specification", "Trigger");
        internal static readonly DevicePropertyMetadata TriggerMetadata = new DevicePropertyMetadata(typeof(string), LlrpResources.ROTriggerDescription, SensorPropertyRelation.Device, ROReportTrigger.None.ToString(), true, false, false, false, Util.GetNames(typeof(ROReportTrigger)));
    }

 

 

}
