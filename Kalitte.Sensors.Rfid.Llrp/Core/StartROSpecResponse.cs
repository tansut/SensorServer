namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class StartROSpecResponse : LlrpMessageResponseBase
    {
        internal StartROSpecResponse(BitArray bitArray) : base(LlrpMessageType.StartROSpecResponse, bitArray)
        {
            int baseLength = base.BaseLength;
            this.Init();
            BitHelper.ValidateEndOfParameterOrMessage(baseLength, (uint) bitArray.Count, base.GetType().FullName);
        }

        public StartROSpecResponse(uint messageId, LlrpStatus status) : base(LlrpMessageType.StartROSpecResponse, messageId, status)
        {
            this.Init();
        }

        internal override byte[] Encode()
        {
            return base.CreateResponseHeaderStream().Merge();
        }

        private void Init()
        {
            this.MessageLength = 0L;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Start RO Spec Response>");
            builder.Append(base.ToString());
            builder.Append("</Start RO Spec Response>");
            return builder.ToString();
        }
    }
}
