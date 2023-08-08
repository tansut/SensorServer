using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.ServerAnalyse;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;
using Kalitte.Sensors.Processing.ServerAnalyse.Context;

namespace Kalitte.Sensors.Processing.Core.Process
{
    internal class ModulePipe
    {
        private VirtualProcessorModule[] modules;
        ProcessMarshall marshal;


        public ModulePipe(ProcessMarshall marshal, VirtualProcessorModule[] modules)
        {
            this.modules = modules;
            this.marshal = marshal;
        }

        private int getModuleIndex(object source)
        {
            if (source != null)
            {
                int result = 0;
                foreach (var module in modules)
                {
                    result++;
                    if (module.InstanceIs(source))
                        break;
                }
                return result;
            }
            else return 0;
        }

        public KeyValuePair<string, SensorEventBase> Notify(string source, SensorEventBase evt, object sourceModule)
        {
            int start = getModuleIndex(sourceModule);
            SensorEventBase moduleResult = evt;

            for (int i = start; i < modules.Length; i++)
            {
                var module = modules[i];
                PipeInfo usedPipe;
                moduleResult = module.Notify(source, moduleResult, out usedPipe);
                moduleResult = handleNullEvent(module, moduleResult, evt);
                if (moduleResult == null)
                    break;
            }
            return new KeyValuePair<string, SensorEventBase>(source, moduleResult);
        }

        private SensorEventBase handleNullEvent(VirtualProcessorModule module, SensorEventBase moduleResult, SensorEventBase originalEvent)
        {
            if (moduleResult == null && module.NullSensorEventBehavior == PipeNullEventBehavior.PassOriginalEventToNextModule)
                return originalEvent;
            else return moduleResult;
        }

        public KeyValuePair<string, SensorEventBase> NotifyWithWatch(string source, SensorEventBase evt, object sourceModule)
        {
            int start = getModuleIndex(sourceModule);
            SensorEventBase moduleResult = evt;

            for (int i = start; i < modules.Length; i++)
            {
                var module = modules[i];
                PipeInfo usedPipe;
                var context = ServerAnalyseManager.CreateContext<DurationAnalyseContext>();
                moduleResult = module.Notify(source, moduleResult, out usedPipe);
                moduleResult = handleNullEvent(module, moduleResult, evt);
                if (usedPipe != null)
                    marshal.DoNotification(this, new ModuleNotifyEventArgs(module.Entity.Name, usedPipe.MethodToCall.Name, evt, moduleResult, context));
                if (moduleResult == null)
                    break;
            }
            return new KeyValuePair<string, SensorEventBase>(source, moduleResult);
        }

    }
}
