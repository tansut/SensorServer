using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;
using Kalitte.Sensors.Processing.ServerAnalyse.Context;
using Kalitte.Sensors.Processing.ServerAnalyse.Provider;
using System.Threading;

namespace Kalitte.Sensors.Processing.ServerAnalyse.Windows
{
    public class WindowsPerformanceAnalyser : ServerAnalyseProvider
    {
        public const string SensorDeviceCategory = "KalitteSensorServer: Sensor Devices";
        public const string SensorDeviceNumberOfEvents = "Event Count";
        public const string SensorDeviceEventsSec = "Events/Sec";

        public const string SensorDeviceProviderCategory = "KalitteSensorServer: Sensor Providers";
        public const string SensorDeviceProviderNumberOfEvents = "Event Count";
        public const string SensorDeviceProviderEventsSec = "Events/Sec";

        public const string LogicalSensorDeviceCategory = "KalitteLogicalSensorServer: LogicalSensor Devices";
        public const string LogicalSensorDeviceNumberOfEvents = "Event Count";
        public const string LogicalSensorDeviceEventsSec = "Events/Sec";

        public const string ProcessorMessageQueCategory = "KalitteSensorServer: ProcessorMessageQue";
        public const string ProcessorMessageQueReadSec = "Read/Sec";
        public const string ProcessorMessageQueSendSec = "Send/Sec";
        public const string ProcessorMessageQueReadAvgTime = "Avg/Read Time";
        public const string ProcessorMessageQueReadAvgTimeBase = "Avg/Read Time Base";
        public const string ProcessorMessageQueSendAvgTime = "Avg/Send Time";
        public const string ProcessorMessageQueSendAvgTimeBase = "Avg/Send Time Base";
        public const string ProcessorMessageQueReadCount = "Read Count";
        public const string ProcessorMessageQueSendCount = "Send Count";

        public const string DispatcherMessageQueCategory = "KalitteSensorServer: DispatcherMessageQue";
        public const string DispatcherMessageQueReadSec = "Read/Sec";
        public const string DispatcherMessageQueSendSec = "Send/Sec";
        public const string DispatcherMessageQueReadAvgTime = "Avg/Read Time";
        public const string DispatcherMessageQueReadAvgTimeBase = "Avg/Read Time Base";
        public const string DispatcherMessageQueSendAvgTime = "Avg/Send Time";
        public const string DispatcherMessageQueSendAvgTimeBase = "Avg/Send Time Base";
        public const string DispatcherMessageQueReadCount = "Read Count";
        public const string DispatcherMessageQueSendCount = "Send Count";

        public const string ProcessorCategory = "KalitteSensorServer: Processors";
        public const string ProcessorStartCount = "Start Count";
        public const string ProcessorAvgStartTime = "Avg/Start Time";
        public const string ProcessorAvgStartTimeBase = "Avg/Start Time Base";

        public const string ProcessorStopCount = "Stop Count";
        public const string ProcessorAvgStopTime = "Avg/Stop Time";
        public const string ProcessorAvgStopTimeBase = "Avg/Stop Time Base";

        public const string ProcessorNotificationCount = "Notification Count";
        public const string ProcessorNotificationSec = "Notification/Sec";
        public const string ProcessorAvgNotificationTime = "Avg/Notification Time";
        public const string ProcessorAvgNotificationTimeBase = "Avg/Notification Time Base";

        public const string EventModuleCategory = "KalitteSensorServer: Event Modules";
        public const string EventModuleStartCount = "Start Count";
        public const string EventModuleAvgStartTime = "Avg/Start Time";
        public const string EventModuleAvgStartTimeBase = "Avg/Start Time Base";

        public const string EventModuleStopCount = "Stop Count";
        public const string EventModuleAvgStopTime = "Avg/Stop Time";
        public const string EventModuleAvgStopTimeBase = "Avg/Stop Time Base";

        public const string EventModuleNotificationCount = "Notification Count";
        public const string EventModuleNotificationSec = "Notification/Sec";
        public const string EventModuleAvgNotificationTime = "Avg/Notification Time";
        public const string EventModuleAvgNotificationTimeBase = "Avg/Notification Time Base";

        public const string EventModuleInstanceCategory = "KalitteSensorServer: Event Module Instances";
        public const string EventModuleInstanceStartCount = "Start Count";
        public const string EventModuleInstanceAvgStartTime = "Avg/Start Time";
        public const string EventModuleInstanceAvgStartTimeBase = "Avg/Start Time Base";

        public const string EventModuleInstanceStopCount = "Stop Count";
        public const string EventModuleInstanceAvgStopTime = "Avg/Stop Time";
        public const string EventModuleInstanceAvgStopTimeBase = "Avg/Stop Time Base";

        public const string EventModuleInstanceNotificationCount = "Notification Count";
        public const string EventModuleInstanceNotificationSec = "Notification/Sec";
        public const string EventModuleInstanceAvgNotificationTime = "Avg/Notification Time";
        public const string EventModuleInstanceAvgNotificationTimeBase = "Avg/Notification Time Base";

        public const string EventModuleMethodCategory = "KalitteSensorServer: Event Module Methods";

        public const string EventModuleMethodNotificationCount = "Notification Count";
        public const string EventModuleMethodNotificationSec = "Notification/Sec";
        public const string EventModuleMethodAvgNotificationTime = "Avg/Notification Time";
        public const string EventModuleMethodAvgNotificationTimeBase = "Avg/Notification Time Base";

        public const string EventTypeCategory = "KalitteSensorServer: Event Types";

        public const string EventTypeSensorNotificationCount = "Sensor Notification Count";
        public const string EventTypeSensorNotificationSec = "Sensor Notification/Sec";

        public const string EventTypeLogicalSensorNotificationCount = "Logical Sensor Notification Count";
        public const string EventTypeLogicalSensorNotificationSec = "Logical Sensor Notification/Sec";

        public const string EventTypeProcessorNotificationCount = "Processor Notification Count";
        public const string EventTypeProcessorNotificationSec = "Processor Notification/Sec";

        public const string EventTypeModuleNotificationCount = "Module Notification Count";
        public const string EventTypeModuleNotificationSec = "Module Notification/Sec";

        public const string EventTypeModuleAvgNotificationTime = "Avg/Module Notification Time";
        public const string EventTypeModuleAvgNotificationTimeBase = "Avg/Module Notification Time Base";

        public const string EventTypeProcessorAvgNotificationTime = "Avg/Processor Notification Time";
        public const string EventTypeProcessorAvgNotificationTimeBase = "Avg/Processor Notification Time Base";

        public const string EventTypeDispatcherNotificationCount = "Dispatcher Notification Count";
        public const string EventTypeDispatcherNotificationSec = "Dispatcher Notification/Sec";
        public const string EventTypeDispatcherAvgNotificationTime = "Avg/Dispatcher Notification Time";
        public const string EventTypeDispatcherAvgNotificationTimeBase = "Avg/Dispatcher Notification Time Base";

        public const string SystemCategory = "KalitteSensorServer: Misc";
        public const string ThreadPoolSize = "Size/Thread Pool";
        public const string ThreadPoolAvailableSize = "Available/Thread Pool";
        public const string ThreadPoolAvailableRatio = "Thread Pool Available %";
        public const string ThreadPoolAvailableRatioBase = "Thread Pool Available Base %";

        public const string DispatcherCategory = "KalitteSensorServer: Dispatchers";

        public const string DispatcherStartCount = "Start Count";
        public const string DispatcherAvgStartTime = "Avg/Start Time";
        public const string DispatcherAvgStartTimeBase = "Avg/Start Time Base";

        public const string DispatcherStopCount = "Stop Count";
        public const string DispatcherAvgStopTime = "Avg/Stop Time";
        public const string DispatcherAvgStopTimeBase = "Avg/Stop Time Base";

        public const string DispatcherNotificationCount = "Notification Count";
        public const string DispatcherNotificationSec = "Notification/Sec";
        public const string DispatcherAvgNotificationTime = "Avg/Notification Time";
        public const string DispatcherAvgNotificationTimeBase = "Avg/Notification Time Base";

        public const string TotalInstance = "_Total";


        private volatile bool isShuttingDown;
        private AutoResetEvent waitShutdown;
        private Thread reportCustomCounterDataThread;

        private const PerformanceCounterType timeCounterType = PerformanceCounterType.AverageTimer32;
        private const PerformanceCounterType timeCounterTypeBase = PerformanceCounterType.AverageBase;

        private Dictionary<ServerAnalyseItem, string[]> relatedCategories = new Dictionary<ServerAnalyseItem, string[]>();

        private object counterLock = new object();
        Dictionary<string, PerformanceCounter> activeCounterInstances = new Dictionary<string, PerformanceCounter>();

        PerformanceCounter retreiveCounter(string category, string measure, string instance, bool isReadonly = false)
        {
            PerformanceCounter counter = null;
            string key = string.Format("{0}{1}{2}", category, measure, instance);
            if (activeCounterInstances.TryGetValue(key, out counter))
                return counter;
            lock (counterLock)
            {
                if (activeCounterInstances.TryGetValue(key, out counter))
                    return counter;
                counter = new PerformanceCounter(category, measure, instance, isReadonly);
                if (counter.CounterType == PerformanceCounterType.RawFraction ||
                    counter.CounterType == PerformanceCounterType.RawBase)
                    counter.RawValue = 0;
                activeCounterInstances.Add(key, counter);
            }
            return counter;
        }

        private long getTickValue(DurationAnalyseContext context)
        {
            return context.Ticks;
        }

        private PerformanceCounter createEventTypeCounter(string name, SensorEventBase sensorEvent)
        {
            var eventType = sensorEvent.GetType();
            var counter = retreiveCounter(EventTypeCategory, name, eventType.Name, false);
            return counter;
        }

        private void incrementEventType(string name, SensorEventBase sensorEvent, ServerAnalyseLevel level, long delta = 1)
        {
            if (EventTypeLevel >= level)
            {
                var counter = createEventTypeCounter(name, sensorEvent);
                counter.IncrementByQuick(delta);

            }
        }

        public override void SensorEvent(string name, SensorEventArgs e)
        {
            var countsec = retreiveCounter(SensorDeviceCategory, SensorDeviceEventsSec, name, false);
            countsec.IncrementQuick();

            if (SensorProviderLevel > ServerAnalyseLevel.None)
            {
                var countProviderSec = retreiveCounter(SensorDeviceProviderCategory, SensorDeviceProviderEventsSec, e.ProviderName, false);
                countProviderSec.IncrementQuick();
            }

            var countsecTotal = retreiveCounter(SensorDeviceCategory, SensorDeviceEventsSec, TotalInstance, false);
            countsecTotal.IncrementQuick();

            if (SensorLevel > ServerAnalyseLevel.Basic)
            {
                var count = retreiveCounter(SensorDeviceCategory, SensorDeviceNumberOfEvents, name, false);
                count.IncrementQuick();

                var countTotal = retreiveCounter(SensorDeviceCategory, SensorDeviceNumberOfEvents, TotalInstance, false);
                countTotal.IncrementQuick();
            }

            if (SensorProviderLevel > ServerAnalyseLevel.Basic)
            {
                var countProvider = retreiveCounter(SensorDeviceProviderCategory, SensorDeviceProviderNumberOfEvents, e.ProviderName, false);
                countProvider.IncrementQuick();

                var countProviderTotal = retreiveCounter(SensorDeviceProviderCategory, SensorDeviceProviderNumberOfEvents, TotalInstance, false);
                countProviderTotal.IncrementQuick();

            }

            incrementEventType(EventTypeSensorNotificationCount, e.Event, ServerAnalyseLevel.Basic);
            incrementEventType(EventTypeSensorNotificationSec, e.Event, ServerAnalyseLevel.Detailed);
        }

        public override void LogicalSensorEvent(string name, LogicalSensorEventArgs e)
        {
            var countsec = retreiveCounter(LogicalSensorDeviceCategory, LogicalSensorDeviceEventsSec, name, false);
            countsec.IncrementQuick();

            var countsecTotal = retreiveCounter(LogicalSensorDeviceCategory, LogicalSensorDeviceEventsSec, TotalInstance, false);
            countsecTotal.IncrementQuick();

            if (LogicalSensorLevel > ServerAnalyseLevel.Basic)
            {
                var count = retreiveCounter(LogicalSensorDeviceCategory, LogicalSensorDeviceNumberOfEvents, name, false);
                count.IncrementQuick();

                var countTotal = retreiveCounter(LogicalSensorDeviceCategory, LogicalSensorDeviceNumberOfEvents, TotalInstance, false);
                countTotal.IncrementQuick();

            }

            incrementEventType(EventTypeLogicalSensorNotificationCount, e.Event, ServerAnalyseLevel.Basic);
            incrementEventType(EventTypeLogicalSensorNotificationSec, e.Event, ServerAnalyseLevel.Detailed);
        }

        public override void ProcessorMessageQueSend(string name, QueSendEventArgs e)
        {
            var readsec = retreiveCounter(ProcessorMessageQueCategory, ProcessorMessageQueSendSec, name, false);
            readsec.IncrementQuick();

            if (ProcessorQueLevel > ServerAnalyseLevel.Basic)
            {
                var readcount = retreiveCounter(ProcessorMessageQueCategory, ProcessorMessageQueSendCount, name, false);
                var time = retreiveCounter(ProcessorMessageQueCategory, ProcessorMessageQueSendAvgTime, name, false);
                var timeBase = retreiveCounter(ProcessorMessageQueCategory, ProcessorMessageQueSendAvgTimeBase, name, false);
                readcount.IncrementQuick();
                time.IncrementByQuick(getTickValue(e.GetContext<DurationAnalyseContext>()));
                timeBase.IncrementQuick();
            }
        }

        public override void ProcessorMessageQueRead(string name, QueReadEventArgs e)
        {
            var readsec = retreiveCounter(ProcessorMessageQueCategory, ProcessorMessageQueReadSec, name, false);
            readsec.IncrementQuick();

            if (ProcessorQueLevel > ServerAnalyseLevel.Basic)
            {
                var time = retreiveCounter(ProcessorMessageQueCategory, ProcessorMessageQueReadAvgTime, name, false);
                var timeBase = retreiveCounter(ProcessorMessageQueCategory, ProcessorMessageQueReadAvgTimeBase, name, false);
                var sendcount = retreiveCounter(ProcessorMessageQueCategory, ProcessorMessageQueReadCount, name, false);

                time.IncrementByQuick(getTickValue(e.GetContext<DurationAnalyseContext>()));
                timeBase.IncrementQuick();
                sendcount.IncrementQuick();
            }
        }

        public override void ProcessorStart(string name, StartEventArgs e)
        {
            var count = retreiveCounter(ProcessorCategory, ProcessorStartCount, name, false);
            count.IncrementQuick();

            if (ProcessorLevel > ServerAnalyseLevel.Basic)
            {
                var time = retreiveCounter(ProcessorCategory, ProcessorAvgStartTime, name, false);
                var timeBase = retreiveCounter(ProcessorCategory, ProcessorAvgStartTimeBase, name, false);
                time.IncrementByQuick(getTickValue(e.GetContext<DurationAnalyseContext>()));
                timeBase.IncrementQuick();
            }
        }

        public override void ProcessorModuleStart(string name, ModuleStartEventArgs e)
        {
            var count = retreiveCounter(EventModuleCategory, EventModuleStartCount, e.ModuleName, false);
            count.IncrementQuick();
            var countInstance = retreiveCounter(EventModuleInstanceCategory, EventModuleInstanceStartCount, string.Format("{0}_{1}", name, e.ModuleName), false);
            countInstance.IncrementQuick();

            if (EventModuleLevel > ServerAnalyseLevel.Basic)
            {
                var time = retreiveCounter(EventModuleCategory, EventModuleAvgStartTime, e.ModuleName, false);
                var timeBase = retreiveCounter(EventModuleCategory, EventModuleAvgStartTimeBase, e.ModuleName, false);

                var timeInstance = retreiveCounter(EventModuleInstanceCategory, EventModuleInstanceAvgStartTime, string.Format("{0}_{1}", name, e.ModuleName), false);
                var timeBaseInstance = retreiveCounter(EventModuleInstanceCategory, EventModuleInstanceAvgStartTimeBase, string.Format("{0}_{1}", name, e.ModuleName), false);

                long ticks = getTickValue(e.GetContext<DurationAnalyseContext>());
                time.IncrementByQuick(ticks);
                timeBase.IncrementByQuick(1);

                timeInstance.IncrementByQuick(ticks);
                timeBaseInstance.IncrementQuick();
            }
        }

        public override void ProcessorModuleNotify(string name, ModuleNotifyEventArgs e)
        {
            long ticks = getTickValue(e.GetContext<DurationAnalyseContext>());

            var time = retreiveCounter(EventModuleCategory, EventModuleAvgNotificationTime, e.ModuleName, false);
            var timeBase = retreiveCounter(EventModuleCategory, EventModuleAvgNotificationTimeBase, e.ModuleName, false);

            var timeInstance = retreiveCounter(EventModuleInstanceCategory, EventModuleInstanceAvgNotificationTime, string.Format("{0}_{1}", name, e.ModuleName), false);
            var timeBaseInstance = retreiveCounter(EventModuleInstanceCategory, EventModuleInstanceAvgNotificationTimeBase, string.Format("{0}_{1}", name, e.ModuleName), false);

            time.IncrementByQuick(ticks);
            timeBase.IncrementQuick();

            timeInstance.IncrementByQuick(ticks);
            timeBaseInstance.IncrementQuick();

            if (EventModuleLevel > ServerAnalyseLevel.Basic)
            {
                var count = retreiveCounter(EventModuleCategory, EventModuleNotificationCount, e.ModuleName, false);
                var countInstance = retreiveCounter(EventModuleInstanceCategory, EventModuleInstanceNotificationCount, string.Format("{0}_{1}", name, e.ModuleName), false);
                var countMethod = retreiveCounter(EventModuleMethodCategory, EventModuleMethodNotificationCount, string.Format("{0}_{1}", e.ModuleName, e.Method), false);

                count.IncrementQuick();
                countInstance.IncrementQuick();
                countMethod.IncrementQuick();

                var countsec = retreiveCounter(EventModuleCategory, EventModuleNotificationSec, e.ModuleName, false);
                var countsecInstance = retreiveCounter(EventModuleInstanceCategory, EventModuleInstanceNotificationSec, string.Format("{0}_{1}", name, e.ModuleName), false);

                var countsecMethod = retreiveCounter(EventModuleMethodCategory, EventModuleMethodNotificationSec, string.Format("{0}_{1}", e.ModuleName, e.Method), false);
                var timeMethod = retreiveCounter(EventModuleMethodCategory, EventModuleMethodAvgNotificationTime, string.Format("{0}_{1}", e.ModuleName, e.Method), false);
                var timeBaseMethod = retreiveCounter(EventModuleMethodCategory, EventModuleMethodAvgNotificationTimeBase, string.Format("{0}_{1}", e.ModuleName, e.Method), false);

                countsec.IncrementQuick();

                countsecInstance.IncrementQuick();


                countsecMethod.IncrementQuick();
                timeMethod.IncrementByQuick(ticks);
                timeBaseMethod.IncrementQuick();
            }

            incrementEventType(EventTypeModuleNotificationCount, e.Event, ServerAnalyseLevel.Basic);
            incrementEventType(EventTypeModuleNotificationSec, e.Event, ServerAnalyseLevel.Detailed);
            incrementEventType(EventTypeModuleAvgNotificationTime, e.Event, ServerAnalyseLevel.Detailed, ticks);
            incrementEventType(EventTypeModuleAvgNotificationTimeBase, e.Event, ServerAnalyseLevel.Detailed);

        }

        public override void ProcessorEventPipeExit(string name, EventPipeExitEventArgs e)
        {
            long ticks = getTickValue(e.GetContext<DurationAnalyseContext>());

            var time = retreiveCounter(ProcessorCategory, ProcessorAvgNotificationTime, name, false);
            var timeBase = retreiveCounter(ProcessorCategory, ProcessorAvgNotificationTimeBase, name, false);

            time.IncrementByQuick(ticks);
            timeBase.IncrementQuick();

            var timeTotal = retreiveCounter(ProcessorCategory, ProcessorAvgNotificationTime, TotalInstance, false);
            var timeBaseTotal = retreiveCounter(ProcessorCategory, ProcessorAvgNotificationTimeBase, TotalInstance, false);

            timeTotal.IncrementByQuick(ticks);
            timeBaseTotal.IncrementQuick();

            if (ProcessorLevel > ServerAnalyseLevel.Basic)
            {
                var countsec = retreiveCounter(ProcessorCategory, ProcessorNotificationSec, name, false);
                var count = retreiveCounter(ProcessorCategory, ProcessorNotificationCount, name, false);
                count.IncrementQuick();
                var countTotal = retreiveCounter(ProcessorCategory, ProcessorNotificationCount, TotalInstance, false);
                countTotal.IncrementQuick();

                countsec.IncrementQuick();

                var countsecTotal = retreiveCounter(ProcessorCategory, ProcessorNotificationSec, TotalInstance, false);
                countsecTotal.IncrementQuick();
            }

            incrementEventType(EventTypeProcessorNotificationCount, e.Event, ServerAnalyseLevel.Basic);
            incrementEventType(EventTypeProcessorNotificationSec, e.Event, ServerAnalyseLevel.Detailed);
            incrementEventType(EventTypeProcessorAvgNotificationTime, e.Event, ServerAnalyseLevel.Detailed, ticks);
            incrementEventType(EventTypeProcessorAvgNotificationTimeBase, e.Event, ServerAnalyseLevel.Detailed);
        }

        public override void ProcessorModuleStop(string name, ModuleStopEventArgs e)
        {
            var count = retreiveCounter(EventModuleCategory, EventModuleStopCount, e.ModuleName, false);
            var countInstance = retreiveCounter(EventModuleInstanceCategory, EventModuleInstanceStopCount, string.Format("{0}_{1}", name, e.ModuleName), false);
            count.IncrementQuick();
            countInstance.IncrementQuick();

            if (EventModuleLevel > ServerAnalyseLevel.Basic)
            {
                var time = retreiveCounter(EventModuleCategory, EventModuleAvgStopTime, e.ModuleName, false);
                var timeBase = retreiveCounter(EventModuleCategory, EventModuleAvgStopTimeBase, e.ModuleName, false);

                var timeInstance = retreiveCounter(EventModuleInstanceCategory, EventModuleInstanceAvgStopTime, string.Format("{0}_{1}", name, e.ModuleName), false);
                var timeBaseInstance = retreiveCounter(EventModuleInstanceCategory, EventModuleInstanceAvgStopTimeBase, string.Format("{0}_{1}", name, e.ModuleName), false);

                long ticks = getTickValue(e.GetContext<DurationAnalyseContext>());

                time.IncrementByQuick(ticks);
                timeBase.IncrementQuick();

                timeInstance.IncrementByQuick(ticks);
                timeBaseInstance.IncrementQuick();
            }
        }

        public override void ProcessorStop(string name, StopEventArgs e)
        {
            var count = retreiveCounter(ProcessorCategory, ProcessorStopCount, name, false);
            count.IncrementQuick();

            if (ProcessorLevel > ServerAnalyseLevel.Basic)
            {

                var time = retreiveCounter(ProcessorCategory, ProcessorAvgStopTime, name, false);
                var timeBase = retreiveCounter(ProcessorCategory, ProcessorAvgStopTimeBase, name, false);

                time.IncrementByQuick(getTickValue(e.GetContext<DurationAnalyseContext>()));
                timeBase.IncrementQuick();
            }
        }

        private void createSensorCounters()
        {


            CounterCreationDataCollection sensorCounters = new CounterCreationDataCollection();
            CounterCreationData totalSensorDeviceEvents = new CounterCreationData(SensorDeviceNumberOfEvents, "", PerformanceCounterType.NumberOfItems64);
            CounterCreationData sensorDeviceEventsSec = new CounterCreationData(SensorDeviceEventsSec, "", PerformanceCounterType.RateOfCountsPerSecond64);
            sensorCounters.Add(totalSensorDeviceEvents);
            sensorCounters.Add(sensorDeviceEventsSec);
            PerformanceCounterCategory.Create(SensorDeviceCategory, "", PerformanceCounterCategoryType.MultiInstance, sensorCounters);
        }

        private void createDispatcherCounters()
        {



            CounterCreationDataCollection counters = new CounterCreationDataCollection();
            var startCount = new CounterCreationData(DispatcherStartCount, "", PerformanceCounterType.NumberOfItems64);
            var startTime = new CounterCreationData(DispatcherAvgStartTime, "", timeCounterType);
            var startTimeBase = new CounterCreationData(DispatcherAvgStartTimeBase, "", timeCounterTypeBase);

            var stopCount = new CounterCreationData(DispatcherStopCount, "", PerformanceCounterType.NumberOfItems64);
            var stopTime = new CounterCreationData(DispatcherAvgStopTime, "", timeCounterType);
            var stopTimeBase = new CounterCreationData(DispatcherAvgStopTimeBase, "", timeCounterTypeBase);

            var notificationCount = new CounterCreationData(DispatcherNotificationCount, "", PerformanceCounterType.NumberOfItems64);
            var notificationTime = new CounterCreationData(DispatcherAvgNotificationTime, "", timeCounterType);
            var notificationTimeBase = new CounterCreationData(DispatcherAvgNotificationTimeBase, "", timeCounterTypeBase);
            var notificationSec = new CounterCreationData(DispatcherNotificationSec, "", PerformanceCounterType.RateOfCountsPerSecond64);

            counters.Add(startCount);
            counters.Add(startTime);
            counters.Add(startTimeBase);
            counters.Add(stopCount);
            counters.Add(stopTime);
            counters.Add(stopTimeBase);
            counters.Add(notificationCount);
            counters.Add(notificationTime);
            counters.Add(notificationTimeBase);
            counters.Add(notificationSec);

            PerformanceCounterCategory.Create(DispatcherCategory, "", PerformanceCounterCategoryType.MultiInstance, counters);
        }

        private void createsensorProviderCounters()
        {
            var sensorProviderCounters = new CounterCreationDataCollection();
            var count = new CounterCreationData(SensorDeviceProviderNumberOfEvents, "", PerformanceCounterType.NumberOfItems64);
            var countSec = new CounterCreationData(SensorDeviceProviderEventsSec, "", PerformanceCounterType.RateOfCountsPerSecond64);
            sensorProviderCounters.Add(count);
            sensorProviderCounters.Add(countSec);
            PerformanceCounterCategory.Create(SensorDeviceProviderCategory, "", PerformanceCounterCategoryType.MultiInstance, sensorProviderCounters);
        }

        private void createLogicalSensorCounters()
        {


            CounterCreationDataCollection sensorCounters = new CounterCreationDataCollection();
            CounterCreationData totalLogicalSensorDeviceEvents = new CounterCreationData(LogicalSensorDeviceNumberOfEvents, "", PerformanceCounterType.NumberOfItems64);
            CounterCreationData sensorDeviceEventsSec = new CounterCreationData(LogicalSensorDeviceEventsSec, "", PerformanceCounterType.RateOfCountsPerSecond64);
            sensorCounters.Add(totalLogicalSensorDeviceEvents);
            sensorCounters.Add(sensorDeviceEventsSec);
            PerformanceCounterCategory.Create(LogicalSensorDeviceCategory, "", PerformanceCounterCategoryType.MultiInstance, sensorCounters);
        }

        private void createDispatcherMqCounters()
        {


            CounterCreationDataCollection dispatcherMessageQueCounters = new CounterCreationDataCollection();
            CounterCreationData dispatcherMessageQueReadEventsSec = new CounterCreationData(DispatcherMessageQueReadSec, "", PerformanceCounterType.RateOfCountsPerSecond64);
            CounterCreationData dispatcherMessageQueSendEventsSec = new CounterCreationData(DispatcherMessageQueSendSec, "", PerformanceCounterType.RateOfCountsPerSecond64);
            CounterCreationData dispatcherMessageQueReadAvgTime = new CounterCreationData(DispatcherMessageQueReadAvgTime, "", timeCounterType);
            CounterCreationData dispatcherMessageQueReadAvgTimeBase = new CounterCreationData(DispatcherMessageQueReadAvgTimeBase, "", timeCounterTypeBase);
            CounterCreationData dispatcherMessageQueSendAvgTime = new CounterCreationData(DispatcherMessageQueSendAvgTime, "", timeCounterType);
            CounterCreationData dispatcherMessageQueSendAvgTimeBase = new CounterCreationData(DispatcherMessageQueSendAvgTimeBase, "", timeCounterTypeBase);
            CounterCreationData dispatchermqreadcount = new CounterCreationData(DispatcherMessageQueReadCount, "", PerformanceCounterType.NumberOfItems64);
            CounterCreationData dispatchermqsendcount = new CounterCreationData(DispatcherMessageQueSendCount, "", PerformanceCounterType.NumberOfItems64);

            dispatcherMessageQueCounters.Add(dispatchermqreadcount);
            dispatcherMessageQueCounters.Add(dispatcherMessageQueReadEventsSec);
            dispatcherMessageQueCounters.Add(dispatcherMessageQueReadAvgTime);
            dispatcherMessageQueCounters.Add(dispatcherMessageQueReadAvgTimeBase);

            dispatcherMessageQueCounters.Add(dispatchermqsendcount);
            dispatcherMessageQueCounters.Add(dispatcherMessageQueSendEventsSec);
            dispatcherMessageQueCounters.Add(dispatcherMessageQueSendAvgTime);
            dispatcherMessageQueCounters.Add(dispatcherMessageQueSendAvgTimeBase);

            PerformanceCounterCategory.Create(DispatcherMessageQueCategory, "", PerformanceCounterCategoryType.MultiInstance, dispatcherMessageQueCounters);
        }

        private void createProcessorMqCounters()
        {


            CounterCreationDataCollection processorMessageQueCounters = new CounterCreationDataCollection();
            CounterCreationData processorMessageQueReadEventsSec = new CounterCreationData(ProcessorMessageQueReadSec, "", PerformanceCounterType.RateOfCountsPerSecond64);
            CounterCreationData processorMessageQueSendEventsSec = new CounterCreationData(ProcessorMessageQueSendSec, "", PerformanceCounterType.RateOfCountsPerSecond64);
            CounterCreationData processorMessageQueReadAvgTime = new CounterCreationData(ProcessorMessageQueReadAvgTime, "", timeCounterType);
            CounterCreationData processorMessageQueReadAvgTimeBase = new CounterCreationData(ProcessorMessageQueReadAvgTimeBase, "", timeCounterTypeBase);
            CounterCreationData processorMessageQueSendAvgTime = new CounterCreationData(ProcessorMessageQueSendAvgTime, "", timeCounterType);
            CounterCreationData processorMessageQueSendAvgTimeBase = new CounterCreationData(ProcessorMessageQueSendAvgTimeBase, "", timeCounterTypeBase);
            CounterCreationData processormqreadcount = new CounterCreationData(ProcessorMessageQueReadCount, "", PerformanceCounterType.NumberOfItems64);
            CounterCreationData processormqsendcount = new CounterCreationData(ProcessorMessageQueSendCount, "", PerformanceCounterType.NumberOfItems64);

            processorMessageQueCounters.Add(processormqreadcount);
            processorMessageQueCounters.Add(processorMessageQueReadEventsSec);
            processorMessageQueCounters.Add(processorMessageQueReadAvgTime);
            processorMessageQueCounters.Add(processorMessageQueReadAvgTimeBase);

            processorMessageQueCounters.Add(processormqsendcount);
            processorMessageQueCounters.Add(processorMessageQueSendEventsSec);
            processorMessageQueCounters.Add(processorMessageQueSendAvgTime);
            processorMessageQueCounters.Add(processorMessageQueSendAvgTimeBase);

            PerformanceCounterCategory.Create(ProcessorMessageQueCategory, "", PerformanceCounterCategoryType.MultiInstance, processorMessageQueCounters);
        }

        private void createProcessorCounters()
        {



            CounterCreationDataCollection counters = new CounterCreationDataCollection();
            var startCount = new CounterCreationData(ProcessorStartCount, "", PerformanceCounterType.NumberOfItems64);
            var startTime = new CounterCreationData(ProcessorAvgStartTime, "", timeCounterType);
            var startTimeBase = new CounterCreationData(ProcessorAvgStartTimeBase, "", timeCounterTypeBase);

            var stopCount = new CounterCreationData(ProcessorStopCount, "", PerformanceCounterType.NumberOfItems64);
            var stopTime = new CounterCreationData(ProcessorAvgStopTime, "", timeCounterType);
            var stopTimeBase = new CounterCreationData(ProcessorAvgStopTimeBase, "", timeCounterTypeBase);

            var notificationCount = new CounterCreationData(ProcessorNotificationCount, "", PerformanceCounterType.NumberOfItems64);
            var notificationTime = new CounterCreationData(ProcessorAvgNotificationTime, "", timeCounterType);
            var notificationTimeBase = new CounterCreationData(ProcessorAvgNotificationTimeBase, "", timeCounterTypeBase);
            var notificationSec = new CounterCreationData(ProcessorNotificationSec, "", PerformanceCounterType.RateOfCountsPerSecond64);

            counters.Add(startCount);
            counters.Add(startTime);
            counters.Add(startTimeBase);
            counters.Add(stopCount);
            counters.Add(stopTime);
            counters.Add(stopTimeBase);
            counters.Add(notificationCount);
            counters.Add(notificationTime);
            counters.Add(notificationTimeBase);
            counters.Add(notificationSec);

            PerformanceCounterCategory.Create(ProcessorCategory, "", PerformanceCounterCategoryType.MultiInstance, counters);
        }

        private void createEventModuleCounters()
        {



            CounterCreationDataCollection counters = new CounterCreationDataCollection();
            var startCount = new CounterCreationData(EventModuleStartCount, "", PerformanceCounterType.NumberOfItems64);
            var startTime = new CounterCreationData(EventModuleAvgStartTime, "", timeCounterType);
            var startTimeBase = new CounterCreationData(EventModuleAvgStartTimeBase, "", timeCounterTypeBase);

            var stopCount = new CounterCreationData(EventModuleStopCount, "", PerformanceCounterType.NumberOfItems64);
            var stopTime = new CounterCreationData(EventModuleAvgStopTime, "", timeCounterType);
            var stopTimeBase = new CounterCreationData(EventModuleAvgStopTimeBase, "", timeCounterTypeBase);

            var notificationCount = new CounterCreationData(EventModuleNotificationCount, "", PerformanceCounterType.NumberOfItems64);
            var notificationTime = new CounterCreationData(EventModuleAvgNotificationTime, "", timeCounterType);
            var notificationTimeBase = new CounterCreationData(EventModuleAvgNotificationTimeBase, "", timeCounterTypeBase);
            var notificationSec = new CounterCreationData(EventModuleNotificationSec, "", PerformanceCounterType.RateOfCountsPerSecond64);

            counters.Add(startCount);
            counters.Add(startTime);
            counters.Add(startTimeBase);
            counters.Add(stopCount);
            counters.Add(stopTime);
            counters.Add(stopTimeBase);
            counters.Add(notificationCount);
            counters.Add(notificationTime);
            counters.Add(notificationTimeBase);
            counters.Add(notificationSec);

            PerformanceCounterCategory.Create(EventModuleCategory, "", PerformanceCounterCategoryType.MultiInstance, counters);
        }

        private void createEventModuleInstanceCounters()
        {


            CounterCreationDataCollection counters = new CounterCreationDataCollection();
            var startCount = new CounterCreationData(EventModuleInstanceStartCount, "", PerformanceCounterType.NumberOfItems64);
            var startTime = new CounterCreationData(EventModuleInstanceAvgStartTime, "", timeCounterType);
            var startTimeBase = new CounterCreationData(EventModuleInstanceAvgStartTimeBase, "", timeCounterTypeBase);

            var stopCount = new CounterCreationData(EventModuleInstanceStopCount, "", PerformanceCounterType.NumberOfItems64);
            var stopTime = new CounterCreationData(EventModuleInstanceAvgStopTime, "", timeCounterType);
            var stopTimeBase = new CounterCreationData(EventModuleInstanceAvgStopTimeBase, "", timeCounterTypeBase);

            var notificationCount = new CounterCreationData(EventModuleInstanceNotificationCount, "", PerformanceCounterType.NumberOfItems64);
            var notificationTime = new CounterCreationData(EventModuleInstanceAvgNotificationTime, "", timeCounterType);
            var notificationTimeBase = new CounterCreationData(EventModuleInstanceAvgNotificationTimeBase, "", timeCounterTypeBase);
            var notificationSec = new CounterCreationData(EventModuleInstanceNotificationSec, "", PerformanceCounterType.RateOfCountsPerSecond64);

            counters.Add(startCount);
            counters.Add(startTime);
            counters.Add(startTimeBase);
            counters.Add(stopCount);
            counters.Add(stopTime);
            counters.Add(stopTimeBase);
            counters.Add(notificationCount);
            counters.Add(notificationTime);
            counters.Add(notificationTimeBase);
            counters.Add(notificationSec);

            PerformanceCounterCategory.Create(EventModuleInstanceCategory, "", PerformanceCounterCategoryType.MultiInstance, counters);
        }

        private void createEventModuleMethodCounters()
        {



            CounterCreationDataCollection counters = new CounterCreationDataCollection();

            var notificationCount = new CounterCreationData(EventModuleMethodNotificationCount, "", PerformanceCounterType.NumberOfItems64);
            var notificationTime = new CounterCreationData(EventModuleMethodAvgNotificationTime, "", timeCounterType);
            var notificationTimeBase = new CounterCreationData(EventModuleMethodAvgNotificationTimeBase, "", timeCounterTypeBase);
            var notificationSec = new CounterCreationData(EventModuleMethodNotificationSec, "", PerformanceCounterType.RateOfCountsPerSecond64);

            counters.Add(notificationCount);
            counters.Add(notificationTime);
            counters.Add(notificationTimeBase);
            counters.Add(notificationSec);

            PerformanceCounterCategory.Create(EventModuleMethodCategory, "", PerformanceCounterCategoryType.MultiInstance, counters);
        }

        private void createEventTypeCounters()
        {

            CounterCreationDataCollection counters = new CounterCreationDataCollection();

            var sensornotificationCount = new CounterCreationData(EventTypeSensorNotificationCount, "", PerformanceCounterType.NumberOfItems64);
            var sensornotificationSec = new CounterCreationData(EventTypeSensorNotificationSec, "", PerformanceCounterType.RateOfCountsPerSecond64);

            var logicalnotificationCount = new CounterCreationData(EventTypeLogicalSensorNotificationCount, "", PerformanceCounterType.NumberOfItems64);
            var logicalnotificationSec = new CounterCreationData(EventTypeLogicalSensorNotificationSec, "", PerformanceCounterType.RateOfCountsPerSecond64);

            var processornotificationCount = new CounterCreationData(EventTypeProcessorNotificationCount, "", PerformanceCounterType.NumberOfItems64);
            var ProcessornotificationSec = new CounterCreationData(EventTypeProcessorNotificationSec, "", PerformanceCounterType.RateOfCountsPerSecond64);

            var dispatchernotificationCount = new CounterCreationData(EventTypeDispatcherNotificationCount, "", PerformanceCounterType.NumberOfItems64);
            var dispatchernotificationSec = new CounterCreationData(EventTypeDispatcherNotificationSec, "", PerformanceCounterType.RateOfCountsPerSecond64);
            var notificationTimeDispatcher = new CounterCreationData(EventTypeDispatcherAvgNotificationTime, "", timeCounterType);
            var notificationTimeDispatcherBase = new CounterCreationData(EventTypeDispatcherAvgNotificationTimeBase, "", timeCounterTypeBase);


            var modulenotificationCount = new CounterCreationData(EventTypeModuleNotificationCount, "", PerformanceCounterType.NumberOfItems64);
            var modulenotificationSec = new CounterCreationData(EventTypeModuleNotificationSec, "", PerformanceCounterType.RateOfCountsPerSecond64);



            var notificationTimeModule = new CounterCreationData(EventTypeModuleAvgNotificationTime, "", timeCounterType);
            var notificationTimeModuleBase = new CounterCreationData(EventTypeModuleAvgNotificationTimeBase, "", timeCounterTypeBase);
            var notificationTimeProcessor = new CounterCreationData(EventTypeProcessorAvgNotificationTime, "", timeCounterType);
            var notificationTimeProcessorBase = new CounterCreationData(EventTypeProcessorAvgNotificationTimeBase, "", timeCounterTypeBase);



            counters.Add(sensornotificationCount);
            counters.Add(sensornotificationSec);
            counters.Add(logicalnotificationCount);
            counters.Add(logicalnotificationSec);
            counters.Add(processornotificationCount);
            counters.Add(ProcessornotificationSec);
            counters.Add(modulenotificationCount);
            counters.Add(modulenotificationSec);

            counters.Add(notificationTimeModule);
            counters.Add(notificationTimeModuleBase);
            counters.Add(notificationTimeProcessor);
            counters.Add(notificationTimeProcessorBase);
            counters.Add(dispatchernotificationCount);
            counters.Add(dispatchernotificationSec);
            counters.Add(notificationTimeDispatcher);
            counters.Add(notificationTimeDispatcherBase);


            PerformanceCounterCategory.Create(EventTypeCategory, "", PerformanceCounterCategoryType.MultiInstance, counters);
        }

        public override void Shutdown()
        {
            isShuttingDown = true;
            waitShutdown.Set();
            lock (counterLock)
            {
                foreach (var item in activeCounterInstances)
                {
                    item.Value.Close();
                }
            }
            activeCounterInstances.Clear();
            base.Shutdown();
        }

        private void registerCounters()
        {
            if (!PerformanceCounterCategory.Exists(SensorDeviceCategory))
                createSensorCounters();
            if (!PerformanceCounterCategory.Exists(SensorDeviceProviderCategory))
                createsensorProviderCounters();
            if (!PerformanceCounterCategory.Exists(LogicalSensorDeviceCategory))
                createLogicalSensorCounters();
            if (!PerformanceCounterCategory.Exists(ProcessorMessageQueCategory))
                createProcessorMqCounters();
            if (!PerformanceCounterCategory.Exists(ProcessorCategory))
                createProcessorCounters();
            if (!PerformanceCounterCategory.Exists(EventModuleCategory))
                createEventModuleCounters();
            if (!PerformanceCounterCategory.Exists(EventModuleInstanceCategory))
                createEventModuleInstanceCounters();
            if (!PerformanceCounterCategory.Exists(EventModuleMethodCategory))
                createEventModuleMethodCounters();
            if (!PerformanceCounterCategory.Exists(EventTypeCategory))
                createEventTypeCounters();
            if (!PerformanceCounterCategory.Exists(SystemCategory))
                createSystemCounters();
            
            if (!PerformanceCounterCategory.Exists(DispatcherCategory))
                createDispatcherCounters();
            if (!PerformanceCounterCategory.Exists(DispatcherMessageQueCategory))
                createDispatcherMqCounters();

        }

        private void createSystemCounters()
        {


            var counters = new CounterCreationDataCollection();
            var size = new CounterCreationData(ThreadPoolSize, "", PerformanceCounterType.NumberOfItems32);
            var asize = new CounterCreationData(ThreadPoolAvailableSize, "", PerformanceCounterType.NumberOfItems32);
            var ratio = new CounterCreationData(ThreadPoolAvailableRatio, "", PerformanceCounterType.RawFraction);
            var ratioBase = new CounterCreationData(ThreadPoolAvailableRatioBase, "", PerformanceCounterType.RawBase);


            counters.Add(size);
            counters.Add(asize);
            counters.Add(ratio);
            counters.Add(ratioBase);


            PerformanceCounterCategory.Create(SystemCategory, "", PerformanceCounterCategoryType.MultiInstance, counters);
        }

        public override void Startup(ServerAnalyseConfiguration configuration)
        {
            base.Startup(configuration);
            isShuttingDown = false;
            waitShutdown = new AutoResetEvent(false);
            registerCounters();
            relatedCategories.Clear();
            relatedCategories.Add(ServerAnalyseItem.SensorDevice, new string[] { SensorDeviceCategory });
            relatedCategories.Add(ServerAnalyseItem.SensorProvider, new string[] { SensorDeviceProviderCategory });
            relatedCategories.Add(ServerAnalyseItem.ProcessorQue, new string[] { ProcessorMessageQueCategory });
            relatedCategories.Add(ServerAnalyseItem.Processor, new string[] { ProcessorCategory });
            relatedCategories.Add(ServerAnalyseItem.LogicalSensor, new string[] { LogicalSensorDeviceCategory });
            relatedCategories.Add(ServerAnalyseItem.EventType, new string[] { EventTypeCategory });
            relatedCategories.Add(ServerAnalyseItem.EventModule, new string[] { EventModuleCategory, EventModuleInstanceCategory });
            relatedCategories.Add(ServerAnalyseItem.System, new string[] { SystemCategory });
            relatedCategories.Add(ServerAnalyseItem.DispatcherQue, new string[] { DispatcherMessageQueCategory });
            relatedCategories.Add(ServerAnalyseItem.Dispatcher, new string[] { DispatcherCategory });
            reportCustomCounterDataThread = new Thread(reportCustomCounterData);
            reportCustomCounterDataThread.Priority = ThreadPriority.BelowNormal;
            reportCustomCounterDataThread.Start();
        }

        private void reportCustomCounterData()
        {
            while (!isShuttingDown)
            {
                DurationAnalyseContext context = ServerAnalyseManager.CreateContext<DurationAnalyseContext>();
                waitShutdown.WaitOne(5000);
                if (!isShuttingDown)
                    try
                    {
                        context.Done();
                        var size = retreiveCounter(SystemCategory, ThreadPoolSize, "current");
                        var asize = retreiveCounter(SystemCategory, ThreadPoolAvailableSize, "current");
                        var ratio = retreiveCounter(SystemCategory, ThreadPoolAvailableRatio, "current");
                        var ratioBase = retreiveCounter(SystemCategory, ThreadPoolAvailableRatioBase, "current");
                        int workerThreadSize; int portThreads;
                        ThreadPool.GetMaxThreads(out workerThreadSize, out portThreads);
                        int availableThreads; int portThreadsAval;
                        ThreadPool.GetAvailableThreads(out availableThreads, out portThreadsAval);
                        size.RawValue = workerThreadSize;
                        asize.RawValue = availableThreads;
                        ratio.RawValue = availableThreads;
                        ratioBase.RawValue = workerThreadSize;

                    }
                    catch
                    {
                        break;
                    }

            }
        }

        public override Metadata.NameDescriptionList GetCategoryNames(ServerAnalyseItem related)
        {
            return new Metadata.NameDescriptionList(relatedCategories[related]);
        }

        private string getCategoryNameWithoutKalitte(string name)
        {
            string[] parts = name.Split(':');
            if (parts.Length > 1)
                return parts[1].Trim();
            else return name;
        }


        public override Metadata.NameDescriptionList GetCategories()
        {
            string[] categories = { 
                        SensorDeviceCategory,
                        LogicalSensorDeviceCategory,
                        ProcessorMessageQueCategory,
                        ProcessorCategory,
                        EventModuleCategory,
                        EventModuleInstanceCategory,
                        EventModuleMethodCategory,
                        EventTypeCategory,
                        DispatcherMessageQueCategory,
                        DispatcherCategory,
                        SystemCategory};

            return new Metadata.NameDescriptionList(categories);

        }

        public override Metadata.NameDescriptionList GetInstanceNames(string category)
        {
            PerformanceCounterCategory pc = new PerformanceCounterCategory(category);
            return new Metadata.NameDescriptionList(pc.GetInstanceNames());
        }

        public override Metadata.NameDescriptionList GetMeasureNames(string category)
        {
            PerformanceCounterCategory pc = new PerformanceCounterCategory(category);
            var instanceNames = pc.GetInstanceNames();
            var result = new Metadata.NameDescriptionList();
            if (instanceNames.Length > 0)
            {
                var counters = pc.GetCounters(instanceNames[0]);
                foreach (var counter in counters)
                {
                    result.Add(new Metadata.NameDescription(counter.CounterName, counter.CounterHelp));
                }
            }
            return result;
        }



        public override float[] GetMeasureValues(string category, string instance, string[] measureNames)
        {
            var result = new float[measureNames.Length];
            for (int i = 0; i < measureNames.Length; i++)
            {
                try
                {
                    var counter = retreiveCounter(category, measureNames[i], instance);
                    result[i] = counter.NextValue();
                }
                catch (Exception exc)
                {

                    result[i] = 0;
                }
            }
            return result;
        }

        public override void DispatcherStart(string name, StartEventArgs e)
        {
            var count = retreiveCounter(DispatcherCategory, DispatcherStartCount, name, false);
            count.IncrementQuick();

            if (DispatcherLevel > ServerAnalyseLevel.Basic)
            {
                var time = retreiveCounter(DispatcherCategory, DispatcherAvgStartTime, name, false);
                var timeBase = retreiveCounter(DispatcherCategory, DispatcherAvgStartTimeBase, name, false);
                time.IncrementByQuick(getTickValue(e.GetContext<DurationAnalyseContext>()));
                timeBase.IncrementQuick();
            }
        }

        public override void DispatcherStop(string name, StopEventArgs e)
        {
            var count = retreiveCounter(DispatcherCategory, DispatcherStopCount, name, false);
            count.IncrementQuick();

            if (DispatcherLevel > ServerAnalyseLevel.Basic)
            {

                var time = retreiveCounter(DispatcherCategory, DispatcherAvgStopTime, name, false);
                var timeBase = retreiveCounter(DispatcherCategory, DispatcherAvgStopTimeBase, name, false);

                time.IncrementByQuick(getTickValue(e.GetContext<DurationAnalyseContext>()));
                timeBase.IncrementQuick();
            }
        }

        public override void DispatcherEventPipeExit(string name, EventPipeExitEventArgs e)
        {
            long ticks = getTickValue(e.GetContext<DurationAnalyseContext>());

            var time = retreiveCounter(DispatcherCategory, DispatcherAvgNotificationTime, name, false);
            var timeBase = retreiveCounter(DispatcherCategory, DispatcherAvgNotificationTimeBase, name, false);

            time.IncrementByQuick(ticks);
            timeBase.IncrementQuick();

            var timeTotal = retreiveCounter(DispatcherCategory, DispatcherAvgNotificationTime, TotalInstance, false);
            var timeBaseTotal = retreiveCounter(DispatcherCategory, DispatcherAvgNotificationTimeBase, TotalInstance, false);

            timeTotal.IncrementByQuick(ticks);
            timeBaseTotal.IncrementQuick();

            if (DispatcherLevel > ServerAnalyseLevel.Basic)
            {
                var countsec = retreiveCounter(DispatcherCategory, DispatcherNotificationSec, name, false);
                var count = retreiveCounter(DispatcherCategory, DispatcherNotificationCount, name, false);
                var countTotal = retreiveCounter(DispatcherCategory, DispatcherNotificationCount, TotalInstance, false);

                count.IncrementQuick();
                countTotal.IncrementQuick();
                countsec.IncrementQuick();

                var countsecTotal = retreiveCounter(DispatcherCategory, DispatcherNotificationSec, TotalInstance, false);
                countsecTotal.IncrementQuick();

            }

            incrementEventType(EventTypeDispatcherNotificationCount, e.Event, ServerAnalyseLevel.Basic);
            incrementEventType(EventTypeDispatcherNotificationSec, e.Event, ServerAnalyseLevel.Detailed);
            incrementEventType(EventTypeDispatcherAvgNotificationTime, e.Event, ServerAnalyseLevel.Detailed, ticks);
            incrementEventType(EventTypeDispatcherAvgNotificationTimeBase, e.Event, ServerAnalyseLevel.Detailed);
        }

        public override void DispatcherMessageQueSend(string name, QueSendEventArgs e)
        {
            var readsec = retreiveCounter(DispatcherMessageQueCategory, DispatcherMessageQueSendSec, name, false);
            readsec.IncrementQuick();

            if (DispatcherQueLevel > ServerAnalyseLevel.Basic)
            {
                var readcount = retreiveCounter(DispatcherMessageQueCategory, DispatcherMessageQueSendCount, name, false);
                var time = retreiveCounter(DispatcherMessageQueCategory, DispatcherMessageQueSendAvgTime, name, false);
                var timeBase = retreiveCounter(DispatcherMessageQueCategory, DispatcherMessageQueSendAvgTimeBase, name, false);
                readcount.IncrementQuick();
                time.IncrementByQuick(getTickValue(e.GetContext<DurationAnalyseContext>()));
                timeBase.IncrementQuick();
            }
        }

        public override void DispatcherMessageQueRead(string name, QueReadEventArgs e)
        {
            var readsec = retreiveCounter(DispatcherMessageQueCategory, DispatcherMessageQueReadSec, name, false);
            readsec.IncrementQuick();

            if (DispatcherQueLevel > ServerAnalyseLevel.Basic)
            {
                var time = retreiveCounter(DispatcherMessageQueCategory, DispatcherMessageQueReadAvgTime, name, false);
                var timeBase = retreiveCounter(DispatcherMessageQueCategory, DispatcherMessageQueReadAvgTimeBase, name, false);
                var sendcount = retreiveCounter(DispatcherMessageQueCategory, DispatcherMessageQueReadCount, name, false);

                time.IncrementByQuick(getTickValue(e.GetContext<DurationAnalyseContext>()));
                timeBase.IncrementQuick();
                sendcount.IncrementQuick();
            }
        }
    }
}
