using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Interfaces;


namespace Kalitte.Sensors.Rfid.Events
{
    [Serializable]
    public class RfidObservation : RfidEventBase, ISensorObservation
    {
        // Fields
        private string m_deviceName;
        private string m_sourceName;
        private DateTime m_time;

        // Methods
        protected internal RfidObservation()
        {
        }

        protected internal RfidObservation(VendorData vendorData)
            : base(vendorData)
        {
        }

        protected internal RfidObservation(string source, string deviceName, DateTime time, VendorData vendorData)
            : base(vendorData)
        {
            this.Source = source;
            this.m_deviceName = deviceName;
            this.Time = time;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<observation>");
            builder.Append(base.ToString());
            builder.Append("<time>");
            builder.Append(this.m_time);
            builder.Append("</time>");
            builder.Append("<sourceName>");
            builder.Append(this.m_sourceName);
            builder.Append("</sourceName>");
            builder.Append("<deviceName>");
            builder.Append(this.m_deviceName);
            builder.Append("</deviceName>");
            builder.Append("</observation>");
            return builder.ToString();
        }

        // Properties
        public virtual string DeviceName
        {
            get
            {
                return this.m_deviceName;
            }
            set
            {
                this.m_deviceName = value;
            }
        }

        public string Source
        {
            get
            {
                return this.m_sourceName;
            }
            set
            {
                this.m_sourceName = value;
            }
        }

        public DateTime Time
        {
            get
            {
                return this.m_time;
            }
            set
            {
                this.m_time = value;
            }
        }
    }




}
