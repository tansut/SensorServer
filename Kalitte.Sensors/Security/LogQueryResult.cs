using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Security
{
    [Serializable]
    public class LogQueryResult
    {
        public LogItemInfo [] Log { get; set; }
        public string [] LogSets { get; set; }
        public string CurrentLogSet { get; set; }
        public string [] NameSet { get; set; }

        public LogQueryResult(LogItemInfo[] log, string[] logSets, string currentLogSet, string [] nameSet)
        {
            this.Log = log;
            this.LogSets = logSets;
            this.CurrentLogSet = currentLogSet;
            this.NameSet = nameSet;
        }
    }
}
