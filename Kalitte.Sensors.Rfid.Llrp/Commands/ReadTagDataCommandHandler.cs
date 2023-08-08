namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    
    
    using System;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Core;

    internal sealed class ReadTagDataCommandHandler : AccessSpecCommandHandler
    {
        private ReadTagDataCommand m_getTagDataCommand;
        private ushort m_length;
        private ushort m_offset;

        internal ReadTagDataCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
            this.m_getTagDataCommand = (ReadTagDataCommand) command;
            base.TargetTagId = this.m_getTagDataCommand.GetTagId();
            this.MemoryBank = C1G2MemoryBank.User;
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
                    if (!(result is C1G2ReadOPSpecResult))
                    {
                        continue;
                    }
                    C1G2ReadOPSpecResult readResult = (C1G2ReadOPSpecResult) result;
                    if (readResult.ResultType == C1G2ReadOPSpecResultType.Success)
                    {
                        base.Logger.Info("Get tag data command on device successful on device {0}", new object[] { base.Device.DeviceName });
                        this.m_getTagDataCommand.Response = new ReadTagDataResponse(Util.GetTagData(readResult));
                    }
                    else
                    {
                        base.Logger.Error("Get tag data command failed on device {0} with error {1}", new object[] { base.Device.DeviceName, readResult.ResultType });
                        base.CommandError = new CommandError(LlrpErrorCode.CommandExecutionFailed, readResult.ResultType.ToString(), readResult.ResultType.ToString(), null);
                    }
                    base.IndicateCommandCompletion();
                }
            }
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            return base.ExecuteCommand();
        }

        protected override Collection<OPSpec> GetOPSpec()
        {
            Collection<OPSpec> collection = new Collection<OPSpec>();
            ReadTagDataCommand getTagDataCommand = this.m_getTagDataCommand;
            C1G2Read item = new C1G2Read(base.GetCode(getTagDataCommand.GetPassCode()),
                MemoryBank, this.TagReadOffset, this.TagReadCount);
            collection.Add(item);
            return collection;
        }

        internal ushort TagReadCount
        {
            private get
            {
                return this.m_length;
            }
            set
            {
                this.m_length = value;
            }
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

        internal C1G2MemoryBank MemoryBank { get; set; }
    }
}
