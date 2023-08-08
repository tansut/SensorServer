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

    public sealed class ReportBufferLevelWarningEvent : LlrpEvent
    {
        private byte m_percentageFull;

        internal ReportBufferLevelWarningEvent(byte percentageFull) : base(LlrpParameterType.ReportBufferLevelWarningEvent)
        {
            this.Init(percentageFull);
        }

        internal override Notification ConvertToRfidNotification()
        {
            return new Notification(new FreeMemoryLowEvent(EventLevel.Warning, LlrpResources.ReportBufferLevelWarningEventDescription, 100 - this.PercentageFull));
        }

 

 


        internal ReportBufferLevelWarningEvent(BitArray bitArray, ref int index) : base(bitArray, index, LlrpParameterType.ReportBufferLevelWarningEvent)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            byte percentage = (byte) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(percentage);
        }


        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.PercentageFull, 8, true);
        }

        private void Init(byte percentage)
        {
            if ((percentage < 0) || (percentage > 100))
            {
                throw new ArgumentOutOfRangeException("percentage");
            }
            this.m_percentageFull = percentage;
            this.ParameterLength = 8;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Report Buffer Level Warning Event>");
            builder.Append(base.ToString());
            builder.Append("<Percentage Full>");
            builder.Append(this.PercentageFull);
            builder.Append("</Percentage Full>");
            builder.Append("</Report Buffer Level Warning Event>");
            return builder.ToString();
        }

        public byte PercentageFull
        {
            get
            {
                return this.m_percentageFull;
            }
        }
    }
}
