namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid.Llrp.Core;
    
    
    using System;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;

    internal sealed class ShutdownCommandHandler : CommandHandler
    {
        internal ShutdownCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            ShutdownCommand command = base.Command as ShutdownCommand;
            base.Logger.Info("Executing shut down clean up command on device {0}", new object[] { base.Device.DeviceName });
            CommandError cmdError = null;
            bool flag = false;
            ROSpec roSpec = null;
            AccessSpec accessSpec = null;
            lock (base.DeviceState)
            {
                flag = base.DeviceState.IsInventoryOn;
                if (flag)
                {
                    roSpec = base.DeviceState.ROSpec;
                    accessSpec = base.DeviceState.InventoryAccessSpec;
                }
            }
            if (flag)
            {
                if (!base.DeleteROSpec(roSpec, out cmdError))
                {
                    base.Logger.Error("Error during deleting the notification spec {0}", new object[] { cmdError });
                }
                if ((accessSpec != null) && !base.DeleteAccessSpec(accessSpec, out cmdError))
                {
                    base.Logger.Error("Error during deleting the notification access spec {0}", new object[] { cmdError });
                }
            }
            lock (base.DeviceState)
            {
                base.DeviceState.Reset();
            }
            if (!command.IsReaderIntiatedConnection)
            {
                try
                {
                    base.Device.Close();
                }
                catch (Exception exception)
                {
                    base.Logger.Error("Closing connection to the device failed {0}.", new object[] { exception });
                }
            }
            if (cmdError == null)
            {
                return new ResponseEventArgs(base.Command);
            }
            return new ResponseEventArgs(command, cmdError);
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
