namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class ChannelIndex : LlrpTVParameterBase, ICloneable
    {
        private ushort m_channelIndex;

        public ChannelIndex(ushort channelIndex) : base(LlrpParameterType.ChannelIndex)
        {
            this.Init(channelIndex);
        }

        internal ChannelIndex(BitArray bitArray, ref int index) : base(LlrpParameterType.ChannelIndex, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort channelIndex = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(channelIndex);
        }

        public object Clone()
        {
            return new ChannelIndex(this.m_channelIndex);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.m_channelIndex, 0x10, true);
        }

        private void Init(ushort channelIndex)
        {
            this.m_channelIndex = channelIndex;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Channel Index>");
            builder.Append(base.ToString());
            builder.Append(this.Index);
            builder.Append("</Channel Index>");
            return builder.ToString();
        }

        public ushort Index
        {
            get
            {
                return this.m_channelIndex;
            }
        }
    }
}
