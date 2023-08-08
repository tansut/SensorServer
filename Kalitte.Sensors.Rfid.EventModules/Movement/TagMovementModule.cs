using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Rfid.EventModules.Client.TagView;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Rfid.Core;
using System.Threading;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Rfid.EventModules.Client.Movement;

namespace Kalitte.Sensors.Rfid.EventModules.Movement
{
    public class TagMovementModule : SensorEventModule
    {
        private static PropertyKey RssiCalculationInterval = new PropertyKey("Sample Collection", "Sample Collect Timeout");
        private static PropertyKey RssiCalculationTagLimit = new PropertyKey("Sample Collection", "Min Sample Count");
        private static PropertyKey ApproachPercentageMin = new PropertyKey("RSSI Limits", "Approach Min Change (%)");
        private static PropertyKey ApproachPercentageMax = new PropertyKey("RSSI Limits", "Approach Max Change (%)");
        private static PropertyKey MoveAwayPercentageMin = new PropertyKey("RSSI Limits", "Move Away Min Change (%)");
        private static PropertyKey MoveAwayPercentageMax = new PropertyKey("RSSI Limits", "Move Away Max Change (%)");
        private static PropertyKey TagLostTimeout = new PropertyKey("Tag Cleanup", "TagLost Timeout");
        private static PropertyKey CleanupInterval = new PropertyKey("Tag Cleanup", "Cleanup Interval");
        private static PropertyKey MinEventInterval = new PropertyKey("Event Generation", "Min Interval");

        static TagMovementModule()
        {

        }

        public class ModuleSettings
        {
            public int CalculationIntervalLimit { get; private set; }

            public int CalculationTagLimit { get; private set; }

            public double MinApproachPercentage { get; private set; }

            public double MaxApproachPercentage { get; private set; }

            public double MinMoveAwayPercentage { get; private set; }

            public double MaxMoveAwayPercentage { get; private set; }

            public int TagLostTimeout { get; private set; }
            public int CleanupInterval { get; private set; }
            public int MinEventInterval { get; private set; }

            public ModuleSettings()
            {
                CalculationIntervalLimit = 2000;
                CalculationTagLimit = 3;
                MinApproachPercentage = 5.0;
                MaxApproachPercentage = 15.0;
                MinMoveAwayPercentage = MinApproachPercentage;
                MaxMoveAwayPercentage = MaxApproachPercentage;
                TagLostTimeout = 10000;
                CleanupInterval = 5000;
                MinEventInterval = 750;
            }

            public void Set(EntityProperty property)
            {
                if (property.Key == TagMovementModule.RssiCalculationInterval)
                    CalculationIntervalLimit = (int)property.PropertyValue;
                else if (property.Key == TagMovementModule.RssiCalculationTagLimit)
                    CalculationTagLimit = (int)property.PropertyValue;
                else if (property.Key == TagMovementModule.ApproachPercentageMin)
                    MinApproachPercentage = (double)property.PropertyValue;
                else if (property.Key == TagMovementModule.ApproachPercentageMax)
                    MaxApproachPercentage = (double)property.PropertyValue;
                else if (property.Key == TagMovementModule.MoveAwayPercentageMin)
                    MinMoveAwayPercentage = (double)property.PropertyValue;
                else if (property.Key == TagMovementModule.MoveAwayPercentageMax)
                    MaxMoveAwayPercentage = (double)property.PropertyValue;
                else if (property.Key == TagMovementModule.TagLostTimeout)
                    TagLostTimeout = (int)property.PropertyValue;
                else if (property.Key == TagMovementModule.CleanupInterval)
                    CleanupInterval = (int)property.PropertyValue;
                else if (property.Key == TagMovementModule.MinEventInterval)
                    MinEventInterval = (int)property.PropertyValue;
            }

        }



        private ModuleSettings currentSettings;
        private ILogger logger;

        private Dictionary<TagIdKey, RssiCalculationPair> tagRssiDict;
        private object tagRssiDictSync = new object();

        private Thread cleanupTagsThread;
        private volatile bool isShuttingdown;
        private AutoResetEvent waitOnShutDown;


        private void doCleanupForLostTags()
        {
            try
            {
                while (!isShuttingdown)
                {
                    lock (tagRssiDictSync)
                    {
                        Dictionary<TagIdKey, RssiCalculationPair> tags = new Dictionary<TagIdKey, RssiCalculationPair>(tagRssiDict);
                        foreach (var item in tags)
                        {
                            var lastSeenMs = (DateTime.Now - item.Value.LastTagReadEventReceived).TotalMilliseconds;
                            if (lastSeenMs >= currentSettings.TagLostTimeout)
                            {
                                tagRssiDict.Remove(item.Key);
                                logger.Verbose("Removed tag {0}. Interval: {1}", item.Key, lastSeenMs); 
                            }
                        }
                    }

                    waitOnShutDown.WaitOne(currentSettings.CleanupInterval);
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

        [SensorEventHandler(true)]
        public SensorEventBase ProcessTagRead(string source, TagReadEvent tagRead)
        {
            var tagKey = new TagIdKey(tagRead.GetId());
            RssiCalculationPair calcPair;
            object rssiValue;
            bool rssiFound = tagRead.VendorSpecificData.TryGetValue(TagReadEvent.Rssi, out rssiValue);
            TagMovementEvent availableEvent = null;
            if (rssiFound & rssiValue.GetType().IsAssignableFrom(typeof(float)))
            {
                bool tagFound;
                
                lock (tagRssiDictSync)
                {
                    tagFound = tagRssiDict.TryGetValue(tagKey, out calcPair);
                    if (tagFound)
                    {
                        availableEvent = calcPair.AddSample(tagRead, currentSettings);
                    }
                    else tagRssiDict.Add(tagKey, new RssiCalculationPair(tagRead, logger));
                }
            }
            return availableEvent;
        }

        public static EventModuleMetadata GetMetadata()
        {
            ModuleSettings settings = new ModuleSettings();
            Dictionary<PropertyKey, EventModulePropertyMetadata> values = new Dictionary<PropertyKey, EventModulePropertyMetadata>();
            var rssiCalcInterval = new EventModulePropertyMetadata(typeof(int), "Defines time interval in miliseconds to measure Rssi (Radio signal strenght indicator) average value", settings.CalculationIntervalLimit, false, 1, int.MaxValue);
            var rssiCalcLimit = new EventModulePropertyMetadata(typeof(int), "Optionally set a value to specify TagRead event count before time interval has elapsed.", settings.CalculationTagLimit, false, 0, int.MaxValue);
            var approachPercentageMin = new EventModulePropertyMetadata(typeof(double), "", settings.MinApproachPercentage, false, 0.0, 100.0);
            var moveAwayPercentageMin = new EventModulePropertyMetadata(typeof(double), "", settings.MinMoveAwayPercentage, false, 0.0, 100.0);
            var approachPercentageMax = new EventModulePropertyMetadata(typeof(double), "", settings.MaxApproachPercentage, false, 0.0, 100.0);
            var moveAwayPercentageMax = new EventModulePropertyMetadata(typeof(double), "", settings.MaxMoveAwayPercentage, false, 0.0, 100.0);
            var cleanupInterval = new EventModulePropertyMetadata(typeof(int), "", settings.CleanupInterval, false);
            var tagLostInterval = new EventModulePropertyMetadata(typeof(int), "", settings.TagLostTimeout, false);
            var minEventInterval = new EventModulePropertyMetadata(typeof(int), "", settings.MinEventInterval, false);


            values.Add(RssiCalculationInterval, rssiCalcInterval);
            values.Add(RssiCalculationTagLimit, rssiCalcLimit);
            values.Add(ApproachPercentageMin, approachPercentageMin);
            values.Add(ApproachPercentageMax, approachPercentageMax);
            values.Add(MoveAwayPercentageMax, moveAwayPercentageMax);
            values.Add(MoveAwayPercentageMin, moveAwayPercentageMin);
            values.Add(CleanupInterval, cleanupInterval);
            values.Add(TagLostTimeout, tagLostInterval);
            values.Add(MinEventInterval, minEventInterval);

            EventModuleMetadata metaData = new EventModuleMetadata(values);
            return metaData;
        }


        public override void Startup(ProcessorContext context, string name, EventModuleInformation moduleInformation)
        {
            PropertyList propertyProfile = moduleInformation.PropertyProfile;
            currentSettings = new ModuleSettings();
            this.logger = ProcessorContext.GetLogger(name);
            this.tagRssiDict = new Dictionary<TagIdKey, RssiCalculationPair>();
            cleanupTagsThread = new Thread(doCleanupForLostTags);
            waitOnShutDown = new AutoResetEvent(false);

            foreach (var item in propertyProfile)
            {
                SetProperty(new EntityProperty(item.Key, item.Value));
            }

            cleanupTagsThread.Start();
        }

        public override void SetProperty(Sensors.Configuration.EntityProperty property)
        {
            currentSettings.Set(property);
        }

        public override void Shutdown()
        {
            isShuttingdown = true;
            waitOnShutDown.Set();
            logger.Info("Waiting cleanup thread to finish.");
            cleanupTagsThread.Join();
        }
    }
}
