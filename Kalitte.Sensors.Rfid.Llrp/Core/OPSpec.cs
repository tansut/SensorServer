namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    [Serializable]
    public abstract class OPSpec : LlrpTlvParameterBase
    {
        protected static readonly int BaseLength = 0x10;
        private ushort m_opSpecId;

        protected OPSpec(LlrpParameterType parameterType) : base(parameterType)
        {
            this.Init(IdGenerator.GenerateOPSpecId());
        }

        protected internal OPSpec(LlrpParameterType parameterType, BitArray bitArray, int index) : base(parameterType, bitArray, index)
        {
            ushort opSpecId = 0;
            index += LlrpTlvParameterBase.HeaderLength;
            opSpecId = (ushort) BitHelper.ConvertBitArrayToNumber(bitArray, ref index, 0x10);
            this.Init(opSpecId);
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            base.Encode(stream);
            stream.Append((long) this.Id, 0x10, true);
        }

        private void Init(ushort opSpecId)
        {
            if (opSpecId == 0)
            {
                throw new ArgumentOutOfRangeException("opSpecId");
            }
            this.m_opSpecId = opSpecId;
        }

        public ushort Id
        {
            get
            {
                return this.m_opSpecId;
            }
        }

        internal override uint ParameterLength
        {
            set
            {
                base.ParameterLength = value + ((uint) BaseLength);
            }
        }
    }
}
