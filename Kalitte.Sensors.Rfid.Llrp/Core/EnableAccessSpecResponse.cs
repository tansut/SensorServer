namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class EnableAccessSpecResponse : LlrpMessageResponseBase
    {
        internal EnableAccessSpecResponse(BitArray bitArray) : base(LlrpMessageType.EnableAccessSpecResponse, bitArray)
        {
            BitHelper.ValidateEndOfParameterOrMessage(base.BaseLength, (uint) bitArray.Count, base.GetType().FullName);
            this.Init();
        }

        public EnableAccessSpecResponse(uint messageId, LlrpStatus status) : base(LlrpMessageType.EnableAccessSpecResponse, messageId, status)
        {
        }

        internal override byte[] Encode()
        {
            return base.CreateResponseHeaderStream().Merge();
        }

        private void Init()
        {
            this.MessageLength = 0L;
        }
    }
}
