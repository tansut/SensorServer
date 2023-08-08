namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;

    public sealed class C1G2Crc : AirProtocolTagData
    {
        private ushort m_crc;

        public C1G2Crc(ushort crc) : base(LlrpParameterType.C1G2Crc)
        {
            this.Init(crc);
        }

        internal C1G2Crc(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2Crc, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort crc = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(crc);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.m_crc, 0x10, true);
        }

        private void Init(ushort crc)
        {
            this.m_crc = crc;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<C1G2 CRC>");
            builder.Append(base.ToString());
            builder.Append(this.CrcBits);
            builder.Append("</C1G2 CRC>");
            return builder.ToString();
        }

        public ushort CrcBits
        {
            get
            {
                return this.m_crc;
            }
        }
    }
}
