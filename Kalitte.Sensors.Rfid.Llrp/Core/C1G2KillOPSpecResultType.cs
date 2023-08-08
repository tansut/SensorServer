namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;

    public enum C1G2KillOPSpecResultType
    {
        Success,
        ZeroKillPasswordError,
        InsufficientPower,
        NonspecificTagError,
        NoResponseFromTag,
        NonspecificReaderError
    }
}
