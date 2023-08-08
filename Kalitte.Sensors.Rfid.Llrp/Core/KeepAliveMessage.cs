namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class KeepAliveMessage : LlrpMessageBase
    {
        public KeepAliveMessage() : base(LlrpMessageType.KeepAlive, IdGenerator.GenerateLlrpMessageId())
        {
            this.Init();
        }

        internal KeepAliveMessage(BitArray bitArray) : base(LlrpMessageType.KeepAlive, bitArray)
        {
            int index = 80;
            this.Init();
            BitHelper.ValidateEndOfParameterOrMessage(index, (uint) bitArray.Count, base.GetType().FullName);
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
            builder.Append("<Keep Alive Message>");
            builder.Append(base.ToString());
            builder.Append("</Keep Alive Message>");
            return builder.ToString();
        }
    }
}
