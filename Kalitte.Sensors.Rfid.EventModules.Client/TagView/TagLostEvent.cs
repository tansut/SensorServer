using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Rfid.EventModules.Client.TagView
{
    [Serializable]
    public sealed class TagLostEvent : RfidEventBase
    {
        public TagLostEvent(TagReadEvent tre, DateTime time)
        {
            this.LastSeen = time;
            this.LastTagReadEvent = tre;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<tagLostEvent>");
            builder.Append(base.ToString());
            builder.Append("<lastSeen>");
            builder.Append(this.LastSeen);
            builder.Append("</lastSeen>");
            builder.Append(this.LastTagReadEvent.ToString());
            builder.Append("</tagLostEvent>");
            return builder.ToString();
        }

        public DateTime LastSeen { get; private set; }

        public TagReadEvent LastTagReadEvent { get; private set; }
    }


}
