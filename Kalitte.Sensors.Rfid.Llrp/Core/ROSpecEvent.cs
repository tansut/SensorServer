namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Events;

    public sealed class ROSpecEvent : LlrpEvent
    {
        private ROSpecEventType m_eventType;
        private uint m_preemptedId;
        private uint m_roSpecId;

        internal override Notification ConvertToRfidNotification()
        {
            return null;
        }

 

 


        internal ROSpecEvent(BitArray bitArray, ref int index) : base(bitArray, index, LlrpParameterType.ROSpecEvent)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ROSpecEventType enumInstance = BitHelper.GetEnumInstance<ROSpecEventType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            uint roSpecId = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint preemtedId = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, roSpecId, preemtedId);
        }


        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.EventType), 8, true);
            stream.Append((long) this.ROSpecId, 0x20, true);
            stream.Append((long) this.PreemptedROSpecId, 0x20, true);
        }

        private void Init(ROSpecEventType type, uint roSpecId, uint preemtedId)
        {
            this.m_eventType = type;
            this.m_roSpecId = roSpecId;
            this.m_preemptedId = preemtedId;
            this.ParameterLength = 0x48;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<RO Spec event>");
            builder.Append(base.ToString());
            builder.Append("<RO Spec Id>");
            builder.Append(this.ROSpecId);
            builder.Append("</RO Spec Id>");
            builder.Append("<Preempted Id>");
            builder.Append(this.PreemptedROSpecId);
            builder.Append("</Preempted Id>");
            builder.Append("<Type>");
            builder.Append(this.EventType);
            builder.Append("</Type>");
            builder.Append("</RO Spec event>");
            return builder.ToString();
        }

        public ROSpecEventType EventType
        {
            get
            {
                return this.m_eventType;
            }
        }

        public uint PreemptedROSpecId
        {
            get
            {
                return this.m_preemptedId;
            }
        }

        public uint ROSpecId
        {
            get
            {
                return this.m_roSpecId;
            }
        }
    }
}
