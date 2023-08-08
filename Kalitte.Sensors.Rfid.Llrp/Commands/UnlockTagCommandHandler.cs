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
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Rfid.Core;

    internal sealed class UnlockTagCommandHandler : AccessSpecCommandHandler
    {
        private UnlockTagCommand m_unlockCommand;

        internal UnlockTagCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
            this.m_unlockCommand = (UnlockTagCommand) command;
            base.TargetTagId = this.m_unlockCommand.GetTagId();
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
                    if (result is C1G2LockOPSpecResult)
                    {
                        C1G2LockOPSpecResult result2 = (C1G2LockOPSpecResult) result;
                        if (result2.ResultType == C1G2LockOPSpecResultType.Success)
                        {
                            base.Logger.Info("UnLock command on device successful on device {0}", new object[] { base.Device.DeviceName });
                            this.m_unlockCommand.Response = new UnlockTagResponse();
                            base.IncrementSuccessfulCount();
                        }
                        else
                        {
                            base.Logger.Error("UnLock command failed on device {0} with error {1}", new object[] { base.Device.DeviceName, result2.ResultType });
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
            UnlockTagCommand unlockCommand = this.m_unlockCommand;
            uint code = base.GetCode(unlockCommand.GetPassCode());
            C1G2LockPrivilege unlock = C1G2LockPrivilege.Unlock;
            Collection<C1G2LockPayload> lockPayloads = new Collection<C1G2LockPayload>();
            if ((unlockCommand.Targets == LockTargets.Id) || (unlockCommand.Targets == LockTargets.Both))
            {
                lockPayloads.Add(new C1G2LockPayload(unlock, C1G2LockDataField.EpcMemory));
            }
            if ((unlockCommand.Targets == LockTargets.Data) || (unlockCommand.Targets == LockTargets.Both))
            {
                lockPayloads.Add(new C1G2LockPayload(unlock, C1G2LockDataField.UserMemory));
            }
            collection.Add(new C1G2Lock(code, lockPayloads));
            return collection;
        }
    }
}
