namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class SetReaderConfigurationResponse : LlrpMessageResponseBase
    {
        internal SetReaderConfigurationResponse(BitArray bitArray) : base(LlrpMessageType.SetReaderConfigResponse, bitArray)
        {
            int baseLength = base.BaseLength;
            this.Init();
            BitHelper.ValidateEndOfParameterOrMessage(baseLength, (uint) bitArray.Count, base.GetType().FullName);
        }

        public SetReaderConfigurationResponse(uint messageId, LlrpStatus status) : base(LlrpMessageType.SetReaderConfigResponse, messageId, status)
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
            builder.Append("<Set Reader Configuration Response>");
            builder.Append(base.ToString());
            builder.Append("</Set Reader Configuration Response>");
            return builder.ToString();
        }
    }
}
