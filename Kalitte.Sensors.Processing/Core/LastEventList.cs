using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Processing.Core
{
    internal class LastEventList
    {
        public int size;
        private LinkedList<LastEvent> internalList;
        private object sync;

        public LastEventList(int size)
        {
            this.size = size;
            sync = new object();
            internalList = new LinkedList<LastEvent>();
        }

        public void Add(DateTime eventTime, string source, SensorEventBase sensorEvent, LastEventFilter filter)
        {
            if (!filter.IsValid(source, sensorEvent))
                return;
            var instance = new LastEvent(eventTime, source, sensorEvent);
            lock (sync)
            {
                int currentCount = internalList.Count;
                if (currentCount >= size)
                    internalList.RemoveLast();
                internalList.AddFirst(instance);
            }
        }

        public void Add(string source, SensorEventBase sensorEvent, LastEventFilter filter)
        {
            Add(DateTime.Now, source, sensorEvent, filter);
        }

        public IList<LastEvent> GetCurrentEvents()
        {
            lock (sync)
            {
                return internalList.ToArray();
            }
        }

        public void Clear()
        {
            lock (sync)
            {
                internalList.Clear();
            }
        }

        public void ChangeSize(int newSize)
        {
            size = newSize;
        }
    }
}
