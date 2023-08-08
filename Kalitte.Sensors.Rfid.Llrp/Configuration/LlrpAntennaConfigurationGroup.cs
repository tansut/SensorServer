using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Commands;
using Kalitte.Sensors.Rfid.Llrp.Properties;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Llrp.Configuration
{
    public sealed class LlrpAntennaConfigurationGroup
    {
        // Fields
        public const string ChannelIndex = "Channel Index";
        internal static readonly PropertyKey ChannelIndexKey = new PropertyKey("LLRP Antenna Configuration", "Channel Index");
        internal static readonly DevicePropertyMetadata ChannelIndexMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.ChannelIndexDescription, SensorPropertyRelation.Source, null, true, false, false, false);
        public const string HopTableId = "Hop Table Id";
        internal static readonly PropertyKey HopTableIdKey = new PropertyKey("LLRP Antenna Configuration", "Hop Table Id");
        internal static readonly DevicePropertyMetadata HopTableIdMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.HopTableIdDescription, SensorPropertyRelation.Source, (ushort)0, true, false, false, false);
        public const string ReceiverSensitivityIndex = "Receiver Sensitivity Index";
        internal static readonly PropertyKey ReceiverSensitivityIndexKey = new PropertyKey("LLRP Antenna Configuration", "Receiver Sensitivity Index");
        public const string TransmitPowerIndex = "Transmit Power Index";
        internal static readonly PropertyKey TransmitPowerIndexKey = new PropertyKey("LLRP Antenna Configuration", "Transmit Power Index");
        internal static readonly DevicePropertyMetadata TransmitPowerIndexMetadata = new DevicePropertyMetadata(typeof(ushort), LlrpResources.TransmitPowerIndexDescription, SensorPropertyRelation.Source, (ushort)0, true, false, false, false);
    }

 

 

    
}
