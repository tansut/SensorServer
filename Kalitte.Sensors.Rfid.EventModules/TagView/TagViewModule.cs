using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Rfid.EventModules.Client.TagView;
using System.Threading;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Utilities;
using System.Collections;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Security;

namespace Kalitte.Sensors.Rfid.EventModules.TagView
{
    public class TagViewModule : SensorEventModule
    {
        class EventData
        {
            public DateTime EventTime { get; private set; }
            public TagReadEvent Event { get; private set; }
            public string Source { get; private set; }

            public EventData(string source, TagReadEvent evt)
            {
                EventTime = DateTime.Now;
                Event = evt;
                Source = source;
            }
        }

        public const int DepartTimeoutDefault = 5;
        public const int DepartCheckIntervalDefault = 1000;
        public const bool UseTagTimeDefault = false;


        int departTimeout = DepartTimeoutDefault;
        int departCheckInterval = DepartCheckIntervalDefault;
        bool useTagTime = UseTagTimeDefault;

        private static PropertyKey DepartTimeoutKey = new PropertyKey("Depart", "Depart Timeout (seconds)");
        private static PropertyKey DepartCheckIntervalKey = new PropertyKey("Depart", "Depart Check Interval (ms)");
        private static PropertyKey UseTagTimeKey = new PropertyKey("Depart", "Use Tag Time");
        private static PropertyKey TotalTagArrivedEventKey = new PropertyKey("Stats", "Total Tag Arrived");
        private static PropertyKey TotalTagDepartedEventKey = new PropertyKey("Stats", "Total Tag Departed");
        private static PropertyKey CustomKey = new PropertyKey("Custom", "Edit Custom");

        private Thread arriveDepartThread;
        private volatile bool isShuttingdown;
        private AutoResetEvent waitOnShutDown;
        private Dictionary<string, EventData> currentEvents;
        private ILogger logger;

        private long totalDeparted;
        private long totalArrived;

        private void initLogger(string name)
        {
            lock (this)
            {
                logger = ProcessorContext.GetLogger(name);
            }
        }

        private void checkArriveDepartThread()
        {
            try
            {
                while (!isShuttingdown)
                {
                    lock (this)
                    {
                        List<string> tagsDepated = new List<string>();
                        foreach (var item in currentEvents)
                        {
                            EventData evt = item.Value;
                            TimeSpan timeElapsed = useTagTime ? (DateTime.Now - item.Value.Event.Time) : (DateTime.Now - evt.EventTime);
                            if (timeElapsed.TotalSeconds > departTimeout)
                            {
                                tagsDepated.Add(item.Key);
                            }
                        }

                        foreach (var tagDepated in tagsDepated)
                        {
                            EventData tagData = currentEvents[tagDepated];
                            currentEvents.Remove(tagDepated);
                            var eventToDispatch = new TagLostEvent(tagData.Event, useTagTime ? tagData.Event.Time : tagData.EventTime);
                            logger.Verbose("Sending TagLost event. TagID:{0}", tagDepated);
                            Interlocked.Increment(ref totalDeparted);
                            ProcessorContext.Current.SetProperty(this, new EntityProperty(TotalTagDepartedEventKey, totalDeparted));
                            ProcessorContext.Current.AddEventToNextPipe(this, null, eventToDispatch);

                        }
                    }
                    waitOnShutDown.WaitOne(departCheckInterval);
                }
            }
            catch (Exception exc)
            {
                string errorMessage;
                SensorCommon.GetDetailedErrorMessage(exc, true, out errorMessage);
                ProcessorContext.Current.Stop(errorMessage);
                isShuttingdown = true;
            }

        }

        public TagViewModule()
        {
            arriveDepartThread = new Thread(checkArriveDepartThread);
            waitOnShutDown = new AutoResetEvent(false);
            currentEvents = new Dictionary<string, EventData>();
        }

        public override void Startup(ProcessorContext context, string name, EventModuleInformation information)
        {
            PropertyList propertyProfile = information.PropertyProfile;
            if (propertyProfile.ContainsKey(DepartTimeoutKey))
                departTimeout = (int)propertyProfile[DepartTimeoutKey];
            if (propertyProfile.ContainsKey(DepartCheckIntervalKey))
                departCheckInterval = (int)propertyProfile[DepartCheckIntervalKey];
            if (propertyProfile.ContainsKey(UseTagTimeKey))
                useTagTime = (bool)propertyProfile[UseTagTimeKey];
            isShuttingdown = false;
            totalDeparted = 0;
            totalArrived = 0;
            initLogger(name);
            arriveDepartThread.Start();
            context.SetProperty(this, new EntityProperty(TotalTagDepartedEventKey, 0));
            context.SetProperty(this, new EntityProperty(TotalTagArrivedEventKey, 0));
        }


        public static EventModuleMetadata GetMetadata()
        {
            Dictionary<PropertyKey, EventModulePropertyMetadata> values = new Dictionary<PropertyKey, EventModulePropertyMetadata>();
            var departTimeout = new EventModulePropertyMetadata(typeof(int), "Set timeout in miliseconds", 5, false, 1, int.MaxValue);
            var checkInterval = new EventModulePropertyMetadata(typeof(int), "Depart check interval", 1000, false, 100, int.MaxValue);
            var useTagTime = new EventModulePropertyMetadata(typeof(bool), "Use Tag time for depart check", false, false);
            var totalArrive = new EventModulePropertyMetadata(typeof(int), "", 0, false, false);
            var totalDepart = new EventModulePropertyMetadata(typeof(int), "", 0, false, false);
            var test = new EventModulePropertyMetadata(typeof(TagStatusCustomData), "Custom", new TagStatusCustomData() { Prop1 = "1111" }, false);
            values.Add(DepartTimeoutKey, departTimeout);
            values.Add(DepartCheckIntervalKey, checkInterval);
            values.Add(UseTagTimeKey, useTagTime);
            values.Add(TotalTagArrivedEventKey, totalArrive);
            values.Add(TotalTagDepartedEventKey, totalDepart);
            values.Add(CustomKey, test);
            EventModuleMetadata metaData = new EventModuleMetadata(values);
            return metaData;
        }

        public static ExtendedMetadata GetMetadataOfItem(ProcessingItem itemType)
        {
            if (itemType == ProcessingItem.LogicalSensor)
            {
                var key = new PropertyKey("TagStatus Module", "Include");
                var value = new ExtendedPropertyMetadata(typeof(bool), "", true, false);
                Dictionary<PropertyKey, ExtendedPropertyMetadata> dict = new Dictionary<PropertyKey, ExtendedPropertyMetadata>();
                dict.Add(key, value);

                return new ExtendedMetadata(dict);
            }
            return null;
        }

        public override void Shutdown()
        {

            isShuttingdown = true;
            waitOnShutDown.Set();
            logger.Info("Waiting arriveDepartThread to finish");
            arriveDepartThread.Join();
        }

        [SensorEventHandler(true)]
        public TagLostEvent PassTagDeparted(TagLostEvent tagDeparted)
        {
            return tagDeparted;
        }

        [SensorEventHandler(true)]
        public TagAppearedEvent PassTagArrived(TagAppearedEvent tagArrived)
        {
            return tagArrived;
        }


        [SensorEventHandler(true)]
        public SensorEventBase ProcessTagReadEvent(string source, TagReadEvent tagRead)
        {

            string tagId = HexHelper.HexEncode(tagRead.GetId());
            lock (this)
            {
                if (!currentEvents.ContainsKey(tagId))
                {
                    var tagArrived = new TagAppearedEvent(tagRead, DateTime.Now);
                    currentEvents.Add(tagId, new EventData(source, tagRead));
                    Interlocked.Increment(ref totalArrived);
                    ProcessorContext.Current.SetProperty(this, new EntityProperty(TotalTagArrivedEventKey, totalArrived));
                    logger.Verbose("Sending TagAppeared event. TagID:{0}", HexHelper.HexEncode(tagArrived.TagReadEvent.GetId()));
                    return tagArrived;
                }
                else
                {
                    currentEvents[tagId] = new EventData(source, tagRead);
                }
            }
            return null;
        }

        public override void SetProperty(EntityProperty property)
        {

        }
    }
}
