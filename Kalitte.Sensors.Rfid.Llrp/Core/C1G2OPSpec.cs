namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Collections;

    [Serializable]
    public abstract class C1G2OPSpec : AirProtocolOPSpec
    {
        protected C1G2OPSpec(LlrpParameterType parameterType) : base(parameterType)
        {
        }

        protected internal C1G2OPSpec(LlrpParameterType parameterType, BitArray bitArray, int index) : base(parameterType, bitArray, index)
        {
        }
    }
}
