namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class InventoryParameterSpec : LlrpTlvParameterBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolId m_airProtocolID;
        private Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaConfiguration> m_antennaConfiguration;
        private Collection<CustomParameterBase> m_custom;
        private ushort m_id;

        internal InventoryParameterSpec(BitArray bitArray, ref int index) : base(LlrpParameterType.InventoryParameterSpec, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort num2 = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolId enumInstance = BitHelper.GetEnumInstance<Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolId>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaConfiguration> antennaConfiguration = new Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaConfiguration>();
            Collection<CustomParameterBase> custom = new Collection<CustomParameterBase>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.AntennaConfiguration, bitArray, index, parameterEndLimit))
            {
                antennaConfiguration.Add(new Kalitte.Sensors.Rfid.Llrp.Core.AntennaConfiguration(bitArray, ref index));
            }
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.Custom, bitArray, index, parameterEndLimit))
            {
                custom.Add(CustomParameterBase.GetInstance(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(num2, enumInstance, antennaConfiguration, custom);
        }

        public InventoryParameterSpec(Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolId protocolId, Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaConfiguration> antennaConfiguration, Collection<CustomParameterBase> custom) : base(LlrpParameterType.InventoryParameterSpec)
        {
            this.Init(IdGenerator.GenerateInventorySpecId(), protocolId, antennaConfiguration, custom);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.Id, 0x10, true);
            stream.Append((long) ((byte) this.AirProtocolId), 8, true);
            Util.Encode<Kalitte.Sensors.Rfid.Llrp.Core.AntennaConfiguration>(this.AntennaConfiguration, stream);
            Util.Encode<CustomParameterBase>(this.CustomParameters, stream);
        }

        private void Init(ushort id, Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolId protocolId, Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaConfiguration> antennaConfiguration, Collection<CustomParameterBase> custom)
        {
            if (id == 0)
            {
                throw new ArgumentOutOfRangeException("id");
            }
            Util.CheckCollectionForNonNullElement<Kalitte.Sensors.Rfid.Llrp.Core.AntennaConfiguration>(antennaConfiguration);
            Util.CheckCollectionForNonNullElement<CustomParameterBase>(custom);
            this.m_id = id;
            this.m_airProtocolID = protocolId;
            this.m_antennaConfiguration = antennaConfiguration;
            this.m_custom = custom;
            this.ParameterLength = (0x18 + Util.GetTotalBitLengthOfParam<Kalitte.Sensors.Rfid.Llrp.Core.AntennaConfiguration>(this.AntennaConfiguration)) + Util.GetTotalBitLengthOfParam<CustomParameterBase>(this.CustomParameters);
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.AirProtocolId AirProtocolId
        {
            get
            {
                return this.m_airProtocolID;
            }
        }

        public Collection<Kalitte.Sensors.Rfid.Llrp.Core.AntennaConfiguration> AntennaConfiguration
        {
            get
            {
                return this.m_antennaConfiguration;
            }
        }

        public Collection<CustomParameterBase> CustomParameters
        {
            get
            {
                return this.m_custom;
            }
        }

        public ushort Id
        {
            get
            {
                return this.m_id;
            }
        }
    }
}
