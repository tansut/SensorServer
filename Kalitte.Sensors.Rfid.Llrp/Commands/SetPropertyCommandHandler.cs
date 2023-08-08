namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    
    
    
    using System;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Configuration;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;

    internal sealed class SetPropertyCommandHandler : CommandHandler
    {
        private SetPropertyCommand m_actualCommand;
        private ApplyPropertyProfileCommandHandler m_internalCmdHandler;
        private PropertyList m_profile;

        internal SetPropertyCommandHandler(string sourceName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourceName, command, state, device, logger)
        {
            this.m_actualCommand = (SetPropertyCommand) command;
            this.m_profile = new PropertyList(LlrpResources.PropertyProfileName);
            this.m_profile.Add(this.m_actualCommand.Property.Key, this.m_actualCommand.Property.PropertyValue);
            this.m_internalCmdHandler = new ApplyPropertyProfileCommandHandler(sourceName, new ApplyPropertyListCommand(this.m_profile), state, device, logger);
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            ResponseEventArgs args = this.m_internalCmdHandler.ExecuteCommand();
            if (args.CommandError != null)
            {
                CommandError commandError = args.CommandError;
                if (commandError is ApplyPropertyListFailedError)
                {
                    ApplyPropertyListFailedError error2 = (ApplyPropertyListFailedError)commandError;
                    commandError = error2.DetailedErrors[this.m_actualCommand.Property.Key];
                }
                return new ResponseEventArgs(base.Command, commandError);
            }
            this.m_actualCommand.Response = new SetPropertyResponse();
            return new ResponseEventArgs(base.Command);
        }

        internal override bool IsConcurrentToInventoryOperation
        {
            get
            {
                 return this.m_internalCmdHandler.IsConcurrentToInventoryOperation;
            }
        }
    }
}
