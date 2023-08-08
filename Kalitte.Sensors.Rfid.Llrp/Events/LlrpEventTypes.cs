namespace Kalitte.Sensors.Rfid.Llrp.Events
{
    using System;
    using Kalitte.Sensors.Events.Management;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    public static class LlrpEventTypes
    {
        private static int s_incrementor = 0x40000000;
        public static readonly EventType ConnectionAttemptEvent = new EventType(GetNextEventTypeValue(), LlrpResources.ConnectionAttemptEventTypeDescription);
        public static readonly EventType HoppingEvent = new EventType(GetNextEventTypeValue(), LlrpResources.HoppingEventTypeDescription);
        public static readonly EventType MultipleNicWarningEvent = new EventType(GetNextEventTypeValue(), LlrpResources.MultipleNicWarningEventMessage);
        public static readonly EventType NoNicWarningEvent = new EventType(GetNextEventTypeValue(), LlrpResources.NoNicWarningEventMessage);
        public static readonly EventType ReaderExceptionEvent = new EventType(GetNextEventTypeValue(), LlrpResources.ReaderExceptionEventDescription);
        public static readonly EventType RFSurveyEvent = new EventType(GetNextEventTypeValue(), LlrpResources.RFSurveyNotificationEventDescription);
        public static readonly EventType RFSurveyReportEvent = new EventType(GetNextEventTypeValue(), LlrpResources.RFSurveyReportEventDescription);
        

        private static int GetNextEventTypeValue()
        {
            return s_incrementor++;
        }
    }
}
