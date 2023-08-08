namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class SpecIndex : LlrpTVParameterBase, ICloneable
    {
        private ushort m_index;

        public SpecIndex(ushort index) : base(LlrpParameterType.SpecIndex)
        {
            this.m_index = index;
        }

        internal SpecIndex(BitArray bitArray, ref int index) : base(LlrpParameterType.SpecIndex, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            this.m_index = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
        }

        public object Clone()
        {
            return new SpecIndex(this.m_index);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.m_index, 0x10, true);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Spec Index>");
            builder.Append(base.ToString());
            builder.Append(this.Index);
            builder.Append("</Spec Index>");
            return builder.ToString();
        }

        public ushort Index
        {
            get
            {
                return this.m_index;
            }
        }
    }
}
