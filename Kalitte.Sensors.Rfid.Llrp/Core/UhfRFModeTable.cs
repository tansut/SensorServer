namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Collections;

    [Serializable]
    public abstract class UhfRFModeTable : LlrpTlvParameterBase
    {
        protected UhfRFModeTable(LlrpParameterType parameterType) : base(parameterType)
        {
        }

        protected internal UhfRFModeTable(LlrpParameterType parameterType, BitArray bitArray, int index) : base(parameterType, bitArray, index)
        {
        }
    }
}
