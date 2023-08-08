using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Security;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing
{
    public class AppContext
    {
        private static object initLock = new object();
        private static bool s_isInitialized;
        private ILogger logger;
        private DomainLogger domainLogger;
        private static AppContext current;
        public ServerConfiguration Configuration { get; private set; }

        public static AppContext Current
        {
            get
            {
                return current;
            }
        }

        public static void Initialize(string logFilePath, string appName, ServerConfiguration configuration, LogLevel customizedLogLevel)
        {
            lock (initLock)
            {
                if (s_isInitialized)
                    return;
                current = new AppContext();
                FileLogger logger = new FileLogger(Path.GetFileNameWithoutExtension(logFilePath), configuration.LogConfiguration.DateTimeFormat);
                FileInfo info = new FileInfo(logFilePath);
                StreamLogger.BackupLog(logFilePath);
                logger.Init(customizedLogLevel, info.Open(FileMode.Create, FileAccess.Write, FileShare.Read));
                logger.LogRotator = new LogRotator(info.FullName, configuration.LogConfiguration.FileCount, (configuration.LogConfiguration.FileSize * 0x400) * 0x400, configuration.LogConfiguration.FileCheckFrequency, 0);
                current.logger = logger;
                current.domainLogger = new DomainLogger(logger, appName);
                s_isInitialized = true;
            }
        }

        public static ILogger GetLogger(string name)
        {
            return current.domainLogger.GetLogger(name);
        }

        public static ILogger Logger
        {
            get
            {
                return current.logger;
            }
        }

        public static bool IsInitialized
        {
            get
            {
                lock (initLock)
                {
                    return s_isInitialized;
                }
            }
            private set
            {
                lock (initLock)
                {
                    s_isInitialized = value;
                }
            }
        }

    }
}
