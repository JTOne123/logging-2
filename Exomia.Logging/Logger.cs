#region License

// Copyright (c) 2018-2019, exomia
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

#endregion

using System;
using System.Runtime.CompilerServices;

namespace Exomia.Logging
{
    /// <summary>
    ///     A logger. This class cannot be inherited.
    /// </summary>
    sealed class Logger : ILogger
    {
        /// <summary>
        ///     The appenders.
        /// </summary>
        private readonly IAppender[] _appenders;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Logger" /> class.
        /// </summary>
        /// <param name="appenders"> The appenders. </param>
        public Logger(IAppender[] appenders)
        {
            _appenders = appenders;
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
                            int    sourceLineNumber = 0)
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
        public void Flush(bool force)
        {
            for (int i = 0; i < _appenders.Length; i++)
            {
                _appenders[i].Flush(force);
            }
        }

        /// <summary>
        ///     Prepare logging.
        /// </summary>
        /// <param name="dateTime"> The date time. </param>
        public void PrepareLogging(DateTime dateTime)
        {
            for (int i = 0; i < _appenders.Length; i++)
            {
                _appenders[i].PrepareLogging(dateTime);
            }
        }

        /// <summary>
        ///     Internals.
        /// </summary>
        /// <param name="logType">          Type of the log. </param>
        /// <param name="message">          The message. </param>
        /// <param name="memberName">       Name of the member. </param>
        /// <param name="sourceFilePath">   Full pathname of the source file. </param>
        /// <param name="sourceLineNumber"> Source line number. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Internal(LogType logType, string message, string memberName, string sourceFilePath,
                              int     sourceLineNumber)
        {
            for (int i = 0; i < _appenders.Length; i++)
            {
                _appenders[i].Enqueue(logType, message, memberName, sourceFilePath, sourceLineNumber);
            }
        }

        #region IDisposable Support

        /// <summary>
        ///     True to disposed value.
        /// </summary>
        private bool _disposedValue;

        /// <summary>
        ///     Releases the unmanaged resources used by the Exomia.Logging.Logger and optionally
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
                if (disposing)
                {
                    for (int i = 0; i < _appenders.Length; i++)
                    {
                        _appenders[i].Dispose();
                    }
                }
                _disposedValue = true;
            }
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="Logger" /> class.
        /// </summary>
        ~Logger()
        {
            Dispose(false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}