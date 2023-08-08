using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Exceptions;

namespace Kalitte.Sensors.Processing.Core.Dispatch
{
    public class DispatchMarshal : RunnableMarshall<DispatcherEntity, DispatcherEntity, DispatcherEntity, VirtualDispatcherModule>
    {
        DispatcherContext context;
        VirtualDispatcherModule module;

        public DispatchMarshal(DispatcherEntity entity, Dictionary<string, object> initItems, ServerConfiguration configuration)
            : base(entity, initItems, configuration)
        {

        }

        protected override VirtualDispatcherModule CreateVirtualModuleInstance(DispatcherEntity module, DispatcherEntity relation)
        {
            return new VirtualDispatcherModule(Entity);
        }

        protected override void DoModuleSetPropertyHandler(object sender, DispatcherEntity relation, Kalitte.Sensors.Configuration.EntityProperty property)
        {
            DoModuleSetProperty(this, new SetPropertyEventArgs(relation.Name, relation.Name, property));
        }

        public override ProcessingItem ItemType
        {
            get { return ProcessingItem.Dispatcher; }
        }

        protected override void Notify(object state)
        {
            KeyValuePair<string, Events.SensorEventBase> eventInfo = (KeyValuePair<string, Events.SensorEventBase>)state;
            module.Notify(eventInfo.Key, eventInfo.Value);
                
            
        }

        protected override Exceptions.SensorException CreateException(string message, Exception exc, string relatedModule)
        {
            return new DispatcherException(message, exc);
        }

        public override void Notify(string source, Events.SensorEventBase evt)
        {
            var eventInfo = new KeyValuePair<string, Events.SensorEventBase>(source, evt);
            QueNotification(eventInfo);
        }

        protected override void InitializeContext(ServerConfiguration configuration)
        {
            base.InitializeContext(configuration);
            context = new ServerDispatcherContext(null, AppContext.Logger, this);
            WatchModuleNotification = false;

        }

        protected override void InitializeModules()
        {
            module = CreateVirtualModule(Entity, Entity);
            module.InitContext(context);


        }
    }
}
