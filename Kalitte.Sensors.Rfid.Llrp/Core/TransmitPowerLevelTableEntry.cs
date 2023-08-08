namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class TransmitPowerLevelTableEntry : LlrpTlvParameterBase
    {
        private ushort m_index;
        private short m_transmitPowerLevel;

        internal TransmitPowerLevelTableEntry(BitArray bitArray, ref int index) : base(LlrpParameterType.TransmitPowerLevelTableEntry, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort num2 = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            short transmitPowerLevel = (short) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(num2, transmitPowerLevel);
        }

        public TransmitPowerLevelTableEntry(ushort index, short transmitPowerLevel) : base(LlrpParameterType.TransmitPowerLevelTableEntry)
        {
            this.Init(index, transmitPowerLevel);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.Index, 0x10, true);
            stream.Append((long) this.TransmitPowerLevel, 0x10, true);
        }

        private void Init(ushort index, short transmitPowerLevel)
        {
            this.m_index = index;
            this.m_transmitPowerLevel = transmitPowerLevel;
            this.ParameterLength = 0x20;
        }

        public ushort Index
        {
            get
            {
                return this.m_index;
            }
        }

        public short TransmitPowerLevel
        {
            get
            {
                return this.m_transmitPowerLevel;
            }
        }
    }
}
