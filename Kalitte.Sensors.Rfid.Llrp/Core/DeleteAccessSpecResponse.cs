namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class DeleteAccessSpecResponse : LlrpMessageResponseBase
    {
        internal DeleteAccessSpecResponse(BitArray bitArray) : base(LlrpMessageType.DeleteAccessSpecResponse, bitArray)
        {
            BitHelper.ValidateEndOfParameterOrMessage(base.BaseLength, (uint) bitArray.Count, base.GetType().FullName);
            this.Init();
        }

        public DeleteAccessSpecResponse(uint messageId, LlrpStatus status) : base(LlrpMessageType.DeleteAccessSpecResponse, messageId, status)
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
