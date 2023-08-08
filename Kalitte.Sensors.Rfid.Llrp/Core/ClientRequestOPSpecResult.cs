namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class ClientRequestOPSpecResult : LlrpTVParameterBase
    {
        private ushort m_opSpecId;

        internal ClientRequestOPSpecResult(BitArray bitArray, ref int index) : base(LlrpParameterType.ClientOperationOPSpecResult, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort opSpecId = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(opSpecId);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.OPSpecId, 0x10, true);
        }

        private void Init(ushort OpSpecId)
        {
            this.m_opSpecId = OpSpecId;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Client Request OP Spec Result>");
            builder.Append(base.ToString());
            builder.Append(this.OPSpecId);
            builder.Append("</Client Request OP Spec Result>");
            return builder.ToString();
        }

        public ushort OPSpecId
        {
            get
            {
                return this.m_opSpecId;
            }
        }
    }
}
