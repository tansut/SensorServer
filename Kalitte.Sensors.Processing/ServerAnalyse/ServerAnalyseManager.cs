using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Utilities;
using System.Threading;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Processing.Core;
using System.Diagnostics;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.ServerAnalyse.Context;
using Kalitte.Sensors.Processing.ServerAnalyse.Windows;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;
using System.Configuration;
using Kalitte.Sensors.Processing.ServerAnalyse.Provider;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.ServerAnalyse
{
    public class ServerAnalyseManager : ServerAnalyseProvider
    {
        public ILogger Logger;
        private static Collection<ServerAnalyseProvider> providers = new Collection<ServerAnalyseProvider>();

        public const int SendTimeout = 60000;

        internal ServerAnalyseProvider ValidateAndGet(string name)
        {
            var item = providers.SingleOrDefault(p => p.Name == name);
            if (item == null)
                throw new ArgumentException("Invalid watch name");
            return item;
        }

        public static T CreateContext<T>() where T : AnalyseContext
        {
            return Activator.CreateInstance<T>();
        }


        private void sendEvent(string methodName, params object[] parameters)
        {
            foreach (ServerAnalyseProvider watcher in providers)
            {
                ThreadPool.QueueUserWorkItem(
                state =>
                {
                    try
                    {
                        RunHelper.Execute(watcher, methodName, SendTimeout, parameters);
                    }
                    catch (Exception exc)
                    {

                        if (Logger != null)
                            Logger.Error("Error in {0} on {1}. {2}", methodName, watcher.GetType().FullName, exc);
                    }
                }
                , null);
            }
        }

        public override void SensorEvent(string name, SensorEventArgs e)
        {
            if (SensorLevel > ServerAnalyseLevel.None)
                sendEvent("SensorEvent", name, e);
        }

        public override void LogicalSensorEvent(string name, LogicalSensorEventArgs e)
        {
            if (LogicalSensorLevel > ServerAnalyseLevel.None)
                sendEvent("LogicalSensorEvent", name, e);
        }

        public override void ProcessorStart(string name, StartEventArgs e)
        {
            if (ProcessorLevel > ServerAnalyseLevel.None)
                sendEvent("ProcessorStart", name, e);
        }

        public override void ProcessorModuleStart(string name, ModuleStartEventArgs e)
        {
            if (EventModuleLevel > ServerAnalyseLevel.None)
                sendEvent("ProcessorModuleStart", name, e);
        }

        public override void ProcessorModuleStop(string name, ModuleStopEventArgs e)
        {
            if (EventModuleLevel > ServerAnalyseLevel.None)
                sendEvent("ProcessorModuleStop", name, e);
        }

        public override void ProcessorStop(string name, StopEventArgs e)
        {
            if (ProcessorLevel > ServerAnalyseLevel.None)
                sendEvent("ProcessorStop", name, e);
        }

        public override void ProcessorMessageQueSend(string name, QueSendEventArgs e)
        {
            if (ProcessorQueLevel > ServerAnalyseLevel.None)
                sendEvent("ProcessorMessageQueSend", name, e);

        }

        public override void ProcessorMessageQueRead(string name, QueReadEventArgs e)
        {
            if (ProcessorQueLevel > ServerAnalyseLevel.None)
                sendEvent("ProcessorMessageQueRead", name, e);
        }

        public override void ProcessorModuleNotify(string name, ModuleNotifyEventArgs e)
        {
            if (EventModuleLevel > ServerAnalyseLevel.None)
                sendEvent("ProcessorModuleNotify", name, e);
        }

        public override void ProcessorEventPipeExit(string name, EventPipeExitEventArgs e)
        {
            if (ProcessorLevel > ServerAnalyseLevel.None)
                sendEvent("ProcessorEventPipeExit", name, e);

        }

        public override void DispatcherStart(string name, StartEventArgs e)
        {
            if (DispatcherLevel > ServerAnalyseLevel.None)
                sendEvent("DispatcherStart", name, e);
        }

        public override void DispatcherStop(string name, StopEventArgs e)
        {
            if (DispatcherLevel > ServerAnalyseLevel.None)
                sendEvent("DispatcherStop", name, e);
        }

        public override void DispatcherEventPipeExit(string name, EventPipeExitEventArgs e)
        {
            if (ProcessorLevel > ServerAnalyseLevel.None)
                sendEvent("DispatcherEventPipeExit", name, e);
        }

        public override void DispatcherMessageQueSend(string name, QueSendEventArgs e)
        {
            if (DispatcherQueLevel > ServerAnalyseLevel.None)
                sendEvent("DispatcherMessageQueSend", name, e);
        }

        public override void DispatcherMessageQueRead(string name, QueReadEventArgs e)
        {
            if (DispatcherQueLevel > ServerAnalyseLevel.None)
                sendEvent("DispatcherMessageQueRead", name, e);
        }

        public void Init()
        {
            SensorServerConfigurationSection section =
                        ConfigurationManager.GetSection("KalitteSensorServer") as SensorServerConfigurationSection;
            foreach (ProviderSettings settings in section.AnalyseProviders)
            {
                Type c = Type.GetType(settings.Type, true, true);
                if (!typeof(ServerAnalyseProvider).IsAssignableFrom(c))
                {
                    throw new ArgumentException("Must be ServerWatchProvider");
                }
                ServerAnalyseProvider p = (ServerAnalyseProvider)Activator.CreateInstance(c);
                NameValueCollection parameters = settings.Parameters;
                NameValueCollection config = new NameValueCollection(parameters.Count, StringComparer.Ordinal);
                foreach (string str2 in parameters)
                {
                    config[str2] = parameters[str2];
                }
                p.Initialize(settings.Name, config);
                providers.Add(p);
            }
        }

        public void Startup(ILogger iLogger, ServerAnalyseConfiguration configuration)
        {
            base.Startup(configuration);
            this.Logger = iLogger;
            Init();
            foreach (var item in providers)
            {
                (item as ServerAnalyseProvider).Startup(configuration);
            }
        }

        public override void Shutdown()
        {
            foreach (var item in providers)
            {
                (item as ServerAnalyseProvider).Shutdown();
            }
            base.Shutdown();
        }

        internal  NameDescriptionList GetProviderNames()
        {
            var result = new NameDescriptionList();
            foreach (var item in providers)
            {
                result.Add(new NameDescription(item.Name, item.Description));
            }
            return result;
        }

        public override NameDescriptionList GetCategoryNames(ServerAnalyseItem related)
        {
            throw new NotImplementedException();
        }

        public override NameDescriptionList GetCategories()
        {
            throw new NotImplementedException();
        }

        public override NameDescriptionList GetInstanceNames(string category)
        {
            throw new NotImplementedException();
        }

        public override NameDescriptionList GetMeasureNames(string category)
        {
            throw new NotImplementedException();
        }

        public override float[] GetMeasureValues(string category, string instance, string[] measureNames)
        {
            throw new NotImplementedException();
        }


    }
}
