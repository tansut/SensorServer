namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Collections;

    public abstract class AirProtocolSingulationDetails : LlrpTVParameterBase
    {
        internal AirProtocolSingulationDetails(BitArray bitArray, int index, LlrpParameterType parameterType) : base(parameterType, bitArray, index)
        {
        }
    }
}
