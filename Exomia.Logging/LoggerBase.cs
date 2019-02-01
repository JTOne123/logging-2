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
using System.Runtime.CompilerServices;

namespace Exomia.Logging
{
    /// <inheritdoc />
    abstract class LoggerBase : ILogger
    {
        /// <summary>
        /// </summary>
        protected readonly string _className;

        internal readonly Queue<string> _queue;
        internal Queue<string> _tempQueue;
        private LogMethod _logMethod = LogMethod.Both;

        /// <inheritdoc />
        public LogMethod LogMethod
        {
            get { return _logMethod; }
            set { _logMethod = value; }
        }

        /// <inheritdoc />
        protected LoggerBase(string className)
        {
            _className = className;
            _queue     = new Queue<string>();
        }

        /// <inheritdoc />
        public void Trace(Exception ex, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Trace, ex.StackTrace, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc />
        public void Trace(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Trace, message, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc />
        public void Debug(Exception ex, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Debug, ex.StackTrace, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc />
        public void Debug(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Debug, message, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc />
        public void Info(Exception ex, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Info, ex.StackTrace, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc />
        public void Info(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Info, message, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc />
        public void Warning(Exception ex, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Warning, ex.StackTrace, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc />
        public void Warning(string message, string memberName = "", string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            Internal(LogType.Warning, message, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc />
        public void Error(Exception ex, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Error, ex.StackTrace, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc />
        public void Error(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Error, message, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc />
        public void Flush()
        {
            if ((_logMethod & LogMethod.Default) == LogMethod.Default)
            {
                lock (_queue)
                {
                    _tempQueue = new Queue<string>(_queue);
                    _queue.Clear();
                }
                while (_tempQueue.Count > 0)
                {
                    Flush(_tempQueue.Dequeue());
                }
                OnFlushFinished();
            }
        }

        /// <summary>
        ///     called at the end of a flush
        /// </summary>
        public abstract void OnFlushFinished();

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        public abstract void Flush(string entry);

        internal abstract void PrepareLogging(DateTime dateTime);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Internal(LogType logType, string message, string memberName, string sourceFilePath,
            int sourceLineNumber)
        {
            message =
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}|{_className}|{logType} [{memberName}|{sourceFilePath}:{sourceLineNumber}] {message}";
            if ((_logMethod & LogMethod.Default) == LogMethod.Default)
            {
                lock (_queue)
                {
                    _queue.Enqueue(message);
                }
            }
            if ((_logMethod & LogMethod.Console) == LogMethod.Console)
            {
                ConsoleColor current = Console.ForegroundColor;
                switch (logType)
                {
                    case LogType.Trace:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                    case LogType.Debug:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case LogType.Info:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case LogType.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogType.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }
                Console.Out.WriteLine(message);
                Console.ForegroundColor = current;
            }
        }

        #region IDisposable Support

        private bool _disposedValue;

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Flush();
                }
                OnDispose(disposing);
                _disposedValue = true;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void OnDispose(bool disposing) { }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}