using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events;
using System.Reflection;
using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Core.Process
{
    public class PipeInfo
    {
        public MethodInfo MethodToCall { get; private set;}
        public bool HasSourceParam { get; private set; }
        public bool ExactMatch { get; private set; }

        public PipeInfo(MethodInfo methodInfo, bool hasSourceParam, bool exactMatch)
        {
            this.MethodToCall = methodInfo;
            this.HasSourceParam = hasSourceParam;
            this.ExactMatch = exactMatch;
        }

    }

    internal class NotificationPipe
    {
        private object module;
        private int TIMEOUT = ServerConfiguration.Current.MethodCallTimeout;

        private NonExistEventHandlerBehavior eventHandlerBehavior;
        private PipeNullEventBehavior nullEventBehavior;
        public Dictionary<Type, PipeInfo> pipeList;

        private bool canCastTo(Type source, Type destination)
        {
            return destination.IsAssignableFrom(source);
        }

        private void validateCast(Type source, Type destination)
        {
            if (!canCastTo(source, destination))
                throw new InvalidOperationException(string.Format("Cannot cast {0} to {1}", source.FullName, destination.FullName));
        }

        private void ValidateParameter(ParameterInfo parameter, Type type)
        {
            if (!canCastTo(parameter.ParameterType, type))
                throw new InvalidOperationException(string.Format("Parameter {0} of {1} should be castable to SensorEventBase", parameter.Name, parameter.Member.Name));
        }

        private void ValidateAndAddMethod(MethodInfo method, SensorEventHandlerAttribute attribute)
        {
            Type eventType;
            var parameters = method.GetParameters();
            if (parameters.Length == 1)
            {
                ValidateParameter(parameters[0], typeof(SensorEventBase));
                eventType = parameters[0].ParameterType;
            }
            else if (parameters.Length == 2)
            {
                ValidateParameter(parameters[0], typeof(string));
                ValidateParameter(parameters[1], typeof(SensorEventBase));
                eventType = parameters[1].ParameterType;
            }
            else throw new InvalidOperationException(string.Format("Method {0} has invalid signature", method.Name));
            validateCast(method.ReturnType, typeof(SensorEventBase));

            if (pipeList.ContainsKey(eventType))
                throw new InvalidOperationException(string.Format("Pipe has already same event. Method {0}.", method.Name));
            else
            {
                PipeInfo pipeLine = new PipeInfo(method, parameters.Length > 1, attribute.ExactTypeMatch);
                pipeList.Add(eventType, pipeLine);
            }
        }

        public NotificationPipe(object module, NonExistEventHandlerBehavior eventHandlerBehavior, PipeNullEventBehavior nullEventBehavior)
        {
            this.module = module;
            this.eventHandlerBehavior = eventHandlerBehavior;
            this.nullEventBehavior = nullEventBehavior;
            pipeList = new Dictionary<Type, PipeInfo>();
            MethodInfo[] methodInfo = module.GetType().GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var item in methodInfo)
            {
                object[] attributes = item.GetCustomAttributes(typeof(SensorEventHandlerAttribute), true);
                if (attributes.Length > 0)
                {
                    ValidateAndAddMethod(item, (SensorEventHandlerAttribute)attributes[0]);
                }
            }
        }

        private SensorEventBase Call(PipeInfo pipe, string source, SensorEventBase sensorEvent)
        {
            object[] parameterValues;

            if (pipe.HasSourceParam)
                parameterValues = new object[] { source, sensorEvent };
            else
                parameterValues = new object[] { sensorEvent };

            return RunHelper.Execute(module,  pipe.MethodToCall, TIMEOUT, parameterValues) as SensorEventBase;
        }

        public SensorEventBase Notify(string source, SensorEventBase sensorEvent, out PipeInfo usedPipe)
        {
            Type incomingEventType = sensorEvent.GetType();
            SensorEventBase result = null;

            if (pipeList.TryGetValue(incomingEventType, out usedPipe))
            {
                result = Call(usedPipe, source, sensorEvent);
            }
            else
            {
                foreach (var item in pipeList)
                {
                    if (canCastTo(incomingEventType, item.Key))
                    {
                        if (!item.Value.ExactMatch)
                        {
                            usedPipe = item.Value;
                            break;
                        }
                    }
                }

                if (usedPipe != null)
                {
                    result = Call(usedPipe, source, sensorEvent);
                }
                else
                {
                    switch (eventHandlerBehavior)
                    {
                        case NonExistEventHandlerBehavior.PassOriginalEventToNextModule:
                            {
                                result = sensorEvent;
                                break;
                            }
                        case NonExistEventHandlerBehavior.IgnoreEvent:
                            {
                                result = null;
                                break;
                            }
                        case NonExistEventHandlerBehavior.RaiseException:
                            {
                                throw new InvalidOperationException(string.Format("Unable to find handler for event {0}", sensorEvent.GetType()));
                            }
                    }

                } 
            }

            return result;
        }
    }
}
