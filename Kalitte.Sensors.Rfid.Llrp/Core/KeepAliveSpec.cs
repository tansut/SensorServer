namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;

    public sealed class KeepAliveSpec : LlrpTlvParameterBase
    {
        private uint m_timeInterval;
        private KeepAliveTriggerType m_triggerType;

        public KeepAliveSpec(KeepAliveTriggerType type, uint interval) : base(LlrpParameterType.KeepAliveSpec)
        {
            this.Init(type, interval);
        }

        internal KeepAliveSpec(BitArray bitArray, ref int index) : base(LlrpParameterType.KeepAliveSpec, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            KeepAliveTriggerType enumInstance = BitHelper.GetEnumInstance<KeepAliveTriggerType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            uint interval = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, interval);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.TriggerType), 8, true);
            stream.Append((long) this.TimeInterval, 0x20, true);
        }

        private void Init(KeepAliveTriggerType type, uint interval)
        {
            this.m_triggerType = type;
            this.m_timeInterval = interval;
            this.ParameterLength = 40;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Keep Alive Spec>");
            builder.Append(base.ToString());
            builder.Append("<Trigger Type>");
            builder.Append(this.TriggerType);
            builder.Append("</Trigger Type>");
            builder.Append("<Time Interval>");
            builder.Append(this.TimeInterval);
            builder.Append("</Time Interval>");
            builder.Append("</Keep Alive Spec>");
            return builder.ToString();
        }

        public uint TimeInterval
        {
            get
            {
                return this.m_timeInterval;
            }
        }

        public KeepAliveTriggerType TriggerType
        {
            get
            {
                return this.m_triggerType;
            }
        }
    }
}
