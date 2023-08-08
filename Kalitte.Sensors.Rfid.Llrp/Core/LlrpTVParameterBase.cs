namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using Kalitte.Sensors.Rfid.Llrp;
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    public abstract class LlrpTVParameterBase : LlrpParameterBase
    {
        public static readonly ushort HeaderLength = 8;

        protected LlrpTVParameterBase(LlrpParameterType parameterType) : base(parameterType)
        {
            this.SetLength();
        }

        internal LlrpTVParameterBase(LlrpParameterType parameterType, BitArray bitArray, int index) : base(parameterType, bitArray, index)
        {
            this.SetLength();
        }

        internal override void Encode(LLRPMessageStream stream)
        {
            stream.Append((long) 1L, 1, true);
            stream.Append((long) ((ushort) base.ParameterType), 7, true);
        }

        private void SetLength()
        {
            this.ParameterLength = (uint) (BitHelper.GetTVParameterLength(base.ParameterType) * 8);
        }

        internal sealed override uint ParameterLength
        {
            get
            {
                return base.ParameterLength;
            }
            set
            {
                base.ParameterLength = value;
            }
        }
    }
}
