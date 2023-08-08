namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class ROSpecId : LlrpTVParameterBase, ICloneable
    {
        private uint m_id;

        public ROSpecId(uint id) : base(LlrpParameterType.ROSpecId)
        {
            this.m_id = id;
        }

        internal ROSpecId(BitArray bitArray, ref int index) : base(LlrpParameterType.ROSpecId, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            this.m_id = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
        }

        public object Clone()
        {
            return new ROSpecId(this.m_id);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.m_id, 0x20, true);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<RO Spec Id>");
            builder.Append(base.ToString());
            builder.Append(this.Id);
            builder.Append("</RO Spec Id>");
            return builder.ToString();
        }

        public uint Id
        {
            get
            {
                return this.m_id;
            }
        }
    }
}
