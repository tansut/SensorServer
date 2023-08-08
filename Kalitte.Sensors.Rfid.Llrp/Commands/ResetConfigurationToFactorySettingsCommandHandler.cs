namespace Kalitte.Sensors.Rfid.Llrp.Commands
{

    using System;
    using System.Globalization;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Commands;

    internal sealed class ResetConfigurationToFactorySettingsCommandHandler : CommandHandler
    {
        internal ResetConfigurationToFactorySettingsCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            return new ResponseEventArgs(base.Command, new CommandError(ErrorCode.FunctionUnsupported, string.Format(CultureInfo.CurrentCulture, LlrpResources.ResetConfigurationToFactorySettingsCommandNotSupportedError, new object[] { "Reset to Factory default", "Llrp Troubleshoot" }), ErrorCode.FunctionUnsupported.ToString(), null));
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
