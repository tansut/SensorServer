using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Security;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public class LogConfiguration
    {
        public LogLevel Level { get; private set; }
        public string DateTimeFormat { get; set; }
        public string BaseDirectory { get; set; }
        public int FileCount { get; set; }
        public int FileSize { get; set; }
        public int FileCheckFrequency { get; set; }
        public string ServerLogFile { get; set; }

        public LogConfiguration()
        {
            Level = LogLevel.Info;
            DateTimeFormat = "MMddyy HH:mm:ss";
            BaseDirectory = @"..\..\Log\";
            FileCount = 5;
            FileSize = 100;
            FileCheckFrequency = 1;
            ServerLogFile = "SensorServer.log";
        }

        public LogConfiguration(LogLevel level, string dateTimeFormat, string baseDirectory)
        {
            Level = level;
            DateTimeFormat = dateTimeFormat;
            BaseDirectory = baseDirectory;
            FileCount = 5;
            FileSize = 100;
            FileCheckFrequency = 1;
            ServerLogFile = "SensorServer.log";
        }

    }
}
