namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Globalization;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Rfid.Llrp.Exceptions;

    [Serializable]
    public abstract class CustomParameterBase : LlrpTlvParameterBase
    {
        protected const uint BaseLength = 0x40;
        private uint m_subType;
        private uint m_vendorIANA;


        protected internal CustomParameterBase(BitArray bitArray, int index) : base(LlrpParameterType.Custom, bitArray, index)
        {
            index += LlrpTlvParameterBase.HeaderLength;
            uint vendorIANA = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            uint subType = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            this.Init(vendorIANA, subType);
        }

        protected CustomParameterBase(uint vendorIANA, uint subtype) : base(LlrpParameterType.Custom)
        {
            this.Init(vendorIANA, subtype);
        }


        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.VendorIana, 0x20, true);
            stream.Append((long) this.CustomSubtype, 0x20, true);
        }

        internal static CustomParameterBase GetInstance(BitArray bitArray, ref int index)
        {
            if (bitArray.Count < (((long) (index + LlrpTlvParameterBase.HeaderLength)) + 0x40L))
            {
                throw new DecodingException("Incomplete parameter", string.Format(CultureInfo.CurrentCulture, LlrpResources.InCompleteParameter, new object[] { LlrpResources.CustomParameter }));
            }
            int startingIndex = index;
            startingIndex += LlrpTlvParameterBase.HeaderLength;
            uint num2 = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 0x20);
            uint num3 = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref startingIndex, 0x20);
            
            return new GenericCustomParameter(bitArray, ref index);
        }

        private void Init(uint vendorIANA, uint subType)
        {
            this.m_vendorIANA = vendorIANA;
            this.m_subType = subType;
        }

        public uint CustomSubtype
        {
            get
            {
                return this.m_subType;
            }
        }

        internal override uint ParameterLength
        {
            set
            {
                base.ParameterLength = value + 0x40;
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
