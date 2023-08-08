namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class LastSeenTimestampUtc : LlrpTVParameterBase, ICloneable
    {
        private ulong m_microSeconds;

        public LastSeenTimestampUtc(ulong microseconds) : base(LlrpParameterType.LastSeenTimestampUtc)
        {
            this.m_microSeconds = microseconds;
        }

        internal LastSeenTimestampUtc(BitArray bitArray, ref int index) : base(LlrpParameterType.LastSeenTimestampUtc, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            this.m_microSeconds = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x40);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
        }

        public object Clone()
        {
            return new LastSeenTimestampUtc(this.m_microSeconds);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append(this.m_microSeconds, 0x40, true);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Last Seen Timestamp UTC>");
            builder.Append(base.ToString());
            builder.Append(this.Microseconds);
            builder.Append("</Last Seen Timestamp UTC>");
            return builder.ToString();
        }

        public ulong Microseconds
        {
            get
            {
                return this.m_microSeconds;
            }
        }
    }
}
