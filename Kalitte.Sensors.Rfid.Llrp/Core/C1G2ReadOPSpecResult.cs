namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class C1G2ReadOPSpecResult : C1G2OPSpecResult
    {
        private ushort m_opSpecId;
        private short[] m_readData;
        private C1G2ReadOPSpecResultType m_resultType;

        internal C1G2ReadOPSpecResult(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2ReadOPSpecResult, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            C1G2ReadOPSpecResultType enumInstance = BitHelper.GetEnumInstance<C1G2ReadOPSpecResultType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            ushort opSpecId = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort num3 = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            short[] readData = null;
            if (num3 > 0)
            {
                readData = new short[num3];
                for (int i = 0; i < num3; i++)
                {
                    readData[i] = (short) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
                }
            }
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(opSpecId, enumInstance, readData);
        }

        public C1G2ReadOPSpecResult(ushort opSpecId, C1G2ReadOPSpecResultType resultType, short[] readData) : base(LlrpParameterType.C1G2ReadOPSpecResult)
        {
            this.Init(opSpecId, resultType, readData);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.ResultType), 8, true);
            stream.Append((long) this.SpecId, 0x10, true);
            ushort length = 0;
            if (this.m_readData != null)
            {
                length = (ushort) this.m_readData.Length;
            }
            stream.Append((long) length, 0x10, true);
            if (length > 0)
            {
                foreach (short num2 in this.m_readData)
                {
                    stream.Append((long) num2, 0x10, true);
                }
            }
        }

        public short[] GetReadData()
        {
            return Util.GetArrayClone<short>(this.m_readData);
        }

        private void Init(ushort opSpecId, C1G2ReadOPSpecResultType resultType, short[] readData)
        {
            this.m_opSpecId = opSpecId;
            this.m_resultType = resultType;
            this.m_readData = readData;
            this.ParameterLength = (uint) (40 + ((readData != null) ? (readData.Length * 0x10) : 0));
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<C1G2 Read OP Spec Result>");
            builder.Append(base.ToString());
            builder.Append("<Spec Id>");
            builder.Append(this.SpecId);
            builder.Append("</Spec Id>");
            builder.Append("<Result>");
            builder.Append(this.ResultType);
            builder.Append("</Result>");
            builder.Append("<Data>");
            builder.Append(Util.GetString<short>(this.m_readData));
            builder.Append("<Data>");
            builder.Append("</C1G2 Read OP Spec Result>");
            return builder.ToString();
        }

        public C1G2ReadOPSpecResultType ResultType
        {
            get
            {
                return this.m_resultType;
            }
        }

        public ushort SpecId
        {
            get
            {
                return this.m_opSpecId;
            }
        }
    }
}
