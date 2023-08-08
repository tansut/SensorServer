namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class AntennaConfiguration : LlrpTlvParameterBase
    {
        private Collection<AirProtocolInventoryCommandSettings> m_airProtocolInventoryCommandParameter;
        private ushort m_antennaID;
        private Kalitte.Sensors.Rfid.Llrp.Core.RFReceiver m_rfReceiver;
        private Kalitte.Sensors.Rfid.Llrp.Core.RFTransmitter m_rfTransmitter;

        internal AntennaConfiguration(BitArray bitArray, ref int index) : base(LlrpParameterType.AntennaConfiguration, bitArray, index)
        {
            LlrpParameterType type;
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort antennaID = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            Kalitte.Sensors.Rfid.Llrp.Core.RFReceiver rfreceiver = null;
            Kalitte.Sensors.Rfid.Llrp.Core.RFTransmitter rfTransmitter = null;
            Collection<AirProtocolInventoryCommandSettings> airProtocolInventoryCommandParameter = new Collection<AirProtocolInventoryCommandSettings>();
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.RFReceiver, bitArray, index, parameterEndLimit))
            {
                rfreceiver = new Kalitte.Sensors.Rfid.Llrp.Core.RFReceiver(bitArray, ref index);
            }
            if (BitHelper.IsLLRPParameterPresent(LlrpParameterType.RFTransmitter, bitArray, index, parameterEndLimit))
            {
                rfTransmitter = new Kalitte.Sensors.Rfid.Llrp.Core.RFTransmitter(bitArray, ref index);
            }
            Collection<LlrpParameterType> expectedTypes = new Collection<LlrpParameterType>();
            expectedTypes.Add(LlrpParameterType.C1G2InventoryCommand);
            while (BitHelper.IsOneOfLLRPParameterPresent(expectedTypes, bitArray, index, parameterEndLimit, out type))
            {
                switch (type)
                {
                    case LlrpParameterType.C1G2InventoryCommand:
                    {
                        airProtocolInventoryCommandParameter.Add(new C1G2InventoryCommand(bitArray, ref index));
                        break;
                    }
                }
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(antennaID, rfreceiver, rfTransmitter, airProtocolInventoryCommandParameter);
        }

        public AntennaConfiguration(ushort antennaID, Kalitte.Sensors.Rfid.Llrp.Core.RFReceiver rfReceiver, Kalitte.Sensors.Rfid.Llrp.Core.RFTransmitter rfTransmitter, Collection<AirProtocolInventoryCommandSettings> airProtocolInventoryCommandParameter) : base(LlrpParameterType.AntennaConfiguration)
        {
            this.Init(antennaID, rfReceiver, rfTransmitter, airProtocolInventoryCommandParameter);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.AntennaId, 0x10, true);
            Util.Encode(this.RFReceiver, stream);
            Util.Encode(this.RFTransmitter, stream);
            Util.Encode<AirProtocolInventoryCommandSettings>(this.AirProtocolInventoryCommand, stream);
        }

        private void Init(ushort antennaID, Kalitte.Sensors.Rfid.Llrp.Core.RFReceiver rfreceiver, Kalitte.Sensors.Rfid.Llrp.Core.RFTransmitter rfTransmitter, Collection<AirProtocolInventoryCommandSettings> airProtocolInventoryCommandParameter)
        {
            Util.CheckCollectionForNonNullElement<AirProtocolInventoryCommandSettings>(airProtocolInventoryCommandParameter);
            this.m_antennaID = antennaID;
            this.m_rfReceiver = rfreceiver;
            this.m_rfTransmitter = rfTransmitter;
            this.m_airProtocolInventoryCommandParameter = airProtocolInventoryCommandParameter;
            this.ParameterLength = ((0x10 + Util.GetBitLengthOfParam(this.RFReceiver)) + Util.GetBitLengthOfParam(this.RFTransmitter)) + Util.GetTotalBitLengthOfParam<AirProtocolInventoryCommandSettings>(this.AirProtocolInventoryCommand);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            Util.ToStringSerialize(this, strBuilder);
            return strBuilder.ToString();
        }

        public Collection<AirProtocolInventoryCommandSettings> AirProtocolInventoryCommand
        {
            get
            {
                return this.m_airProtocolInventoryCommandParameter;
            }
        }

        public ushort AntennaId
        {
            get
            {
                return this.m_antennaID;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.RFReceiver RFReceiver
        {
            get
            {
                return this.m_rfReceiver;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.RFTransmitter RFTransmitter
        {
            get
            {
                return this.m_rfTransmitter;
            }
        }
    }
}
