namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class C1G2RFControl : LlrpTlvParameterBase
    {
        private ushort m_modeIndex;
        private short m_tari;

        internal C1G2RFControl(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2RFControl, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort modeIndex = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            short tari = (short) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(modeIndex, tari);
        }

        public C1G2RFControl(ushort modeIndex, short tari) : base(LlrpParameterType.C1G2RFControl)
        {
            this.Init(modeIndex, tari);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.ModeIndex, 0x10, true);
            stream.Append((long) this.Tari, 0x10, true);
        }

        private void Init(ushort modeIndex, short tari)
        {
            this.m_modeIndex = modeIndex;
            this.m_tari = tari;
            this.ParameterLength = 0x20;
        }

        public ushort ModeIndex
        {
            get
            {
                return this.m_modeIndex;
            }
        }

        public short Tari
        {
            get
            {
                return this.m_tari;
            }
        }
    }
}
