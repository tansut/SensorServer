namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    
    
    using System;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;

    internal sealed class StartInventoryCommandHandler : CommandHandler
    {
        internal StartInventoryCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
        }

        private void EnableEventsAndReportsMessageIfSupported()
        {
            if (base.DoesDeviceSupportHoldingEventsAndReportOnReconnect())
            {
                try
                {
                    base.Logger.Info("Sending Enable and events report message to the device {0}", new object[] { base.Device.DeviceName });
                    base.Device.Send(new EnableEventsAndReportsMessage());
                }
                catch (Exception exception)
                {
                    base.Logger.Error("Error during sending enable events and reports message to the device {0} : {1}", new object[] { base.Device.DeviceName, exception });
                }
            }
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            base.Logger.Info("Executing start inventory command on device {0}", new object[] { base.Device.DeviceName });
            ROSpec roSpec = null;
            AccessSpec accessSpec = null;
            lock (base.DeviceState)
            {
                roSpec = base.DeviceState.ROSpec;
                accessSpec = base.DeviceState.InventoryAccessSpec;
            }
            bool flag = this.IsMatchingAccessSpecOnDevice(accessSpec);
            bool flag2 = this.IsMatchingROSpecOnDevice(roSpec);
            base.Logger.Info("Adding and Enabling the notification spec {0}", new object[] { roSpec == null ? 0: roSpec.Id });
            CommandError cmdError = null;
            if ((flag2 || base.AddAndEnableROSpec(roSpec, out cmdError)) && (((accessSpec != null) && !flag) && (!base.AddAndEnableAccessSpec(accessSpec, out cmdError) && !flag2)))
            {
                base.Logger.Info("Deleting the RO Spec as addition/enabling access spec failed on device {0}", new object[] { base.Device.DeviceName });
                base.DeleteROSpec(roSpec);
            }
            lock (base.DeviceState)
            {
                bool flag3 = false;
                if (cmdError == null)
                {
                    flag3 = true;
                }
                base.DeviceState.IsInventoryOn = flag3;
            }
            if (cmdError == null)
            {
                this.EnableEventsAndReportsMessageIfSupported();
                return new ResponseEventArgs(base.Command);
            }
            return new ResponseEventArgs(base.Command, cmdError);
        }

        

        private bool IsMatchingAccessSpecOnDevice(AccessSpec accessSpec)
        {
            if (accessSpec == null)
            {
                return false;
            }
            try
            {
                base.Logger.Info("Getting All Access Spec on the device {0}", new object[] { base.Device.DeviceName });
                LlrpMessageBase message = base.Device.Request(new GetAccessSpecMessage());
                Util.ThrowIfFailed(message);
                GetAccessSpecResponse response = message as GetAccessSpecResponse;
                Util.ThrowIfNull(response, LlrpMessageType.GetAccessSpec);
                if ((response.AccessSpecs == null) || (response.AccessSpecs.Count == 0))
                {
                    base.Logger.Info("No pre-existing AccessSpecs on the device {0}.", new object[] { base.Device.DeviceName });
                    return false;
                }
                bool flag = false;
                foreach (AccessSpec spec in response.AccessSpecs)
                {
                    if (spec.Id.Equals(accessSpec.Id))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    base.Logger.Info("Device {0} does have the Access Spec corresponding to the ID which we want to add. Thus not doing anything", new object[] { base.Device.DeviceName });
                }
                return flag;
            }
            catch (Exception exception)
            {
                base.Logger.Error("Error during getting access spec on device {0} : {1}", new object[] { base.Device.DeviceName, exception });
                return false;
            }
        }

        private bool IsMatchingROSpecOnDevice(ROSpec roSpec)
        {
            try
            {
                base.Logger.Info("Getting All RO Spec on the device {0}", new object[] { base.Device.DeviceName });
                LlrpMessageBase message = base.Device.Request(new GetROSpecMessage());
                Util.ThrowIfFailed(message);
                GetROSpecResponse response = message as GetROSpecResponse;
                Util.ThrowIfNull(response, LlrpMessageType.GetROSpecs);
                if ((response.Specs == null) || (response.Specs.Count == 0))
                {
                    base.Logger.Info("No pre-existing ROSpecs on the device {0}.", new object[] { base.Device.DeviceName });
                    return false;
                }
                bool flag = false;
                foreach (ROSpec spec in response.Specs)
                {
                    if (spec.Id.Equals(roSpec.Id))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    base.Logger.Info("Device {0} does have the RO Spec corresponding to the ID which we want to add. Thus not adding again", new object[] { base.Device.DeviceName });
                }
                return flag;
            }
            catch (Exception exception)
            {
                base.Logger.Error("Error during getting ro spec on device {0} : {1}", new object[] { base.Device.DeviceName, exception });
                return false;
            }
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
