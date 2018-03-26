using System;

namespace Exomia.Logging
{
    public enum LogType
    {
        Info,
        Warning,
        Error
    }
    public enum LogMethod
    {
        Default = 1 << 0,
        Console = 1 << 1,
        Both = Default | Console
    }


    public interface ILogger : IDisposable
    {
        /// <summary>
        /// get or set the log method
        /// </summary>
        LogMethod LogMethod { get; set; }

        /// <summary>
        /// a info log
        /// </summary>
        /// <param name="message">Message</param>
        void Info(string message);

        /// <summary>
        /// a info log
        /// </summary>
        /// <param name="format">Format</param>
        /// <param name="args">Arguments</param>
        void Info(string format, params object[] args);

        /// <summary>
        /// a info log
        /// </summary>
        /// <param name="ex">Exception</param>
        void Info(Exception ex);

        /// <summary>
        /// a warning log
        /// </summary>
        /// <param name="message">Message</param>
        void Warning(string message);

        /// <summary>
        /// a warning log
        /// </summary>
        /// <param name="format">Format</param>
        /// <param name="args">Arguments</param>
        void Warning(string format, params object[] args);

        /// <summary>
        /// a warning log
        /// </summary>
        /// <param name="ex">Exception</param>
        void Warning(Exception ex);

        /// <summary>
        /// a error log
        /// </summary>
        /// <param name="message">Message</param>
        void Error(string message);

        /// <summary>
        /// a error log
        /// </summary>
        /// <param name="format">Format</param>
        /// <param name="args">Arguments</param>
        void Error(string format, params object[] args);

        /// <summary>
        /// a error log
        /// </summary>
        /// <param name="ex">Exception</param>
        void Error(Exception ex);

        /// <summary>
        /// Flushes the Queue to the physical log file
        /// </summary>
        void Flush();
    }
}
