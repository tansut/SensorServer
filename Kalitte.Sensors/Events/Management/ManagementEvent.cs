using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Instrumentation;
using System.Threading;
using System.Runtime.Serialization;
using Kalitte.Sensors.Core;


namespace Kalitte.Sensors.Events.Management
{
    [Serializable, InstrumentationClass(InstrumentationType.Event)]
    public class ManagementEvent : SensorEventBase
    {
        // Fields
        [IgnoreMember]
        private readonly string description;
        [IgnoreMember]
        private readonly EventLevel eventLevel;
        [IgnoreMember]
        private readonly EventType eventType;
        [IgnoreMember]
        private long occured;
        [IgnoreMember]
        private static object s_lock = new object();

        // Methods
        public ManagementEvent(EventLevel eventLevel, EventType eventType, string description)
            : this(eventLevel, eventType, description, null)
        {
        }

        protected ManagementEvent(EventLevel eventLevel, EventType eventType, string description, VendorData vendorData)
            : base(vendorData)
        {
            this.occured = GetTicks();
            this.eventLevel = eventLevel;
            this.eventType = eventType;
            if ((description == null) || (description.Length == 0))
            {
                this.description = eventType.Description;
            }
            else
            {
                this.description = description;
            }
            this.ValidateParameters();
        }

        private static long GetTicks()
        {
            lock (s_lock)
            {
                Thread.Sleep(1);
                return DateTime.Now.Ticks;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<managementEvent>");
            builder.Append("<eventLevel>");
            builder.Append(this.eventLevel);
            builder.Append("</eventLevel>");
            builder.Append("<eventType>");
            builder.Append(this.eventType);
            builder.Append("</eventType>");
            builder.Append("<description>");
            builder.Append(this.description);
            builder.Append("</description>");
            builder.Append("</managementEvent>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.eventLevel == EventLevel.Unknown)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            if (EventType.Uninitialized.Value >= this.eventType.Value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        [IgnoreMember]
        public EventLevel EventLevel
        {
            get
            {
                return this.eventLevel;
            }
        }

        public int EventLevelId
        {
            get
            {
                return (int)this.eventLevel;
            }
        }

        [IgnoreMember]
        public EventType EventType
        {
            get
            {
                return this.eventType;
            }
        }

        public string EventTypeDescription
        {
            get
            {
                return this.eventType.Description;
            }
        }

        public int EventTypeId
        {
            get
            {
                return this.eventType.Value;
            }
        }

        public long OccuranceTime
        {
            get
            {
                return this.occured;
            }
        }
    }




}
