namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;

    public enum ConnectionAttemptEventType
    {
        Success,
        FailedReaderInitiatedConnectionAlreadyExists,
        FailedClientInitiatedConnectionAlreadyExists,
        FailedReasonOtherThanConnectionAlreadyExists,
        AnotherConnectionAttempted
    }
}
