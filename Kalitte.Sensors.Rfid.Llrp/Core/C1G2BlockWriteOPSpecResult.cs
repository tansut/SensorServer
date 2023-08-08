namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class C1G2BlockWriteOPSpecResult : C1G2OPSpecResult
    {
        private ushort m_numberOfWordsWritten;
        private ushort m_opSpecId;
        private C1G2BlockWriteOPSpecResultType m_resultType;

        internal C1G2BlockWriteOPSpecResult(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2BlockWriteOPSpecResult, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            C1G2BlockWriteOPSpecResultType enumInstance = BitHelper.GetEnumInstance<C1G2BlockWriteOPSpecResultType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            ushort specId = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            ushort numberOfWordsWritten = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(specId, enumInstance, numberOfWordsWritten);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.ResultType), 8, true);
            stream.Append((long) this.SpecId, 0x10, true);
            stream.Append((long) this.NumberOfWordsWritten, 0x10, true);
        }

        private void Init(ushort specId, C1G2BlockWriteOPSpecResultType resultType, ushort numberOfWordsWritten)
        {
            this.m_opSpecId = specId;
            this.m_resultType = resultType;
            this.m_numberOfWordsWritten = numberOfWordsWritten;
            this.ParameterLength = 40;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<C1G2 Block write OP Spec Result>");
            builder.Append(base.ToString());
            builder.Append("<Spec Id>");
            builder.Append(this.SpecId);
            builder.Append("</Spec Id>");
            builder.Append("<Result>");
            builder.Append(this.ResultType);
            builder.Append("</Result>");
            builder.Append("<Number of words written>");
            builder.Append(this.NumberOfWordsWritten);
            builder.Append("<Number of words written>");
            builder.Append("</C1G2 Block write OP Spec Result>");
            return builder.ToString();
        }

        public ushort NumberOfWordsWritten
        {
            get
            {
                return this.m_numberOfWordsWritten;
            }
        }

        public C1G2BlockWriteOPSpecResultType ResultType
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
