namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class OPSpecId : LlrpTVParameterBase
    {
        private ushort m_id;

        public OPSpecId(ushort id) : base(LlrpParameterType.OPSpecId)
        {
            this.Init(id);
        }

        internal OPSpecId(BitArray bitArray, ref int index) : base(LlrpParameterType.OPSpecId, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            ushort id = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(id);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.Id, 0x10, true);
        }

        private void Init(ushort id)
        {
            if (id == 0)
            {
                throw new ArgumentOutOfRangeException("id");
            }
            this.m_id = id;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<OP Spec Id>");
            builder.Append(base.ToString());
            builder.Append(this.Id);
            builder.Append("</OP Spec Id>");
            return builder.ToString();
        }

        public ushort Id
        {
            get
            {
                return this.m_id;
            }
        }
    }
}
