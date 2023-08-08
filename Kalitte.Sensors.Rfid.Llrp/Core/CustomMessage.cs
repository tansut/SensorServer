namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class CustomMessage : CustomMessageBase
    {
        private byte[] m_data;

        internal CustomMessage(BitArray bitArray) : base(bitArray)
        {
            int startingIndex = 120;
            byte[] data = null;
            if (bitArray.Count > startingIndex)
            {
                data = BitHelper.ConvertBitArrayToByteArray(bitArray, ref startingIndex, bitArray.Count - startingIndex, false);
            }
            BitHelper.ValidateEndOfParameterOrMessage(startingIndex, (uint) bitArray.Count, base.GetType().FullName);
            this.Init(data);
        }

        public CustomMessage(uint vendorIana, uint subtype, byte[] data) : base(vendorIana, subtype)
        {
            this.Init(data);
        }

        internal override byte[] Encode()
        {
            LLRPMessageStream stream = this.CreateHeaderStream();
            if ((this.m_data != null) && (this.m_data.Length > 0))
            {
                stream.Append(this.m_data, (uint) (this.m_data.Length * 8), false);
            }
            return stream.Merge();
        }

        public byte[] GetVendorData()
        {
            return Util.GetByteArrayClone(this.m_data);
        }

        private void Init(byte[] data)
        {
            this.m_data = data;
            this.MessageLength = (this.m_data != null) ? ((ulong) (this.m_data.Length * 8)) : ((ulong) 0);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Custom Message>");
            builder.Append(base.ToString());
            builder.Append("</Custom Message>");
            return builder.ToString();
        }
    }
}
