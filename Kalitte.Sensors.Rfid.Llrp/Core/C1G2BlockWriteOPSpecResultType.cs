namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;

    public enum C1G2BlockWriteOPSpecResultType
    {
        Success,
        TagMemoryOverrunError,
        TagMemoryLockedError,
        InsufficientPower,
        NonspecificTagError,
        NoResponseFromTag,
        NonspecificReaderError
    }
}
