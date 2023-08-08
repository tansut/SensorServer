namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Collections;

    public abstract class C1G2OPSpecResult : AirProtocolSpecificOPSpecResult
    {
        internal C1G2OPSpecResult(LlrpParameterType parameterType) : base(parameterType)
        {
        }

        internal C1G2OPSpecResult(LlrpParameterType parameterType, BitArray bitArray, int index) : base(parameterType, bitArray, index)
        {
        }
    }
}
