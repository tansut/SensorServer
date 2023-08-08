namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    
    
    
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Configuration;
    using Kalitte.Sensors.Exceptions;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;

    internal sealed class GetMetadataCommandHandler : CommandHandler
    {
        internal GetMetadataCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            base.Logger.Info("Executing Get metadata command on device {0}", new object[] { base.Device.DeviceName });
            Dictionary<PropertyKey, DevicePropertyMetadata> dictionary = null;
            GetMetadataCommand command = (GetMetadataCommand) base.Command;
            CommandError cmdError = null;
            lock (base.DeviceState)
            {
                if (base.DeviceState.Metadata != null)
                {
                    dictionary = base.DeviceState.Metadata;
                    base.Logger.Info("Metadata is already available for device {0}, so returning", new object[] { base.Device.DeviceName });
                }
            }
            if ((dictionary == null) && base.TryPopulateReaderCapabilities(out cmdError))
            {
                lock (base.DeviceState)
                {
                    dictionary = base.DeviceState.Metadata;
                }
            }
            if (cmdError != null)
            {
                return new ResponseEventArgs(base.Command, cmdError);
            }
            Dictionary<PropertyKey, DevicePropertyMetadata> metadata = dictionary;
            if (command.GroupName != null)
            {
                metadata = new Dictionary<PropertyKey, DevicePropertyMetadata>();
                if (dictionary != null)
                {
                    foreach (PropertyKey key in dictionary.Keys)
                    {
                        if (string.Compare(key.GroupName, command.GroupName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            metadata[key] = dictionary[key];
                        }
                    }
                }
                if (metadata.Count == 0)
                {
                    throw new SensorProviderException(string.Format(CultureInfo.CurrentCulture, "UnknownDevicePropertyGroupName", new object[] { command.GroupName }));
                }
            }
            command.Response = new GetMetadataResponse(metadata);
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
