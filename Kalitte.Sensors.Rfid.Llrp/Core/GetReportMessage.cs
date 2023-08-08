namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class GetReportMessage : LlrpMessageRequestBase
    {
        public GetReportMessage() : base(LlrpMessageType.GetReport)
        {
            this.Init();
        }

        internal GetReportMessage(BitArray bitArray) : base(LlrpMessageType.GetReport, bitArray)
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
            builder.Append("<Get Report Message>");
            builder.Append(base.ToString());
            builder.Append("</Get Report Message>");
            return builder.ToString();
        }
    }
}
