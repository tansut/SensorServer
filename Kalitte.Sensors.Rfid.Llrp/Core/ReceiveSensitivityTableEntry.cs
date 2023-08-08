namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class ReceiveSensitivityTableEntry : LlrpTlvParameterBase
    {
        private ushort m_index;
        private short m_receiveSensitivityValue;

        internal ReceiveSensitivityTableEntry(BitArray bitArray, ref int index) : base(LlrpParameterType.ReceiveSensitivityTableEntry, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort num2 = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            short receiveSensitivityValue = (short) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(num2, receiveSensitivityValue);
        }

        public ReceiveSensitivityTableEntry(ushort index, short receiveSensitivityValue) : base(LlrpParameterType.ReceiveSensitivityTableEntry)
        {
            this.Init(index, receiveSensitivityValue);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.Index, 0x10, true);
            stream.Append((long) this.ReceiveSensitivityValue, 0x10, true);
        }

        private void Init(ushort index, short receiveSensitivityValue)
        {
            if ((receiveSensitivityValue < 0) || (receiveSensitivityValue > 0x80))
            {
                throw new ArgumentOutOfRangeException("receiveSensitivityValue");
            }
            this.m_index = index;
            this.m_receiveSensitivityValue = receiveSensitivityValue;
            this.ParameterLength = 0x20;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Receive Sensitivity Table Entry>");
            builder.Append(base.ToString());
            builder.Append("<Index>");
            builder.Append(this.Index);
            builder.Append("</Index>");
            builder.Append("<Value>");
            builder.Append(this.ReceiveSensitivityValue);
            builder.Append("</Value>");
            builder.Append("</Receive Sensitivity Table Entry>");
            return builder.ToString();
        }

        public ushort Index
        {
            get
            {
                return this.m_index;
            }
        }

        public short ReceiveSensitivityValue
        {
            get
            {
                return this.m_receiveSensitivityValue;
            }
        }
    }
}
