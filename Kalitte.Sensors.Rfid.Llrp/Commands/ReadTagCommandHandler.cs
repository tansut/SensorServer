namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    
    
    using System;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;

    internal sealed class ReadTagCommandHandler : AccessSpecCommandHandler
    {
        internal ReadTagCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
        }

        protected override void Device_MessageReceivedEvent(Collection<TagReportData> datas)
        {
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            base.Logger.Info("Executing Get partial tag data command on device {0}", new object[] { base.Device.DeviceName });
            ReadTagCommand command = (ReadTagCommand) base.Command;
            base.ValidateSeekOrigin(command.SeekOrigin);
            base.ValidatePartialReadTagCommandOffset(command.Offset);
            base.ValidatePartialTagCommandLength(command.Length);
            ReadTagDataCommand command2 = new ReadTagDataCommand(command.GetPassCode(), command.GetTagId());
            ReadTagDataCommandHandler handler = new ReadTagDataCommandHandler(base.SourceName, command2, base.DeviceState, base.Device, base.Logger);
            handler.TagReadOffset = (ushort) (command.Offset / 2);
            handler.TagReadCount = this.GetReadCount(command.Length, command.Offset);
            handler.MemoryBank = (Rfid.Core.C1G2MemoryBank)command.MemoryBank;
            ResponseEventArgs args = handler.ExecuteCommand();
            if (args.CommandError != null)
            {
                return new ResponseEventArgs(base.Command, args.CommandError);
            }
            byte[] tagData = command2.Response.GetTagData();
            tagData = this.GetAppropriateTagData(tagData, command.Offset, command.Length);
            command.Response = new ReadTagResponse(tagData);
            return new ResponseEventArgs(command);
        }

        private byte[] GetAppropriateTagData(byte[] tagData, int offset, int length)
        {
            if ((tagData == null) || ((tagData.Length % 2) != 0))
            {
                base.Logger.Warning("No tag data returned or is not of word multiple on device {0}", new object[] { base.Device.DeviceName });
                return tagData;
            }
            bool flag = (offset % 2) == 0;
            bool flag2 = (length % 2) == 0;
            bool flag3 = !flag && !flag2;
            if (flag && flag2)
            {
                return tagData;
            }
            int num = tagData.Length - ((!flag && flag2) ? 2 : 1);
            byte[] buffer = new byte[num];
            int index = 0;
            if (flag)
            {
                buffer[index] = tagData[0];
            }
            for (int i = 1; i < (tagData.Length - 1); i++)
            {
                buffer[index++] = tagData[i];
            }
            if (flag3)
            {
                buffer[index++] = tagData[tagData.Length - 1];
            }
            return buffer;
        }

        protected override Collection<OPSpec> GetOPSpec()
        {
            return null;
        }

        private ushort GetReadCount(int bytesToRead, int offset)
        {
            ushort num = (ushort) (bytesToRead / 2);
            if (((offset % 2) == 0) && ((bytesToRead % 2) == 0))
            {
                return num;
            }
            return (ushort) (num + 1);
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
