using System;

namespace GGJ2017.Shared.Logging
{
    public static class Log
    {
        public static Action<string> logHandler = Console.WriteLine;

        public enum Severity
        {
            Debug,
            Info,
            Warning,
            Error,
            Fatal,
        }

        public static void Assert(bool condition, string message)
        {
            if (!condition)
            {                
                HandleLog(Severity.Error, message);
            }
        }

        public static void AssertFormat(bool condition, string message, params object[] pars)
        {
            Assert(condition, string.Format(message, pars));
        }

        public static void Debug(string message)
        {
            HandleLog(Severity.Debug, message);
        }

        public static void DebugFormat(string message, params object[] pars)
        {
            Debug(string.Format(message, pars));
        }

        public static void Info(string message)
        {
            HandleLog(Severity.Info, message);
        }

        public static void InfoFormat(string message, params object[] pars)
        {
            Info(string.Format(message, pars));
        }

        public static void Warning(string message)
        {
            HandleLog(Severity.Warning, message);
        }

        public static void WarningFormat(string message, params object[] pars)
        {
            Warning(string.Format(message, pars));
        }

        public static void Error(string message)
        {
            HandleLog(Severity.Error, message);
        }

        public static void ErrorFormat(string message, params object[] pars)
        {
            Error(string.Format(message, pars));
        }

        public static void Fatal(string message)
        {
            HandleLog(Severity.Fatal, message);
        }

        public static void FatalFormat(string message, params object[] pars)
        {
            Fatal(string.Format(message, pars));
        }

        static void HandleLog(Severity severity, string message)
        {
            logHandler(FormatMessage(severity, message));
        }

        static string FormatMessage(Severity severity, string message)
        {
            return string.Format("{0} [{1}] {2}", DateTime.UtcNow, severity.ToString().ToUpper(), message);
        }
    }
}