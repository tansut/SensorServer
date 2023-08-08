namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class C1G2PC : AirProtocolTagData
    {
        private ushort m_pcBits;

        public C1G2PC(ushort pcBits) : base(LlrpParameterType.C1G2PC)
        {
            this.Init(pcBits);
        }

        internal C1G2PC(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2PC, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort pcBits = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(pcBits);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.m_pcBits, 0x10, true);
        }

        private void Init(ushort pcBits)
        {
            this.m_pcBits = pcBits;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<C1G2 PC>");
            builder.Append(base.ToString());
            builder.Append(this.PCBits);
            builder.Append("</C1G2 PC>");
            return builder.ToString();
        }

        public ushort PCBits
        {
            get
            {
                return this.m_pcBits;
            }
        }
    }
}
