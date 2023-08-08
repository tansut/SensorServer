namespace Kalitte.Sensors.Rfid.Llrp.Commands
{
    using Kalitte.Sensors.Rfid;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    
    
    using System;
    using System.Collections.ObjectModel;
    using Kalitte.Sensors.Rfid.Commands;
    using Kalitte.Sensors.Rfid.Llrp.Core;
    using Kalitte.Sensors.Utilities;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Rfid.Events;
    using Kalitte.Sensors.Rfid.Llrp.Events;
    using Kalitte.Sensors.Rfid.Llrp.PhysicalDevices;
    using Kalitte.Sensors.Rfid.Core;
    using Kalitte.Sensors.Commands;

    internal sealed class QueryTagsCommandHandler : AccessSpecCommandHandler
    {
        private TagDataSelector m_selector;
        private Collection<TagReadEvent> m_tags;

        internal QueryTagsCommandHandler(string sourcName, SensorCommand command, PDPState state, LlrpDevice device, ILogger logger) : base(sourcName, command, state, device, logger)
        {
            this.m_tags = new Collection<TagReadEvent>();
            QueryTagsCommand command2 = (QueryTagsCommand) base.Command;
            this.m_selector = command2.DataSelector;
        }

        protected override void Device_MessageReceivedEvent(Collection<TagReportData> datas)
        {
        }

        protected override void Device_MessageReceivedEvent(object sender, LlrpMessageEventArgs args)
        {
            if (args.IsSuccess)
            {
                base.Logger.Info("Received response {0} in the Get Tags event handler for device {1}", new object[] { args.Response, base.Device.DeviceName });
                if (args.Response is ROAccessReport)
                {
                    ROAccessReport response = (ROAccessReport) args.Response;
                    Collection<TagReadEvent> collection = null;
                    lock (base.DeviceState)
                    {
                        collection = Util.GetTags(base.DeviceState, response, base.ROSpec, base.SourceName, this.m_selector, base.Logger);
                    }
                    lock (this.m_tags)
                    {
                        foreach (TagReadEvent event2 in collection)
                        {
                            this.m_tags.Add(event2);
                        }
                        QueryTagsCommand command = (QueryTagsCommand) base.Command;
                        command.Response = new QueryTagsResponse(this.m_tags);
                    }
                }
            }
        }

        internal override ResponseEventArgs ExecuteCommand()
        {
            base.ValidateTagDatatSelectorForSyncCommand(this.m_selector);
            base.CommandCompletion = true;
            QueryTagsCommand command = (QueryTagsCommand) base.Command;
            command.Response = new QueryTagsResponse(new Collection<TagReadEvent>());
            return base.ExecuteCommand();
        }

        protected override Collection<OPSpec> GetOPSpec()
        {
            //Collection<OPSpec> collection = new Collection<OPSpec>();
            //GetTagsCommand cmd = (GetTagsCommand)base.Command;
            //C1G2Read item = new Core.C1G2Read(base.GetCode(cmd.GetPassCode()), MemoryBank.User, 0, 0);
            //collection.Add(item);
            //return collection;
            return null;
        }

        protected override ROSpec GetROSpec()
        {
            return base.CreateROSpecForSynchronousCommand(this.m_selector);
        }
    }
}
