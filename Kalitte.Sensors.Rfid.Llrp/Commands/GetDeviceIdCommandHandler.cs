namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    
    
    
    using System;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Exceptions;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;

    internal sealed class GetDeviceIdCommandHandler : CommandHandler
    {
        internal GetDeviceIdCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            base.Logger.Info("Getting the device id for device {0}", new object[] { base.Device.DeviceName });
            CommandError error = null;
            GetReaderConfigurationMessage message = new GetReaderConfigurationMessage(ReaderConfigurationRequestedData.Identification, 0, 0, 0, null);
            GetReaderConfigurationResponse response = null;
            try
            {
                response = base.Device.Request(message) as GetReaderConfigurationResponse;
                Util.ThrowIfNull(response, message.MessageType);
                Util.ThrowIfFailed(response.Status);
                if (response.Identification == null)
                {
                    error = new CommandError(LlrpErrorCode.DeviceIdNotAvailable, LlrpResources.ErrorDeviceIdNotAvailable, LlrpErrorCode.DeviceIdNotAvailable.Description, null);
                }
            }
            catch (TimeoutException exception)
            {
                base.Logger.Error("Timed out getting the device id");
                error = new CommandError(LlrpErrorCode.DeviceIdNotAvailable, exception.Message, LlrpErrorCode.DeviceIdNotAvailable.Description, null);
            }
            catch (SensorProviderException exception2)
            {
                base.Logger.Error("Error getting the device id {0}", new object[] { exception2 });
                error = new CommandError(LlrpErrorCode.DeviceIdNotAvailable, exception2, exception2.Message, LlrpErrorCode.DeviceIdNotAvailable.Description, null);
            }
            if (error == null)
            {
                string deviceId = Util.GetDeviceId(response.Identification);
                GetDeviceIdCommand command = base.Command as GetDeviceIdCommand;
                command.Response = new GetDeviceIdResponse(deviceId);
                return new ResponseEventArgs(base.Command);
            }
            return new ResponseEventArgs(base.Command, error);
        }

        internal override bool IsConcurrentToInventoryOperation
        {
            get
            {
                return true;
            }
        }
    }
}
