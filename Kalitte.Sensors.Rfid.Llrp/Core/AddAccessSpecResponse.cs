namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class AddAccessSpecResponse : LlrpMessageResponseBase
    {
        internal AddAccessSpecResponse(BitArray bitArray) : base(LlrpMessageType.AddAccessSpecResponse, bitArray)
        {
            BitHelper.ValidateEndOfParameterOrMessage(base.BaseLength, (uint) bitArray.Count, base.GetType().FullName);
            this.Init();
        }

        public AddAccessSpecResponse(uint messageId, LlrpStatus status) : base(LlrpMessageType.AddAccessSpecResponse, messageId, status)
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
