namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class ErrorMessage : LlrpMessageResponseBase
    {
        internal ErrorMessage(BitArray bitArray) : base(LlrpMessageType.ErrorMessage, bitArray)
        {
            BitHelper.ValidateEndOfParameterOrMessage(base.BaseLength, (uint) bitArray.Count, base.GetType().FullName);
        }

        public ErrorMessage(uint messageId, LlrpStatus status) : base(LlrpMessageType.ErrorMessage, messageId, status)
        {
            this.Init();
        }

        //public VendorDefinedCommand CreateVendorDefinedCommand()
        //{
        //    return Util.CreateVendorDefinedCommand(this);
        //}

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
            builder.Append("<Error Message>");
            builder.Append(base.ToString());
            builder.Append("</Error Message>");
            return builder.ToString();
        }
    }
}
