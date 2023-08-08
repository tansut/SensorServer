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
    public sealed class TagMovingAwayEvent : TagMovementEvent
    {
        public TagMovingAwayEvent(TagReadEvent firstTagRead, TagReadEvent nextTagRead, double firstRssi, double nextRssi): base(firstTagRead, nextTagRead, firstRssi, nextRssi)
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<tagMovingAwayEvent>");
            builder.Append(base.ToString());
            builder.Append("</tagMovingAwayEvent>");
            return builder.ToString();
        }
    }
}
