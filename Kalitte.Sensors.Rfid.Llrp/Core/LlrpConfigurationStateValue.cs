namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;

    public sealed class LlrpConfigurationStateValue : LlrpTlvParameterBase
    {
        private uint m_stateValue;

        public LlrpConfigurationStateValue(uint value) : base(LlrpParameterType.LlrpConfigurationStateValue)
        {
            this.Init(value);
        }

        internal LlrpConfigurationStateValue(BitArray bitArray, ref int index) : base(LlrpParameterType.LlrpConfigurationStateValue, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            uint num2 = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(num2);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.StateValue, 0x20, true);
        }

        private void Init(uint value)
        {
            this.m_stateValue = value;
            this.ParameterLength = 0x20;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<LLRP Configuration State Value>");
            builder.Append(base.ToString());
            builder.Append(this.StateValue);
            builder.Append("</LLRP Configuration State Value>");
            return builder.ToString();
        }

        public uint StateValue
        {
            get
            {
                return this.m_stateValue;
            }
        }
    }
}
