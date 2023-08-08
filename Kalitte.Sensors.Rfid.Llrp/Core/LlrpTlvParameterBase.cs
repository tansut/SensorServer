namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;

    [Serializable]
    public abstract class LlrpTlvParameterBase : LlrpParameterBase
    {
        public static readonly ushort HeaderLength = 0x20;

        protected LlrpTlvParameterBase(LlrpParameterType parameterType) : base(parameterType)
        {
        }

        internal LlrpTlvParameterBase(LlrpParameterType parameterType, BitArray bitArray, int index) : base(parameterType, bitArray, index)
        {
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            stream.Append((long) 0L, 1, true);
            stream.Append((ulong) 0L, 5, true);
            stream.Append((long) ((ushort) base.ParameterType), 10, true);
            stream.Append((long) (this.ParameterLength / 8), 0x10, true);
        }

        internal override uint ParameterLength
        {
            set
            {
                base.ParameterLength = HeaderLength + value;
            }
        }
    }
}
