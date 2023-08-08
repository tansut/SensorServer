namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class IdentificationParameter : LlrpTlvParameterBase
    {
        private Kalitte.Sensors.Rfid.Llrp.Core.IdentificationType m_identificationType;
        private byte[] m_readerId;

        public IdentificationParameter(Kalitte.Sensors.Rfid.Llrp.Core.IdentificationType type, byte[] readerId) : base(LlrpParameterType.Identification)
        {
            this.Init(type, readerId);
        }

        internal IdentificationParameter(BitArray bitArray, ref int index) : base(LlrpParameterType.Identification, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            Kalitte.Sensors.Rfid.Llrp.Core.IdentificationType enumInstance = BitHelper.GetEnumInstance<Kalitte.Sensors.Rfid.Llrp.Core.IdentificationType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            ushort num2 = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            byte[] readerId = null;
            if (num2 > 0)
            {
                readerId = BitHelper.ConvertBitArrayToByteArray(bitArray, ref index, num2 * 8, false);
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(enumInstance, readerId);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((ulong) ((long) this.m_identificationType), 8, true);
            if (this.m_readerId != null)
            {
                stream.Append((long) this.m_readerId.Length, 0x10, true);
                stream.Append(this.m_readerId, (uint) (this.m_readerId.Length * 8), false);
            }
        }

        public byte[] GetReaderId()
        {
            return Util.GetByteArrayClone(this.m_readerId);
        }

        private void Init(Kalitte.Sensors.Rfid.Llrp.Core.IdentificationType type, byte[] readerId)
        {
            this.m_identificationType = type;
            this.m_readerId = readerId;
            this.ParameterLength = (uint) (0x18 + ((this.m_readerId != null) ? (this.m_readerId.Length * 8) : 0));
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Identification parameter>");
            builder.Append(base.ToString());
            builder.Append("<Type>");
            builder.Append(this.IdentificationType);
            builder.Append("</Type>");
            builder.Append("<Reader Id>");
            builder.Append(Util.GetString<byte>(this.m_readerId));
            builder.Append("</Reader Id>");
            builder.Append("</Identification parameter>");
            return builder.ToString();
        }

        public Kalitte.Sensors.Rfid.Llrp.Core.IdentificationType IdentificationType
        {
            get
            {
                return this.m_identificationType;
            }
        }
    }
}
