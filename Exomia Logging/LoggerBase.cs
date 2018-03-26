using System;
using System.Collections.Generic;

namespace Exomia.Logging
{
    /// <inheritdoc />
    public abstract class LoggerBase : ILogger
    {
        private LogMethod _logMethod = LogMethod.Both;

        /// <summary>
        /// 
        /// </summary>
        protected readonly string _className = string.Empty;

        internal readonly Queue<string> _queue = null;
        internal Queue<string> _tempQueue = null;

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
            _queue = new Queue<string>();
        }

        /// <inheritdoc />
        public void Info(Exception ex)
        {
            if ((_logMethod & LogMethod.Default) == LogMethod.Default)
            {
                lock (_queue)
                {
                    _queue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Info}] {ex.StackTrace}");
                }
            }
            if ((_logMethod & LogMethod.Console) == LogMethod.Console)
            {
                Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Info}] {ex.StackTrace}");
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
                    _queue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Warning}] {ex.StackTrace}");
                }
            }
            if ((_logMethod & LogMethod.Console) == LogMethod.Console)
            {
                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Warning}] {ex.StackTrace}");
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
                Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Warning}] {message}");
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
                Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Warning}] {format}");
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
                    _queue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Error}] {ex.StackTrace}");
                }
            }
            if ((_logMethod & LogMethod.Console) == LogMethod.Console)
            {
                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_className}] [{LogType.Error}] {ex.StackTrace}");
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

        internal abstract void PrepareLogging(DateTime dateTime);

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
        /// called at the end of a flush
        /// </summary>
        public abstract void OnFlushFinished();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        public abstract void Flush(string entry);

        #region IDisposable Support
        private bool _disposedValue = false;

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
        /// 
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
