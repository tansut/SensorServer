namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Collections;

    [Serializable]
    public abstract class AirProtocolOPSpec : OPSpec
    {
        protected AirProtocolOPSpec(LlrpParameterType parameterType) : base(parameterType)
        {
        }

        protected internal AirProtocolOPSpec(LlrpParameterType parameterType, BitArray bitArray, int index) : base(parameterType, bitArray, index)
        {
        }
    }
}
