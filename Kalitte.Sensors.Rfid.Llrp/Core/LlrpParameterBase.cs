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

    [Serializable]
    public abstract class LlrpParameterBase
    {
        private uint m_parameterLength;
        private LlrpParameterType m_parameterType;

        protected LlrpParameterBase(LlrpParameterType parameterType)
        {
            this.m_parameterType = parameterType;
        }

        internal LlrpParameterBase(LlrpParameterType parameterType, BitArray bitArray, int index) : this(parameterType)
        {
            if (bitArray == null)
            {
                throw new ArgumentNullException("bitArray");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (BitHelper.IsTVParameter(bitArray, index))
            {
                if (bitArray.Count < (index + LlrpTVParameterBase.HeaderLength))
                {
                    throw new DecodingException("Incomplete Message", LlrpResources.InCompleteMessage);
                }
            }
            else if (bitArray.Count < (index + LlrpTlvParameterBase.HeaderLength))
            {
                throw new DecodingException("Incomplete Message", LlrpResources.InCompleteMessage);
            }
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            if (bitArray.Count < parameterEndLimit)
            {
                throw new DecodingException("Incomplete Message", LlrpResources.InCompleteMessage);
            }
        }

        internal abstract void Encode(LLRPMessageStream stream);
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            Util.ToStringBase(this, str);
            return str.ToString();
        }

        internal virtual uint ParameterLength
        {
            get
            {
                return this.m_parameterLength;
            }
            set
            {
                if ((value / 8) > 0xffff)
                {
                    throw new ArgumentOutOfRangeException("ParameterLength", string.Format(CultureInfo.CurrentCulture, LlrpResources.ParameterLengthExceeded, new object[] { (ushort) 0xffff }));
                }
                this.m_parameterLength = value;
            }
        }

        internal LlrpParameterType ParameterType
        {
            get
            {
                return this.m_parameterType;
            }
        }
    }
}
