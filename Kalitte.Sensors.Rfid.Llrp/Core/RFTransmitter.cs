namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class RFTransmitter : LlrpTlvParameterBase
    {
        private ushort m_channelIndex;
        private ushort m_hopTableID;
        private ushort m_transmitPowerIndex;

        internal RFTransmitter(BitArray bitArray, ref int index) : base(LlrpParameterType.RFTransmitter, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort hopTableID = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort channelIndex = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort transmitPowerIndex = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(transmitPowerIndex, hopTableID, channelIndex);
        }

        public RFTransmitter(ushort transmitPowerIndex, ushort hopTableID, ushort channelIndex) : base(LlrpParameterType.RFTransmitter)
        {
            this.Init(transmitPowerIndex, hopTableID, channelIndex);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.HopTableId, 0x10, true);
            stream.Append((long) this.ChannelIndex, 0x10, true);
            stream.Append((long) this.TransmitPowerIndex, 0x10, true);
        }

        private void Init(ushort transmitPowerIndex, ushort hopTableID, ushort channelIndex)
        {
            this.m_transmitPowerIndex = transmitPowerIndex;
            this.m_hopTableID = hopTableID;
            this.m_channelIndex = channelIndex;
            this.ParameterLength = 0x30;
        }

        public ushort ChannelIndex
        {
            get
            {
                return this.m_channelIndex;
            }
        }

        public ushort HopTableId
        {
            get
            {
                return this.m_hopTableID;
            }
        }

        public ushort TransmitPowerIndex
        {
            get
            {
                return this.m_transmitPowerIndex;
            }
        }
    }
}
