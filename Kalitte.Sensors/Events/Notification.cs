using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events.Management;

namespace Kalitte.Sensors.Events
{
    [Serializable]
    public sealed class Notification
    {
        // Fields
        private readonly SensorEventBase m_event;


        // Methods
        //public Notification(IOPortValueChangedEvent ioEvent)
        //{
        //    this.m_event = ioEvent;
        //}

        public Notification(ManagementEvent managementEvent)
        {
            this.m_event = managementEvent;
        }

        public Notification(SensorObservation obs)
        {
            this.m_event = obs;
        }

        public Notification(SensorEventBase reb)
        {
            this.m_event = reb;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<notification>");
            builder.Append("<event>");
            builder.Append(this.Event);
            builder.Append("</event>");
            builder.Append("</notification>");
            return builder.ToString();
        }

        // Properties
        public SensorEventBase Event
        {
            get
            {
                return this.m_event;
            }
            //protected set
            //{
            //    this.m_event = value;
            //}
        }
    }




}
