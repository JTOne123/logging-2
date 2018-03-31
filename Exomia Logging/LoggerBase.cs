#region MIT License

// Copyright (c) 2018 exomia - Daniel Bätz
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

namespace Exomia.Logging
{
    /// <inheritdoc />
    public abstract class LoggerBase : ILogger
    {
        #region Variables

        /// <summary>
        /// </summary>
        protected readonly string _className = string.Empty;

        internal readonly Queue<string> _queue;
        private LogMethod _logMethod = LogMethod.Both;
        internal Queue<string> _tempQueue;

        #endregion

        #region Properties

        /// <inheritdoc />
        public LogMethod LogMethod
        {
            get { return _logMethod; }
            set { _logMethod = value; }
        }

        #endregion

        #region Constructors

        /// <inheritdoc />
        protected LoggerBase(string className)
        {
            _className = className;
            _queue = new Queue<string>();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Info(Exception ex)
        {
            if ((_logMethod & LogMethod.Default) == LogMethod.Default)
            {
                lock (_queue)
                {
                    _queue.Enqueue(
                        $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Info}] {ex.StackTrace}");
                }
            }
            if ((_logMethod & LogMethod.Console) == LogMethod.Console)
            {
                Console.Out.WriteLine(
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Info}] {ex.StackTrace}");
            }
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            if ((_logMethod & LogMethod.Default) == LogMethod.Default)
            {
                lock (_queue)
                {
                    _queue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Info}] {message}");
                }
            }
            if ((_logMethod & LogMethod.Console) == LogMethod.Console)
            {
                Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Info}] {message}");
            }
        }

        /// <inheritdoc />
        public void Info(string format, params object[] args)
        {
            format = string.Format(format, args);
            if ((_logMethod & LogMethod.Default) == LogMethod.Default)
            {
                lock (_queue)
                {
                    _queue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Info}] {format}");
                }
            }
            if ((_logMethod & LogMethod.Console) == LogMethod.Console)
            {
                Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Info}] {format}");
            }
        }

        /// <inheritdoc />
        public void Warning(Exception ex)
        {
            if ((_logMethod & LogMethod.Default) == LogMethod.Default)
            {
                lock (_queue)
                {
                    _queue.Enqueue(
                        $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Warning}] {ex.StackTrace}");
                }
            }
            if ((_logMethod & LogMethod.Console) == LogMethod.Console)
            {
                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Out.WriteLine(
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Warning}] {ex.StackTrace}");
                Console.ForegroundColor = current;
            }
        }

        /// <inheritdoc />
        public void Warning(string message)
        {
            if ((_logMethod & LogMethod.Default) == LogMethod.Default)
            {
                lock (_queue)
                {
                    _queue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Warning}] {message}");
                }
            }
            if ((_logMethod & LogMethod.Console) == LogMethod.Console)
            {
                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Out.WriteLine(
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Warning}] {message}");
                Console.ForegroundColor = current;
            }
        }

        /// <inheritdoc />
        public void Warning(string format, params object[] args)
        {
            format = string.Format(format, args);
            if ((_logMethod & LogMethod.Default) == LogMethod.Default)
            {
                lock (_queue)
                {
                    _queue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Warning}] {format}");
                }
            }
            if ((_logMethod & LogMethod.Console) == LogMethod.Console)
            {
                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Out.WriteLine(
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Warning}] {format}");
                Console.ForegroundColor = current;
            }
        }

        /// <inheritdoc />
        public void Error(Exception ex)
        {
            if ((_logMethod & LogMethod.Default) == LogMethod.Default)
            {
                lock (_queue)
                {
                    _queue.Enqueue(
                        $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Error}] {ex.StackTrace}");
                }
            }
            if ((_logMethod & LogMethod.Console) == LogMethod.Console)
            {
                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine(
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Error}] {ex.StackTrace}");
                Console.ForegroundColor = current;
            }
        }

        /// <inheritdoc />
        public void Error(string message)
        {
            if ((_logMethod & LogMethod.Default) == LogMethod.Default)
            {
                lock (_queue)
                {
                    _queue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Error}] {message}");
                }
            }
            if ((_logMethod & LogMethod.Console) == LogMethod.Console)
            {
                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Error}] {message}");
                Console.ForegroundColor = current;
            }
        }

        /// <inheritdoc />
        public void Error(string format, params object[] args)
        {
            format = string.Format(format, args);
            if ((_logMethod & LogMethod.Default) == LogMethod.Default)
            {
                _queue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{LogType.Error}] {format}");
            }
            if ((_logMethod & LogMethod.Console) == LogMethod.Console)
            {
                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Error}] {format}");
                Console.ForegroundColor = current;
            }
        }

        /// <inheritdoc />
        public void Flush()
        {
            lock (_queue)
            {
                _tempQueue = new Queue<string>(_queue);
                _queue.Clear();
            }
            while (_tempQueue.Count > 0)
            {
                string entry = _tempQueue.Dequeue();
                if ((_logMethod & LogMethod.Default) == LogMethod.Default)
                {
                    Flush(entry);
                }
            }
            OnFlushFinished();
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

        #endregion

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