using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Management.Instrumentation;

namespace Kalitte.Sensors.Events.Management
{
    [Serializable]
    public sealed class SourceDownEvent : DeviceManagementEvent
    {
        // Fields
        [IgnoreMember]
        private readonly string sourceName;

        // Methods
        public SourceDownEvent(string description, string sourceName)
            : base(EventLevel.Error, EventType.SourceDown, description)
        {
            this.sourceName = sourceName;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<sourceDownEvent>");
            builder.Append(base.ToString());
            builder.Append("<sourceName>");
            builder.Append(this.sourceName);
            builder.Append("</sourceName>");
            builder.Append("</sourceDownEvent>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.sourceName == null) || (this.sourceName.Length == 0))
            {
                throw new ArgumentNullException("sourceName");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public string SourceName
        {
            get
            {
                return this.sourceName;
            }
        }
    }





}
