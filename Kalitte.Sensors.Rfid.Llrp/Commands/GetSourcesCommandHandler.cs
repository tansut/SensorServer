namespace Kalitte.Sensors.Rfid.Llrp.Commands
{

    using System;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;

    internal sealed class GetSourcesCommandHandler : CommandHandler
    {
        internal GetSourcesCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            base.Logger.Info("Executing Get Sources command on device {0}", new object[] { base.Device.DeviceName });
            GetSourcesCommand command = (GetSourcesCommand) base.Command;
            CommandError cmdError = null;
            lock (base.DeviceState)
            {
                if (base.DeviceState.Sources != null)
                {
                    command.Response = new GetSourcesResponse(base.DeviceState.Sources);
                    base.Logger.Info("Sources is already available for device {0}, so returning", new object[] { base.Device.DeviceName });
                    return new ResponseEventArgs(base.Command);
                }
            }
            if (base.TryPopulateReaderCapabilities(out cmdError))
            {
                lock (base.DeviceState)
                {
                    command.Response = new GetSourcesResponse(base.DeviceState.Sources);
                }
            }
            if (cmdError != null)
            {
                return new ResponseEventArgs(base.Command, cmdError);
            }
            return new ResponseEventArgs(base.Command);
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
