namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    
    
    using System;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;

    internal sealed class WriteTagCommandHandler : AccessSpecCommandHandler
    {
        internal WriteTagCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
        }

        protected override void Device_MessageReceivedEvent(Collection<TagReportData> datas)
        {
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            base.Logger.Info("Executing Write partial tag data command on device {0}", new object[] { base.Device.DeviceName });
            WriteTagCommand command = (WriteTagCommand) base.Command;
            base.ValidateSeekOrigin(command.SeekOrigin);
            base.ValidatePartialWriteTagCommandOffset(command.Offset);
            WriteTagDataCommand command2 = new WriteTagDataCommand(command.GetPassCode(), command.GetTagId(), command.GetTagData());
            WriteTagDataCommandHandler handler = new WriteTagDataCommandHandler(base.SourceName, command2, base.DeviceState, base.Device, base.Logger);
            handler.TagReadOffset = (ushort) (command.Offset / 2);
            handler.MemoryBank = (Rfid.Core.C1G2MemoryBank)command.MemoryBank;
            ResponseEventArgs args = handler.ExecuteCommand();
            if (args.CommandError != null)
            {
                return new ResponseEventArgs(base.Command, args.CommandError);
            }
            command.Response = new WriteTagResponse();
            return new ResponseEventArgs(command);
        }

        protected override Collection<OPSpec> GetOPSpec()
        {
            return null;
        }

        internal override bool IsConcurrentToInventoryOperation
        {
            get
            {
                return false;
            }
        }
    }
}
