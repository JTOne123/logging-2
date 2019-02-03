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
using System.Runtime.CompilerServices;

namespace Exomia.Logging
{
    /// <inheritdoc />
    sealed class Logger : ILogger
    {
        private readonly IAppender[] _appenders;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Internal(LogType logType, string message, string memberName, string sourceFilePath,
            int sourceLineNumber)
        {
            for (int i = 0; i < _appenders.Length; i++)
            {
                _appenders[i].Enqueue(logType, message, memberName, sourceFilePath, sourceLineNumber);
            }
        }

        /// <inheritdoc />
        public void Flush(bool force)
        {
            for (int i = 0; i < _appenders.Length; i++)
            {
                _appenders[i].Flush(force);
            }
        }

        public void PrepareLogging(DateTime dateTime)
        {
            for (int i = 0; i < _appenders.Length; i++)
            {
                _appenders[i].PrepareLogging(dateTime);
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
                    for (int i = 0; i < _appenders.Length; i++)
                    {
                        _appenders[i].Dispose();
                    }
                }
                _disposedValue = true;
            }
        }

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