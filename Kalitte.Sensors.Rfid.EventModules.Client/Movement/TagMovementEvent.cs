using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Events;

namespace Kalitte.Sensors.Rfid.EventModules.Client.Movement
{
    [Serializable]
    public abstract class TagMovementEvent : RfidEventBase
    {
        public TagReadEvent FirstTagRead { get; private set; }
        public TagReadEvent NextTagRead { get; private set; }

        public double FirstRssi { get; private set; }
        public double NextRssi { get; private set; }

        protected TagMovementEvent(TagReadEvent firstTagRead, TagReadEvent nextTagRead, double firstRssi, double nextRssi)
        {
            this.FirstTagRead = firstTagRead;
            this.NextTagRead = nextTagRead;
            this.FirstRssi = firstRssi;
            this.NextRssi = nextRssi;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<firstRead>");
            builder.Append(FirstTagRead.ToString());
            builder.Append("</firstRead>");
            builder.Append("<nextRead>");
            builder.Append(NextTagRead.ToString());
            builder.Append("</nextRead>");
            builder.Append("<firstRssi>");
            builder.Append(FirstRssi.ToString());
            builder.Append("</firstRssi>");
            builder.Append("<nextRssi>");
            builder.Append(FirstRssi.ToString());
            builder.Append("</nextRssi>");
            return builder.ToString();
        }

        public double GetChangePercentage()
        {
            return ((NextRssi - FirstRssi) / Math.Abs(NextRssi)) * 100.0;
        }

    }
}
