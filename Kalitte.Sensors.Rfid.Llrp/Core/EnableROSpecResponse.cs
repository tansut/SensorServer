namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class EnableROSpecResponse : LlrpMessageResponseBase
    {
        internal EnableROSpecResponse(BitArray bitArray) : base(LlrpMessageType.EnableROSpecResponse, bitArray)
        {
            int baseLength = base.BaseLength;
            this.Init();
            BitHelper.ValidateEndOfParameterOrMessage(baseLength, (uint) bitArray.Count, base.GetType().FullName);
        }

        public EnableROSpecResponse(uint messageId, LlrpStatus status) : base(LlrpMessageType.EnableROSpecResponse, messageId, status)
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
            builder.Append("<Enable RO Spec Response>");
            builder.Append(base.ToString());
            builder.Append("</Enable RO Spec Response>");
            return builder.ToString();
        }
    }
}
