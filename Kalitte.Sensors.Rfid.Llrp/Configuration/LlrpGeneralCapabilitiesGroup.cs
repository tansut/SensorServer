using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpGeneralCapabilitiesGroup
    {
        // Fields
        public const string AntennaSensitivityMaximumIndex = "Antenna Sensitivity Maximum Index";
        internal static readonly PropertyKey AntennaSensitivityMaximumIndexKey = new PropertyKey("LLRP General Capabilities", "Antenna Sensitivity Maximum Index");
        internal static readonly DevicePropertyMetadata AntennaSensitivityMaximumIndexMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.AntennaSensitivityMaximumIndexDescription, SensorPropertyRelation.Source, null, false, false, true, false);
        public const string AntennaSensitivityMinimumIndex = "Antenna Sensitivity Minimum Index";
        internal static readonly PropertyKey AntennaSensitivityMinimumIndexKey = new PropertyKey("LLRP General Capabilities", "Antenna Sensitivity Minimum Index");
        internal static readonly DevicePropertyMetadata AntennaSensitivityMinimumIndexMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.AntennaSensitivityMinimumIndexDescription, SensorPropertyRelation.Source, null, false, false, true, false);
        public const string CanSetAntennaProperties = "Can Set Antenna Properties";
        internal static readonly PropertyKey CanSetAntennaPropertiesKey = new PropertyKey("LLRP General Capabilities", "Can Set Antenna Properties");
        internal static readonly DevicePropertyMetadata CanSetAntennaPropertiesMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.CanSetAntennaPropertiesDescription, SensorPropertyRelation.Device, true, false, false, true, false);
        public const string HasUtcClockCapability = "Has UTC Clock Capability";
        internal static readonly PropertyKey HasUtcClockCapabilityKey = new PropertyKey("LLRP General Capabilities", "Has UTC Clock Capability");
        internal static readonly DevicePropertyMetadata HasUtcClockCapabilityMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.HasUtcClockCapabilityDescription, SensorPropertyRelation.Device, true, false, false, true, false);
        public const string MaxAntennaSupported = "Maximum Antenna Supported";
        internal static readonly PropertyKey MaxAntennaSupportedKey = new PropertyKey("LLRP General Capabilities", "Maximum Antenna Supported");
        internal static readonly DevicePropertyMetadata MaxAntennaSupportedMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.MaxAntennaSupportedDescription, SensorPropertyRelation.Device, (ushort)0, false, false, true, false);
        public const string ModelName = "Model Name";
        internal static readonly PropertyKey ModelNameKey = new PropertyKey("LLRP General Capabilities", "Model Name");
        internal static readonly DevicePropertyMetadata ModelNameMetadata = new DevicePropertyMetadata(typeof(uint), LlrpResources.ModelNameDescription, SensorPropertyRelation.Device, (uint)0, false, false, true, false);
        public const string NumberOfGpi = "Number of GPI";
        internal static readonly PropertyKey NumberOfGpiKey = new PropertyKey("LLRP General Capabilities", "Number of GPI");
        internal static readonly DevicePropertyMetadata NumberOfGpiMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.NumberOfGpiDescription, SensorPropertyRelation.Device, (ushort)0, false, false, true, false);
        public const string NumberOfGpo = "Number of GPO";
        internal static readonly PropertyKey NumberOfGpoKey = new PropertyKey("LLRP General Capabilities", "Number of GPO");
        internal static readonly DevicePropertyMetadata NumberOfGPOMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.NumberOfGpoDescription, SensorPropertyRelation.Device, (ushort)0, false, false, true, false);
        public const string ReceiveSensitivityTable = "Receive Sensitivity Table";
        internal static readonly PropertyKey ReceiveSensitivityTableKey = new PropertyKey("LLRP General Capabilities", "Receive Sensitivity Table");
        internal static readonly DevicePropertyMetadata ReceiveSensitivityTableMetadata = new DevicePropertyMetadata(typeof(short[]), LlrpResources.ReceiveSensitivityTableDescription, SensorPropertyRelation.Device, null, false, false, true, false);
    }

 

 

}
