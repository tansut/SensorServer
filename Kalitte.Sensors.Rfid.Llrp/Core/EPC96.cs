namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Utilities;

    public sealed class EPC96 : LlrpTVParameterBase
    {
        private byte[] m_epcData;

        public EPC96(byte[] epcData) : base(LlrpParameterType.EPC96)
        {
            this.Init(epcData);
        }

        internal EPC96(BitArray bitArray, ref int index) : base(LlrpParameterType.EPC96, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            byte[] epcData = BitHelper.ConvertBitArrayToByteArray(bitArray, ref index, 0x60, false);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(epcData);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append(this.m_epcData, 0x60, false);
        }

        public byte[] GetData()
        {
            return Util.GetByteArrayClone(this.m_epcData);
        }

        private void Init(byte[] epcData)
        {
            if (epcData == null)
            {
                throw new ArgumentNullException("epcData");
            }
            if (epcData.Length != 12)
            {
                throw new ArgumentOutOfRangeException("epcData");
            }
            this.m_epcData = epcData;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<EPC96>");
            builder.Append(base.ToString());
            builder.Append(HexHelper.HexEncode(this.m_epcData));
            builder.Append("</EPC96>");
            return builder.ToString();
        }
    }
}
