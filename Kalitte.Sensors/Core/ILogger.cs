namespace Kalitte.Sensors.Core
{
    using System;
    using Kalitte.Sensors.Security;

    public interface ILogger
    {
        void Error(string message);
        void Error(string format, params object[] obj);
        void Error(string message, string file, int line);
        void ErrorIf(bool condition, string message, string file, int line);
        void ErrorWithoutFormat(params object[] obj);
        void Fatal(string message);
        void Fatal(string format, params object[] obj);
        void Fatal(string message, string file, int line);
        void FatalIf(bool condition, string message, string file, int line);
        void FatalWithoutFormat(params object[] obj);
        void Info(string message);
        void Info(string format, params object[] obj);
        void Info(string message, string file, int line);
        void InfoIf(bool condition, string message, string file, int line);
        void InfoWithoutFormat(params object[] obj);
        void Log(string message, LogLevel LogLevel);
        void Log(LogLevel LogLevel, string format, params object[] obj);
        void Log(string message, LogLevel LogLevel, string file, int line);
        void LogIf(bool condition, string message, LogLevel LogLevel, string file, int line);
        void LogWithoutFormat(LogLevel LogLevel, params object[] obj);
        void Verbose(string message);
        void Verbose(string format, params object[] obj);
        void Verbose(string message, string file, int line);
        void VerboseIf(bool condition, string message, string file, int line);
        void VerboseWithoutFormat(params object[] obj);
        void Warning(string message);
        void Warning(string format, params object[] obj);
        void Warning(string message, string file, int line);
        void WarningIf(bool condition, string message, string file, int line);
        void WarningWithoutFormat(params object[] obj);
        void LogException(string message, Exception exc, params object[] obj);

        LogLevel CurrentLevel { get; set; }
    }
}
