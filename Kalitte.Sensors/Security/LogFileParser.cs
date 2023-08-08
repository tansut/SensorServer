using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Security
{
    public class LogFileParser
    {
        private string itemLogPath;
        private LogQuery query;


        public LogFileParser(string itemLogPath, LogQuery query)
        {
            this.itemLogPath = itemLogPath;
            this.query = query;
        }

        public List<LogItemInfo> GetItems(string file, List<string> names)
        {
            if (File.Exists(file))
            {
                List<LogItemInfo> result = new List<LogItemInfo>(query.MaxTopItems);
                using (FileStream f = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    ReverseLineReader reader = new ReverseLineReader(() => { return f; });
                    int currentIndex = 0;

                    foreach (var line in reader)
                    {
                        if (string.IsNullOrEmpty(line.Trim()))
                            continue;
                        if (currentIndex++ >= query.MaxTopItems) break;
                        bool addToList = true;
                        var item = LogItemInfo.FromText(line);
                        if (!string.IsNullOrEmpty(query.MessageSearch))
                            addToList = item.Message.ToUpper().Contains(query.MessageSearch.ToUpper());
                        if (addToList && !string.IsNullOrEmpty(query.NameSearch))
                            addToList = item.Name.Equals(query.NameSearch, StringComparison.InvariantCultureIgnoreCase);
                        if (!names.Contains(item.Name))
                            names.Add(item.Name);
                        if (addToList && (query.Level == LogLevel.Off || query.Level >= item.Level))
                            result.Add(item);
                    }
                    f.Close();
                }
                return result;
            }
            else return new List<LogItemInfo>();
        }

        public LogQueryResult GetResult()
        {
            string actualFile = itemLogPath;
            string directory = Path.GetDirectoryName(actualFile);
            if (!string.IsNullOrEmpty(query.LogSet))
            {
                actualFile = Path.Combine(directory, query.LogSet);
            }

            List<string> names = new List<string>();

            LogItemInfo[] log = GetItems(actualFile, names).ToArray();
            List<string> logSets = new List<string>(SensorCommon.GetFiles(directory, Path.GetFileNameWithoutExtension(itemLogPath)));
            LogQueryResult result = new LogQueryResult(log, logSets.ToArray(), Path.GetFileName(actualFile), names.ToArray());

            return result;
        }
    }
}
