namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;

    public enum ReaderConfigurationRequestedData
    {
        All,
        Identification,
        AntennaProperties,
        AntennaConfiguration,
        ROReportSpec,
        ReaderEventNotificationSpec,
        AccessReportSpec,
        LlrpConfigurationStateValue,
        KeepAliveSpec,
        GpiPortCurrentState,
        GpoWriteData,
        EventsAndReports
    }
}
