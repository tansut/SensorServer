using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using System.Collections;

namespace Kalitte.Sensors.Security
{
    public class DomainLogger : ILogger
    {
        // Fields
        private LogLevel m_defaultLogLevel;
        private bool m_disposed;
        private Hashtable m_names2Loggers;
        private StreamLogger m_realLogger;
        private object m_syncRoot;
        private ILogger m_wrappedLogger;

        // Methods
        public DomainLogger(StreamLogger logger, string name)
        {
            this.m_syncRoot = new object();
            this.m_names2Loggers = new Hashtable();
            this.m_defaultLogLevel = LogLevel.Info;
            this.m_wrappedLogger = logger.Clone(name);
            this.m_realLogger = logger;
            lock (this.m_syncRoot)
            {
                this.m_names2Loggers[name] = this;
                this.CurrentLevel = logger.CurrentLevel;
            }
        }

        private DomainLogger(DomainLogger logger, string name)
        {
            this.m_syncRoot = new object();
            this.m_names2Loggers = new Hashtable();
            this.m_defaultLogLevel = logger.m_defaultLogLevel;
            this.m_wrappedLogger = logger.m_realLogger.Clone(name);
            this.m_realLogger = logger.m_realLogger;
            lock (this.m_syncRoot)
            {
                this.m_names2Loggers[name] = this;
                this.CurrentLevel = logger.CurrentLevel;
            }
        }

        public void Close()
        {
            this.m_disposed = true;
        }

        public void Error(string message)
        {
            this.m_wrappedLogger.Error(message);
        }

        public void Error(string format, params object[] obj)
        {
            this.m_wrappedLogger.Error(format, obj);
        }

        public void Error(string message, string file, int line)
        {
            this.m_wrappedLogger.Error(message, file, line);
        }

        public void ErrorIf(bool condition, string message, string file, int line)
        {
            if (condition)
            {
                this.m_wrappedLogger.Error(message, file, line);
            }
        }

        public void ErrorWithoutFormat(params object[] obj)
        {
            this.m_wrappedLogger.ErrorWithoutFormat(obj);
        }

        public void Fatal(string message)
        {
            this.m_wrappedLogger.Fatal(message);
        }

        public void Fatal(string format, params object[] obj)
        {
            this.m_wrappedLogger.Fatal(format, obj);
        }

        public void Fatal(string message, string file, int line)
        {
            this.m_wrappedLogger.Fatal(message, file, line);
        }

        public void FatalIf(bool condition, string message, string file, int line)
        {
            if (condition)
            {
                this.m_wrappedLogger.Fatal(message, file, line);
            }
        }

        public void FatalWithoutFormat(params object[] obj)
        {
            this.m_wrappedLogger.FatalWithoutFormat(obj);
        }

        public ILogger GetLogger(string name)
        {
            lock (this.m_syncRoot)
            {
                DomainLogger logger = (DomainLogger)this.m_names2Loggers[name];
                if (logger == null)
                {
                    logger = new DomainLogger(this, name);
                    this.m_names2Loggers[name] = logger;
                }
                return logger;
            }
        }

        public void Info(string message)
        {
            this.m_wrappedLogger.Info(message);
        }

        public void Info(string format, params object[] obj)
        {
            this.m_wrappedLogger.Info(format, obj);
        }

        public void Info(string message, string file, int line)
        {
            this.m_wrappedLogger.Info(message, file, line);
        }

        public void InfoIf(bool condition, string message, string file, int line)
        {
            if (condition)
            {
                this.m_wrappedLogger.Info(message, file, line);
            }
        }

        public void InfoWithoutFormat(params object[] obj)
        {
            this.m_wrappedLogger.InfoWithoutFormat(obj);
        }

        public void Log(string message, LogLevel level)
        {
            this.m_wrappedLogger.Log(level, message, new object[0]);
        }

        public void Log(LogLevel level, string format, params object[] obj)
        {
            this.m_wrappedLogger.Log(level, format, obj);
        }

        public void Log(string message, LogLevel level, string file, int line)
        {
            this.m_wrappedLogger.Log(level, message, new object[] { file, line });
        }

        public void LogIf(bool condition, string message, LogLevel level, string file, int line)
        {
            if (condition)
            {
                this.m_wrappedLogger.Log(level, message, new object[] { file, line });
            }
        }

        public void LogWithoutFormat(LogLevel level, params object[] obj)
        {
            this.m_wrappedLogger.LogWithoutFormat(level, obj);
        }

        public void Verbose(string message)
        {
            this.m_wrappedLogger.Verbose(message);
        }

        public void Verbose(string format, params object[] obj)
        {
            this.m_wrappedLogger.Verbose(format, obj);
        }

        public void Verbose(string message, string file, int line)
        {
            this.m_wrappedLogger.Verbose(message, file, line);
        }

        public void VerboseIf(bool condition, string message, string file, int line)
        {
            if (condition)
            {
                this.m_wrappedLogger.Verbose(message, file, line);
            }
        }

        public void VerboseWithoutFormat(params object[] obj)
        {
            this.m_wrappedLogger.VerboseWithoutFormat(obj);
        }

        public void Warning(string message)
        {
            this.m_wrappedLogger.Warning(message);
        }

        public void Warning(string format, params object[] obj)
        {
            this.m_wrappedLogger.Warning(format, obj);
        }

        public void Warning(string message, string file, int line)
        {
            this.m_wrappedLogger.Warning(message, file, line);
        }

        public void WarningIf(bool condition, string message, string file, int line)
        {
            if (condition)
            {
                this.m_wrappedLogger.Warning(message, file, line);
            }
        }

        public void WarningWithoutFormat(params object[] obj)
        {
            this.m_wrappedLogger.WarningWithoutFormat(obj);
        }

        // Properties
        public LogLevel CurrentLevel
        {
            get
            {
                return this.m_wrappedLogger.CurrentLevel;
            }
            set
            {
                this.m_wrappedLogger.CurrentLevel = value;
            }
        }

        internal bool IsDisposed
        {
            get
            {
                return this.m_disposed;
            }
        }

        #region ILogger Members


        public void LogException(string message, System.Exception exc, params object[] obj)
        {
            this.m_wrappedLogger.LogException(message, exc, obj);
        }

        #endregion
    }





}
