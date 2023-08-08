namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class LastSeenTimestampUptime : LlrpTVParameterBase, ICloneable
    {
        private ulong m_microSeconds;

        public LastSeenTimestampUptime(ulong microseconds) : base(LlrpParameterType.LastSeenTimestampUptime)
        {
            this.m_microSeconds = microseconds;
        }

        internal LastSeenTimestampUptime(BitArray bitArray, ref int index) : base(LlrpParameterType.LastSeenTimestampUptime, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            this.m_microSeconds = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x40);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
        }

        public object Clone()
        {
            return new LastSeenTimestampUptime(this.m_microSeconds);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append(this.m_microSeconds, 0x40, true);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Last Seen Timestamp Uptime>");
            builder.Append(base.ToString());
            builder.Append(this.Microseconds);
            builder.Append("</Last Seen Timestamp Uptime>");
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
