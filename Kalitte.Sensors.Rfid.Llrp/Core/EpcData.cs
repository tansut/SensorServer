namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Utilities;

    public sealed class EpcData : LlrpTlvParameterBase
    {
        private byte[] m_EPC;
        private ushort m_EPCLength;

        internal EpcData(BitArray bitArray, ref int index) : base(LlrpParameterType.EpcData, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort length = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            byte[] epcData = null;
            if (length > 0)
            {
                epcData = BitHelper.ConvertBitArrayToByteArray(bitArray, ref index, length, false);
                int num3 = Util.BitsToPad(length);
                index += num3;
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(epcData, length);
        }

        public EpcData(byte[] epcData, ushort lengthInBits) : base(LlrpParameterType.EpcData)
        {
            this.Init(epcData, lengthInBits);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.EpcLength, 0x10, true);
            if (this.EpcLength > 0)
            {
                Util.AppendBytesAndPadToByteBoundary(stream, this.m_EPC, this.EpcLength, false);
            }
        }

        public byte[] GetData()
        {
            return Util.GetByteArrayClone(this.m_EPC);
        }

        private void Init(byte[] epcData, ushort lengthInBits)
        {
            if ((lengthInBits < 0) || (lengthInBits > ((epcData == null) ? 0 : (epcData.Length * 8))))
            {
                throw new ArgumentOutOfRangeException("lengthInBits");
            }
            this.m_EPC = epcData;
            this.m_EPCLength = lengthInBits;
            ushort num = Util.BitsToPad(lengthInBits);
            this.ParameterLength = (uint) ((0x10 + this.EpcLength) + num);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<EPC Data>");
            builder.Append(base.ToString());
            builder.Append(HexHelper.HexEncode(this.m_EPC));
            builder.Append("<EPC Length>");
            builder.Append(this.EpcLength);
            builder.Append("</EPC Length>");
            builder.Append("</EPC Data>");
            return builder.ToString();
        }

        public ushort EpcLength
        {
            get
            {
                return this.m_EPCLength;
            }
        }
    }
}
