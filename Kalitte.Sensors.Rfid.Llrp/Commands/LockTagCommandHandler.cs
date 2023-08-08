namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    
    
    using System;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Rfid.Llrp.Utilities;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Rfid.Core;

    internal sealed class LockTagCommandHandler : AccessSpecCommandHandler
    {
        private LockTagCommand m_lockCommand;

        internal LockTagCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
            this.m_lockCommand = (LockTagCommand) base.Command;
            base.TargetTagId = this.m_lockCommand.GetTagId();
        }

        protected override void Device_MessageReceivedEvent(Collection<TagReportData> datas)
        {
            foreach (TagReportData data in datas)
            {
                base.Logger.Info("Report corresponding to the lock command on device {0}", new object[] { base.Device.DeviceName });
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
                            base.Logger.Info("Lock command on device successful on device {0}", new object[] { base.Device.DeviceName });
                            this.m_lockCommand.Response = new LockTagResponse();
                            base.IncrementSuccessfulCount();
                        }
                        else
                        {
                            base.Logger.Error("Lock command failed on device {0} with error {1}", new object[] { base.Device.DeviceName, result2.ResultType });
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
            uint code = base.GetCode(this.m_lockCommand.GetPassCode());
            C1G2LockPrivilege privilege = this.m_lockCommand.IsPermanent ? C1G2LockPrivilege.PermanentLock : C1G2LockPrivilege.ReadWrite;
            Collection<C1G2LockPayload> lockPayloads = new Collection<C1G2LockPayload>();
            if ((this.m_lockCommand.Targets == LockTargets.Id) || (this.m_lockCommand.Targets == LockTargets.Both))
            {
                lockPayloads.Add(new C1G2LockPayload(privilege, C1G2LockDataField.EpcMemory));
            }
            if ((this.m_lockCommand.Targets == LockTargets.Data) || (this.m_lockCommand.Targets == LockTargets.Both))
            {
                lockPayloads.Add(new C1G2LockPayload(privilege, C1G2LockDataField.UserMemory));
            }
            collection.Add(new C1G2Lock(code, lockPayloads));
            return collection;
        }
    }
}
