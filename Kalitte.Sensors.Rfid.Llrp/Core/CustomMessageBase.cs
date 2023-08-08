namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Rfid.Llrp.Exceptions;

    public abstract class CustomMessageBase : LlrpMessageRequestBase
    {
        protected const uint BaseLength = 120;
        private const uint CustomMessageHeaderLength = 40;
        private uint m_subtype;
        private uint m_vendorIANA;


        protected CustomMessageBase(BitArray bitArray) : base(LlrpMessageType.CustomMessage, bitArray)
        {
            int startingIndex = 80;
            uint vendorIana = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 0x20);
            uint subtype = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 8);
            this.Init(vendorIana, subtype);
        }



        protected CustomMessageBase(uint vendorIana, uint subtype) : base(LlrpMessageType.CustomMessage)
        {
            this.Init(vendorIana, subtype);
        }

        protected CustomMessageBase(uint vendorIana, uint subtype, uint messageId) : base(LlrpMessageType.CustomMessage, messageId)
        {
            this.Init(vendorIana, subtype);
        }

        internal override LLRPMessageStream CreateHeaderStream()
        {
            LLRPMessageStream stream = base.CreateHeaderStream();
            stream.Append((long) this.VendorIana, 0x20, true);
            stream.Append((long) this.MessageSubtype, 8, true);
            return stream;
        }

        internal static CustomMessageBase GetInstance(BitArray bitArray)
        {
            if (bitArray.Count < 120L)
            {
                throw new DecodingException("Incomplete Message", string.Format(CultureInfo.CurrentCulture, LlrpResources.InCompleteMessageWithName, new object[] { LlrpResources.CustomMessage }));
            }
            int startingIndex = 0;
            startingIndex += 80;
            uint num2 = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 0x20);
            uint num3 = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 8);
            return new CustomMessage(bitArray);
        }

        private void Init(uint vendorIana, uint subtype)
        {
            if ((subtype < 0) || (subtype > 0xff))
            {
                throw new ArgumentOutOfRangeException("subtype");
            }
            this.m_vendorIANA = vendorIana;
            this.m_subtype = subtype;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Custom Message Base>");
            builder.Append(base.ToString());
            builder.Append("<Vendor Identifier>");
            builder.Append(this.VendorIana);
            builder.Append("</Vendor Identifier>");
            builder.Append("<Sub Type>");
            builder.Append(this.MessageSubtype);
            builder.Append("</Sub Type>");
            builder.Append("</Custom Message Base>");
            return builder.ToString();
        }

        internal override ulong MessageLength
        {
            get
            {
                return base.MessageLength;
            }
            set
            {
                base.MessageLength = value + ((ulong) 40L);
            }
        }

        public uint MessageSubtype
        {
            get
            {
                return this.m_subtype;
            }
        }

        public uint VendorIana
        {
            get
            {
                return this.m_vendorIANA;
            }
        }
    }
}
