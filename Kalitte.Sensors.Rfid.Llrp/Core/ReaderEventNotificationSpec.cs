namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class ReaderEventNotificationSpec : LlrpTlvParameterBase
    {
        private Collection<EventNotificationState> m_eventStates;

        public ReaderEventNotificationSpec(Collection<EventNotificationState> eventStates) : base(LlrpParameterType.ReaderEventNotificationSpec)
        {
            this.Init(eventStates);
        }

        internal ReaderEventNotificationSpec(BitArray bitArray, ref int index) : base(LlrpParameterType.ReaderEventNotificationSpec, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Collection<EventNotificationState> eventStates = new Collection<EventNotificationState>();
            while (BitHelper.IsLLRPParameterPresent(LlrpParameterType.EventNotificationState, bitArray, index, parameterEndLimit))
            {
                eventStates.Add(new EventNotificationState(bitArray, ref index));
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(eventStates);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            Util.Encode<EventNotificationState>(this.EventNotificationStates, stream);
        }

        private void Init(Collection<EventNotificationState> eventStates)
        {
            if ((eventStates == null) || (eventStates.Count == 0))
            {
                throw new ArgumentNullException("eventStates");
            }
            this.m_eventStates = eventStates;
            this.ParameterLength = Util.GetTotalBitLengthOfParam<EventNotificationState>(this.m_eventStates);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Reader Event Notification Spec>");
            strBuilder.Append(base.ToString());
            Util.ToString<EventNotificationState>(this.EventNotificationStates, strBuilder);
            strBuilder.Append("</Reader Event Notification Spec>");
            return strBuilder.ToString();
        }

        public Collection<EventNotificationState> EventNotificationStates
        {
            get
            {
                return this.m_eventStates;
            }
        }
    }
}
