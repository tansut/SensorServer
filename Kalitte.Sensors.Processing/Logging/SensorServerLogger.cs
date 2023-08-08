using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kalitte.Sensors.Core;
using System.IO;
using System.Diagnostics;
using System.Collections;
using Kalitte.Sensors.Security;
using Kalitte.Sensors.Exceptions;
using System.Net;
using System.Security;
using Kalitte.Sensors.Processing.Logging;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Logging
{


    internal class SensorServerLogger : MarshalBase, ILogger, ISensorServerLogger
    {
        // Fields
        //private IRfidConfigHandler configHandler;
        internal const string DEFAULT_EVENT_LOG_NAME = "Application";
        private FileInfo logFileInfo;
        //private ComponentLogLevelType[] m_componentLogLevels;
        private LogLevel m_defaultLogLevel;
        private bool m_disposed;
        private LogLevel m_exceptionLogLevel;
        //private IFormatBasedEventLogger m_formatBasedEventLogger;
        private bool m_logNonRfidServerExceptions;
        private Hashtable m_names2Loggers;
        private StreamLogger m_realLogger;
        private object m_syncRoot;
        private ILogger m_wrappedLogger;
        private const string MAIN_LOGGER_NAME = "KalitteSensorServer";
        private const EventLogEntryType NON_RFID_SERVER_EXCEPTION_LOG_LEVEL = EventLogEntryType.Warning;
        //private RfidServerConfiguration serverConfiguration;

        // Methods
        public SensorServerLogger()
        {
            this.m_syncRoot = new object();
            this.m_names2Loggers = new Hashtable();
            this.m_defaultLogLevel = LogLevel.Info;
            this.m_exceptionLogLevel = LogLevel.Warning;
        }

        private SensorServerLogger(SensorServerLogger logger, string name)
        {
            this.m_syncRoot = new object();
            this.m_names2Loggers = new Hashtable();
            this.m_defaultLogLevel = LogLevel.Info;
            this.m_exceptionLogLevel = LogLevel.Warning;
            this.m_wrappedLogger = logger.m_realLogger.Clone(name); //new FileLogger(name);// StreamLogger.GetRfidLogger(logger.m_realLogger.Clone(name));
            //this.m_formatBasedEventLogger = logger.m_formatBasedEventLogger;
            this.m_realLogger = logger.m_realLogger;
            lock (this.m_syncRoot)
            {
                this.m_names2Loggers[name] = this;
                this.setLogLevel(name);
            }
            this.m_exceptionLogLevel = logger.m_exceptionLogLevel;
            this.m_logNonRfidServerExceptions = logger.m_logNonRfidServerExceptions;
        }

        public void Close()
        {
            //((RfidLoggerWrapper) this.m_wrappedLogger).Close();
            //((EventLogger) this.m_formatBasedEventLogger).Close();
            this.m_realLogger.Close();
        }

        //public void DelayedStartup()
        //{
        //    //if (ServerConfiguration.Current.LogConfiguration.FileCount > 1)
        //    //{
        //    //    LogRotator logRotator = new LogRotator(this.logFileInfo.FullName, ServerConfiguration.Current.LogConfiguration.FileCount, (ServerConfiguration.Current.LogConfiguration.FileSize * 0x400), ServerConfiguration.Current.LogConfiguration.FileCheckFrequency, 0);
        //    //    this.setLogRotator(logRotator);
        //    //}

        //    //if ((this.serverConfiguration != null) && (this.serverConfiguration.RfidServerRuntimeConfiguration != null))
        //    //{
        //    //    LogConfiguration logConfiguration = this.serverConfiguration.RfidServerRuntimeConfiguration.logConfiguration;
        //    //    if (logConfiguration == null)
        //    //    {
        //    //        logConfiguration = new LogConfiguration();
        //    //        this.serverConfiguration.RfidServerRuntimeConfiguration.logConfiguration = logConfiguration;
        //    //    }
        //    //    if (logConfiguration.fileCount > 1)
        //    //    {
        //    //        LogRotator logRotator = new LogRotator(this.logFileInfo.FullName, logConfiguration.fileCount, (logConfiguration.fileSize * 0x400) * 0x400, logConfiguration.fileCheckFrequency, logConfiguration.currentBackupIndex);
        //    //        this.setLogRotator(logRotator);
        //    //    }
        //    //    if (this.m_formatBasedEventLogger != null)
        //    //    {
        //    //        ((EventLogger)this.m_formatBasedEventLogger).SpamControlTimePeriodInMinutes = logConfiguration.spamControlTimePeriodInMinutes;
        //    //        ((EventLogger)this.m_formatBasedEventLogger).MaxAllowedDuplicates = logConfiguration.maxAllowedDuplicates;
        //    //    }
        //    //}
        //}

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    this.Close();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
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
                SensorServerLogger logger = (SensorServerLogger)this.m_names2Loggers[name];
                if (logger == null)
                {
                    logger = new SensorServerLogger(this, name);
                    this.m_names2Loggers[name] = logger;
                }
                this.setLogLevel(name);
                return logger;
            }
        }

        //private void HandleConfigurationChange(object source, ConfigurationChangedEventArgs args)
        //{
        //    lock (this.m_syncRoot)
        //    {
        //        if (this.serverConfiguration == null)
        //        {
        //            this.serverConfiguration = ((IRfidConfigHandler) source).ServerConfiguration;
        //        }
        //        this.m_defaultLogLevel = ServerConfigUtilities.ParseLogLevel(this.serverConfiguration.RfidServerBootstrapConfiguration.log.defaultLogLevel);
        //        this.setDateTimeFormat(this.serverConfiguration.RfidServerBootstrapConfiguration);
        //        this.m_componentLogLevels = this.serverConfiguration.RfidServerBootstrapConfiguration.log.component;
        //        if (this.serverConfiguration.RfidServerBootstrapConfiguration.log.component != null)
        //        {
        //            StringCollection strings = new StringCollection();
        //            foreach (ComponentLogLevelType type in this.serverConfiguration.RfidServerBootstrapConfiguration.log.component)
        //            {
        //                strings.Add(type.name);
        //                SensorServerLogger logger = (SensorServerLogger) this.m_names2Loggers[type.name];
        //                if (logger != null)
        //                {
        //                    logger.CurrentLevel = ServerConfigUtilities.ParseLogLevel(type.level);
        //                }
        //            }
        //            foreach (string str in this.m_names2Loggers.Keys)
        //            {
        //                if (!strings.Contains(str))
        //                {
        //                    SensorServerLogger logger2 = (SensorServerLogger) this.m_names2Loggers[str];
        //                    logger2.CurrentLevel = this.m_defaultLogLevel;
        //                }
        //            }
        //            if (this.serverConfiguration.RfidServerBootstrapConfiguration.eventLoggingPolicyForExceptions != null)
        //            {
        //                foreach (SensorServerLogger logger3 in this.m_names2Loggers.Values)
        //                {
        //                    if (logger3 != null)
        //                    {
        //                        logger3.m_exceptionLogLevel = ServerConfigUtilities.ParseLogLevel(this.serverConfiguration.RfidServerBootstrapConfiguration.eventLoggingPolicyForExceptions.minimumExceptionSeverityForLogging);
        //                        logger3.m_logNonRfidServerExceptions = this.serverConfiguration.RfidServerBootstrapConfiguration.eventLoggingPolicyForExceptions.logNonRfidServerExceptions;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

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

        //public void Log(EventLogEntryType level, long instanceId, long categoryId, params object[] obj)
        //{
        //    EventInstance instance = new EventInstance(instanceId, (int) categoryId, level);
        //    this.m_formatBasedEventLogger.Log(instance, obj);
        //}

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

        private void logUnusableLogFile(string logFile, System.Exception e)
        {
            //this.Log(EventLogEntryType.Error, RfidEventMessages.UnusableLogFile, RfidEventMessages.ServerManagerCategory, new object[] { logFile, e.Message, @"..\Logs\RfidServices.Log" });
        }

        public void LogWithoutFormat(LogLevel level, params object[] obj)
        {
            this.m_wrappedLogger.LogWithoutFormat(level, obj);
        }

        //private void persistCurrentBackup(object sender, EventArgs e)
        //{
        //    if (this.configHandler != null)
        //    {
        //        RfidServerConfiguration serverConfiguration = this.configHandler.ServerConfiguration;
        //        if (((FileLogger) this.m_realLogger).LogRotator != null)
        //        {
        //            serverConfiguration.RfidServerRuntimeConfiguration.logConfiguration.currentBackupIndex = ((FileLogger) this.m_realLogger).LogRotator.CurrentBackupIndex;
        //        }
        //        this.configHandler.saveConfig();
        //    }
        //}

        //public void Report(SensorException serverException)
        //{
        //    if (this.m_exceptionLogLevel >= serverException.LogLevel)
        //    {
        //        EventLogEntryType error = EventLogEntryType.Error;
        //        switch (serverException.LogLevel)
        //        {
        //            case LogLevel.Warning:
        //                error = EventLogEntryType.Warning;
        //                break;

        //            case LogLevel.Info:
        //            case LogLevel.Verbose:
        //                error = EventLogEntryType.Information;
        //                break;

        //            case LogLevel.Off:
        //                return;

        //            case LogLevel.Fatal:
        //            case LogLevel.Error:
        //                error = EventLogEntryType.Error;
        //                break;
        //        }
        //        if ((serverException.Parameters != null) && (serverException.Parameters.Length != 0))
        //        {
        //            object[] args = new object[serverException.Parameters.Length];
        //            int num = 0;
        //            foreach (object obj2 in serverException.Parameters)
        //            {
        //                args[num++] = obj2;
        //            }
        //            this.Log(error, RfidEventMessages.RfidServiceError, RfidEventMessages.ServerManagerCategory, new object[] { SensorException.GetString(serverException.ErrorCode, args) });
        //        }
        //        else
        //        {
        //            this.Log(error, RfidEventMessages.RfidServiceError, RfidEventMessages.ServerManagerCategory, new object[] { SensorException.GetString(serverException.ErrorCode, new object[0]) });
        //        }
        //    }
        //}

        //public void Report(System.Exception exception)
        //{
        //    SensorException serverException = exception as SensorException;
        //    if (serverException != null)
        //    {
        //        this.Report(serverException);
        //    }
        //    else if (this.m_logNonRfidServerExceptions)
        //    {
        //        EventInstance instance = new EventInstance(RfidEventMessages.RfidServiceError, (int) RfidEventMessages.ServerManagerCategory) {
        //            EntryType = EventLogEntryType.Warning
        //        };
        //        this.m_formatBasedEventLogger.Log(instance, new object[] { RfidCommon.AppendAllInnerExceptionMessages(exception) });
        //    }
        //}

        //private void setDateTimeFormat(RfidServerBootstrapConfiguration bootstrapConfiguration)
        //{
        //    FileLogger realLogger = this.m_realLogger as FileLogger;
        //    if (((realLogger != null) && (bootstrapConfiguration != null)) && ((bootstrapConfiguration.log != null) && (bootstrapConfiguration.log.dateTimeFormat != null)))
        //    {
        //        string dateTimeFormat = bootstrapConfiguration.log.dateTimeFormat;
        //        realLogger.DateTimeFormat = dateTimeFormat;
        //        if (this.m_names2Loggers != null)
        //        {
        //            foreach (SensorServerLogger logger2 in this.m_names2Loggers.Values)
        //            {
        //                if (logger2 != null)
        //                {
        //                    FileLogger logger = ((RfidLoggerWrapper)logger2.m_wrappedLogger).Logger as FileLogger;
        //                    if (logger != null)
        //                    {
        //                        logger.DateTimeFormat = dateTimeFormat;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        private void setLogLevel(string name)
        {
            bool flag = false;
            //if (this.m_componentLogLevels != null)
            //{
            //    foreach (ComponentLogLevelType type in this.m_componentLogLevels)
            //    {
            //        if (type.name.Equals(name))
            //        {
            //            SensorServerLogger logger = (SensorServerLogger) this.m_names2Loggers[name];
            //            logger.CurrentLevel = ServerConfigUtilities.ParseLogLevel(type.level);
            //            flag = true;
            //            break;
            //        }
            //    }
            //}
            if (!flag)
            {
                SensorServerLogger logger2 = (SensorServerLogger)this.m_names2Loggers[name];
                logger2.CurrentLevel = this.m_defaultLogLevel;
            }
        }

        private void setLogRotator(LogRotator logRotator)
        {
            if (this.m_names2Loggers != null)
            {
                foreach (SensorServerLogger logger in this.m_names2Loggers.Values)
                {
                    if (logger != null)
                    {
                        //FileLogger logger2 = ((RfidLoggerWrapper)logger.m_wrappedLogger).Logger as FileLogger;
                        FileLogger logger2 = (logger.m_wrappedLogger) as FileLogger;
                        if (logger2 != null)
                        {
                            logger2.LogRotator = logRotator;
                            //logger2.LogRotatedEvent = (EventHandler<EventArgs>)Delegate.Combine(logger2.LogRotatedEvent, new EventHandler<EventArgs>(this.persistCurrentBackup));
                        }
                    }
                }
            }
        }

        public void Shutdown()
        {
            this.m_disposed = true;
            this.Close();
        }

        public void Startup()
        {
            lock (this.m_syncRoot)
            {
                this.m_names2Loggers["KalitteSensorServer"] = this;
                //this.configHandler = (IRfidConfigHandler)interfaceDictionary["configHandler"];
                //this.configHandler.configurationChangedEvent += new ConfigurationChangeHandler(this.HandleConfigurationChange);
                //RfidServerBootstrapConfiguration bootstrapConfiguration = ((RfidConfigHandler)this.configHandler).BootstrapConfiguration;
                //string logFile = "SensorServer.log";// bootstrapConfiguration.log.logFile;
                //string logFilePath = Path.Combine(DEFAULT_LOG_FILE, logFile); // Path.Combine(ServerUtils.GetRfidAppDir(), logFile);
                //this.m_componentLogLevels = bootstrapConfiguration.log.component;
                string logFilePath = ServerConfiguration.GetItemLogPath("", ServerConfiguration.Current.LogConfiguration.ServerLogFile);
                this.m_defaultLogLevel = ServerConfiguration.Current.LogConfiguration.Level ;//ServerConfigUtilities.ParseLogLevel(bootstrapConfiguration.log.defaultLogLevel);
                //if (bootstrapConfiguration.eventLoggingPolicyForExceptions != null)
                //{
                //    this.m_exceptionLogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), bootstrapConfiguration.eventLoggingPolicyForExceptions.minimumExceptionSeverityForLogging.ToString());
                //    this.m_logNonRfidServerExceptions = bootstrapConfiguration.eventLoggingPolicyForExceptions.logNonRfidServerExceptions;
                //}
                string logName = "Application";
                EventLog eventLog = new EventLog(logName, Dns.GetHostName(), "KalitteSensorServer");
                if (!EventLog.Exists(eventLog.Log))
                {
                    throw new SensorException("NoEventLog");//, new object[] { logName });
                }
                //EventLogger logger = new EventLogger("MSBizTalkRFID", eventLog, this.m_exceptionLogLevel);
                //this.m_formatBasedEventLogger = logger;
                FileInfo info = null;
                try
                {
                    StreamLogger.BackupLog(logFilePath);
                    FileInfo info2 = new FileInfo(logFilePath);
                    info2.Open(FileMode.Create, FileAccess.Write, FileShare.Read).Close();
                    info = info2;
                }
                catch (DirectoryNotFoundException exception)
                {
                    this.logUnusableLogFile(logFilePath, exception);
                }
                catch (ArgumentNullException exception2)
                {
                    this.logUnusableLogFile(logFilePath, exception2);
                }
                catch (SecurityException exception3)
                {
                    this.logUnusableLogFile(logFilePath, exception3);
                }
                catch (ArgumentException exception4)
                {
                    this.logUnusableLogFile(logFilePath, exception4);
                }
                catch (UnauthorizedAccessException exception5)
                {
                    this.logUnusableLogFile(logFilePath, exception5);
                }
                catch (PathTooLongException exception6)
                {
                    this.logUnusableLogFile(logFilePath, exception6);
                }
                catch (NotSupportedException exception7)
                {
                    this.logUnusableLogFile(logFilePath, exception7);
                }
                finally
                {
                    if (info == null)
                    {
                        info = new FileInfo(logFilePath);// Environment.CurrentDirectory + Path.DirectorySeparatorChar + @"..\Log\..\SensorServer.Log");
                    }
                }
                this.m_realLogger = new FileLogger("KalitteSensorServer", ServerConfiguration.Current.LogConfiguration.DateTimeFormat);
                this.m_realLogger.Init(this.m_defaultLogLevel, info.Open(FileMode.Create, FileAccess.Write, FileShare.Read));
                this.logFileInfo = info;
                this.m_wrappedLogger = this.m_realLogger; //StreamLogger.GetRfidLogger(this.m_realLogger);
                //this.setDateTimeFormat(bootstrapConfiguration);
                this.setLogLevel("KalitteSensorServer");
                this.m_disposed = false;
                if (ServerConfiguration.Current.LogConfiguration.FileCount > 1)
                {
                    LogRotator logRotator = new LogRotator(this.logFileInfo.FullName, ServerConfiguration.Current.LogConfiguration.FileCount, (ServerConfiguration.Current.LogConfiguration.FileSize * 0x400) * 0x400, ServerConfiguration.Current.LogConfiguration.FileCheckFrequency, 0);
                    this.setLogRotator(logRotator);
                }
            }
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

        //internal RfidServerConfiguration ServerConfiguration
        //{
        //    get
        //    {
        //        return this.serverConfiguration;
        //    }
        //    set
        //    {
        //        this.serverConfiguration = value;
        //    }
        //}

        #region ISensorServerLogger Members


        public void Report(SensorException sensorException)
        {
            
        }

        public void Report(System.Exception exception)
        {
            
        }

        #endregion

        #region ILogger Members


        public void LogException(string message, System.Exception exc, params object[] obj)
        {
            this.m_wrappedLogger.LogException(message, exc, obj);
        }

        #endregion
    }





}
