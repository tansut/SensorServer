namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using System;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Core;


    internal sealed class WriteTagDataCommandHandler : AccessSpecCommandHandler
    {
        private ushort m_offset;
        private WriteTagDataCommand m_writeTagDataCommand;

        internal WriteTagDataCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
            this.m_writeTagDataCommand = (WriteTagDataCommand) command;
            base.TargetTagId = this.m_writeTagDataCommand.GetTagId();
            this.MemoryBank = C1G2MemoryBank.User;

        }



        internal C1G2MemoryBank MemoryBank { get; set; }

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
                            base.Logger.Info("write tag data command on device successful on device {0}", new object[] { base.Device.DeviceName });
                            this.m_writeTagDataCommand.Response = new WriteTagDataResponse();
                            base.IncrementSuccessfulCount();
                        }
                        else
                        {
                            base.Logger.Error("write tag data command failed on device {0} with error {1}", new object[] { base.Device.DeviceName, result2.ResultType });
                            base.CommandError = new CommandError(LlrpErrorCode.CommandExecutionFailed, result2.ResultType.ToString(), result2.ResultType.ToString(), null);
                            base.IndicateCommandCompletion();
                        }
                    }
                }
            }
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            base.ValidateTagData(this.m_writeTagDataCommand.GetTagData());
            return base.ExecuteCommand();
        }

        protected override Collection<OPSpec> GetOPSpec()
        {
            Collection<OPSpec> collection = new Collection<OPSpec>();
            WriteTagDataCommand writeTagDataCommand = this.m_writeTagDataCommand;
            C1G2Write item = new C1G2Write(base.GetCode(writeTagDataCommand.GetPassCode()), MemoryBank, this.TagReadOffset, BitHelper.GetInt16Array(writeTagDataCommand.GetTagData()));
            collection.Add(item);
            return collection;
        }

        internal ushort TagReadOffset
        {
            private get
            {
                return this.m_offset;
            }
            set
            {
                this.m_offset = value;
            }
        }
    }
}
