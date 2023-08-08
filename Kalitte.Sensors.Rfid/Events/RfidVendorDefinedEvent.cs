using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Events.Management;


namespace Kalitte.Sensors.Rfid.Events
{
    [Serializable]
    public sealed class RfidVendorDefinedEvent : RfidObservation
    {
        // Fields
        private readonly EventType eventType;
        private readonly string name;

        // Methods
        public RfidVendorDefinedEvent(string name, EventType eventType, VendorData vendorData)
            : base(vendorData)
        {
            this.name = name;
            this.eventType = eventType;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<rfidVendorDefinedEvent>");
            builder.Append(base.ToString());
            builder.Append("<name>");
            builder.Append(this.name);
            builder.Append("</name>");
            builder.Append("<eventType>");
            builder.Append(this.eventType);
            builder.Append("</eventType>");
            builder.Append("</rfidVendorDefinedEvent>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.name == null) || (this.name.Length == 0))
            {
                throw new ArgumentNullException("name");
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
        public EventType EventType
        {
            get
            {
                return this.eventType;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }



}
