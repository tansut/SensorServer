using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;
using System.IO;
using System.Globalization;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing;

namespace Kalitte.Sensors.Security
{
public abstract class StreamLogger : MarshalBase, ILogger, IDisposable
{
    // Fields
    private const int CALLERS_CALLER = 2;
    private const int CALLERS_CALLERS_CALLER = 3;
    protected const string ComponentName = "componentName";
    protected const string Entry = "entry";
    protected const string File = "file";
    protected const string Line = "line";
    protected const string LoggingLevel = "level";
    protected const string LoggingTime = "time";
    private LogLevel m_currentLevel;
    private bool m_isDisposed;
    private object m_lockObj;
    private ChangeableStreamWriter m_logStream;
    private string m_name;
    protected const string Message = "message";
    protected const string ThreadId = "threadId";

    // Methods
    protected StreamLogger(string name)
    {
        this.m_lockObj = new object();
        this.m_name = name;
    }

    protected StreamLogger(StreamLogger logger, string name)
    {
        this.m_lockObj = new object();
        this.m_currentLevel = logger.CurrentLevel;
        this.m_logStream = logger.m_logStream;
        this.m_lockObj = logger.m_lockObj;
        this.m_name = name;
    }

    public static void BackupLog(string logFilePath)
    {
        if (logFilePath == null)
        {
            throw new ArgumentNullException("logFilePath");
        }
        FileInfo info = new FileInfo(logFilePath);
        if (info.Exists)
        {
            try
            {
                DateTimeFormatInfo provider = new DateTimeFormatInfo();
                provider.FullDateTimePattern = "d_MMM_yy_HH_mm_ss";
                string str = Path.Combine(Path.GetDirectoryName(logFilePath), Path.GetFileNameWithoutExtension(logFilePath)) + info.CreationTime.ToString("F", provider);
                FileInfo info3 = new FileInfo(logFilePath);
                for (int i = 0; info3.Exists; i++)
                {
                    info3 = new FileInfo(string.Format(CultureInfo.InvariantCulture, "{0}_{1}{2}", new object[] { str, i, info.Extension }));
                }
                info.CopyTo(info3.FullName);
                info.CreationTime = DateTime.Now;
            }
            finally
            {
                info.Refresh();
            }
        }
    }

    public abstract StreamLogger Clone(string name);
    public virtual void Close()
    {
        try
        {
            lock (this.LockObject)
            {
                if (!this.IsDisposed)
                {
                    this.IsDisposed = true;
                    this.m_logStream.StreamWriter.Close();
                }
            }
        }
        catch (System.Exception)
        {
        }
    }

    protected void Dispose(bool disposing)
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
            
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
    }



    public void Error(string message)
    {
        if (LogLevel.Error <= this.m_currentLevel)
        {
            this.logWithStackLevel(message, LogLevel.Error, 3);
        }
    }


    public void LogException(string message, System.Exception exc, params object[] obj)
    {
        StringBuilder sb = new StringBuilder();
        SensorCommon.AppendAllInnerExceptions(sb, exc);
        Error(message + " System.Exception details:" + sb.ToString(), obj);
    }


    public void Error(string format, params object[] obj)
    {
        if ((LogLevel.Error <= this.m_currentLevel) && (obj != null))
        {
            StringBuilder sb = new StringBuilder();
            SensorCommon.AppendFormat(sb, format, obj);
            this.logWithStackLevel(sb.ToString(), LogLevel.Error, 3);
        }
    }

    public void Error(string message, string file, int line)
    {
        if (LogLevel.Error <= this.m_currentLevel)
        {
            this.Log(message, LogLevel.Error, file, line);
        }
    }

    public void ErrorIf(bool condition, string message, string file, int line)
    {
        if (condition)
        {
            this.Error(message, file, line);
        }
    }

    public void ErrorWithoutFormat(params object[] obj)
    {
        SensorCommon.ValidateParamsAreNonNull(obj);
        if (LogLevel.Error <= this.m_currentLevel)
        {
            StringBuilder builder = new StringBuilder();
            foreach (object obj2 in obj)
            {
                builder.Append(obj2);
                builder.Append(' ');
            }
            this.logWithStackLevel(builder.ToString(), LogLevel.Error, 3);
        }
    }

    public void Fatal(string message)
    {
        if (LogLevel.Fatal <= this.m_currentLevel)
        {
            this.logWithStackLevel(message, LogLevel.Fatal, 3);
        }
    }

    public void Fatal(string format, params object[] obj)
    {
        if ((LogLevel.Fatal <= this.m_currentLevel) && (obj != null))
        {
            StringBuilder sb = new StringBuilder();
            SensorCommon.AppendFormat(sb, format, obj);
            this.logWithStackLevel(sb.ToString(), LogLevel.Fatal, 3);
        }
    }

    public void Fatal(string message, string file, int line)
    {
        if (LogLevel.Fatal <= this.m_currentLevel)
        {
            this.Log(message, LogLevel.Fatal, file, line);
        }
    }

    public void FatalIf(bool condition, string message, string file, int line)
    {
        if (condition)
        {
            this.Fatal(message, file, line);
        }
    }

    public void FatalWithoutFormat(params object[] obj)
    {
        if (LogLevel.Fatal <= this.m_currentLevel)
        {
            StringBuilder builder = new StringBuilder();
            foreach (object obj2 in obj)
            {
                builder.Append(obj2);
                builder.Append(' ');
            }
            this.logWithStackLevel(builder.ToString(), LogLevel.Fatal, 3);
        }
    }


    public void Info(string message)
    {
        if (LogLevel.Info <= this.m_currentLevel)
        {
            this.logWithStackLevel(message, LogLevel.Info, 3);
        }
    }

    public void Info(string format, params object[] obj)
    {
        if ((LogLevel.Info <= this.m_currentLevel) && (obj != null))
        {
            StringBuilder sb = new StringBuilder();
            SensorCommon.AppendFormat(sb, format, obj);
            this.logWithStackLevel(sb.ToString(), LogLevel.Info, 3);
        }
    }

    public void Info(string message, string file, int line)
    {
        if (LogLevel.Info <= this.m_currentLevel)
        {
            this.Log(message, LogLevel.Info, file, line);
        }
    }

    public void InfoIf(bool condition, string message, string file, int line)
    {
        if (condition)
        {
            this.Info(message, file, line);
        }
    }

    public void InfoWithoutFormat(params object[] obj)
    {
        SensorCommon.ValidateParamsAreNonNull(obj);
        if (LogLevel.Info <= this.m_currentLevel)
        {
            StringBuilder builder = new StringBuilder();
            foreach (object obj2 in obj)
            {
                builder.Append(obj2);
                builder.Append(' ');
            }
            this.logWithStackLevel(builder.ToString(), LogLevel.Info, 3);
        }
    }

    public abstract void Init(LogLevel level, Stream stream);
    protected void InitLogStreamAndLevel(LogLevel level, Stream stream)
    {
        this.m_currentLevel = level;
        this.m_logStream = new ChangeableStreamWriter(new StreamWriter(stream, Encoding.UTF8));
        this.m_logStream.StreamWriter.AutoFlush = true;
    }

    public void Log(string message, LogLevel level)
    {
        if (level <= this.m_currentLevel)
        {
            this.Log(message, level, string.Empty, 0);
        }
    }

    public void Log(LogLevel level, string format, params object[] obj)
    {
        if ((level <= this.m_currentLevel) && (obj != null))
        {
            StringBuilder sb = new StringBuilder();
            SensorCommon.AppendFormat(sb, format, obj);
            this.logWithStackLevel(sb.ToString(), level, 3);
        }
    }

    public abstract void Log(string message, LogLevel level, string file, int line);

    public void LogIf(bool condition, string message, LogLevel level, string file, int line)
    {
        if (condition)
        {
            this.Log(message, level, file, line);
        }
    }

    public void LogWithoutFormat(LogLevel level, params object[] obj)
    {
        SensorCommon.ValidateParamsAreNonNull(obj);
        if (level <= this.m_currentLevel)
        {
            StringBuilder builder = new StringBuilder();
            foreach (object obj2 in obj)
            {
                builder.Append(obj2);
                builder.Append(' ');
            }
            this.logWithStackLevel(builder.ToString(), level, 3);
        }
    }

    private void logWithStackLevel(string message, LogLevel level, int stackLevel)
    {
        if (level <= this.m_currentLevel)
        {
            this.Log(message, level, string.Empty, 0);
        }
    }

    public void Verbose(string message)
    {
        if (LogLevel.Verbose <= this.m_currentLevel)
        {
            this.logWithStackLevel(message, LogLevel.Verbose, 3);
        }
    }

    public void Verbose(string format, params object[] obj)
    {
        if ((LogLevel.Verbose <= this.m_currentLevel) && (obj != null))
        {
            StringBuilder sb = new StringBuilder();
            SensorCommon.AppendFormat(sb, format, obj);
            this.logWithStackLevel(sb.ToString(), LogLevel.Verbose, 3);
        }
    }

    public void Verbose(string message, string file, int line)
    {
        if (LogLevel.Verbose <= this.m_currentLevel)
        {
            this.Log(message, LogLevel.Verbose, file, line);
        }
    }

    public void VerboseIf(bool condition, string message, string file, int line)
    {
        if (condition)
        {
            this.Verbose(message, file, line);
        }
    }

    public void VerboseWithoutFormat(params object[] obj)
    {
        SensorCommon.ValidateParamsAreNonNull(obj);
        if (LogLevel.Verbose <= this.m_currentLevel)
        {
            StringBuilder builder = new StringBuilder();
            foreach (object obj2 in obj)
            {
                builder.Append(obj2);
                builder.Append(' ');
            }
            this.logWithStackLevel(builder.ToString(), LogLevel.Verbose, 3);
        }
    }

    public void Warning(string message)
    {
        if (LogLevel.Warning <= this.m_currentLevel)
        {
            this.logWithStackLevel(message, LogLevel.Warning, 3);
        }
    }

    public void Warning(string format, params object[] obj)
    {
        if ((LogLevel.Warning <= this.m_currentLevel) && (obj != null))
        {
            StringBuilder sb = new StringBuilder();
            SensorCommon.AppendFormat(sb, format, obj);
            this.logWithStackLevel(sb.ToString(), LogLevel.Warning, 3);
        }
    }

    public void Warning(string message, string file, int line)
    {
        if (LogLevel.Warning <= this.m_currentLevel)
        {
            this.Log(message, LogLevel.Warning, file, line);
        }
    }

    public void WarningIf(bool condition, string message, string file, int line)
    {
        if (condition)
        {
            this.Warning(message, file, line);
        }
    }

    public void WarningWithoutFormat(params object[] obj)
    {
        SensorCommon.ValidateParamsAreNonNull(obj);
        if (LogLevel.Warning <= this.m_currentLevel)
        {
            StringBuilder builder = new StringBuilder();
            foreach (object obj2 in obj)
            {
                builder.Append(obj2);
                builder.Append(' ');
            }
            this.logWithStackLevel(builder.ToString(), LogLevel.Warning, 3);
        }
    }

    // Properties
    public LogLevel CurrentLevel
    {
        get
        {
            return this.m_currentLevel;
        }
        set
        {
            this.m_currentLevel = value;
        }
    }

    protected bool IsDisposed
    {
        get
        {
            return this.m_isDisposed;
        }
        set
        {
            this.m_isDisposed = value;
        }
    }

    protected object LockObject
    {
        get
        {
            return this.m_lockObj;
        }
    }

    protected ChangeableStreamWriter LogStream
    {
        get
        {
            return this.m_logStream;
        }
        set
        {
            this.m_logStream = value;
        }
    }

    protected string Name
    {
        get
        {
            return this.m_name;
        }
        set
        {
            this.m_name = value;
        }
    }
}

 

 

}
