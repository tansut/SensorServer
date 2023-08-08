namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class KeepAliveAcknowledgementResponse : LlrpMessageBase
    {
        internal KeepAliveAcknowledgementResponse(BitArray bitArray) : base(LlrpMessageType.KeepAliveAcknowledgement, bitArray)
        {
            int index = 80;
            this.Init();
            BitHelper.ValidateEndOfParameterOrMessage(index, (uint) bitArray.Count, base.GetType().FullName);
        }

        public KeepAliveAcknowledgementResponse(uint messageId) : base(LlrpMessageType.KeepAliveAcknowledgement, messageId)
        {
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
            builder.Append("<Keep Alive Acknowledgement Response>");
            builder.Append(base.ToString());
            builder.Append("</Keep Alive Acknowledgement Response>");
            return builder.ToString();
        }
    }
}
