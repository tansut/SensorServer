using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Rfid.EventModules.Client.Movement
{
    [Serializable]
    public sealed class TagApproachingEvent : TagMovementEvent
    {
        public TagApproachingEvent(TagReadEvent firstTagRead, TagReadEvent nextTagRead, double firstRssi, double nextRssi)
            : base(firstTagRead, nextTagRead, firstRssi, nextRssi)
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<tagApproachingEvent>");
            builder.Append(base.ToString());
            builder.Append("</tagApproachingEvent>");
            return builder.ToString();
        }

    }
}
