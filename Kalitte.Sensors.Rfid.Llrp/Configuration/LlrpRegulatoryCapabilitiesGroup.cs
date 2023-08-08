using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpRegulatoryCapabilitiesGroup
    {
        // Fields
        public const string CountryCode = "Country Code";
        internal static readonly PropertyKey CountryCodeKey = new PropertyKey("LLRP Regulatory Capabilities", "Country Code");
        internal static readonly DevicePropertyMetadata CountryCodeMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.CountryCodeDescription, SensorPropertyRelation.Device, (ushort)0, false, false, true, false);
        public const string FixedFrequencyTable = "Fixed Frequency Table";
        internal static readonly PropertyKey FixedFrequencyTableKey = new PropertyKey("LLRP Regulatory Capabilities", "Fixed Frequency Table");
        internal static readonly DevicePropertyMetadata FixedFrequencyTableMetadata = new DevicePropertyMetadata(typeof(uint[]), LlrpResources.FixedFrequencyTableDescription, SensorPropertyRelation.Device, null, false, false, true, false);
        public const string FrequencyHopTable = "Frequency Table";
        internal static readonly PropertyKey FrequencyHopTableKey = new PropertyKey("LLRP Regulatory Capabilities", "Frequency Table");
        internal static readonly DevicePropertyMetadata FrequencyHopTableMetadata = new DevicePropertyMetadata(typeof(string), LlrpResources.FrequencyHopTableDescription, SensorPropertyRelation.Device, null, false, false, true, false);
        public const string Hopping = "Hopping";
        internal static readonly PropertyKey HoppingKey = new PropertyKey("LLRP Regulatory Capabilities", "Hopping");
        internal static readonly DevicePropertyMetadata HoppingMetadata = new DevicePropertyMetadata(typeof(bool), LlrpResources.HoppingDescription, SensorPropertyRelation.Device, false, false, false, true, false);
        public const string RFModeTable = "RF Mode Table";
        internal static readonly PropertyKey RFModeTableKey = new PropertyKey("LLRP Regulatory Capabilities", "RF Mode Table");
        internal static readonly DevicePropertyMetadata RFModeTableMetadata = new DevicePropertyMetadata(typeof(string), LlrpResources.RFModeTableDescription, SensorPropertyRelation.Device, null, false, false, true, false);
        public const string TransmitPowerTable = "Transmit Power Table";
        internal static readonly PropertyKey TransmitPowerTableKey = new PropertyKey("LLRP Regulatory Capabilities", "Transmit Power Table");
        internal static readonly DevicePropertyMetadata TransmitPowerTableMetadata = new DevicePropertyMetadata(typeof(short[]), LlrpResources.TransmitPowerTableDescription, SensorPropertyRelation.Device, null, false, false, true, false);
    }

 

 

}
