namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Collections;

    public abstract class AirProtocolLlrpCapabilities : LlrpTlvParameterBase
    {
        protected AirProtocolLlrpCapabilities(LlrpParameterType parameterType) : base(parameterType)
        {
        }

        protected internal AirProtocolLlrpCapabilities(LlrpParameterType parameterType, BitArray bitArray, int index) : base(parameterType, bitArray, index)
        {
        }
    }
}
