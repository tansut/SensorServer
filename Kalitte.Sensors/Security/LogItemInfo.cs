using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Configuration;
using System.Globalization;

namespace Kalitte.Sensors.Security
{

    [Serializable]
    public class LogItemInfo
    {
        public int ManagedThreadId { get; set; }
        public LogLevel Level { get; private set; }
        public DateTime Time { get; private set; }
        public string Name { get; private set; }
        public string Message { get; private set; }
        public string File { get; private set; }
        public int LineNumber { get; private set; }

        public LogItemInfo(string name, string message, LogLevel level, DateTime time, string file, int lineNumber)
        {
            this.Name = name;
            this.Message = message;
            this.Level = level;
            this.Time = time;
            this.File = file;
            this.LineNumber = lineNumber;
        }

        public LogItemInfo(string name, string message, LogLevel level, DateTime time):
            this(name, message, level, time, null, 0)
        {

        }

        public static LogItemInfo FromText(string text, char seperator = '|')
        {
            try
            {
                string[] parts = text.Split(seperator);
                int threadId = -1;
                try
                {
                    threadId = int.Parse(parts[0].Trim());
                }
                catch (FormatException)
                {
                    
                    
                }
                LogLevel level = (LogLevel)Enum.Parse(typeof(LogLevel), parts[1].Trim());
                DateTime logTime = DateTime.ParseExact(parts[2].Trim(), ServerConfiguration.Current.LogConfiguration.DateTimeFormat, CultureInfo.InvariantCulture);
                string message = parts[3].Trim();
                string name = parts[4].Trim();

                return new LogItemInfo(name, message, level, logTime) { ManagedThreadId = threadId };
                   
            }
            catch (Exception exc)
            {

                return new LogItemInfo("!LOGPARSEERROR!", exc.Message, LogLevel.Error, DateTime.Now);
            }
            



            return null;


        }
    }

}
