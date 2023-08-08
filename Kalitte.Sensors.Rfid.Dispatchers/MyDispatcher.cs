using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Interfaces;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Rfid.Utilities;
using System.Threading;

using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Rfid.EventModules.Client.TagView;
using Kalitte.Sensors.Rfid.EventModules.Client.Movement;

namespace Kalitte.Sensors.Dispatchers
{
    public class MyDispatcher : DispatcherModule
    {
        private List<string> tags = new List<string>();
        Timer reportTimer;
        #region IEventDispatcher Members

        private void reportTagCount(object state)
        {
            string[] copiedTags;
            lock (tags)
            {
                copiedTags = tags.ToArray();
            }
            if (copiedTags.Length > 0)
            {
                Console.WriteLine("Unique Tag Count:" + copiedTags.Length.ToString() + DateTime.Now.ToString());
                foreach (var tagId in copiedTags)
                    Console.WriteLine("{0:20}", tagId);
            }
        }

        public MyDispatcher()
        {

            
        }

        public override void Notify(string source, SensorEventBase sensorEvent)
        {
            TagAppearedEvent arrived = sensorEvent as TagAppearedEvent;
            TagLostEvent departed = sensorEvent as TagLostEvent;
            TagReadEvent readEvent = sensorEvent as TagReadEvent;
            TagMovementEvent movementEvent = sensorEvent as TagMovementEvent;
            //if (arrived != null)
            //{
            //    string tagID = HexHelper.HexEncode(arrived.TagReadEvent.GetId());
            //    lock (tags)
            //    {
            //        if (!tags.Contains(tagID))
            //            tags.Add(tagID);
            //    }
            //    Console.WriteLine("Arrvied: " + tagID);
            //}
            //if (departed != null)
            //{
            //    string tagID = HexHelper.HexEncode(departed.TagReadEvent.GetId());
            //    Console.WriteLine("Departed: " + tagID);
            //}
            //if (readEvent != null)
            //{
            //    Console.WriteLine(string.Format("Read: {0}. Rssi: {1} Count: {2}",
            //        HexHelper.HexEncode(readEvent.GetId()),
            //        readEvent.VendorSpecificData[TagReadEvent.Rssi],
            //        readEvent.VendorSpecificData[TagReadEvent.ReadCount]));
            //}

            //Console.WriteLine(sensorEvent.GetType().Name);
            if (movementEvent != null)
            {
                Console.WriteLine("Type:{0} Tag:{1} FirstRssi: {2:0.##} Next Rssi: {3:0.##} Change: {4:0.##}",
                    movementEvent.GetType().Name,
                    HexHelper.HexEncode(movementEvent.FirstTagRead.GetId()),
                    movementEvent.FirstRssi,
                    movementEvent.NextRssi,
                    movementEvent.GetChangePercentage());
            }
        }

        #endregion


        public override void SetProperty(Sensors.Configuration.EntityProperty property)
        {

        }

        public override void Shutdown()
        {
            //reportTimer.Dispose();
        }

        public override void Startup(DispatcherContext providerContext, string providerName, 
            DispatcherModuleInformation dispatcherInformation)
        {
            //TimerCallback cb = new TimerCallback(reportTagCount);
            //reportTimer = new Timer(cb, null, 0, 1000);
        }
    }
}
