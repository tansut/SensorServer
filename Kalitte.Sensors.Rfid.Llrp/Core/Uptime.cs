namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;

    [Serializable]
    public sealed class Uptime : LlrpTlvParameterBase
    {
        private ulong m_timeElapsed;

        public Uptime(ulong timeElapsed) : base(LlrpParameterType.Uptime)
        {
            this.Init(timeElapsed);
        }

        internal Uptime(BitArray bitArray, ref int index) : base(LlrpParameterType.Uptime, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ulong timeElapsed = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x40);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(timeElapsed);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append(this.TimeElapsed, 0x40, true);
        }

        private void Init(ulong timeElapsed)
        {
            this.m_timeElapsed = timeElapsed;
            this.ParameterLength = 0x40;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Uptime>");
            builder.Append(base.ToString());
            builder.Append(this.TimeElapsed);
            builder.Append("</Uptime>");
            return builder.ToString();
        }

        public ulong TimeElapsed
        {
            get
            {
                return this.m_timeElapsed;
            }
        }
    }
}
