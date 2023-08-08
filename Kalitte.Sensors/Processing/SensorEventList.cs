using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Processing
{
    public class SensorEventList: List<KeyValuePair<string, SensorEventBase>>
    {
        public List<T> GetEventList<T>() where T : SensorEventBase
        {
            var listOfType = this.Where(p => p.Value.GetType() == typeof(T)).Select(p => (T)p.Value).ToArray();
            return new List<T>(listOfType);
        }

        public T GetEvent<T> () where T: SensorEventBase
        {
            var evt = this.FirstOrDefault(p => p.Value.GetType() == typeof(T));
            return evt as T;
        }

        public List<KeyValuePair<string, T>> GetEvents<T>() where T : SensorEventBase
        {
            var listOfType = this.Where(p => p.Value.GetType() == typeof(T)).Select(p => new KeyValuePair<string, T>(p.Key, (T)p.Value)).ToArray();
            return new List<KeyValuePair<string, T>>(listOfType);
        }

        public SensorEventList(IEnumerable<KeyValuePair<string, SensorEventBase>> initialItems): base(initialItems)
        {

        }

        public SensorEventList(int capacity)
            : base(capacity)
        {
        }

        public KeyValuePair<string, SensorEventBase> Add(string source, SensorEventBase sensorEvent)
        {
            var item = new KeyValuePair<string, SensorEventBase>(source, sensorEvent);
            Add(item);
            return item;
        }

        public SensorEventList(string source, SensorEventBase sensorEvent): base(1)
        {
            Add(source, sensorEvent);
        }

        public SensorEventList()
            : base()
        {

        }
    }
}
