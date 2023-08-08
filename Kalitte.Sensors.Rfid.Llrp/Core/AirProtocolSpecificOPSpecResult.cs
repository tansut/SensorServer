namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Collections;

    public abstract class AirProtocolSpecificOPSpecResult : LlrpTlvParameterBase
    {
        internal AirProtocolSpecificOPSpecResult(LlrpParameterType parameterType) : base(parameterType)
        {
        }

        internal AirProtocolSpecificOPSpecResult(LlrpParameterType parameterType, BitArray bitArray, int index) : base(parameterType, bitArray, index)
        {
        }
    }
}
