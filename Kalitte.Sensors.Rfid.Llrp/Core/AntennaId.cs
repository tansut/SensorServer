namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class AntennaId : LlrpTVParameterBase, ICloneable
    {
        private ushort m_id;

        public AntennaId(ushort id) : base(LlrpParameterType.AntennaId)
        {
            this.m_id = id;
        }

        internal AntennaId(BitArray bitArray, ref int index) : base(LlrpParameterType.AntennaId, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            this.m_id = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
        }

        public object Clone()
        {
            return new AntennaId(this.m_id);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.m_id, 0x10, true);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Antenna Id>");
            builder.Append(base.ToString());
            builder.Append(this.Id);
            builder.Append("</Antenna Id>");
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
