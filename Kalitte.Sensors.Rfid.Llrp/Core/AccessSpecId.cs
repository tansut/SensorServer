namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public sealed class AccessSpecId : LlrpTVParameterBase, ICloneable
    {
        private uint m_accessSpecId;

        public AccessSpecId(uint id) : base(LlrpParameterType.AccessSpecId)
        {
            this.Init(id);
        }

        internal AccessSpecId(BitArray bitArray, ref int index) : base(LlrpParameterType.AccessSpecId, bitArray, index)
        {
            uint parameterEndLimit = BitHelper.GetParameterEndLimit(bitArray, ref index);
            uint specId = (uint) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x20);
            BitHelper.ValidateEndOfParameterOrMessage(index, parameterEndLimit, base.GetType().FullName);
            this.Init(specId);
        }

        public object Clone()
        {
            return new AccessSpecId(this.m_accessSpecId);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.m_accessSpecId, 0x20, true);
        }

        private void Init(uint specId)
        {
            this.m_accessSpecId = specId;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Access Spec Id>");
            builder.Append(base.ToString());
            builder.Append(this.Id);
            builder.Append("</Access Spec Id>");
            return builder.ToString();
        }

        public uint Id
        {
            get
            {
                return this.m_accessSpecId;
            }
        }
    }
}
