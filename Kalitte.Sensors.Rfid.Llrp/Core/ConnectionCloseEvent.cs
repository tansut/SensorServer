namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    
    
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Events.Management;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Events;

    public sealed class ConnectionCloseEvent : LlrpEvent
    {
        internal ConnectionCloseEvent() : base(LlrpParameterType.ConnectionCloseEvent)
        {
            this.Init();
        }

        internal override Notification ConvertToRfidNotification()
        {
            return new Notification(new DeviceConnectionDownEvent(EventType.DeviceConnectionClosed.Description));
        }

 

 


        internal ConnectionCloseEvent(BitArray bitArray, ref int index) : base(bitArray, index, LlrpParameterType.ConnectionCloseEvent)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init();
        }


        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
        }

        private void Init()
        {
            this.ParameterLength = 0;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Connection Close Event>");
            builder.Append(base.ToString());
            builder.Append("</Connection Close Event>");
            return builder.ToString();
        }
    }
}
