namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Core;

    public static class LlrpErrorCode
    {
        private static int s_start = 0x40000000;
        public static readonly ErrorCode AddingAccessSpecFailed = new ErrorCode(GetNextErrorCodeValue(), LlrpResources.AddingAccessSpecFailed);
        public static readonly ErrorCode AddROSpecFailed = new ErrorCode(GetNextErrorCodeValue(), LlrpResources.AddROSpecFailedDescription);
        public static readonly ErrorCode CommandExecutionFailed = new ErrorCode(GetNextErrorCodeValue(), LlrpResources.CommandExecutionFailedDescription);
        public static readonly ErrorCode DeleteNotificationSpecError = new ErrorCode(GetNextErrorCodeValue(), LlrpResources.DeleteNotificationSpecFailed);
        public static readonly ErrorCode DeletingAccessSpecFailed = new ErrorCode(GetNextErrorCodeValue(), LlrpResources.DeletingAccessSpecFailed);
        public static readonly ErrorCode DeletingROSpecFailed = new ErrorCode(GetNextErrorCodeValue(), LlrpResources.DeletingROSpecFailed);
        public static readonly ErrorCode DeviceIdNotAvailable = new ErrorCode(GetNextErrorCodeValue(), LlrpResources.ErrorDeviceIdNotAvailable);
        public static readonly ErrorCode EnableROSpecFailed = new ErrorCode(GetNextErrorCodeValue(), LlrpResources.EnableROSpecFailedDescription);
        public static readonly ErrorCode EnablingAccessSpecFailed = new ErrorCode(GetNextErrorCodeValue(), LlrpResources.EnablingAccessSpecFailed);
        public static readonly ErrorCode GettingReaderCapabilitiesFailed = new ErrorCode(GetNextErrorCodeValue(), LlrpResources.GettingReaderCapabilitiesFailed);
        public static readonly ErrorCode GettingReaderConfigurationFailed = new ErrorCode(GetNextErrorCodeValue(), LlrpResources.GettingReaderConfigurationFailed);
        public static readonly ErrorCode InvalidAntennaName = new ErrorCode(GetNextErrorCodeValue(), LlrpResources.InvalidAntennaName);
        

        private static int GetNextErrorCodeValue()
        {
            return s_start++;
        }
    }
}
