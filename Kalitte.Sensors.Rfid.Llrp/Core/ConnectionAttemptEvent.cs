namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Events.Management;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Llrp.Events;
    using Kalitte.Sensors.Events;

    public sealed class ConnectionAttemptEvent : LlrpEvent
    {
        private ConnectionAttemptEventType m_eventType;

        public ConnectionAttemptEvent(ConnectionAttemptEventType type) : base(LlrpParameterType.ConnectionAttemptEvent)
        {
            this.Init(type);
        }

        internal ConnectionAttemptEvent(BitArray bitArray, ref int index) : base(bitArray, index, LlrpParameterType.ConnectionAttemptEvent)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ConnectionAttemptEventType enumInstance = BitHelper.GetEnumInstance<ConnectionAttemptEventType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10));
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance);
        }

        internal override Notification ConvertToRfidNotification()
        {
            VendorData vendorData = new VendorData();
            vendorData.Add("Message", this.Status.ToString());
            return new Notification(new VendorDefinedManagementEvent(EventLevel.Info, LlrpEventTypes.ConnectionAttemptEvent, LlrpEventTypes.ConnectionAttemptEvent.Description, typeof(ConnectionAttemptEvent).Name, vendorData));
        }

 

 

       

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((ulong) ((long) this.m_eventType), 0x10, true);
        }

        private void Init(ConnectionAttemptEventType type)
        {
            this.m_eventType = type;
            this.ParameterLength = 0x10;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Connection Attempt Event>");
            builder.Append(base.ToString());
            builder.Append("<Status>");
            builder.Append(this.Status);
            builder.Append("</Status>");
            builder.Append("</Connection Attempt Event>");
            return builder.ToString();
        }

        public ConnectionAttemptEventType Status
        {
            get
            {
                return this.m_eventType;
            }
        }
    }
}
