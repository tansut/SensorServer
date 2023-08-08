namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class EnableROSpecMessage : LlrpMessageRequestBase
    {
        private uint m_specId;

        internal EnableROSpecMessage(BitArray bitArray) : base(LlrpMessageType.EnableROSpec, bitArray)
        {
            int startingIndex = 80;
            uint specId = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 0x20);
            this.Init(specId);
            BitHelper.ValidateEndOfParameterOrMessage(startingIndex, (uint) bitArray.Count, base.GetType().FullName);
        }

        public EnableROSpecMessage(uint specId) : base(LlrpMessageType.EnableROSpec)
        {
            this.Init(specId);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = this.CreateHeaderStream();
            stream.Append((long) this.SpecId, 0x20, true);
            return stream.Merge();
        }

        private void Init(uint specId)
        {
            this.m_specId = specId;
            this.MessageLength = 0x20L;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Enable RO Spec Message>");
            builder.Append(base.ToString());
            builder.Append("<Spec Id>");
            builder.Append(this.SpecId);
            builder.Append("</Spec Id>");
            builder.Append("</Enable RO Spec Message>");
            return builder.ToString();
        }

        public uint SpecId
        {
            get
            {
                return this.m_specId;
            }
        }
    }
}
