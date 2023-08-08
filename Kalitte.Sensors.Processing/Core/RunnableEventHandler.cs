using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Processing.ServerAnalyse;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Core
{
    [Serializable]
    public class ShortLogEventArgs: EventArgs
    {
        //public ShortLogItem LogItem { get; private set; }
        //public ShortLogEventArgs(ShortLogItem item)
        //{
        //    this.LogItem = item;
        //}
    }

    [Serializable]
    public class ExceptionEventArgs : EventArgs
    {
        public System.Exception Exception { get; private set; }

        public ExceptionEventArgs(System.Exception exc)
        {
            this.Exception = exc;
        }
    }

    [Serializable]
    public class SetPropertyEventArgs : EventArgs
    {
        public EntityProperty Property { get; set; }
        public string Module { get; set; }
        public string ProcessorName { get; set; }

        public SetPropertyEventArgs(string processor, string module, EntityProperty property)
        {
            this.Property = property;
            this.Module = module;
            this.ProcessorName = processor;
        }
    }

    public class RunnableEventHandler: MarshalBase
    {
        public EventHandler<ExceptionEventArgs> onException;
        public EventHandler<ShortLogEventArgs> onShortLog;
        
        public EventHandler<ModuleStartEventArgs> onModuleStart;
        public EventHandler<ModuleNotifyEventArgs> onModuleNotify;
        public EventHandler<ModuleStopEventArgs> onModuleStop;
        public EventHandler<SetPropertyEventArgs> onSetModuleProperty;




        public RunnableEventHandler(EventHandler<ExceptionEventArgs> onException, 
            EventHandler<ShortLogEventArgs> onShortLog, 
            EventHandler<ModuleStartEventArgs> onModuleStart,  
            EventHandler<ModuleNotifyEventArgs> onModuleNotify, 
            EventHandler<ModuleStopEventArgs> onModuleStop,
            EventHandler<SetPropertyEventArgs> onSetModuleProperty)
            : base()
        {
            this.onException = onException;
            this.onShortLog = onShortLog;
            this.onModuleNotify = onModuleNotify;
            this.onModuleStart = onModuleStart;
            this.onModuleStop = onModuleStop;
            this.onSetModuleProperty = onSetModuleProperty;
        }

        public void ShortLogEvent(object sender, ShortLogEventArgs e)
        {
            this.onShortLog(sender, e);
        }

        public void ExceptionEvent(object sender, ExceptionEventArgs e)
        {
            this.onException(sender, e);
        }

        public void ModuleNotifyEvent(object sender, ModuleNotifyEventArgs e)
        {
            this.onModuleNotify(sender, e);
        }

        public void ModuleStartEvent(object sender, ModuleStartEventArgs e)
        {
            this.onModuleStart(sender, e);
        }

        public void ModuleStopEvent(object sender, ModuleStopEventArgs e)
        {
            this.onModuleStop(sender, e);
        }

        public void SetModulePropertyEvent(object sender, SetPropertyEventArgs e)
        {
            this.onSetModuleProperty(sender, e);
        }
    }
}
