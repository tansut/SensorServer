namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public sealed class GenericCustomParameter : CustomParameterBase
    {
        private byte[] m_data;

        internal GenericCustomParameter(BitArray bitArray, ref int index) : base(bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            index += 0x40;
            byte[] data = null;
            if (parameterEndLimit > ((long) index))
            {
                data = BitHelper.ConvertBitArrayToByteArray(bitArray, ref index, (int) (parameterEndLimit - ((long) index)), false);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(data);
        }

        public GenericCustomParameter(uint vendorIANA, uint subtype, byte[] data) : base(vendorIANA, subtype)
        {
            this.Init(data);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            if ((this.m_data != null) && (this.m_data.Length > 0))
            {
                stream.Append(this.m_data, (ushort) (this.m_data.Length * 8), false);
            }
        }

        public byte[] GetData()
        {
            return Util.GetByteArrayClone(this.m_data);
        }

        private void Init(byte[] data)
        {
            if ((data != null) && (data.Length > ConstantValues.MaximumUserByteDataInCustomParameter))
            {
                throw new ArgumentOutOfRangeException("data");
            }
            this.m_data = data;
            this.ParameterLength = (this.m_data != null) ? ((uint) (this.m_data.Length * 8)) : 0;
        }
    }
}
