#region License

// Copyright (c) 2018-2019, exomia
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

#endregion

using System;
using System.IO;
using System.Text;

namespace Exomia.Logging
{
    /// <summary>
    ///     A file appender. This class cannot be inherited.
    /// </summary>
    sealed class FileAppender : IAppender
    {
        /// <summary>
        ///     Name of the class.
        /// </summary>
        private readonly string _className;

        /// <summary>
        ///     Pathname of the current log directory.
        /// </summary>
        private readonly string _currentLogDirectory;

        /// <summary>
        ///     Size of the maximum queue.
        /// </summary>
        private readonly int _maxQueueSize;

        /// <summary>
        ///     The queue.
        /// </summary>
        private readonly Queue<string> _queue;

        /// <summary>
        ///     Queue of temporaries.
        /// </summary>
        private readonly Queue<string> _tempQueue;

        /// <summary>
        ///     The current log file.
        /// </summary>
        private string _currentLogFile = string.Empty;

        /// <summary>
        ///     The file stream.
        /// </summary>
        private FileStream _fileStream;

        /// <summary>
        ///     Gets the current log file.
        /// </summary>
        /// <value>
        ///     The current log file.
        /// </value>
        public string CurrentLogFile
        {
            get { return _currentLogFile; }
        }

        /// <inheritdoc />
        public FileAppender(string className, string logDirectory, int maxQueueSize)
        {
            _className           = className;
            _currentLogDirectory = logDirectory;
            _maxQueueSize        = maxQueueSize;

            _queue     = new Queue<string>(32);
            _tempQueue = new Queue<string>(32);
        }

        /// <summary>
        ///     Adds an object onto the end of this queue.
        /// </summary>
        /// <param name="logType">          Type of the log. </param>
        /// <param name="message">          The message. </param>
        /// <param name="memberName">       Name of the member. </param>
        /// <param name="sourceFilePath">   Full pathname of the source file. </param>
        /// <param name="sourceLineNumber"> Source line number. </param>
        public void Enqueue(LogType logType, string message, string memberName, string sourceFilePath,
                            int     sourceLineNumber)
        {
            lock (_queue)
            {
                _queue.Enqueue(
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}|{_className}|{logType} [{memberName}|{sourceFilePath}:{sourceLineNumber}] {message}");
            }
        }

        /// <summary>
        ///     Flushes the given force.
        /// </summary>
        /// <param name="force"> True to force. </param>
        public void Flush(bool force)
        {
            lock (_queue)
            {
                if (!force && _queue.Count < _maxQueueSize) { return; }
                _tempQueue.Clear(_queue);
                _queue.Clear();
            }
            while (_tempQueue.Count > 0)
            {
                byte[] b = Encoding.Default.GetBytes(_tempQueue.Dequeue() + Environment.NewLine);
                _fileStream.Write(b, 0, b.Length);
            }
            _fileStream.Flush();
        }

        /// <summary>
        ///     Prepare logging.
        /// </summary>
        /// <param name="dateTime"> The date time. </param>
        public void PrepareLogging(DateTime dateTime)
        {
            if (_fileStream != null)
            {
                _fileStream.Close();
                _fileStream.Dispose();
                _fileStream = null;
            }
            _currentLogFile = _className + "_" + dateTime.ToString("yyyy-MM-dd") + ".log";
            if (!Directory.Exists(_currentLogDirectory))
            {
                Directory.CreateDirectory(_currentLogDirectory);
            }
            _fileStream = new FileStream(
                Path.Combine(_currentLogDirectory, _currentLogFile), FileMode.Append, FileAccess.Write);
        }

        #region IDisposable Support

        /// <summary>
        ///     True to disposed value.
        /// </summary>
        private bool _disposedValue;

        /// <summary>
        ///     Releases the unmanaged resources used by the Exomia.Logging.FileAppender and optionally
        ///     releases the managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="FileAppender" /> class.
        /// </summary>
        ~FileAppender()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the Exomia.Logging.FileAppender and optionally
        ///     releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to
        ///     release only unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposedValue = true;
                if (disposing)
                {
                    if (_fileStream != null)
                    {
                        Flush(true);
                        _fileStream.Close();
                        _fileStream.Dispose();
                        _fileStream = null;
                    }
                }
            }
        }

        #endregion
    }
}