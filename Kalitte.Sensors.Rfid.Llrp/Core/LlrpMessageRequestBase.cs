namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    

    public abstract class LlrpMessageRequestBase : LlrpMessageBase
    {
        protected LlrpMessageRequestBase(LlrpMessageType type) : base(type)
        {
        }

        protected internal LlrpMessageRequestBase(LlrpMessageType type, BitArray bitArray) : base(type, bitArray)
        {
        }

        protected LlrpMessageRequestBase(LlrpMessageType messageType, uint messageId) : base(messageType, messageId)
        {
        }

        //public VendorDefinedCommand CreateVendorDefinedCommand()
        //{
        //    return Util.CreateVendorDefinedCommand(this);
        //}
    }
}
