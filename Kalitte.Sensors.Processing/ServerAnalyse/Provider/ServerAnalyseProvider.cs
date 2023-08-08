using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;
using System.Configuration.Provider;

namespace Kalitte.Sensors.Processing.ServerAnalyse.Provider
{
    public abstract class ServerAnalyseProvider : ProviderBase
    {
        protected ServerAnalyseConfiguration CurrentConfiguration { get; private set; }

        protected volatile ServerAnalyseLevel SensorLevel;
        protected volatile ServerAnalyseLevel SensorProviderLevel;
        protected volatile ServerAnalyseLevel LogicalSensorLevel;
        protected volatile ServerAnalyseLevel ProcessorLevel;
        protected volatile ServerAnalyseLevel EventModuleLevel;
        protected volatile ServerAnalyseLevel DispatcherLevel;
        protected volatile ServerAnalyseLevel EventTypeLevel;
        protected volatile ServerAnalyseLevel ProcessorQueLevel;
        protected volatile ServerAnalyseLevel DispatcherQueLevel;

        public abstract void SensorEvent(string name, SensorEventArgs e);
        public abstract void LogicalSensorEvent(string name, LogicalSensorEventArgs e);
        public abstract void ProcessorMessageQueSend(string name, QueSendEventArgs e);
        public abstract void ProcessorMessageQueRead(string name, QueReadEventArgs e);

        public abstract void ProcessorStart(string name, StartEventArgs e);
        public abstract void ProcessorModuleStart(string name, ModuleStartEventArgs e);
        public abstract void ProcessorModuleNotify(string name, ModuleNotifyEventArgs e);
        public abstract void ProcessorEventPipeExit(string name, EventPipeExitEventArgs e);
        public abstract void ProcessorModuleStop(string name, ModuleStopEventArgs e);
        public abstract void ProcessorStop(string name, StopEventArgs e);

        public abstract void DispatcherStart(string name, StartEventArgs e);
        public abstract void DispatcherStop(string name, StopEventArgs e);
        public abstract void DispatcherEventPipeExit(string name, EventPipeExitEventArgs e);
        public abstract void DispatcherMessageQueSend(string name, QueSendEventArgs e);
        public abstract void DispatcherMessageQueRead(string name, QueReadEventArgs e);

        

        public virtual void ChangeConfiguration(ServerAnalyseConfiguration configuration)
        {
            this.CurrentConfiguration = configuration;
            SensorLevel = configuration.GetLevel(ServerAnalyseItem.SensorDevice);
            SensorProviderLevel = configuration.GetLevel(ServerAnalyseItem.SensorProvider);
            LogicalSensorLevel = configuration.GetLevel(ServerAnalyseItem.LogicalSensor);
            ProcessorLevel = configuration.GetLevel(ServerAnalyseItem.Processor);
            EventModuleLevel = configuration.GetLevel(ServerAnalyseItem.EventModule);
            DispatcherLevel = configuration.GetLevel(ServerAnalyseItem.Dispatcher);
            EventTypeLevel = configuration.GetLevel(ServerAnalyseItem.EventType);
            ProcessorQueLevel = configuration.GetLevel(ServerAnalyseItem.ProcessorQue);
            DispatcherQueLevel = configuration.GetLevel(ServerAnalyseItem.DispatcherQue);
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
        }

        public virtual void Startup(ServerAnalyseConfiguration configuration)
        {
            ChangeConfiguration(configuration);
        }

        public abstract NameDescriptionList GetCategoryNames(ServerAnalyseItem related);
        public abstract NameDescriptionList GetCategories();
        public abstract NameDescriptionList GetInstanceNames(string category);
        public abstract NameDescriptionList GetMeasureNames(string category);
        public abstract float[] GetMeasureValues(string category, string instance, string[] measureNames);

        public virtual void Shutdown()
        {

        }
    }
}
