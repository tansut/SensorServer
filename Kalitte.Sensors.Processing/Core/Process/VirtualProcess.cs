using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;

namespace Kalitte.Sensors.Processing.Core.Process
{
    internal class VirtualProcess : VirtualRunnable<ProcessorEntity, ProcessMarshall>
    {
        private ProcessorManager manager;
        private ProcessPipeNotificationHandler notification;
        public List<Processor2ModuleBindingEntity> Relations { get; private set;}

        public VirtualProcess(ProcessorManager manager, ProcessorEntity entity, INotificationErrorHandler ErrorHandler, List<Processor2ModuleBindingEntity> relations)
            : base(entity, ErrorHandler, manager)
        {
            this.manager = manager;
            this.Relations = relations;
            EventHandler<ProcessPipeNotificationEventArgs> notificationCallback = new EventHandler<ProcessPipeNotificationEventArgs>(this.OnProcessNotification);
            this.notification = new ProcessPipeNotificationHandler(notificationCallback);
            CreateMarshallObject();
        }

        protected override void StartDone(ServerAnalyse.Context.AnalyseContext context)
        {
            manager.WatchManager.ProcessorStart(Entity.Name, new StartEventArgs(context));
            base.StartDone(context);
        }

        protected override void StopDone(ServerAnalyse.Context.AnalyseContext context)
        {
            manager.WatchManager.ProcessorStop(Entity.Name, new StopEventArgs(context));
            base.StopDone(context);
        }

        protected override void NotificationDone(string source, SensorEventBase evt, ServerAnalyse.Context.AnalyseContext context)
        {
            manager.WatchManager.ProcessorEventPipeExit(Entity.Name, new EventPipeExitEventArgs(source, evt, context));
            base.NotificationDone(source, evt, context);
        }


        protected override ProcessMarshall CreateMarshall()
        {
            var marshal = base.CreateMarshall();
            marshal.ProcessPipeNotificationEvent += new EventHandler<ProcessPipeNotificationEventArgs>(this.notification.NotificationEvent);
            return marshal;
        }

        public virtual void OnProcessNotification(object sender, ProcessPipeNotificationEventArgs e)
        {
            this.manager.NotifyDispatcher(Entity, e.ProcessPipeEvent.Key, e.ProcessPipeEvent.Value);
        }

        protected override object[] GetConstructorParamsOfMarshall()
        {
            var relatedModules = this.manager.ServerManager.EventModuleManager.GetModulesOfRelation(Relations);
            var initItems = new Dictionary<string, object>();
            return new object[] { Entity, relatedModules, Relations, initItems, ServerConfiguration.Current };
        }

        protected override string DomainFriendlyName
        {
            get { return string.Format("Processor{0}Domain", Entity.Name); }
        }

        public override void OnModuleNotify(object sender, ModuleNotifyEventArgs e)
        {
            base.OnModuleNotify(sender, e);
            manager.WatchManager.ProcessorModuleNotify(Entity.Name, e);
        }

        public override void OnModuleStart(object sender, ModuleStartEventArgs e)
        {
            base.OnModuleStart(sender, e);
            manager.WatchManager.ProcessorModuleStart(Entity.Name, e);
        }

        public override void OnModuleStop(object sender, ModuleStopEventArgs e)
        {
            base.OnModuleStop(sender, e);
            manager.WatchManager.ProcessorModuleStop(Entity.Name, e);
        }

        public override void OnModuleSetProperty(object sender, SetPropertyEventArgs e)
        {
            base.OnModuleSetProperty(sender, e);
            manager.HandlePropertyChangeFromModule(sender, e);
        }
    }
}
