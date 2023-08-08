namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class GetAccessSpecMessage : LlrpMessageRequestBase
    {
        public GetAccessSpecMessage() : base(LlrpMessageType.GetAccessSpec)
        {
            this.Init();
        }

        internal GetAccessSpecMessage(BitArray bitArray) : base(LlrpMessageType.GetAccessSpec, bitArray)
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
    }
}
