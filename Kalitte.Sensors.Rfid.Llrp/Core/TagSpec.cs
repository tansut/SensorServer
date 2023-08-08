namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Collections;

    [Serializable]
    public abstract class TagSpec : LlrpTlvParameterBase
    {
        protected TagSpec(LlrpParameterType parameterType) : base(parameterType)
        {
        }

        protected internal TagSpec(LlrpParameterType parameterType, BitArray bitArray, int index) : base(parameterType, bitArray, index)
        {
        }
    }
}
