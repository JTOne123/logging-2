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
        private static readonly Dictionary<Type, LoggerBase> s_typeLoggers;
        private static LoggerBase[] s_loggers;
        private static int s_loggerCount;
        private static readonly Thread s_loggingThread;
        private static readonly Thread s_mainThread;
        private static bool s_exit;

        private static int s_maxLogAge = 2 * 1000;
        private static int s_maxQueueSize = 100;

        /// <summary>
        ///     MaxLogAge
        /// </summary>
        public static int MaxLogAge
        {
            get { return s_maxLogAge; }
            set { s_maxLogAge = value; }
        }

        /// <summary>
        ///     MaxQueueSize
        /// </summary>
        public static int MaxQueueSize
        {
            get { return s_maxQueueSize; }
            set { s_maxQueueSize = value; }
        }

        static LogManager()
        {
            s_mainThread  = Thread.CurrentThread;
            s_typeLoggers = new Dictionary<Type, LoggerBase>(16);
            s_loggers     = new LoggerBase[16];
            s_loggingThread = new Thread(LoggingThread)
            {
                Name = "Exomia.Logging.LogManager", Priority = ThreadPriority.Lowest, IsBackground = true
            };
        }

        /// <summary>
        ///     start logging
        /// </summary>
        public static void Start()
        {
            s_loggingThread.Start();
        }

        /// <summary>
        ///     stop logging
        /// </summary>
        public static void Stop()
        {
            s_exit = true;
            s_loggingThread.Join();
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="directory"></param>
        /// <param name="logMethod"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static ILogger GetFileLogger(Type type, string directory = "./", LogMethod logMethod = LogMethod.Both,
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
                if (!s_typeLoggers.TryGetValue(type, out LoggerBase logger))
                {
                    logger = new FileLogger(className, directory) { LogMethod = logMethod };
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
        /// <param name="directory"></param>
        /// <param name="logMethod"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static ILogger GetFileLogger<T>(string directory = "./", LogMethod logMethod = LogMethod.Both,
            string className = null)
            where T : class
        {
            return GetFileLogger(typeof(T), directory, logMethod, className);
        }

        private static void LoggingThread()
        {
            DateTime current = DateTime.Now;

            while (!s_exit && s_mainThread.IsAlive)
            {
                DateTime now = DateTime.Now;
                if (current.Day != now.Day)
                {
                    current = now;
                    for (int i = s_loggerCount - 1; i >= 0 && !s_exit; i--)
                    {
                        LoggerBase logger = s_loggers[i];
                        if (logger != null)
                        {
                            logger.Flush();
                            logger.PrepareLogging(now);
                        }
                    }
                }
                for (int s = 0; s < s_maxLogAge && !s_exit; s++)
                {
                    for (int i = s_loggerCount - 1; i >= 0 && !s_exit; i--)
                    {
                        LoggerBase logger = s_loggers[i];
                        if (logger != null && logger._queue.Count > s_maxQueueSize) { logger.Flush(); }
                    }
                    Thread.Sleep(1);
                }
                for (int i = s_loggerCount - 1; i >= 0 && !s_exit; i--)
                {
                    s_loggers[i]?.Flush();
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