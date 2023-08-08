namespace Kalitte.Sensors.Rfid.Llrp.Commands
{

    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;

    using System.Threading;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Rfid.Llrp.Properties;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Llrp.Events;
    using Kalitte.Sensors.Rfid.Llrp.Configuration;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Rfid.Core;
    using Kalitte.Sensors.Commands;
    using Kalitte.Sensors.Rfid.Utilities;

    internal abstract class AccessSpecCommandHandler : CommandHandler
    {
        private AccessSpec m_accessSpec;
        private ushort m_actualTagsOperated;
        private bool m_commandCompletedSuccessfully;
        private CommandError m_commandError;
        private string m_commandName;
        private ushort m_desiredOperationCount;
        private ROSpec m_roSpec;
        private byte[] m_targetTagId;
        private AutoResetEvent m_waitEvent;

        internal AccessSpecCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger)
            : base(sourcName, command, state, device, logger)
        {
            this.m_waitEvent = new AutoResetEvent(false);
            this.m_commandName = command.GetType().Name;
        }

        protected ROSpec CreateROSpecForSynchronousCommand()
        {
            return this.CreateROSpecForSynchronousCommand(TagDataSelector.All);
        }

        protected ROSpec CreateROSpecForSynchronousCommand(TagDataSelector dataSelector)
        {
            int num;
            string sourceName = base.SourceName;
            lock (base.DeviceState)
            {
                num = (int)base.DeviceState.ProviderMaintainedProperties[LlrpManagementGroup.SynchronousCommandInventoryDurationKey];
            }
            base.Logger.Info("Duration for which inventory will be done on the device {0} is {1}", base.Device.DeviceName, num);
            ushort antennaId = base.GetAntennaId(sourceName);
            Collection<ushort> antennaIds = new Collection<ushort>();
            antennaIds.Add(antennaId);
            Collection<InventoryParameterSpec> inventoryParameterSpecs = new Collection<InventoryParameterSpec>();
            InventoryParameterSpec item = new InventoryParameterSpec(AirProtocolId.EpcClass1Gen2, null, null);
            inventoryParameterSpecs.Add(item);
            AISpec spec2 = new AISpec(antennaIds, new AISpecStopTrigger(AISpecStopTriggerType.Null, uint.MaxValue, null, null), inventoryParameterSpecs, null);
            Collection<AISpec> aiSpec = new Collection<AISpec>();
            aiSpec.Add(spec2);
            bool enablePC = false;
            if (null != dataSelector)
            {
                enablePC = dataSelector.IsNumberingSystemIdentifier;
            }
            Collection<AirProtocolSpecificEpcMemorySelectorParameter> memorySelector = new Collection<AirProtocolSpecificEpcMemorySelectorParameter>();
            memorySelector.Add(new C1G2EpcMemorySelector(false, enablePC));
            return new ROSpec(IdGenerator.GenerateROSpecIdForProvider(), 0, new ROBoundarySpec(new ROSpecStartTrigger(ROSpecStartTriggerType.Immediate, null, null), new ROSpecStopTrigger((uint)num)), aiSpec, null, null, 
                new ROReportSpec(ROReportTrigger.NTagReportDataOrROSpecEnd, 1, new TagReportContentSelector(true, true, true, true, true, true, true, true, true, true, memorySelector), null));
        }

        protected abstract void Device_MessageReceivedEvent(Collection<TagReportData> datas);
        protected virtual void Device_MessageReceivedEvent(object sender, LlrpMessageEventArgs args)
        {
            if (args.IsSuccess)
            {
                base.Logger.Info("Received response {0} in command {1} event handler for device {2}", new object[] { args.Response, this.m_commandName, base.Device.DeviceName });
                if (args.Response is ROAccessReport)
                {
                    ROAccessReport response = (ROAccessReport)args.Response;
                    Collection<TagReportData> tagReports = response.TagReports;
                    if (tagReports != null)
                    {
                        Collection<TagReportData> datas = new Collection<TagReportData>();
                        foreach (TagReportData data in tagReports)
                        {
                            if ((data.ROSpecId == null) || (data.ROSpecId.Id != this.m_roSpec.Id))
                            {
                                base.Logger.Warning("Report does not correspond to the RO Spec added for this command {0} on device {1}.Expected id {2}, received id {3}", new object[] { this.m_commandName, base.Device.DeviceName, this.m_roSpec.Id, (data.ROSpecId == null) ? "N/A" : data.ROSpecId.Id.ToString() });
                                continue;
                            }
                            if (this.m_accessSpec != null)
                            {
                                if (data.AccessSpecId == null)
                                {
                                    base.Logger.Warning("Report does not have access spec id corresponding to the command {0} on device {1}", new object[] { this.m_commandName, base.Device.DeviceName });
                                    continue;
                                }

                                if (data.AccessSpecId.Id != this.m_accessSpec.Id)
                                {
                                    base.Logger.Warning("Report not corresponding to the command {0} on device {1}. Access spec expected {2} but message has {3}", new object[] { this.m_commandName, base.Device.DeviceName, this.m_accessSpec.Id, data.AccessSpecId.Id });
                                    continue;
                                }
                            }
                            base.Logger.Info("Report corresponding to the command {0} on device {1}", new object[] { this.m_commandName, base.Device.DeviceName });
                            this.Print(data);
                            datas.Add(data);
                        }
                        this.Device_MessageReceivedEvent(datas);
                    }
                }
            }
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            base.Logger.Info("Executing command {0} on device {1} on source {2}", new object[] { base.Command, base.Device.DeviceName, base.SourceName });
            base.AssertInventoryIsOff();
            base.ValidateThatSourceIsAntennaIfPresent();
            this.PopulateOperationCount();
            this.m_roSpec = this.GetROSpec();
            this.m_accessSpec = this.GetAccessSpec();
            this.ExecuteTagRelatedSynchronousCommand();
            base.Logger.Info("Command completed {0} on device {1} on source {2}", new object[] { this.m_commandName, base.Device.DeviceName, base.SourceName });
            if (this.m_commandError != null)
            {
                return new ResponseEventArgs(base.Command, this.m_commandError);
            }
            if (this.m_commandCompletedSuccessfully)
            {
                return new ResponseEventArgs(base.Command);
            }
            if (this.IsSingleTagCommand())
            {
                return new ResponseEventArgs(base.Command, new CommandError(ErrorCode.TimedOut, ErrorCode.TimedOut.Description, ErrorCode.TimedOut.Description, null));
            }
            if ((this.TagsToOperate == 0) && (this.TagsOperatedSuccessfully > 0))
            {
                return new ResponseEventArgs(base.Command);
            }

            return new ResponseEventArgs(base.Command,
                new CommandError(ErrorCode.TimedOut,
                    string.Format(CultureInfo.CurrentCulture,
                    LlrpResources.CommandCouldNotCompleteOnDesiredTags,
                    new object[] { this.TagsOperatedSuccessfully, (this.TagsToOperate == 0) ? LlrpResources.Infinite : this.TagsToOperate.ToString(CultureInfo.CurrentCulture), "LlrpManagementGroup.SynchronousCommandInventoryDurationKey", "LLRP Management" }), ErrorCode.TimedOut.Description, null));



        }

        private void ExecuteTagRelatedSynchronousCommand()
        {
            int num;
            this.m_commandError = null;
            lock (base.DeviceState)
            {
                num = (int)base.DeviceState.ProviderMaintainedProperties[LlrpManagementGroup.SynchronousCommandInventoryDurationKey];
            }
            bool flag = false;
            bool flag2 = false;
            try
            {
                base.Device.MessageReceivedEvent += new EventHandler<LlrpMessageEventArgs>(this.Device_MessageReceivedEvent);
                if ((this.m_accessSpec == null) || base.AddAndEnableAccessSpec(this.m_accessSpec, out this.m_commandError))
                {
                    if (this.m_accessSpec != null)
                    {
                        base.Logger.Info("Access Spec used for the command {0} on device {1} is {2}", new object[] { this.m_commandName, base.Device.DeviceName, this.m_accessSpec.ToString() });
                        flag = true;
                    }
                    base.Logger.Info("RO Spec used for the command {0} on device {1} is {2}", new object[] { this.m_commandName, base.Device.DeviceName, this.m_roSpec.ToString() });

                    if (this.m_roSpec != null && base.AddAndEnableROSpec(this.m_roSpec, out this.m_commandError))
                    {
                        flag2 = true;
                        this.m_waitEvent.WaitOne(num, false);
                    }
                }
            }
            finally
            {
                base.Device.MessageReceivedEvent -= new EventHandler<LlrpMessageEventArgs>(this.Device_MessageReceivedEvent);
                if (flag2)
                {
                    base.DeleteROSpec(this.m_roSpec);
                }
                if (flag)
                {
                    base.DeleteAccessSpec(this.m_accessSpec);
                }
            }
        }

        private AccessSpec GetAccessSpec()
        {
            Collection<OPSpec> oPSpec = this.GetOPSpec();
            if (oPSpec == null)
            {
                return null;
            }
            byte[] passCode = (base.Command as TagCommand).GetPassCode();
            base.Validatecode(passCode);
            byte[] targetTagId = this.TargetTagId;
            byte[] mask = null;
            int num = 0;
            if (targetTagId != null)
            {
                num = targetTagId.Length * 8;
                mask = Util.GetInitialisedByteArray(targetTagId.Length);
            }
            return new AccessSpec(IdGenerator.GenerateAccessSpecIdForProvider(),
                base.GetAntennaId(base.SourceName),
                AirProtocolId.EpcClass1Gen2,
                this.m_roSpec.Id,
                this.GetAccessSpecStopTrigger(),
                new AccessCommand(new C1G2TagSpec(new C1G2TargetTag(C1G2MemoryBank.Epc, true, 0x20, (ushort)num, mask, targetTagId), null), oPSpec, null),
                new AccessReportSpec(AccessReportTrigger.OnEndOfAccessSpec), null);
        }

        protected virtual AccessSpecStopTrigger GetAccessSpecStopTrigger()
        {
            return new AccessSpecStopTrigger(AccessSpecStopTriggerType.OperationCount, this.m_desiredOperationCount);
        }

        protected abstract Collection<OPSpec> GetOPSpec();
        protected virtual ROSpec GetROSpec()
        {
            return this.CreateROSpecForSynchronousCommand();
        }

        protected void IncrementSuccessfulCount()
        {
            this.m_actualTagsOperated = (ushort)(this.m_actualTagsOperated + 1);
            if ((this.TagsToOperate != 0) && (this.TagsOperatedSuccessfully == this.TagsToOperate))
            {
                base.Logger.Info("Successfully acted on {0} tags for device {1}. Thus indicating the completion of the command {2}", new object[] { this.TagsOperatedSuccessfully, base.Device.DeviceName, this.m_commandName });
                this.IndicateCommandCompletion();
            }
        }

        protected void IndicateCommandCompletion()
        {
            this.CommandCompletion = true;
            this.WaitEvent.Set();
        }

        private bool IsSingleTagCommand()
        {
            Type item = base.Command.GetType();
            Collection<Type> collection = new Collection<Type>();
            collection.Add(typeof(ReadTagCommand));
            collection.Add(typeof(ReadTagDataCommand));
            return collection.Contains(item);
        }

        private void PopulateOperationCount()
        {
            lock (base.DeviceState)
            {
                //this.m_desiredOperationCount = 1;
                this.m_desiredOperationCount = (ushort)base.DeviceState.ProviderMaintainedProperties[LlrpManagementGroup.SynchronousCommandInventoryOperationCountKey];
            }
        }

        private void Print(TagReportData tagReport)
        {
            if (tagReport.EPC96 != null)
            {
                base.Logger.Info("Tag Id (EPC96) is {0}", new object[] { HexHelper.HexEncode(tagReport.EPC96.GetData()) });
            }
            else if (tagReport.EpcData != null)
            {
                base.Logger.Info("Tag Id (EPC Data) is {0}", new object[] { HexHelper.HexEncode(tagReport.EpcData.GetData()) });
            }
        }

        protected bool CommandCompletion
        {
            set
            {
                this.m_commandCompletedSuccessfully = value;
            }
        }

        protected CommandError CommandError
        {
            set
            {
                this.m_commandError = value;
            }
        }

        internal override bool IsConcurrentToInventoryOperation
        {
            get
            {
                return false;
            }
        }

        protected ROSpec ROSpec
        {
            get
            {
                return this.m_roSpec;
            }
        }

        protected ushort TagsOperatedSuccessfully
        {
            get
            {
                return this.m_actualTagsOperated;
            }
        }

        protected ushort TagsToOperate
        {
            get
            {
                return this.m_desiredOperationCount;
            }
        }

        protected byte[] TargetTagId
        {
            get
            {
                return this.m_targetTagId;
            }
            set
            {
                this.m_targetTagId = value;
            }
        }

        protected AutoResetEvent WaitEvent
        {
            get
            {
                return this.m_waitEvent;
            }
        }
    }
}
