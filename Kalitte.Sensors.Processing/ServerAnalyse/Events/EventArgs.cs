using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Processing.ServerAnalyse.Context;

namespace Kalitte.Sensors.Processing.ServerAnalyse.Events
{
    [Serializable]
    public abstract class AnalyseEventArgs: EventArgs
    {
        public Dictionary<Type, AnalyseContext> ContextObjects { get; private set; }

        protected AnalyseEventArgs(Dictionary<Type, AnalyseContext> contextObjects)
        {
            if (contextObjects == null)
                this.ContextObjects = new Dictionary<Type, AnalyseContext>();
            else
                this.ContextObjects = contextObjects;
        }

        protected AnalyseEventArgs(AnalyseContext contextObject)
        {
            this.ContextObjects = new Dictionary<Type, AnalyseContext>();
            if (contextObject != null)
            {
                contextObject.Done();
                ContextObjects.Add(contextObject.GetType(), contextObject);
            }
        }

        public T GetContext<T>() where T : AnalyseContext
        {
            AnalyseContext context;
            if (ContextObjects.TryGetValue(typeof(T), out context))
                return (T)context;
            else return null;
        }
    }

    [Serializable]
    public class SensorEventArgs : AnalyseEventArgs
    {
        public string ProviderName { get; private set; }
        public SensorEventBase Event { get; private set; }

        public SensorEventArgs(string providerName, SensorEventBase evt): this(providerName, evt, null)
        {
        }
        public SensorEventArgs(string providerName, SensorEventBase evt, AnalyseContext context): base(context)
        {
            this.ProviderName = providerName;
            this.Event = evt;
        }

    }

    [Serializable]
    public class LogicalSensorEventArgs : AnalyseEventArgs
    {
        public SensorEventBase Event { get; private set; }

        public LogicalSensorEventArgs(SensorEventBase evt): this(evt, null)
        {
        }

        public LogicalSensorEventArgs(SensorEventBase evt, AnalyseContext context): base(context)
        {
            this.Event = evt;
        }
    }

    [Serializable]
    public class QueSendEventArgs : AnalyseEventArgs
    {
        public string Source { get; private set; }
        public SensorEventBase Event { get; private set; }

        public QueSendEventArgs(string source, SensorEventBase evt): this(source, evt, null)
        {
        }

        public QueSendEventArgs(string source, SensorEventBase evt, AnalyseContext context): base(context)
        {
            this.Source = source;
            this.Event = evt;
        }
    }

    [Serializable]
    public class QueReadEventArgs : AnalyseEventArgs
    {
        public string Source { get; private set; }
        public SensorEventBase Event { get; private set; }

        public QueReadEventArgs(string source, SensorEventBase evt): this(source, evt, null)
        {

        }

        public QueReadEventArgs(string source, SensorEventBase evt, AnalyseContext contextObject)
            : base(contextObject)
        {
            this.Source = source;
            this.Event = evt;
        }
    }

    [Serializable]
    public class StartEventArgs : AnalyseEventArgs
    {
        public StartEventArgs(): this(null)
        {

        }

        public StartEventArgs(AnalyseContext contextObject)
            : base(contextObject)
        {

        }
    }

    [Serializable]
    public class ModuleStartEventArgs : AnalyseEventArgs
    {
        public string ModuleName { get; private set; }

        public ModuleStartEventArgs(string moduleName): this(moduleName, null)
        {
        }

        public ModuleStartEventArgs(string moduleName, AnalyseContext contextObject)
            : base(contextObject)
        {
            this.ModuleName = moduleName;
        }
    }

    [Serializable]
    public class ModuleNotifyEventArgs : AnalyseEventArgs
    {
        public string ModuleName { get; private set; }
        public string Method { get; private set; }
        public SensorEventBase Event { get; private set; }
        public SensorEventBase ReturnEvent { get; private set; }


        public ModuleNotifyEventArgs(string module, string method, SensorEventBase evt, SensorEventBase returnEvent) : 
            this(module, method, evt, returnEvent, null)
        {
        }

        public ModuleNotifyEventArgs(string module, string method, SensorEventBase evt,SensorEventBase returnEvent, AnalyseContext contextObject)
            : base(contextObject)
        {
            this.ModuleName = module;
            this.Method = method;
            this.Event = evt;
        }
    }

    [Serializable]
    public class EventPipeExitEventArgs : AnalyseEventArgs
    {
        public string Source { get; private set; }
        public SensorEventBase Event { get; private set; }

        public EventPipeExitEventArgs(string source, SensorEventBase evt): this(source, evt, null)
        {

        }

        public EventPipeExitEventArgs(string source, SensorEventBase evt, AnalyseContext contextObject)
            : base(contextObject)
        {
            this.Source = source;
            this.Event = evt;
        }
    }

    [Serializable]
    public class StopEventArgs : AnalyseEventArgs
    {
        public StopEventArgs():this(null)
        {

        }

        public StopEventArgs(AnalyseContext contextObject): base(contextObject)
        {

        }
    }

    [Serializable]
    public class ModuleStopEventArgs : AnalyseEventArgs
    {
        public string ModuleName { get; private set; }

        public ModuleStopEventArgs(string moduleName): this(moduleName, null)
        {
        }

        public ModuleStopEventArgs(string moduleName, AnalyseContext contextObject)
            : base(contextObject)
        {
            this.ModuleName = moduleName;
        }
    }



}
