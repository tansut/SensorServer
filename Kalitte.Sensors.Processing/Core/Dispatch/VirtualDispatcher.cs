using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Processing.Core.Dispatch
{
    internal class VirtualDispatcher : VirtualRunnable<DispatcherEntity, DispatchMarshal>
    {
        private DispatcherManager manager;

        public VirtualDispatcher(DispatcherManager manager, DispatcherEntity entity, INotificationErrorHandler ErrorHandler)
            : base(entity, ErrorHandler, manager)
        {
            this.manager = manager;
            CreateMarshallObject();
        }

        protected override object[] GetConstructorParamsOfMarshall()
        {
            var initItems = new Dictionary<string, object>();
            return new object[] { Entity, initItems, ServerConfiguration.Current };
        }
        

        protected override string DomainFriendlyName
        {
            get { return string.Format("Dispatcher{0}Domain", Entity.Name); }
        }

        protected override void StartDone(ServerAnalyse.Context.AnalyseContext context)
        {
            manager.WatchManager.DispatcherStart(Entity.Name, new StartEventArgs(context));
            base.StartDone(context);
        }

        protected override void StopDone(ServerAnalyse.Context.AnalyseContext context)
        {
            manager.WatchManager.DispatcherStop(Entity.Name, new StopEventArgs(context));
            base.StopDone(context);
        }

        protected override void NotificationDone(string source, SensorEventBase evt, ServerAnalyse.Context.AnalyseContext context)
        {
            manager.WatchManager.DispatcherEventPipeExit(Entity.Name, new EventPipeExitEventArgs(source, evt, context));
            base.NotificationDone(source, evt, context);
        }

    }
}
