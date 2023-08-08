namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    
    
    using System;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid.Llrp;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Core;

    internal sealed class KillTagCommandHandler : AccessSpecCommandHandler
    {
        private KillCommand m_killCommand;

        internal KillTagCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
            this.m_killCommand = (KillCommand) command;
            base.TargetTagId = this.m_killCommand.GetTagId();
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
                    if (result is C1G2KillOPSpecResult)
                    {
                        C1G2KillOPSpecResult result2 = (C1G2KillOPSpecResult) result;
                        if (result2.ResultType == C1G2KillOPSpecResultType.Success)
                        {
                            base.Logger.Info("Kill command on device successful on device {0}", new object[] { base.Device.DeviceName });
                            this.m_killCommand.Response = new KillResponse();
                            base.IncrementSuccessfulCount();
                        }
                        else
                        {
                            base.Logger.Error("Kill command failed on device {0} with error {1}", new object[] { base.Device.DeviceName, result2.ResultType });
                            base.CommandError = new CommandError(LlrpErrorCode.CommandExecutionFailed, result2.ResultType.ToString(), result2.ResultType.ToString(), null);
                            base.IndicateCommandCompletion();
                        }
                    }
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
            KillCommand killCommand = this.m_killCommand;
            C1G2Kill item = new C1G2Kill(base.GetCode(killCommand.GetPassCode()));
            collection.Add(item);
            return collection;
        }
    }
}
