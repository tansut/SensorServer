namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Events.Management;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Events;

    public sealed class ReportBufferOverflowErrorEvent : LlrpEvent
    {
        internal ReportBufferOverflowErrorEvent(BitArray bitArray, ref int index) : base(bitArray, index, LlrpParameterType.ReportBufferOverflowErrorEvent)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
        }

        internal override Notification ConvertToRfidNotification()
        {
            return new Notification(new FreeMemoryLowEvent(EventLevel.Fatal, LlrpResources.ReportBufferOverflowErrorEventDescription, 0));
        }

 

 


        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Report Buffer overflow Event>");
            builder.Append(base.ToString());
            builder.Append("</Report Buffer overflow Event>");
            return builder.ToString();
        }
    }
}
