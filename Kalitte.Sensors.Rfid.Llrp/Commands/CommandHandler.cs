namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using Kalitte.Sensors.Rfid.Commands;
    using System.Runtime.InteropServices;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Configuration;
    using Kalitte.Sensors.Configuration;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Exceptions;
    using Kalitte.Sensors.SensorDevices;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Rfid.Llrp.Configuration;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Exceptions;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Rfid.Core;

    internal abstract class CommandHandler
    {
        private SensorCommand m_command;
        private LlrpDevice m_device;
        private PDPState m_deviceState;
        private ILogger m_logger;
        private string m_sourceName;

        protected CommandHandler(string sourceName, SensorCommand command, PDPState deviceState, LlrpDevice device, ILogger logger)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            this.m_device = device;
            this.m_sourceName = sourceName;
            this.m_command = command;
            this.m_deviceState = deviceState;
            this.m_logger = logger;
            this.ValidateSourceName();
        }

        protected bool AddAndEnableAccessSpec(AccessSpec accessSpec, out CommandError cmdError)
        {
            cmdError = null;
            bool accessSpecAdded = false;
            try
            {
                this.Logger.Info("Adding accessSpec with Id {0} on device {1}", new object[] { accessSpec.Id, this.Device.DeviceName });
                this.Logger.Verbose("Access spec is {0}", new object[] { accessSpec });
                AddAccessSpecResponse message = this.Device.Request(new AddAccessSpecMessage(accessSpec)) as AddAccessSpecResponse;
                Util.ThrowIfNull(message, LlrpMessageType.AddAccessSpec);
                Util.ThrowIfFailed(message.Status);
                accessSpecAdded = true;
                this.Logger.Info("Enabling accessSpec {0} on device {1}", new object[] { accessSpec.Id, this.Device.DeviceName });
                EnableAccessSpecResponse response2 = this.Device.Request(new EnableAccessSpecMessage(accessSpec.Id)) as EnableAccessSpecResponse;
                Util.ThrowIfNull(response2, LlrpMessageType.EnableAccessSpec);
                Util.ThrowIfFailed(response2.Status);
                return true;
            }
            catch (SensorProviderException exception)
            {
                this.HandleAccessSpecAddEnableException(exception, out cmdError, accessSpecAdded, accessSpec);
            }
            catch (DecodingException exception2)
            {
                this.HandleAccessSpecAddEnableException(exception2, out cmdError, accessSpecAdded, accessSpec);
            }
            catch (TimeoutException exception3)
            {
                this.HandleAccessSpecAddEnableException(exception3, out cmdError, accessSpecAdded, accessSpec);
            }
            return false;
        }

        protected bool AddAndEnableROSpec(ROSpec roSpec)
        {
            CommandError cmdError = null;
            return this.AddAndEnableROSpec(roSpec, out cmdError);
        }

        protected bool AddAndEnableROSpec(ROSpec roSpec, out CommandError cmdError)
        {
            cmdError = null;
            bool roSpecAdded = false;
            try
            {
                this.Logger.Info("Adding RO Spec with Id {0} on device {1}", new object[] { roSpec.Id, this.Device.DeviceName });
                this.Logger.Verbose("RO Spec is {0}", new object[] { roSpec });
                AddROSpecResponse message = this.Device.Request(new AddROSpecMessage(roSpec)) as AddROSpecResponse;
                Util.ThrowIfNull(message, LlrpMessageType.AddROSpec);
                Util.ThrowIfFailed(message.Status);
                roSpecAdded = true;
                this.Logger.Info("Enabling RO Spec with Id {0} on device {1}", new object[] { roSpec.Id, this.Device.DeviceName });
                EnableROSpecResponse response2 = this.Device.Request(new EnableROSpecMessage(roSpec.Id)) as EnableROSpecResponse;
                Util.ThrowIfNull(response2, LlrpMessageType.EnableROSpec);
                Util.ThrowIfFailed(response2.Status);
                if (((roSpec.BoundarySpec != null) && (roSpec.BoundarySpec.StartTrigger != null)) && (roSpec.BoundarySpec.StartTrigger.TriggerType == ROSpecStartTriggerType.Null))
                {
                    this.Logger.Info("Starting the RO Spec as the trigger is null on device {0}", new object[] { this.Device.DeviceName });
                    StartROSpecResponse response3 = this.Device.Request(new StartROSpecMessage(roSpec.Id)) as StartROSpecResponse;
                    Util.ThrowIfNull(response3, LlrpMessageType.StartROSpec);
                    Util.ThrowIfFailed(response3.Status);
                }
                return true;
            }
            catch (SensorProviderException exception)
            {
                this.HandleROSpecAddEnableException(exception, out cmdError, roSpecAdded, roSpec);
            }
            catch (DecodingException exception2)
            {
                this.HandleROSpecAddEnableException(exception2, out cmdError, roSpecAdded, roSpec);
            }
            catch (TimeoutException exception3)
            {
                this.HandleROSpecAddEnableException(exception3, out cmdError, roSpecAdded, roSpec);
            }
            return false;
        }

        protected void AssertInventoryIsOff()
        {
        }

        protected bool CleanupSpecs(out CommandError cmdError)
        {
            cmdError = null;
            bool roSpecErr = true;
            try
            {
                DeleteROSpecResponse message = this.Device.Request(new DeleteROSpecMessage(0)) as DeleteROSpecResponse;
                Util.ThrowIfNull(message, LlrpMessageType.DeleteROSpec);
                Util.ThrowIfFailed(message.Status);
                roSpecErr = false;
                DeleteAccessSpecResponse response2 = this.Device.Request(new DeleteAccessSpecMessage(0)) as DeleteAccessSpecResponse;
                Util.ThrowIfNull(response2, LlrpMessageType.DeleteAccessSpec);
                Util.ThrowIfFailed(response2.Status);
                return true;
            }
            catch (SensorProviderException exception)
            {
                this.HandleCleanupSpecsException(exception, out cmdError, roSpecErr);
            }
            catch (DecodingException exception2)
            {
                this.HandleCleanupSpecsException(exception2, out cmdError, roSpecErr);
            }
            catch (TimeoutException exception3)
            {
                this.HandleCleanupSpecsException(exception3, out cmdError, roSpecErr);
            }
            return false;
        }

        private PropertyList CreateSourceProfile(ushort id, string sourceName, RfidSourceType type)
        {
            PropertyList profile = new PropertyList(LlrpResources.PropertyProfileName);
            profile.Add(SourceGroup.LlrpSourceIdKey, id);
            profile.Add(GeneralGroup.NameKey, sourceName);
            profile.Add(SourceGroup.SourceTypeKey, type);
            return profile;
        }

        protected bool DeleteAccessSpec(AccessSpec accessSpec)
        {
            try
            {
                CommandError cmdError = null;
                if (!this.DeleteAccessSpec(accessSpec, out cmdError))
                {
                    this.Logger.Error("Deleting access Spec failed {0}", new object[] { cmdError });
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.Logger.Error("Deleting access spec failed {0}", new object[] { exception.ToString() });
            }
            return false;
        }

        protected bool DeleteAccessSpec(AccessSpec accessSpec, out CommandError cmdError)
        {
            cmdError = null;
            this.Logger.Info("Deleting the accessSpec {0} on device {1}", new object[] { accessSpec.Id, this.Device.DeviceName });
            this.Logger.Verbose("Access spec is {0}", new object[] { accessSpec });
            try
            {
                DeleteAccessSpecResponse message = this.Device.Request(new DeleteAccessSpecMessage(accessSpec.Id)) as DeleteAccessSpecResponse;
                Util.ThrowIfNull(message, LlrpMessageType.DeleteAccessSpec);
                //tansu: patch. accessspec uçabilir.
                if (message.Status.StatusString != null && message.Status.StatusString.ToLowerInvariant().Contains("not found"))
                    return true;
                else Util.ThrowIfFailed(message.Status);
                return true;
            }
            catch (SensorProviderException exception)
            {
                cmdError = new CommandError(LlrpErrorCode.DeletingAccessSpecFailed, exception, null, LlrpErrorCode.DeletingAccessSpecFailed.Description, null);
            }
            catch (DecodingException exception2)
            {
                cmdError = new CommandError(LlrpErrorCode.DeletingAccessSpecFailed, exception2.Message, LlrpErrorCode.DeletingAccessSpecFailed.Description, null);
            }
            catch (TimeoutException exception3)
            {
                cmdError = new CommandError(LlrpErrorCode.DeletingAccessSpecFailed, exception3.Message, LlrpErrorCode.DeletingAccessSpecFailed.Description, null);
            }
            return false;
        }

        protected bool DeleteROSpec(ROSpec roSpec)
        {
            try
            {
                CommandError cmdError = null;
                if (!this.DeleteROSpec(roSpec, out cmdError))
                {
                    this.Logger.Error("Deleting RO Spec failed {0}", new object[] { cmdError });
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.Logger.Error("Deleting ro spec failed {0}", new object[] { exception.ToString() });
            }
            return false;
        }

        protected bool DeleteROSpec(ROSpec roSpec, out CommandError cmdError)
        {
            cmdError = null;
            this.Logger.Info("Deleting the RO Spec with Id {0} on device {1}", new object[] { roSpec.Id, this.Device.DeviceName });
            this.Logger.Verbose("RO Spec being {0}", new object[] { roSpec });
            try
            {
                DeleteROSpecResponse message = this.Device.Request(new DeleteROSpecMessage(roSpec.Id)) as DeleteROSpecResponse;
                Util.ThrowIfNull(message, LlrpMessageType.DeleteROSpec);
                Util.ThrowIfFailed(message.Status);
                return true;
            }
            catch (SensorProviderException exception)
            {
                cmdError = new CommandError(LlrpErrorCode.DeleteNotificationSpecError, exception, null, LlrpErrorCode.DeleteNotificationSpecError.Description, null);
            }
            catch (DecodingException exception2)
            {
                cmdError = new CommandError(LlrpErrorCode.DeleteNotificationSpecError, exception2.Message, LlrpErrorCode.DeleteNotificationSpecError.Description, null);
            }
            catch (TimeoutException exception3)
            {
                cmdError = new CommandError(LlrpErrorCode.DeleteNotificationSpecError, exception3.Message, LlrpErrorCode.DeleteNotificationSpecError.Description, null);
            }
            return false;
        }

        protected bool DoesDeviceSupportHoldingEventsAndReportOnReconnect()
        {
            bool flag = false;
            lock (this.DeviceState)
            {
                if ((this.DeviceState.LlrpSpecificCapabilities != null) && this.DeviceState.LlrpSpecificCapabilities.ContainsKey(LlrpCapabilitiesGroup.SupportsEventAndReportHoldingKey))
                {
                    flag = (bool)this.DeviceState.LlrpSpecificCapabilities[LlrpCapabilitiesGroup.SupportsEventAndReportHoldingKey];
                }
                this.Logger.Info("Device {0} {1} support holding on events and reports on reconnect", new object[] { this.Device.DeviceName, flag ? "does" : "does not" });
            }
            return flag;
        }

        internal abstract ResponseEventArgs ExecuteCommand();
        protected ushort GetAntennaId(string sourceName)
        {
            ushort sourceId = 0;
            if (sourceName != null)
            {
                sourceId = Util.GetSourceId(sourceName, LLRPSourceType.Antenna);
            }
            return sourceId;
        }

        protected uint GetCode(byte[] code)
        {
            try
            {
                this.Validatecode(code);
            }
            catch (SensorProviderException)
            {
            }
            if (code == null)
            {
                return 0;
            }
            return (uint)BitHelper.GetNumber(code, 0, 4);
        }

        protected Dictionary<PropertyKey, DevicePropertyMetadata> GetCustomMetadata()
        {
            Dictionary<PropertyKey, DevicePropertyMetadata> microsoftSpecificMetadata = null;
            try
            {
                ResponseEventArgs args = new GetMetadataCommandHandler(this.SourceName, new GetMetadataCommand(null), this.DeviceState, this.Device, this.Logger).ExecuteCommand();
                if (args.CommandError == null)
                {
                    lock (this.DeviceState)
                    {
                        microsoftSpecificMetadata = null;// this.DeviceState.MicrosoftSpecificMetadata;
                        goto Label_00BF;
                    }
                }
                this.Logger.Error("Error {0} getting the metadata from the device {1}. Assuming that metadata is not available for the device", new object[] { args.CommandError, this.Device.DeviceName });
            }
            catch (Exception exception)
            {
                this.Logger.Error("Error {0} getting the metadata from the device {1}. Assuming that metadata is not available for the device", new object[] { exception, this.Device.DeviceName });
            }
        Label_00BF:
            if (microsoftSpecificMetadata == null)
            {
                microsoftSpecificMetadata = new Dictionary<PropertyKey, DevicePropertyMetadata>();
            }
            return microsoftSpecificMetadata;
        }

        internal static CommandHandler GetInstance(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (command is GetActivePropertyListCommand)
            {
                return new GetPropertyProfileCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is ApplyPropertyListCommand)
            {
                return new ApplyPropertyProfileCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is GetDeviceIdCommand)
            {
                return new GetDeviceIdCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is ShutdownCommand)
            {
                return new ShutdownCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is StopInventoryCommand)
            {
                return new StopInventoryCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is StartInventoryCommand)
            {
                return new StartInventoryCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is QueryTagsCommand)
            {
                return new QueryTagsCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is GetSourcesCommand)
            {
                return new GetSourcesCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is GetMetadataCommand)
            {
                return new GetMetadataCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is WriteIdCommand)
            {
                return new WriteIdCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is LockTagCommand)
            {
                return new LockTagCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is UnlockTagCommand)
            {
                return new UnlockTagCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is KillCommand)
            {
                return new KillTagCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is ReadTagDataCommand)
            {
                return new ReadTagDataCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is WriteTagDataCommand)
            {
                return new WriteTagDataCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is GetPropertyCommand)
            {
                return new GetPropertyCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is SetPropertyCommand)
            {
                return new SetPropertyCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is ReadTagCommand)
            {
                return new ReadTagCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is WriteTagCommand)
            {
                return new WriteTagCommandHandler(sourcName, command, state, device, logger);
            }
            if (command is VendorCommand)
            {
                return new LlrpMessageVendorDefinedCommandHandler(sourcName, command, state, device, logger);
            }
            //if (command is LlrpGenericCommand)
            //{
            //    return new LlrpGenericCommandHandler(sourcName, command, state, device, logger);
            //}

            if (!(command is ResetConfigurationToFactorySettingsCommand))
            {
                throw new SensorProviderException(LlrpResources.UnsupportedCommand);
            }
            return new ResetConfigurationToFactorySettingsCommandHandler(sourcName, command, state, device, logger);
        }

        private static byte[] GetPortInputValue(GpiState state)
        {
            switch (state)
            {
                case GpiState.Low:
                    return new byte[1];

                case GpiState.High:
                    return new byte[] { 1 };

                case GpiState.Unknown:
                    return new byte[] { 2 };
            }
            return new byte[1];
        }

        protected bool GetReaderConfigurationAndCreateProfile(out CommandError cmdError, ref PropertyList profile)
        {
            if (profile == null)
            {
                profile = new PropertyList(LlrpResources.PropertyProfileName);
            }
            cmdError = null;
            this.Logger.Info("Getting the reader configuration of the device {0} for source {1}", new object[] { this.Device.DeviceName, this.SourceName });
            try
            {
                GetReaderConfigurationResponse response;
                ushort antennaId = 0;
                ushort gpiNumber = 0;
                ushort gpoNumber = 0;
                LLRPSourceType antenna = LLRPSourceType.Antenna;
                if (this.SourceName != null)
                {
                    Util.IsValidSourceName(this.SourceName, out antenna);
                    switch (antenna)
                    {
                        case LLRPSourceType.Antenna:
                            antennaId = Util.GetSourceId(this.SourceName, antenna);
                            goto Label_00DF;

                        case LLRPSourceType.GPI:
                            gpiNumber = Util.GetSourceId(this.SourceName, antenna);
                            goto Label_00DF;

                        case LLRPSourceType.GPO:
                            gpoNumber = Util.GetSourceId(this.SourceName, antenna);
                            goto Label_00DF;
                    }
                    this.Logger.Error("Invalid source type {0} in device {1}", new object[] { antenna, this.Device.DeviceName });
                }
            Label_00DF:
                response = this.Device.Request(new GetReaderConfigurationMessage(ReaderConfigurationRequestedData.All, antennaId, gpiNumber, gpoNumber, null)) as GetReaderConfigurationResponse;
                Util.ThrowIfNull(response, LlrpMessageType.GetReaderConfig);
                Util.ThrowIfFailed(response.Status);
                if (this.SourceName == null)
                {
                    this.PopulateDeviceReaderConfiguration(response, ref profile);
                }
                else
                {
                    switch (antenna)
                    {
                        case LLRPSourceType.Antenna:
                            this.PopulateAntennaReaderConfiguration(response, antennaId, ref profile);
                            goto Label_015C;

                        case LLRPSourceType.GPI:
                            this.PopulateGpiReaderConfiguration(response, gpiNumber, ref profile);
                            goto Label_015C;

                        case LLRPSourceType.GPO:
                            this.PopulateGpoReaderConfiguration(response, gpoNumber, ref profile);
                            goto Label_015C;
                    }
                }
            Label_015C:
                return true;
            }
            catch (SensorProviderException exception)
            {
                cmdError = new CommandError(LlrpErrorCode.GettingReaderConfigurationFailed, exception, null, LlrpErrorCode.GettingReaderConfigurationFailed.Description, null);
            }
            catch (DecodingException exception2)
            {
                cmdError = new CommandError(LlrpErrorCode.GettingReaderConfigurationFailed, exception2.Message, LlrpErrorCode.GettingReaderConfigurationFailed.Description, null);
            }
            catch (TimeoutException exception3)
            {
                cmdError = new CommandError(LlrpErrorCode.GettingReaderConfigurationFailed, exception3.Message, LlrpErrorCode.GettingReaderConfigurationFailed.Description, null);
            }
            return false;
        }

        private void GetSources(GetReaderCapabilitiesResponse response)
        {
            GeneralDeviceCapabilities generalDeviceCapabilities = response.GeneralDeviceCapabilities;
            string sourceName = string.Empty;
            if (generalDeviceCapabilities != null)
            {
                for (int i = 1; i <= generalDeviceCapabilities.MaxNumberOfAntennaSupported; i++)
                {
                    sourceName = Util.GetAntennaName((ushort)i);
                    this.DeviceState.Sources[sourceName] = this.CreateSourceProfile((ushort)i, sourceName, RfidSourceType.Antenna);
                }
                if (generalDeviceCapabilities.GpioCapabilities != null)
                {
                    for (int j = 1; j <= generalDeviceCapabilities.GpioCapabilities.NumberOfGpi; j++)
                    {
                        sourceName = Util.GetGpiName((ushort)j);
                        this.DeviceState.Sources[sourceName] = this.CreateSourceProfile((ushort)j, sourceName, RfidSourceType.IOPort);
                    }
                    for (int k = 1; k <= generalDeviceCapabilities.GpioCapabilities.NumberOfGpo; k++)
                    {
                        sourceName = Util.GetGpoName((ushort)k);
                        this.DeviceState.Sources[sourceName] = this.CreateSourceProfile((ushort)k, sourceName, RfidSourceType.IOPort);
                    }
                }
            }
        }

        private void HandleAccessSpecAddEnableException(Exception ex, out CommandError cmdError, bool accessSpecAdded, AccessSpec accessSpec)
        {
            ErrorCode addingAccessSpecFailed = LlrpErrorCode.AddingAccessSpecFailed;
            if (accessSpecAdded)
            {
                addingAccessSpecFailed = LlrpErrorCode.EnablingAccessSpecFailed;
            }
            if (ex is SensorProviderException)
            {
                cmdError = new CommandError(addingAccessSpecFailed, (SensorProviderException)ex, ex.Message, addingAccessSpecFailed.Description, null);
            }
            else
            {
                cmdError = new CommandError(addingAccessSpecFailed, ex.Message, addingAccessSpecFailed.Description, null);
            }
            if (accessSpecAdded)
            {
                this.DeleteAccessSpec(accessSpec);
            }
        }

        private void HandleCleanupSpecsException(Exception ex, out CommandError cmdError, bool roSpecErr)
        {
            ErrorCode deletingAccessSpecFailed = LlrpErrorCode.DeletingAccessSpecFailed;
            if (roSpecErr)
            {
                deletingAccessSpecFailed = LlrpErrorCode.DeletingROSpecFailed;
            }
            if (ex is SensorProviderException)
            {
                cmdError = new CommandError(deletingAccessSpecFailed, (SensorProviderException)ex, ex.Message, deletingAccessSpecFailed.Description, null);
            }
            else
            {
                cmdError = new CommandError(deletingAccessSpecFailed, ex.Message, deletingAccessSpecFailed.Description, null);
            }
        }

        private void HandleROSpecAddEnableException(Exception ex, out CommandError cmdError, bool roSpecAdded, ROSpec roSpec)
        {
            this.m_logger.Error("Add/Enable RO Spec failed for device {0} with error {1}", new object[] { this.Device.DeviceName, ex });
            ErrorCode addROSpecFailed = LlrpErrorCode.AddROSpecFailed;
            if (roSpecAdded)
            {
                addROSpecFailed = LlrpErrorCode.EnableROSpecFailed;
            }
            if (ex is SensorProviderException)
            {
                cmdError = new CommandError(addROSpecFailed, (SensorProviderException)ex, ex.Message, addROSpecFailed.Description, null);
            }
            else
            {
                cmdError = new CommandError(addROSpecFailed, ex.Message, addROSpecFailed.Description, null);
            }
            if (roSpecAdded)
            {
                this.DeleteROSpec(roSpec);
            }
        }

        private bool IsAntennaPropertiesSettable()
        {
            CommandError error;
            bool flag = false;
            lock (this.DeviceState)
            {
                if (this.DeviceState.Metadata == null)
                {
                    flag = true;
                }
            }
            if ((flag && this.TryPopulateReaderCapabilities(out error)) || !flag)
            {
                lock (this.DeviceState)
                {
                    bool flag2;
                    if (((this.DeviceState.Metadata != null) && this.DeviceState.Metadata.ContainsKey(LlrpGeneralCapabilitiesGroup.CanSetAntennaPropertiesKey)) && ((this.DeviceState.Metadata[LlrpGeneralCapabilitiesGroup.CanSetAntennaPropertiesKey] != null) && bool.TryParse(this.DeviceState.LlrpSpecificCapabilities[LlrpGeneralCapabilitiesGroup.CanSetAntennaPropertiesKey].ToString(), out flag2)))
                    {
                        return flag2;
                    }
                }
            }
            return false;
        }

        private void PopulateAntennaReaderConfiguration(GetReaderConfigurationResponse response, ushort antennaId, ref PropertyList profile)
        {
            if (response.AntennaProperties != null)
            {
                foreach (AntennaProperties properties in response.AntennaProperties)
                {
                    if (properties.AntennaId == antennaId)
                    {
                        profile[LlrpAntennaPropertiesGroup.AntennaGainKey] = properties.AntennaGain;
                        if (this.IsAntennaPropertiesSettable())
                        {
                            profile[SourceGroup.EnabledKey] = properties.IsConnected;
                        }
                        else
                        {
                            profile[SourceGroup.SystemEnabledKey] = properties.IsConnected;
                        }
                    }
                }
            }
            if (response.AntennaConfigurations != null)
            {
                foreach (AntennaConfiguration configuration in response.AntennaConfigurations)
                {
                    if (configuration.AntennaId == antennaId)
                    {
                        if (configuration.RFTransmitter != null)
                        {
                            profile[LlrpAntennaConfigurationGroup.HopTableIdKey] = configuration.RFTransmitter.HopTableId;
                            profile[LlrpAntennaConfigurationGroup.TransmitPowerIndexKey] = configuration.RFTransmitter.TransmitPowerIndex;
                            profile[LlrpAntennaConfigurationGroup.ChannelIndexKey] = configuration.RFTransmitter.ChannelIndex;
                        }
                        if (configuration.RFReceiver != null)
                        {
                            profile[LlrpAntennaConfigurationGroup.ReceiverSensitivityIndexKey] = configuration.RFReceiver.IndexIntoReceiverSensitivityTable;
                        }
                        this.PopulateInventoryCommandConfiguration(configuration, ref profile);
                    }
                }
            }
            this.PopulateSourceCustomProperties(response, antennaId, LLRPSourceType.Antenna, ref profile);
        }

        private static void PopulateC1G2InventoryCommandConfiguration(C1G2InventoryCommand c1g2InventoryCommand, ref PropertyList profile)
        {
            if (c1g2InventoryCommand != null)
            {
                profile[LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareKey] = c1g2InventoryCommand.StateAware;
                if (c1g2InventoryCommand.Filters != null)
                {
                    profile[LlrpC1G2InventoryCommandGroup.FiltersKey] = LlrpSerializationHelper.SerializeToXmlDataContract(c1g2InventoryCommand.Filters, false);
                }
                else
                {
                    profile[LlrpC1G2InventoryCommandGroup.FiltersKey] = null;
                }
                if (c1g2InventoryCommand.RFControl != null)
                {
                    C1G2RFControl rFControl = c1g2InventoryCommand.RFControl;
                    profile[LlrpC1G2InventoryCommandGroup.ModeIndexKey] = rFControl.ModeIndex;
                    profile[LlrpC1G2InventoryCommandGroup.TariKey] = rFControl.Tari;
                }
                if (c1g2InventoryCommand.SingulationControl != null)
                {
                    C1G2SingulationControl singulationControl = c1g2InventoryCommand.SingulationControl;
                    profile[LlrpC1G2InventoryCommandGroup.SessionKey] = singulationControl.TagSession.ToString();
                    profile[LlrpC1G2InventoryCommandGroup.TagPopulationKey] = singulationControl.TagPopulation;
                    profile[LlrpC1G2InventoryCommandGroup.TagTransitTimeKey] = singulationControl.TagTransitTime;
                    if (singulationControl.Action != null)
                    {
                        C1G2TagInventoryStateAwareSingulationAction action = singulationControl.Action;
                        profile[LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareSingulationActionStateKey] = action.Inventory.ToString();
                        profile[LlrpC1G2InventoryCommandGroup.TagInventoryStateAwareSingulationActionSLFlagKey] = action.State.ToString();
                    }
                }
            }
        }

        private void PopulateDeviceReaderConfiguration(GetReaderConfigurationResponse response, ref PropertyList profile)
        {
            profile[LlrpTroubleshootGroup.ResetToFactoryDefaultKey] = false;
            if (response.EventAndReport != null)
            {
                profile[LlrpEventsAndReportGroup.HoldsEventsAndReportKey] = response.EventAndReport.HoldEventAndReportUponReconnect;
            }
            else
            {
                lock (this.DeviceState)
                {
                    if (((this.DeviceState.LlrpSpecificCapabilities != null) && this.DeviceState.LlrpSpecificCapabilities.ContainsKey(LlrpCapabilitiesGroup.SupportsEventAndReportHoldingKey)) && ((bool)this.DeviceState.LlrpSpecificCapabilities[LlrpCapabilitiesGroup.SupportsEventAndReportHoldingKey]))
                    {
                        profile[LlrpEventsAndReportGroup.HoldsEventsAndReportKey] = false;
                    }
                }
            }
            if (response.KeepAlive != null)
            {
                KeepAliveSpec keepAlive = response.KeepAlive;
                profile[LlrpKeepAliveSpecGroup.SendKeepaliveKey] = keepAlive.TriggerType != KeepAliveTriggerType.Null;
                profile[LlrpKeepAliveSpecGroup.TimeIntervalKey] = keepAlive.TimeInterval;
            }
            if (response.LlrpConfigurationState != null)
            {
                profile[LlrpConfigurationStateGroup.StateKey] = response.LlrpConfigurationState.StateValue;
            }
            if (response.AccessReportSpec != null)
            {
                profile[LlrpAccessReportSpecGroup.TriggerKey] = response.AccessReportSpec.Trigger.ToString();
            }
            this.PopulateDeviceReaderEventNotificationState(response, ref profile);
            PopulateDeviceROReportSpec(response, ref profile);
            Util.PopulateDeviceCustomProperties(response, ref profile, this.GetCustomMetadata());
        }

        private void PopulateDeviceReaderEventNotificationState(GetReaderConfigurationResponse response, ref PropertyList profile)
        {
            if (response.ReaderEventNotification != null)
            {
                ReaderEventNotificationSpec readerEventNotification = response.ReaderEventNotification;
                if (readerEventNotification.EventNotificationStates != null)
                {
                    foreach (EventNotificationState state in readerEventNotification.EventNotificationStates)
                    {
                        switch (state.EventNotificationStateEventType)
                        {
                            case EventNotificationStateEventType.HoppingToNextChannel:
                                {
                                    profile[LlrpReaderEventNotificationSpecGroup.HoppingEventKey] = state.Enabled;
                                    continue;
                                }
                            case EventNotificationStateEventType.Gpi:
                                {
                                    profile[LlrpReaderEventNotificationSpecGroup.GpiEventKey] = state.Enabled;
                                    continue;
                                }
                            case EventNotificationStateEventType.ROSpec:
                                {
                                    profile[LlrpReaderEventNotificationSpecGroup.ROSpecEventKey] = state.Enabled;
                                    continue;
                                }
                            case EventNotificationStateEventType.ReportBufferFillWarning:
                                {
                                    profile[LlrpReaderEventNotificationSpecGroup.BufferFillWarningEventKey] = state.Enabled;
                                    continue;
                                }
                            case EventNotificationStateEventType.ReaderExceptionEvent:
                                {
                                    profile[LlrpReaderEventNotificationSpecGroup.ReaderExceptionEventKey] = state.Enabled;
                                    continue;
                                }
                            case EventNotificationStateEventType.RFSurveyEvent:
                                {
                                    profile[LlrpReaderEventNotificationSpecGroup.RFSurveyEventKey] = state.Enabled;
                                    continue;
                                }
                            case EventNotificationStateEventType.AISpecEvent:
                                {
                                    profile[LlrpReaderEventNotificationSpecGroup.AISpecEndEventKey] = state.Enabled;
                                    continue;
                                }
                            case EventNotificationStateEventType.AISpecWithSingulation:
                                {
                                    profile[LlrpReaderEventNotificationSpecGroup.AISpecEndWithSingulationEventKey] = state.Enabled;
                                    continue;
                                }
                            case EventNotificationStateEventType.AntennaEvent:
                                {
                                    profile[LlrpReaderEventNotificationSpecGroup.AntennaEventKey] = state.Enabled;
                                    continue;
                                }
                        }
                        this.Logger.Error("Invalid event notification state event {0}", new object[] { state.EventNotificationStateEventType.ToString() });
                    }
                }
            }
        }

        private static void PopulateDeviceROReportSpec(GetReaderConfigurationResponse response, ref PropertyList profile)
        {
            if (response.ROReportSpec != null)
            {
                ROReportSpec rOReportSpec = response.ROReportSpec;
                profile[LlrpROReportSpecGroup.NumberOfTagReportDataKey] = rOReportSpec.NumberOfTagReportData;
                profile[LlrpROReportSpecGroup.TriggerKey] = rOReportSpec.ReportTrigger.ToString();
                if (rOReportSpec.ContentSelector != null)
                {
                    TagReportContentSelector contentSelector = rOReportSpec.ContentSelector;
                    profile[LlrpROReportSpecGroup.EnableAccessSpecIdKey] = contentSelector.EnableAccessSpecId;
                    profile[LlrpROReportSpecGroup.EnableAntennaIdKey] = contentSelector.EnableAntennaId;
                    profile[LlrpROReportSpecGroup.EnableChannelIndexKey] = contentSelector.EnableChannelIndex;
                    profile[LlrpROReportSpecGroup.EnableFirstSeenTimestampKey] = contentSelector.EnableFirstSeenTimestamp;
                    profile[LlrpROReportSpecGroup.EnableInventoryParameterSpecIdKey] = contentSelector.EnableInventoryParameterSpecId;
                    profile[LlrpROReportSpecGroup.EnableLastSeenTimestampKey] = contentSelector.EnableLastSeenTimestamp;
                    profile[LlrpROReportSpecGroup.EnablePeakRssiKey] = contentSelector.EnablePeakRssi;
                    profile[LlrpROReportSpecGroup.EnableROSpecIdKey] = contentSelector.EnableROSpecId;
                    profile[LlrpROReportSpecGroup.EnableSpecIndexKey] = contentSelector.EnableSpecIndex;
                    profile[LlrpROReportSpecGroup.EnableTagSeenCountKey] = contentSelector.EnableTagSeenCount;
                    if (contentSelector.MemorySelector != null)
                    {
                        C1G2EpcMemorySelector selector2 = null;
                        foreach (AirProtocolSpecificEpcMemorySelectorParameter parameter in contentSelector.MemorySelector)
                        {
                            selector2 = (C1G2EpcMemorySelector)parameter;
                            profile[LlrpROReportSpecGroup.C1G2EnableCrcKey] = selector2.EnableCrc;
                            profile[LlrpROReportSpecGroup.C1G2EnablePCKey] = selector2.EnablePC;
                        }
                    }
                }
            }
        }

        private void PopulateGpiReaderConfiguration(GetReaderConfigurationResponse response, ushort gpiId, ref PropertyList profile)
        {
            if (response.GpiCurrentStates != null)
            {
                foreach (GpiPortCurrentState state in response.GpiCurrentStates)
                {
                    if (state.PortNumber == gpiId)
                    {
                        profile[SourceGroup.PortInputValueKey] = GetPortInputValue(state.GpiState);
                        profile[SourceGroup.EnabledKey] = state.Enabled;
                    }
                }
            }
            this.PopulateSourceCustomProperties(response, gpiId, LLRPSourceType.GPI, ref profile);
        }

        private void PopulateGpoReaderConfiguration(GetReaderConfigurationResponse response, ushort gpoId, ref PropertyList profile)
        {
            if (response.GpoData != null)
            {
                foreach (GpoWriteData data in response.GpoData)
                {
                    if (data.PortNumber == gpoId)
                    {
                        profile[SourceGroup.PortOutputValueKey] = data.Data ? new byte[] { 1 } : new byte[1];
                    }
                }
            }
            this.PopulateSourceCustomProperties(response, gpoId, LLRPSourceType.GPO, ref profile);
        }

        private void PopulateInventoryCommandConfiguration(AntennaConfiguration antennaConfiguration, ref PropertyList profile)
        {
            if (antennaConfiguration.AirProtocolInventoryCommand != null)
            {
                foreach (AirProtocolInventoryCommandSettings settings in antennaConfiguration.AirProtocolInventoryCommand)
                {
                    if (settings is C1G2InventoryCommand)
                    {
                        PopulateC1G2InventoryCommandConfiguration((C1G2InventoryCommand)settings, ref profile);
                    }
                }
            }
        }

        private void PopulateSourceCustomProperties(GetReaderConfigurationResponse response, ushort sourceId, LLRPSourceType sourceType, ref PropertyList profile)
        {
            if (((response != null) && (response.CustomParameters != null)) && (response.CustomParameters.Count != 0))
            {
                foreach (CustomParameterBase base2 in response.CustomParameters)
                {
                //    if (!(base2 is MicrosoftDevicePropertiesParameter))
                //    {
                //        continue;
                //    }
                //    MicrosoftDevicePropertiesParameter parameter = (MicrosoftDevicePropertiesParameter)base2;
                //    if ((parameter != null) && (parameter.RfidProperties != null))
                //    {
                //        PropertyProfile source = null;
                //        switch (sourceType)
                //        {
                //            case LLRPSourceType.Antenna:
                //                if (((parameter.RfidProperties.AntennaProfiles == null) || !parameter.RfidProperties.AntennaProfiles.ContainsKey(sourceId)) || (parameter.RfidProperties.AntennaProfiles[sourceId] == null))
                //                {
                //                    continue;
                //                }
                //                source = parameter.RfidProperties.AntennaProfiles[sourceId];
                //                break;

                //            case LLRPSourceType.GPI:
                //                if (((parameter.RfidProperties.GpiProfiles == null) || !parameter.RfidProperties.GpiProfiles.ContainsKey(sourceId)) || (parameter.RfidProperties.GpiProfiles[sourceId] == null))
                //                {
                //                    continue;
                //                }
                //                source = parameter.RfidProperties.GpiProfiles[sourceId];
                //                break;

                //            case LLRPSourceType.GPO:
                //                if (((parameter.RfidProperties.GpoProfiles == null) || !parameter.RfidProperties.GpoProfiles.ContainsKey(sourceId)) || (parameter.RfidProperties.GpoProfiles[sourceId] == null))
                //                {
                //                    continue;
                //                }
                //                source = parameter.RfidProperties.GpoProfiles[sourceId];
                //                break;
                //        }
                //        Util.AppendProperties(profile, source, this.GetCustomMetadata());
                //    }
                }
            }
        }

        private void ProcessLlrpAirProtocolSpecificCapabilities(GetReaderCapabilitiesResponse response)
        {
            if ((response != null) && (response.AirProtocolLlrpCapabilities != null))
            {
                C1G2LlrpCapabilities airProtocolLlrpCapabilities = response.AirProtocolLlrpCapabilities as C1G2LlrpCapabilities;
                PropertyList llrpSpecificCapabilities = this.DeviceState.LlrpSpecificCapabilities;
                llrpSpecificCapabilities[LlrpC1G2CapabilitiesGroup.MaximumSelectFiltersPerQueryKey] = airProtocolLlrpCapabilities.MaximumNumberSelectFiltersPerQuery;
                llrpSpecificCapabilities[LlrpC1G2CapabilitiesGroup.SupportsBlockEraseKey] = airProtocolLlrpCapabilities.CanSupportBlockErase;
                llrpSpecificCapabilities[LlrpC1G2CapabilitiesGroup.SupportsBlockWriteKey] = airProtocolLlrpCapabilities.CanSupportBlockWrite;
            }
        }

        private void ProcessLlrpCapabilities(GetReaderCapabilitiesResponse response)
        {
            if ((response != null) && (response.LlrpCapabilities != null))
            {
                LlrpCapabilities llrpCapabilities = response.LlrpCapabilities;
                PropertyList llrpSpecificCapabilities = this.DeviceState.LlrpSpecificCapabilities;
                llrpSpecificCapabilities[LlrpCapabilitiesGroup.CanDoRFSurveyKey] = llrpCapabilities.CanDoRFSurvey;
                llrpSpecificCapabilities[LlrpCapabilitiesGroup.CanDOTagInventoryStateAwareSingulationKey] = llrpCapabilities.CanDoTagInventoryStateAwareSingulation;
                llrpSpecificCapabilities[LlrpCapabilitiesGroup.CanReportBufferFillWarningKey] = llrpCapabilities.CanReportBufferFillWarning;
                llrpSpecificCapabilities[LlrpCapabilitiesGroup.MaximumROSpecKey] = llrpCapabilities.MaximumROSpec;
                llrpSpecificCapabilities[LlrpCapabilitiesGroup.MaximumSpecsPerROSpecKey] = llrpCapabilities.MaximumNumberOfSpecPerROSpec;
                llrpSpecificCapabilities[LlrpCapabilitiesGroup.MaximumInventoryPerAIKey] = llrpCapabilities.MaximumNumberInventoryParameterSpecPerAISpecs;
                llrpSpecificCapabilities[LlrpCapabilitiesGroup.MaximumPriorityLevelSupportedKey] = llrpCapabilities.MaximumPriorityLevelSupported;
                llrpSpecificCapabilities[LlrpCapabilitiesGroup.MaximumAccessSpecKey] = llrpCapabilities.MaximumNumberOfAccessSpecs;
                llrpSpecificCapabilities[LlrpCapabilitiesGroup.MaximumOperationSpecPerAccessSpecKey] = llrpCapabilities.MaximumNumberOfOPSpecsPerAccessSpecs;
                llrpSpecificCapabilities[LlrpCapabilitiesGroup.SupportsClientOperationSpecKey] = llrpCapabilities.SupportsClientRequestOPSpec;
                llrpSpecificCapabilities[LlrpCapabilitiesGroup.ClientRequestOperationSpecTimeoutKey] = llrpCapabilities.ClientRequestsOPSpecTimeout;
                llrpSpecificCapabilities[LlrpCapabilitiesGroup.SupportsEventAndReportHoldingKey] = llrpCapabilities.SupportsEventAndReportHandling;
            }
        }

        private void ProcessLlrpGeneralDeviceCapabilities(GetReaderCapabilitiesResponse response)
        {
            if (response.GeneralDeviceCapabilities != null)
            {
                GeneralDeviceCapabilities generalDeviceCapabilities = response.GeneralDeviceCapabilities;
                PropertyList llrpSpecificCapabilities = this.DeviceState.LlrpSpecificCapabilities;
                llrpSpecificCapabilities[GeneralGroup.VendorKey] = generalDeviceCapabilities.DeviceManufacturerName.ToString(CultureInfo.CurrentCulture);
                llrpSpecificCapabilities[LlrpGeneralCapabilitiesGroup.ModelNameKey] = generalDeviceCapabilities.ModelName;
                llrpSpecificCapabilities[GeneralGroup.FirmwareVersionKey] = generalDeviceCapabilities.ReaderFirmwareVersion;
                llrpSpecificCapabilities[LlrpGeneralCapabilitiesGroup.MaxAntennaSupportedKey] = generalDeviceCapabilities.MaxNumberOfAntennaSupported;
                llrpSpecificCapabilities[LlrpGeneralCapabilitiesGroup.CanSetAntennaPropertiesKey] = generalDeviceCapabilities.CanSetAntennaProperties;
                llrpSpecificCapabilities[LlrpGeneralCapabilitiesGroup.HasUtcClockCapabilityKey] = generalDeviceCapabilities.HasUtcClockCapability;
                if (generalDeviceCapabilities.ReceiveTableEntries != null)
                {
                    Collection<ReceiveSensitivityTableEntry> receiveTableEntries = generalDeviceCapabilities.ReceiveTableEntries;
                    short[] numArray = new short[receiveTableEntries.Count];
                    for (int i = 0; i < receiveTableEntries.Count; i++)
                    {
                        numArray[i] = receiveTableEntries[i].ReceiveSensitivityValue;
                    }
                    llrpSpecificCapabilities[LlrpGeneralCapabilitiesGroup.ReceiveSensitivityTableKey] = numArray;
                }
                if (generalDeviceCapabilities.GpioCapabilities != null)
                {
                    llrpSpecificCapabilities[LlrpGeneralCapabilitiesGroup.NumberOfGpiKey] = generalDeviceCapabilities.GpioCapabilities.NumberOfGpi;
                    llrpSpecificCapabilities[LlrpGeneralCapabilitiesGroup.NumberOfGpoKey] = generalDeviceCapabilities.GpioCapabilities.NumberOfGpo;
                }
            }
        }

        private void ProcessLlrpGeneralSourceCapabilities(GetReaderCapabilitiesResponse response)
        {
            if ((response != null) && (response.GeneralDeviceCapabilities != null))
            {
                GeneralDeviceCapabilities generalDeviceCapabilities = response.GeneralDeviceCapabilities;
                if (generalDeviceCapabilities.AntennaReceiveSensitivityRange != null)
                {
                    string key = string.Empty;
                    foreach (PerAntennaReceiveSensitivityRange range in generalDeviceCapabilities.AntennaReceiveSensitivityRange)
                    {
                        key = Util.GetAntennaName(range.AntennaId);
                        if (this.DeviceState.Sources.ContainsKey(key))
                        {
                            PropertyList profile = this.DeviceState.Sources[key];
                            profile[LlrpGeneralCapabilitiesGroup.AntennaSensitivityMinimumIndexKey] = range.MinimumIndex;
                            profile[LlrpGeneralCapabilitiesGroup.AntennaSensitivityMaximumIndexKey] = range.MaximumIndex;
                        }
                        else
                        {
                            this.Logger.Error("Device {0} returns antenna sensitivty range of antenna {1} outside the max antenna", this.Device.DeviceName, range.AntennaId);
                        }
                    }
                }
                if (generalDeviceCapabilities.AntennasAirProtocol != null)
                {
                    string antennaName = string.Empty;
                    foreach (PerAntennaAirProtocol protocol in generalDeviceCapabilities.AntennasAirProtocol)
                    {
                        antennaName = Util.GetAntennaName(protocol.AntennaId);
                        if (this.DeviceState.Sources.ContainsKey(antennaName))
                        {
                            PropertyList profile2 = this.DeviceState.Sources[antennaName];
                            if (protocol.AirProtocolSupported != null)
                            {
                                string[] strArray = new string[protocol.AirProtocolSupported.Count];
                                for (int i = 0; i < strArray.Length; i++)
                                {
                                    strArray[i] = protocol.AirProtocolSupported[i].ToString();
                                }
                                profile2[RFGroup.AirProtocolsSupportedKey] = strArray;
                            }
                            continue;
                        }
                        this.Logger.Error("Device {0} returns air protocol of antenna {1} outside the max antenna", this.Device.DeviceName, protocol.AntennaId);
                    }
                }
            }
        }

        private void ProcessLlrpRegulatoryCapabilities(GetReaderCapabilitiesResponse response)
        {
            if ((response != null) && (response.RegulatoryCapabilities != null))
            {
                RegulatoryCapabilities regulatoryCapabilities = response.RegulatoryCapabilities;
                PropertyList llrpSpecificCapabilities = this.DeviceState.LlrpSpecificCapabilities;
                llrpSpecificCapabilities[LlrpRegulatoryCapabilitiesGroup.CountryCodeKey] = regulatoryCapabilities.CountryCode;
                llrpSpecificCapabilities[GeneralGroup.RegulatoryRegionKey] = regulatoryCapabilities.CommunicationStandard.ToString();
                if (regulatoryCapabilities.UhfBandCapabilities != null)
                {
                    UhfBandCapabilities uhfBandCapabilities = regulatoryCapabilities.UhfBandCapabilities;
                    if (uhfBandCapabilities.TransmitPowerTableEntries != null)
                    {
                        short[] numArray = new short[uhfBandCapabilities.TransmitPowerTableEntries.Count];
                        for (int i = 0; i < uhfBandCapabilities.TransmitPowerTableEntries.Count; i++)
                        {
                            numArray[i] = uhfBandCapabilities.TransmitPowerTableEntries[i].TransmitPowerLevel;
                        }
                        llrpSpecificCapabilities[LlrpRegulatoryCapabilitiesGroup.TransmitPowerTableKey] = numArray;
                    }
                    if (uhfBandCapabilities.FrequencyInformation != null)
                    {
                        FrequencyInformation frequencyInformation = uhfBandCapabilities.FrequencyInformation;
                        llrpSpecificCapabilities[LlrpRegulatoryCapabilitiesGroup.HoppingKey] = frequencyInformation.IsHopping;
                        if (frequencyInformation.IsHopping)
                        {
                            if (frequencyInformation.HopTables != null)
                            {
                                llrpSpecificCapabilities[LlrpRegulatoryCapabilitiesGroup.FrequencyHopTableKey] = LlrpSerializationHelper.SerializeToXmlDataContract(frequencyInformation.HopTables, false);
                            }
                            else
                            {
                                llrpSpecificCapabilities[LlrpRegulatoryCapabilitiesGroup.FrequencyHopTableKey] = null;
                                this.Logger.Error("Device {0} does not have hopping frequency table in spite of hopping", new object[] { this.Device.DeviceName });
                            }
                        }
                        else if (frequencyInformation.FixedTable != null)
                        {
                            FixedFrequencyTable fixedTable = frequencyInformation.FixedTable;
                            if (fixedTable.Frequencies != null)
                            {
                                uint[] numArray2 = new uint[fixedTable.Frequencies.Count];
                                for (int j = 0; j < fixedTable.Frequencies.Count; j++)
                                {
                                    numArray2[j] = fixedTable.Frequencies[j];
                                }
                                llrpSpecificCapabilities[LlrpRegulatoryCapabilitiesGroup.FixedFrequencyTableKey] = numArray2;
                            }
                        }
                        else
                        {
                            llrpSpecificCapabilities[LlrpRegulatoryCapabilitiesGroup.FixedFrequencyTableKey] = null;
                            this.Logger.Error("Device {0} does not have fixed frequency table in spite of not hopping", new object[] { this.Device.DeviceName });
                        }
                    }
                    if (uhfBandCapabilities.RFModeTable != null)
                    {
                        llrpSpecificCapabilities[LlrpRegulatoryCapabilitiesGroup.RFModeTableKey] = LlrpSerializationHelper.SerializeToXmlDataContract(uhfBandCapabilities.RFModeTable, false);
                    }
                }
            }
        }

        private void ProcessReaderCapabilitiesResponse(GetReaderCapabilitiesResponse response)
        {
            this.DeviceState.Sources = new Dictionary<string, PropertyList>(StringComparer.OrdinalIgnoreCase);
            this.GetSources(response);
            this.DeviceState.LlrpSpecificCapabilities = new PropertyList(LlrpResources.PropertyProfileName);
            this.ProcessLlrpGeneralDeviceCapabilities(response);
            this.ProcessLlrpGeneralSourceCapabilities(response);
            this.ProcessLlrpCapabilities(response);
            this.ProcessLlrpAirProtocolSpecificCapabilities(response);
            this.ProcessLlrpRegulatoryCapabilities(response);
            this.DeviceState.Metadata = Util.GetStandardDeviceMetadata(response);
            //this.DeviceState.MicrosoftSpecificMetadata = Util.GetMicrosoftCustomMetadata(response);
            //Collection<PropertyKey> collection = new Collection<PropertyKey>();
            //foreach (PropertyKey key in this.DeviceState.MicrosoftSpecificMetadata.Keys)
            //{
            //    if (this.DeviceState.Metadata.ContainsKey(key))
            //    {
            //        if (PDPState.DeviceOverrideAbleProperties.Contains(key))
            //        {
            //            this.Logger.Info("Property {0} supported by the device {1}. Removing it from provider maintained list...", new object[] { key, this.Device.DeviceName });
            //            this.DeviceState.Metadata[key] = this.DeviceState.MicrosoftSpecificMetadata[key];
            //            if (this.DeviceState.ProviderMaintainedProperties.ContainsKey(key))
            //            {
            //                this.DeviceState.ProviderMaintainedProperties.Remove(key);
            //            }
            //        }
            //        else
            //        {
            //            this.Logger.Info("Property {0} present already as a standard property on the device {1}. Thus the dropping the metadata in custom property", new object[] { key, this.Device.DeviceName });
            //            collection.Add(key);
            //        }
            //        continue;
            //    }
            //    this.DeviceState.Metadata.Add(key, this.DeviceState.MicrosoftSpecificMetadata[key]);
            //}
            //foreach (PropertyKey key2 in collection)
            //{
            //    this.DeviceState.MicrosoftSpecificMetadata.Remove(key2);
            //}
        }

        protected bool TryPopulateReaderCapabilities(out CommandError cmdError)
        {
            cmdError = null;
            this.Logger.Info("Getting the reader capabilities of the device {0}", new object[] { this.Device.DeviceName });
            try
            {
                GetReaderCapabilitiesResponse message = this.Device.Request(new GetReaderCapabilitiesMessage(ReaderCapabilitiesRequestedData.All, null)) as GetReaderCapabilitiesResponse;
                Util.ThrowIfNull(message, LlrpMessageType.GetReaderCapabilities);
                Util.ThrowIfFailed(message.Status);
                lock (this.DeviceState)
                {
                    this.ProcessReaderCapabilitiesResponse(message);
                }
                return true;
            }
            catch (SensorProviderException exception)
            {
                cmdError = new CommandError(LlrpErrorCode.GettingReaderCapabilitiesFailed, exception, null, LlrpErrorCode.GettingReaderCapabilitiesFailed.Description, null);
            }
            catch (DecodingException exception2)
            {
                cmdError = new CommandError(LlrpErrorCode.GettingReaderCapabilitiesFailed, exception2.Message, LlrpErrorCode.GettingReaderCapabilitiesFailed.Description, null);
            }
            catch (TimeoutException exception3)
            {
                cmdError = new CommandError(LlrpErrorCode.GettingReaderCapabilitiesFailed, exception3.Message, LlrpErrorCode.GettingReaderCapabilitiesFailed.Description, null);
            }
            return false;
        }

        protected void Validatecode(byte[] code)
        {
            if ((code != null) && (code.Length != 4))
            {
                throw new SensorProviderException(LlrpResources.InvalidCode);
            }
        }

        protected bool ValidateIfSourceIsAntenna(out CommandError cmdError)
        {
            cmdError = null;
            LLRPSourceType antenna = LLRPSourceType.Antenna;
            if (Util.IsValidSourceName(this.SourceName, out antenna) && (antenna == LLRPSourceType.Antenna))
            {
                return true;
            }
            cmdError = new CommandError(LlrpErrorCode.InvalidAntennaName, LlrpErrorCode.InvalidAntennaName.Description, LlrpErrorCode.InvalidAntennaName.Description, null);
            return false;
        }

        protected void ValidatePartialReadTagCommandOffset(int offset)
        {
            if ((offset < 0) || (offset > 0x1fffe))
            {
                throw new SensorProviderException(LlrpResources.InvalidReadTagOffset);
            }
        }

        protected void ValidatePartialTagCommandLength(int length)
        {
            if ((length < 0) || (length > 0x1fffe))
            {
                throw new SensorProviderException(LlrpResources.InvalidTagReadLength);
            }
        }

        protected void ValidatePartialWriteTagCommandOffset(int offset)
        {
            if (((offset < 0) || ((offset % 2) != 0)) || ((offset / 2) > 0xffff))
            {
                throw new SensorProviderException(LlrpResources.InvalidWriteTagOffset);
            }
        }

        protected void ValidateSeekOrigin(SeekOrigin seekOrigin)
        {
            if (seekOrigin != SeekOrigin.Begin)
            {
                throw new SensorProviderException(LlrpResources.InvalidSeekOrigin);
            }
        }

        private void ValidateSourceName()
        {
            if (this.SourceName != null)
            {
                bool flag = true;
                lock (this.DeviceState)
                {
                    if (this.DeviceState.Sources == null)
                    {
                        flag = false;
                    }
                }
                CommandError cmdError = null;
                if (flag || this.TryPopulateReaderCapabilities(out cmdError))
                {
                    lock (this.DeviceState)
                    {
                        if (!this.DeviceState.Sources.ContainsKey(this.SourceName))
                        {
                            this.Logger.Error("Source {0} not found on device {1}", new object[] { this.SourceName, this.Device.DeviceName });
                            throw new SensorProviderException(ErrorCode.SourceNotFound.Description);
                        }
                        return;
                    }
                }
                this.Logger.Error("Get_Reader_Capabilities failed on device {0}", new object[] { this.Device.DeviceName });
                throw new SensorProviderException(cmdError.Message);
            }
        }

        protected void ValidateTagData(byte[] data)
        {
            if ((data == null) || (data.Length == 0))
            {
                throw new SensorProviderException(LlrpResources.TagDataNullOrEmpty);
            }
            if ((data.Length % 2) != 0)
            {
                throw new SensorProviderException(LlrpResources.InvalidWriteTagData);
            }
        }

        protected void ValidateTagDatatSelectorForSyncCommand(TagDataSelector selector)
        {
            if ((null == selector) || !selector.IsInitialized)
            {
                throw new SensorProviderException(LlrpResources.TagDataSelectorUnInitialized);
            }
        }

        protected void ValidateThatSourceIsAntennaIfPresent()
        {
            CommandError cmdError = null;
            if ((this.SourceName != null) && !this.ValidateIfSourceIsAntenna(out cmdError))
            {
                throw new SensorProviderException(cmdError.Message);
            }
        }

        protected SensorCommand Command
        {
            get
            {
                return this.m_command;
            }
        }

        protected LlrpDevice Device
        {
            get
            {
                return this.m_device;
            }
        }

        protected PDPState DeviceState
        {
            get
            {
                return this.m_deviceState;
            }
        }

        internal abstract bool IsConcurrentToInventoryOperation { get; }

        protected ILogger Logger
        {
            get
            {
                return this.m_logger;
            }
        }

        protected string SourceName
        {
            get
            {
                return this.m_sourceName;
            }
        }
    }
}
