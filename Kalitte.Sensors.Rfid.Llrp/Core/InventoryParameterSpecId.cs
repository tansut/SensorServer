namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class InventoryParameterSpecId : LlrpTVParameterBase, ICloneable
    {
        private ushort m_id;

        public InventoryParameterSpecId(ushort id) : base(LlrpParameterType.InventoryParameterSpecId)
        {
            this.m_id = id;
        }

        internal InventoryParameterSpecId(BitArray bitArray, ref int index) : base(LlrpParameterType.InventoryParameterSpecId, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            this.m_id = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
        }

        public object Clone()
        {
            return new InventoryParameterSpecId(this.m_id);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.m_id, 0x10, true);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Inventory Spec Id>");
            builder.Append(base.ToString());
            builder.Append(this.Id);
            builder.Append("</Inventory Spec Id>");
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
