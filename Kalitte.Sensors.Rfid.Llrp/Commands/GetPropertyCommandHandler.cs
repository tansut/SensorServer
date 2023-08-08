namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    
    
    
    using System;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Configuration;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Commands;

    internal sealed class GetPropertyCommandHandler : CommandHandler
    {
        private GetPropertyProfileCommandHandler m_internalCmdHandler;

        internal GetPropertyCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
            this.m_internalCmdHandler = new GetPropertyProfileCommandHandler(sourcName, new GetActivePropertyListCommand(), state, device, logger);
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            GetPropertyCommand command = (GetPropertyCommand) base.Command;
            this.m_internalCmdHandler.PropertyKeyOfInterest = command.PropertyKey;
            ResponseEventArgs args = this.m_internalCmdHandler.ExecuteCommand();
            if (args.CommandError != null)
            {
                return new ResponseEventArgs(base.Command, args.CommandError);
            }
            GetActivePropertyListCommand command2 = (GetActivePropertyListCommand) args.Command;
            GetActivePropertyListResponse response = command2.Response;
            if (((response.CurrentProfile == null) || (response.CurrentProfile.Count == 0)) || !response.CurrentProfile.ContainsKey(command.PropertyKey))
            {
                return new ResponseEventArgs(base.Command, new CommandError(ErrorCode.InvalidParameter, LlrpResources.InvalidKey, ErrorCode.InvalidParameter.Description, null));
            }
            command.Response = new GetPropertyResponse(new EntityProperty(command.PropertyKey, response.CurrentProfile[command.PropertyKey]));
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
