namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Collections;
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Events.Management;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Events;

    public abstract class LlrpEvent : LlrpTlvParameterBase
    {
        internal LlrpEvent(LlrpParameterType parameterType) : base(parameterType)
        {
        }

        internal LlrpEvent(BitArray bitArray, int index, LlrpParameterType parameterType) : base(parameterType, bitArray, index)
        {
        }

        internal abstract Notification ConvertToRfidNotification();
    }
}
