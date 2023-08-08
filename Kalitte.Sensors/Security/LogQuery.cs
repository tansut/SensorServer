using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Security
{
    [Serializable]
    public class LogQuery
    {
        public int MaxTopItems { get; set; }
        public LogLevel Level { get; set; }
        public string MessageSearch { get; set; }
        public string LogSet { get; set; }
        public string NameSearch { get; set; }

        public LogQuery()
        {
            MaxTopItems = 1000;
            Level = LogLevel.Off;
        }
    }
}
