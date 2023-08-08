namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;

    public enum EventNotificationStateEventType
    {
        HoppingToNextChannel,
        Gpi,
        ROSpec,
        ReportBufferFillWarning,
        ReaderExceptionEvent,
        RFSurveyEvent,
        AISpecEvent,
        AISpecWithSingulation,
        AntennaEvent
    }
}
