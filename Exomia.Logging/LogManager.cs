#region MIT License

// Copyright (c) 2019 exomia - Daniel Bätz
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Exomia.Logging
{
    /// <summary>
    ///     LogManager class
    /// </summary>
    public static class LogManager
    {
        private static readonly Dictionary<Type, Logger> s_typeLoggers;
        private static Logger[] s_loggers;
        private static int s_loggerCount;
        private static readonly Thread s_mainThread;

        /// <summary>
        ///     Define the log directory for all file appenders
        /// </summary>
        public static string LogDirectory { get; set; } = "./";

        /// <summary>
        ///     MaxLogAge
        /// </summary>
        public static int MaxLogAge { get; set; } = 5 * 1000;

        /// <summary>
        ///     MaxQueueSize
        /// </summary>
        public static int MaxQueueSize { get; set; } = 100;

        static LogManager()
        {
            s_mainThread = Thread.CurrentThread;
            s_typeLoggers = new Dictionary<Type, Logger>(16);
            s_loggers = new Logger[16];
            new Thread(LoggingThread)
            {
                Name         = "Exomia.Logging.LogManager",
                Priority     = ThreadPriority.Lowest,
                IsBackground = false
            }.Start();
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="logAppender"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static ILogger GetLogger(Type type, LogAppender logAppender = LogAppender.File | LogAppender.Console,
            string className = null)
        {
            if (string.IsNullOrEmpty(className))
            {
                className = !type.IsGenericType
                    ? type.Name
                    : Regex.Replace(
                        type.Name, "`[0-9]+", string.Join(" ", type.GetGenericArguments().Select(v => v.Name)));
            }

            lock (s_typeLoggers)
            {
                if (!s_typeLoggers.TryGetValue(type, out Logger logger))
                {
                    List<IAppender> appenders = new List<IAppender>();
                    if ((logAppender & LogAppender.File) == LogAppender.File)
                    {
                        appenders.Add(new FileAppender(className, LogDirectory, MaxQueueSize));
                    }
                    if ((logAppender & LogAppender.Console) == LogAppender.Console)
                    {
                        appenders.Add(new ConsoleAppender(className));
                    }
                    logger = new Logger(appenders.ToArray());
                    logger.PrepareLogging(DateTime.Now);
                    s_typeLoggers.Add(type, logger);

                    if (s_loggerCount + 1 >= s_loggers.Length)
                    {
                        Array.Resize(ref s_loggers, s_loggers.Length * 2);
                    }
                    s_loggers[s_loggerCount] = logger;
                    s_loggerCount++;
                }

                return logger;
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logAppender"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static ILogger GetLogger<T>(LogAppender logAppender = LogAppender.File | LogAppender.Console,
            string className = null)
            where T : class
        {
            return GetLogger(typeof(T), logAppender, className);
        }

        private static void LoggingThread()
        {
            DateTime current = DateTime.Now;
            Stopwatch sw = new Stopwatch();
            
            while (s_mainThread.IsAlive)
            {
                DateTime now = DateTime.Now;
                if (current.Day != now.Day)
                {
                    current = now;
                    for (int i = s_loggerCount - 1; i >= 0 && s_mainThread.IsAlive; i--)
                    {
                        Logger logger = s_loggers[i];
                        if (logger != null)
                        {
                            logger.Flush(true);
                            logger.PrepareLogging(now);
                        }
                    }
                }
                sw.Restart();
                while(sw.Elapsed.TotalMilliseconds < MaxLogAge)
                {
                    for (int i = s_loggerCount - 1; i >= 0 && s_mainThread.IsAlive; i--)
                    {
                        s_loggers[i]?.Flush(false);
                    }
                    Thread.Sleep(1);
                }
                for (int i = s_loggerCount - 1; i >= 0 && s_mainThread.IsAlive; i--)
                {
                    s_loggers[i]?.Flush(true);
                }
            }

            for (int i = s_loggerCount - 1; i >= 0; i--)
            {
                s_loggers[i]?.Dispose();
                s_loggers[i] = null;
            }
            s_loggerCount = 0;
        }
    }
}