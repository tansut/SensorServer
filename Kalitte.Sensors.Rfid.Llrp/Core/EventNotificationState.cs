namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class EventNotificationState : LlrpTlvParameterBase
    {
        private bool m_enabled;
        private Kalitte.Sensors.Rfid.Llrp.Core.EventNotificationStateEventType m_eventType;

        public EventNotificationState(Kalitte.Sensors.Rfid.Llrp.Core.EventNotificationStateEventType type, bool enabled) : base(LlrpParameterType.EventNotificationState)
        {
            this.Init(type, enabled);
        }

        internal EventNotificationState(BitArray bitArray, ref int index) : base(LlrpParameterType.EventNotificationState, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Kalitte.Sensors.Rfid.Llrp.Core.EventNotificationStateEventType enumInstance = BitHelper.GetEnumInstance<Kalitte.Sensors.Rfid.Llrp.Core.EventNotificationStateEventType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10));
            bool enabled = BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 1) == 1L;
            index += 7;
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, enabled);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((ushort) this.EventNotificationStateEventType), 0x10, true);
            stream.Append(this.Enabled, 1, true);
            stream.Append((ulong) 0L, 7, true);
        }

        private void Init(Kalitte.Sensors.Rfid.Llrp.Core.EventNotificationStateEventType type, bool enabled)
        {
            this.m_eventType = type;
            this.m_enabled = enabled;
            this.ParameterLength = 0x18;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Event Notification state>");
            builder.Append(base.ToString());
            builder.Append("<Type>");
            builder.Append(this.EventNotificationStateEventType);
            builder.Append("</Type>");
            builder.Append("<Enabled>");
            builder.Append(this.Enabled);
            builder.Append("</Enabled>");
            builder.Append("</Event Notification state>");
            return builder.ToString();
        }

        public bool Enabled
        {
            get
            {
                return this.m_enabled;
            }
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.EventNotificationStateEventType EventNotificationStateEventType
        {
            get
            {
                return this.m_eventType;
            }
        }
    }
}
