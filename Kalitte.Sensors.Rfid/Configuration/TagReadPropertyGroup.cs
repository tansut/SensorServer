using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Rfid.Configuration
{
    public sealed class TagReadPropertyGroup
    {
        // Fields
        public const string DuplicateEliminationTime = "Duplicate Elimination Time";
        public static readonly DevicePropertyMetadata DuplicationEliminationTimeMetadata = new DevicePropertyMetadata(typeof(long), "DuplicateEliminationTime", SensorPropertyRelation.DeviceAndSource, 0xea60L, true, false, true, false, 0.0, 9.2233720368547758E+18);
        public const string RssiCutoff = "RSSICutOff";
        public static readonly DevicePropertyMetadata RssiCutoffMetadata = new DevicePropertyMetadata(typeof(float), "RSSICutOff", SensorPropertyRelation.DeviceAndSource, 0f, true, false, true, false, 0.0, 100.0);
    }

 

 

}
