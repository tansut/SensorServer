using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Events
{
    [Serializable, DataContract]
    public sealed class LastEventFilter
    {
        [DataMember]
        public HashSet<string> ValidSources { get; private set; }

        public HashSet<Type> ValidEventTypes { get; private set; }

        [DataMember]
        public bool IsEmpty { get; private set; }

        public LastEventFilter(HashSet<string> validSources, HashSet<Type> validEventTypes)
        {
            this.ValidSources = validSources;
            this.ValidEventTypes = validEventTypes;
            this.IsEmpty = false;
        }

        private LastEventFilter()
        {
            this.IsEmpty = true;
            ValidEventTypes = new HashSet<Type>();
            ValidSources = new HashSet<string>();
        }

        [DataMember]
        private string[] TypesForXml
        {
            get
            {
                return ValidEventTypes.Select(p => p.FullName).ToArray();
            }
            set
            {
                ValidEventTypes = new HashSet<Type>();
                foreach (var item in value)
                {
                    Type t = Type.GetType(item);
                    if (t == null)
                        t = TypesHelper.GetType(item);
                    if (t != null)
                        ValidEventTypes.Add(t);
                }
            }
        }

        public static LastEventFilter Empty
        {
            get
            {
                return new LastEventFilter();
            }
        }

        internal bool IsValid(string source, SensorEventBase sensorEvent)
        {
            if (IsEmpty)
                return true;
            bool isValid = true;
            if (ValidSources.Count > 0)
                isValid = ValidSources.Contains(source);
            if (!isValid)
                return false;
            if (ValidEventTypes.Count > 0)
                return ValidEventTypes.Contains(sensorEvent.GetType());
            return isValid;
        }
    }
}
