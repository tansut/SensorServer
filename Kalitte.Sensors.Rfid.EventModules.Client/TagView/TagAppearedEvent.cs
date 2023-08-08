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
    public sealed class TagAppearedEvent : RfidEventBase
    {
        public TagAppearedEvent(TagReadEvent tre, DateTime time)
        {
            this.AppearTime = time;
            this.TagReadEvent = tre;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<tagAppearedEvent>");
            builder.Append(base.ToString());
            builder.Append("<appearTime>");
            builder.Append(this.AppearTime);
            builder.Append("</appearTime>");
            builder.Append(this.TagReadEvent.ToString());
            builder.Append("</tagAppearedEvent>");
            return builder.ToString();
        }


        public DateTime AppearTime { get; private set; }

        public TagReadEvent TagReadEvent { get; private set; }

    }
}
