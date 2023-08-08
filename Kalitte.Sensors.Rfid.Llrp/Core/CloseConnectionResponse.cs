namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class CloseConnectionResponse : LlrpMessageResponseBase
    {
        internal CloseConnectionResponse(BitArray bitArray) : base(LlrpMessageType.CloseConnectionResponse, bitArray)
        {
            int baseLength = base.BaseLength;
            this.Init();
            BitHelper.ValidateEndOfParameterOrMessage(baseLength, (uint) bitArray.Count, base.GetType().FullName);
        }

        public CloseConnectionResponse(uint messageId, LlrpStatus status) : base(LlrpMessageType.CloseConnectionResponse, messageId, status)
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
            builder.Append("<Close connection Response>");
            builder.Append(base.ToString());
            builder.Append("</Close connection Response>");
            return builder.ToString();
        }
    }
}
