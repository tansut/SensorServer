using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using System.Threading;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Interfaces;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Security;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.ServerAnalyse;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;
using Kalitte.Sensors.Processing.ServerAnalyse.Context;


namespace Kalitte.Sensors.Processing.Core.Process
{
    public class ProcessMarshall : RunnableMarshall<ProcessorEntity, EventModuleEntity, Processor2ModuleBindingEntity,
        VirtualProcessorModule>, IProcessorDispatcher
    {
        private List<EventModuleEntity> modules;
        ProcessorContext context;
        ModulePipe pipe = null;
        public event EventHandler<ProcessPipeNotificationEventArgs> ProcessPipeNotificationEvent;
        List<Processor2ModuleBindingEntity> relations;
        public delegate KeyValuePair<string, SensorEventBase> NotifyHandler(string source, SensorEventBase evt, object sourceModule);
        public NotifyHandler notifyHandler;

        public ProcessMarshall(ProcessorEntity entity, List<EventModuleEntity> modules, List<Processor2ModuleBindingEntity> relations, Dictionary<string, object> initItems, ServerConfiguration configuration)
            : base(entity, initItems, configuration)
        {
            this.modules = modules;
            this.relations = relations;
        }

        protected override VirtualProcessorModule CreateVirtualModuleInstance(EventModuleEntity module, Processor2ModuleBindingEntity relation)
        {
            return new VirtualProcessorModule(this.Entity, module, relation);
        }

        protected override void DoModuleSetPropertyHandler(object sender, Kalitte.Sensors.Processing.Metadata.Processor2ModuleBindingEntity relation, Kalitte.Sensors.Configuration.EntityProperty property)
        {
            DoModuleSetProperty(this, new SetPropertyEventArgs(relation.Processor, relation.Name, property));
        }

        protected override void InitializeModules()
        {
            string relationName = string.Empty;
            try
            {
                foreach (var relation in relations)
                {
                    relationName = relation.Name;
                    var moduleEntity = modules.Single(p => p.Name == relation.Module);
                    VirtualProcessorModule module = CreateVirtualModule(moduleEntity, relation);
                    module.InitContext(context);
                }
                pipe = new ModulePipe(this, Modules.GetCopiedList().ToArray());
                if (WatchModuleNotification)
                    notifyHandler = pipe.NotifyWithWatch;
                else notifyHandler = pipe.Notify;
            }
            catch (System.Exception exc)
            {
                throw CreateException("Error initializing module.", exc, relationName);
            }
        }

        protected override void Notify(object state)
        {
            var sensorEventInfo = (SensorEventInfo)state;
            var notificationResult = notifyHandler(sensorEventInfo.Source, sensorEventInfo.Event, sensorEventInfo.SourceModule);
            if (notificationResult.Value != null && this.ProcessPipeNotificationEvent != null)
                ProcessPipeNotificationEvent(this, new ProcessPipeNotificationEventArgs(notificationResult));
        }

        protected override void InitializeContext(ServerConfiguration configuration)
        {
            base.InitializeContext(configuration);
            context = new ServerProcessorContext(null, AppContext.Logger, this);
            WatchModuleNotification = GetWatchLevel(ServerAnalyseItem.EventModule) != ServerAnalyseLevel.None;

        }

        protected override Exceptions.SensorException CreateException(string message, System.Exception exc, string relatedModule)
        {
            if (exc == null)
                return new ProcessorException(message, relatedModule);
            if (!(exc is ProcessorException))
                return new ProcessorException(exc.Message, exc, "", relatedModule);
            else return (ProcessorException)exc;
        }

        public override void Notify(string source, SensorEventBase sensorEvent)
        {
            SensorEventInfo eventInfo = new SensorEventInfo(source, sensorEvent);
            QueNotification(eventInfo);
        }

        public override ProcessingItem ItemType
        {
            get { return ProcessingItem.Processor; }
        }

        internal void NotifyPipeBasedOnModule(object sender, string source, SensorEventBase sensorEvent)
        {
            SensorEventInfo eventInfo = new SensorEventInfo(source, sensorEvent, sender);
            Thread t = new Thread(new ParameterizedThreadStart(this.Notification));
            t.Priority = ThreadPriority.Highest;
            t.Start(eventInfo);
        }

        public void AddEventToNextPipe(object sender, string source, SensorEventBase sensorEvent)
        {
            NotifyPipeBasedOnModule(sender, source, sensorEvent);
        }

        protected override TimeSpan StartModule(VirtualProcessorModule module)
        {
            var watchContext = ServerAnalyseManager.CreateContext<DurationAnalyseContext>();
            base.StartModule(module);
            DoModuleStart(this, new ModuleStartEventArgs(module.Entity.Name, watchContext));
            return watchContext.Duration;
        }

        protected override TimeSpan StopModule(VirtualProcessorModule module)
        {
            var watchContext = ServerAnalyseManager.CreateContext<DurationAnalyseContext>();
            base.StopModule(module);
            DoModuleStop(this, new ModuleStopEventArgs(module.Entity.Name, watchContext));
            return watchContext.Duration;
        }

        internal void NotifyImmediate(string source, SensorEventBase sensorEvent)
        {
            NotifyPipeBasedOnModule(null, source, sensorEvent);
        }




    }
}
