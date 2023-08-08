namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Collections;

    [Serializable]
    public abstract class AirProtocolInventoryCommandSettings : LlrpTlvParameterBase
    {
        protected AirProtocolInventoryCommandSettings(LlrpParameterType parameterType) : base(parameterType)
        {
        }

        protected internal AirProtocolInventoryCommandSettings(LlrpParameterType parameterType, BitArray bitArray, int index) : base(parameterType, bitArray, index)
        {
        }
    }
}
