namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class EnableEventsAndReportsMessage : LlrpMessageRequestBase
    {
        public EnableEventsAndReportsMessage() : base(LlrpMessageType.EnableEventsAndReports)
        {
            this.Init();
        }

        internal EnableEventsAndReportsMessage(BitArray bitArray) : base(LlrpMessageType.EnableEventsAndReports, bitArray)
        {
            int index = 80;
            BitHelper.ValidateEndOfParameterOrMessage(index, (uint) bitArray.Count, base.GetType().FullName);
            this.Init();
        }

        internal override byte[] Encode()
        {
            return this.CreateHeaderStream().Merge();
        }

        private void Init()
        {
            this.MessageLength = 0L;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Enable Events And Reports Message>");
            builder.Append(base.ToString());
            builder.Append("</Enable Events And Reports Message>");
            return builder.ToString();
        }
    }
}
