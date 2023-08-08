namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Collections;

    public abstract class AirProtocolTagData : LlrpTVParameterBase
    {
        protected AirProtocolTagData(LlrpParameterType type) : base(type)
        {
        }

        protected AirProtocolTagData(LlrpParameterType type, BitArray bitArray, int index) : base(type, bitArray, index)
        {
        }
    }
}
