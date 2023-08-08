namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class TagSeenCount : LlrpTVParameterBase, ICloneable
    {
        private ushort m_tagSeenCount;

        public TagSeenCount(ushort tagSeenCount) : base(LlrpParameterType.TagSeenCount)
        {
            this.m_tagSeenCount = tagSeenCount;
        }

        internal TagSeenCount(BitArray bitArray, ref int index) : base(LlrpParameterType.TagSeenCount, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            this.m_tagSeenCount = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
        }

        public object Clone()
        {
            return new TagSeenCount(this.m_tagSeenCount);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.m_tagSeenCount, 0x10, true);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Tag Seen count>");
            builder.Append(base.ToString());
            builder.Append(this.TagCount);
            builder.Append("</Tag Seen count>");
            return builder.ToString();
        }

        public ushort TagCount
        {
            get
            {
                return this.m_tagSeenCount;
            }
        }
    }
}
