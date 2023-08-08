using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Kalitte.Sensors.Security
{
    public static class ExceptionManager
    {
        public static void SaveExceptionToLog(Exception ee)
        {
            Exception e = ee;
            StringBuilder sb = new StringBuilder();
            while (e != null)
            {
                sb.AppendLine(e.Message);
                sb.AppendLine(e.StackTrace);
                e = e.InnerException;
            }
            if (!EventLog.SourceExists("KalitteSensorsServer"))
                EventLog.CreateEventSource("KalitteSensorsServer", "Application");
            EventLog.WriteEntry("KalitteSensorsServer", sb.ToString(), EventLogEntryType.Error);
        }

        public static void WriteConsoleException(Exception ee)
        {
            Exception e = ee;
            if (e is TargetInvocationException && e.InnerException != null) e = e.InnerException;
            Console.WriteLine(e);
        }
    }
}
