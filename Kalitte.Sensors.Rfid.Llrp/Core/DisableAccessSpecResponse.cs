namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class DisableAccessSpecResponse : LlrpMessageResponseBase
    {
        internal DisableAccessSpecResponse(BitArray bitArray) : base(LlrpMessageType.DisableAccessSpecResponse, bitArray)
        {
            BitHelper.ValidateEndOfParameterOrMessage(base.BaseLength, (uint) bitArray.Count, base.GetType().FullName);
            this.Init();
        }

        public DisableAccessSpecResponse(uint messageId, LlrpStatus status) : base(LlrpMessageType.DisableAccessSpecResponse, messageId, status)
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
