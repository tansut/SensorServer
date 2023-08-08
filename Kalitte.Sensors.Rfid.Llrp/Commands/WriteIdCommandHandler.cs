namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    
    
    
    using System;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Rfid.Llrp;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Exceptions;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Core;

    internal sealed class WriteIdCommandHandler : AccessSpecCommandHandler
    {
        private WriteIdCommand m_writeIdCommand;

        internal WriteIdCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
            this.m_writeIdCommand = (WriteIdCommand) command;
        }

        protected override void Device_MessageReceivedEvent(Collection<TagReportData> datas)
        {
            foreach (TagReportData data in datas)
            {
                Collection<AirProtocolSpecificOPSpecResult> airProtocolSpecificOPSpecResults = data.AirProtocolSpecificOPSpecResults;
                if (airProtocolSpecificOPSpecResults == null)
                {
                    break;
                }
                foreach (AirProtocolSpecificOPSpecResult result in airProtocolSpecificOPSpecResults)
                {
                    if (result is C1G2WriteOPSpecResult)
                    {
                        C1G2WriteOPSpecResult result2 = (C1G2WriteOPSpecResult) result;
                        if (result2.ResultType == C1G2WriteOPSpecResultType.Success)
                        {
                            base.Logger.Info("Write id command on device successful on device {0}", new object[] { base.Device.DeviceName });
                            this.m_writeIdCommand.Response = new WriteIdResponse();
                            base.IncrementSuccessfulCount();
                        }
                        else
                        {
                            base.Logger.Error("Write id command failed on device {0} with error {1}", new object[] { base.Device.DeviceName, result2.ResultType });
                            base.CommandError = new CommandError(LlrpErrorCode.CommandExecutionFailed, result2.ResultType.ToString(), result2.ResultType.ToString(), null);
                            base.IndicateCommandCompletion();
                        }
                    }
                }
            }
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            WriteIdCommand command = (WriteIdCommand) base.Command;
            base.Validatecode(command.GetPassCode());
            base.Validatecode(command.GetNewAccessCode());
            base.Validatecode(command.GetNewKillCode());
            return base.ExecuteCommand();
        }

        protected override Collection<OPSpec> GetOPSpec()
        {
            Collection<OPSpec> collection = new Collection<OPSpec>();
            WriteIdCommand command = (WriteIdCommand) base.Command;
            uint code = base.GetCode(command.GetPassCode());
            byte[] tagId = command.GetTagId();
            byte[] newAccessCode = command.GetNewAccessCode();
            byte[] newKillCode = command.GetNewKillCode();
            if (((tagId == null) && (newAccessCode == null)) && (newKillCode == null))
            {
                throw new SensorProviderException(LlrpResources.InvalidWriteIdCommand);
            }
            if (tagId != null)
            {
                collection.Add(new C1G2Write(code, C1G2MemoryBank.Epc, 2, BitHelper.GetInt16Array(tagId)));
            }
            if (newKillCode != null)
            {
                base.Validatecode(newKillCode);
                collection.Add(new C1G2Write(code, C1G2MemoryBank.Reserved, 0, BitHelper.GetInt16Array(newKillCode)));
            }
            if (newAccessCode != null)
            {
                base.Validatecode(newAccessCode);
                collection.Add(new C1G2Write(code, C1G2MemoryBank.Reserved, 2, BitHelper.GetInt16Array(newAccessCode)));
            }
            return collection;
        }
    }
}
