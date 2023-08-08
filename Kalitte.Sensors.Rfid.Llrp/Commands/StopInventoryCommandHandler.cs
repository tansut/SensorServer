namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid.Llrp.Core;
    
    
    using System;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;

    internal sealed class StopInventoryCommandHandler : CommandHandler
    {
        internal StopInventoryCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            base.Logger.Info("Executing stop inventory command on device {0}", new object[] { base.Device.DeviceName });
            ROSpec roSpec = null;
            AccessSpec accessSpec = null;
            lock (base.DeviceState)
            {
                bool flag1 = base.DeviceState.IsInventoryOn;
                roSpec = base.DeviceState.ROSpec;
                accessSpec = base.DeviceState.InventoryAccessSpec;
            }
            CommandError cmdError = null;
            base.Logger.Info("Disabling/stopping the notification spec {0}", new object[] { roSpec.Id });
            if (base.DeleteROSpec(roSpec, out cmdError))
            {
                if ((accessSpec != null) && !base.DeleteAccessSpec(accessSpec, out cmdError))
                {
                    base.Logger.Info("Roll back, adding ro spec as deletion of access spec failed on device {0}", new object[] { base.Device.DeviceName });
                    base.AddAndEnableROSpec(roSpec);
                }
                if (cmdError == null)
                {
                    lock (base.DeviceState)
                    {
                        base.DeviceState.IsInventoryOn = false;
                    }
                }
            }
            if (cmdError == null)
            {
                return new ResponseEventArgs(base.Command);
            }
            return new ResponseEventArgs(base.Command, cmdError);
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
