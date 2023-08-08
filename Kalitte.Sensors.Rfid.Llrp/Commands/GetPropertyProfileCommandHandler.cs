namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    
    
    
    using System;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Configuration;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Configuration;
    using Kalitte.Sensors.Rfid.Llrp.Configuration;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;

    internal sealed class GetPropertyProfileCommandHandler : CommandHandler
    {
        private PropertyKey m_propertyKeyOfInterest;

        internal GetPropertyProfileCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            base.Logger.Info("Executing get property profile command on device {0}", new object[] { base.Device.DeviceName });
            CommandError cmdError = null;
            this.HandleFirstEverConnectionToDevice();
            bool flag = false;
            lock (base.DeviceState)
            {
                if (base.DeviceState.LlrpSpecificCapabilities != null)
                {
                    flag = true;
                }
            }
            if (!flag && !base.TryPopulateReaderCapabilities(out cmdError))
            {
                return new ResponseEventArgs(base.Command, cmdError);
            }
            PropertyList destination = new PropertyList(LlrpResources.PropertyProfileName);
            GetActivePropertyListCommand command = base.Command as GetActivePropertyListCommand;
            lock (base.DeviceState)
            {
                if (base.SourceName == null)
                {
                    Util.AppendPropertyProfile(base.DeviceState.ProviderMaintainedProperties, ref destination);
                    Util.AppendPropertyProfile(base.DeviceState.LlrpSpecificCapabilities, ref destination);
                }
                else
                {
                    Util.AppendPropertyProfile(base.DeviceState.Sources[base.SourceName], ref destination);
                }
            }
            if (!base.GetReaderConfigurationAndCreateProfile(out cmdError, ref destination))
            {
                return new ResponseEventArgs(base.Command, cmdError);
            }
            command.Response = new GetActivePropertyListResponse(destination);
            return new ResponseEventArgs(command);
        }

        private void HandleFirstEverConnectionToDevice()
        {
            if (base.SourceName == null)
            {
                bool flag = false;
                bool flag2 = false;
                lock (base.DeviceState)
                {
                    flag = (bool)base.DeviceState.ProviderMaintainedProperties[NotificationGroup.EventModeKey];
                    flag2 = base.DeviceState.IsInventoryOn;
                }
                if (flag && !flag2)
                {
                    base.Logger.Info("Indicates the case of first connection on device {0}", new object[] { base.Device.DeviceName });
                    if (((this.PropertyKeyOfInterest == null) || this.PropertyKeyOfInterest.Equals(LlrpCapabilitiesGroup.SupportsEventAndReportHoldingKey)) && base.DoesDeviceSupportHoldingEventsAndReportOnReconnect())
                    {
                        try
                        {
                            LlrpMessageBase message = base.Device.Request(new SetReaderConfigurationMessage(false, null, null, null, null, null, null, null, null, new EventsAndReport(true), null));
                            Util.ThrowIfFailed(message);
                            SetReaderConfigurationResponse response = message as SetReaderConfigurationResponse;
                            Util.ThrowIfNull(response, LlrpMessageType.SetReaderConfig);
                        }
                        catch (Exception exception)
                        {
                            base.Logger.Error("Error during setting the hold on event and report to default value on the device {0} : {1}", new object[] { base.Device.DeviceName, exception });
                        }
                    }
                    if ((this.PropertyKeyOfInterest == null) || this.PropertyKeyOfInterest.Equals(NotificationGroup.EventModeKey))
                    {
                        ResponseEventArgs args = new StartInventoryCommandHandler(base.SourceName, new StartInventoryCommand(), base.DeviceState, base.Device, base.Logger).ExecuteCommand();
                        if (args.CommandError != null)
                        {
                            base.Logger.Error("Error {0} setting up inventory on the device {1}", new object[] { args.CommandError, base.Device.DeviceName });
                        }
                    }
                }
            }
        }

        internal override bool IsConcurrentToInventoryOperation
        {
            get
            {
                return true;
            }
        }

        internal PropertyKey PropertyKeyOfInterest
        {
            get
            {
                return this.m_propertyKeyOfInterest;
            }
            set
            {
                this.m_propertyKeyOfInterest = value;
            }
        }
    }
}
