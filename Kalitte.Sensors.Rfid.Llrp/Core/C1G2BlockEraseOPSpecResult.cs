namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class C1G2BlockEraseOPSpecResult : C1G2OPSpecResult
    {
        private ushort m_opSpecId;
        private C1G2BlockEraseOPSpecResultType m_resultType;

        internal C1G2BlockEraseOPSpecResult(BitArray bitArray, ref int index) : base(LlrpParameterType.C1G2BlockEraseOPSpecResult, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            C1G2BlockEraseOPSpecResultType enumInstance = BitHelper.GetEnumInstance<C1G2BlockEraseOPSpecResultType>(BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 8));
            ushort opSpecId = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(opSpecId, enumInstance);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) ((byte) this.ResultType), 8, true);
            stream.Append((long) this.SpecId, 0x10, true);
        }

        private void Init(ushort opSpecId, C1G2BlockEraseOPSpecResultType resultType)
        {
            this.m_opSpecId = opSpecId;
            this.m_resultType = resultType;
            this.ParameterLength = 0x18;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<C1G2 Block erase OP Spec Result>");
            builder.Append(base.ToString());
            builder.Append("<Spec Id>");
            builder.Append(this.SpecId);
            builder.Append("</Spec Id>");
            builder.Append("<Result>");
            builder.Append(this.ResultType);
            builder.Append("</Result>");
            builder.Append("<Number of words written>");
            builder.Append("</C1G2 Block Erase OP Spec Result>");
            return builder.ToString();
        }

        public C1G2BlockEraseOPSpecResultType ResultType
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
