using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Utilities;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Net;

namespace Kalitte.Sensors.Security
{
    public class FileLogger : StreamLogger
    {
        // Fields
        private string dateTimeFormat;
        private const int GENERAL_CATEGORY = 1;
        private EventHandler<EventArgs> logRotatedEvent;
        private static long LOGROTATIONDISABLED_ID = 0x29fL;
        private static long LOGROTATIONREENABLED_ID = 0x2a0L;
        private static bool logRotationFailEventLogged = false;
        private LogRotator logRotator;
        private static string SERVICE_NAME = "KalitteSensorServer";

        // Methods
        public FileLogger(string name, string dateTimeFormat)
            : base(name)
        {
            this.dateTimeFormat = dateTimeFormat;
        }

        protected FileLogger(FileLogger logger, string name)
            : base(logger, name)
        {
            
            if (logger != null)
            {
                this.dateTimeFormat = logger.dateTimeFormat;
                this.logRotator = logger.logRotator;
            }
        }

        public FileLogger(string name, LogLevel level, Stream stream, string dateTimeFormat)
            : this(name, dateTimeFormat)
        {
            this.MyInit(level, stream);
        }

        public override StreamLogger Clone(string name)
        {
            return new FileLogger(this, name);
        }

        public override void Init(LogLevel level, Stream stream)
        {
            this.MyInit(level, stream);
        }

        public override void Log(string message, LogLevel level, string file, int line)
        {
            if ((level != LogLevel.Off) && (level <= base.CurrentLevel))
            {
                object obj2;
                StringBuilder sb = new StringBuilder();
                SensorCommon.AppendFormat(sb, "{0}|", new object[] { Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture).PadLeft(4) });
                SensorCommon.AppendFormat(sb, "{0}|", new object[] { level.ToString(CultureInfo.InvariantCulture).PadLeft(7) });
                SensorCommon.AppendFormat(sb, "{0}|", new object[] { DateTime.Now.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture) });

                StringBuilder messagesb = new StringBuilder(message); 
                messagesb.Replace('|', '\t');
                messagesb.Replace(Environment.NewLine, "\t");
                messagesb.Replace("\0", "");
                SensorCommon.AppendFormat(sb, "{0}|", new object[] { messagesb.ToString() });
                SensorCommon.AppendFormat(sb, "[{0}]", new object[] { base.Name });
                if (!string.IsNullOrEmpty(file))
                {
                    SensorCommon.AppendFormat(sb, "[{0},{1}]", new object[] { file, line.ToString(CultureInfo.InvariantCulture) });
                }
                Monitor.Enter(obj2 = base.LockObject);
                try
                {
                    if (!base.IsDisposed)
                    {
                        base.LogStream.StreamWriter.WriteLine(sb);
                    }
                    if ((this.logRotator != null) && this.logRotator.IncrementCountAndIsFileNeedsToBeChanged())
                    {
                        this.RotateLog();
                    }
                }
                catch (ObjectDisposedException)
                {
                    base.Dispose();
                }
                finally
                {
                    Monitor.Exit(obj2);
                }
            }
        }

        private void MyInit(LogLevel level, Stream stream)
        {
            base.InitLogStreamAndLevel(level, stream);
        }

        internal void RotateLog()
        {
            Encoding encoding = base.LogStream.StreamWriter.Encoding;
            base.LogStream.StreamWriter.Close();
            System.Exception exception = null;
            try
            {
                this.logRotator.RotateLog();
                if (logRotationFailEventLogged)
                {
                    EventLog log = new EventLog("Application", Dns.GetHostName(), SERVICE_NAME);
                    EventInstance instance = new EventInstance(LOGROTATIONREENABLED_ID, 1, EventLogEntryType.Information);
                    log.WriteEvent(instance, new object[] { this.logRotator.LogFile });
                    log.Close();
                    logRotationFailEventLogged = false;
                }
            }
            catch (System.Exception exception2)
            {
                exception = exception2;
                if (!logRotationFailEventLogged)
                {
                    EventLog log2 = new EventLog("Application", Dns.GetHostName(), SERVICE_NAME);
                    EventInstance instance2 = new EventInstance(LOGROTATIONDISABLED_ID, 1, EventLogEntryType.Error);
                    log2.WriteEvent(instance2, new object[] { this.logRotator.LogFile, exception2.ToString() });
                    log2.Close();
                    logRotationFailEventLogged = true;
                }
            }
            finally
            {
                base.LogStream.StreamWriter = new StreamWriter(this.logRotator.LogFile.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read), encoding);
                base.LogStream.StreamWriter.BaseStream.Seek(0L, SeekOrigin.End);
                base.LogStream.StreamWriter.AutoFlush = true;
                if (exception != null)
                {
                    base.LogStream.StreamWriter.WriteLine("Got exception during log rotation {0}", exception);
                }
            }
            try
            {
                if (this.logRotatedEvent != null)
                {
                    this.logRotatedEvent(this, null);
                }
            }
            catch (System.Exception exception3)
            {
                base.LogStream.StreamWriter.WriteLine("{0}|{1}|{2}|Calling LogRotatedEvent failed. Exception: {3}", new object[] { Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture).PadLeft(4), LogLevel.Error.ToString(CultureInfo.InvariantCulture).PadLeft(7), DateTime.Now.ToString(this.dateTimeFormat, CultureInfo.InvariantCulture), exception3.ToString() });
            }
        }

        // Properties
        public string DateTimeFormat
        {
            get
            {
                return this.dateTimeFormat;
            }
            set
            {
                DateTime.Now.ToString(value, CultureInfo.InvariantCulture);
                this.dateTimeFormat = value;
            }
        }

        public EventHandler<EventArgs> LogRotatedEvent
        {
            get
            {
                return this.logRotatedEvent;
            }
            set
            {
                this.logRotatedEvent = value;
            }
        }

        public LogRotator LogRotator
        {
            get
            {
                return this.logRotator;
            }
            set
            {
                this.logRotator = value;
            }
        }
    }




}
