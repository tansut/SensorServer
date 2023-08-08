namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Collections;

    [Serializable]
    public abstract class AirProtocolSpecificEpcMemorySelectorParameter : LlrpTlvParameterBase
    {
        protected AirProtocolSpecificEpcMemorySelectorParameter(LlrpParameterType parameterType) : base(parameterType)
        {
        }

        protected internal AirProtocolSpecificEpcMemorySelectorParameter(LlrpParameterType parameterType, BitArray bitArray, int index) : base(parameterType, bitArray, index)
        {
        }
    }
}
