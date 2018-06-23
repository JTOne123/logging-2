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
using System.IO;
using System.Text;

namespace Exomia.Logging
{
    /// <inheritdoc />
    public sealed class FileLogger : LoggerBase
    {
        private readonly string _currentLogDirectory;
        private string _currentLogFile = string.Empty;

        private FileStream _fileStream;

        /// <summary>
        /// </summary>
        public string CurrentLogDirectory
        {
            get { return _currentLogDirectory; }
        }

        /// <summary>
        /// </summary>
        public string CurrentLogFile
        {
            get { return _currentLogFile; }
        }

        internal FileLogger(string className, string logDirectory)
            : base(className)
        {
            _currentLogDirectory = logDirectory;
        }

        /// <inheritdoc />
        public override void Flush(string entry)
        {
            byte[] b = Encoding.Default.GetBytes(entry + Environment.NewLine);
            _fileStream.Write(b, 0, b.Length);
        }

        /// <inheritdoc />
        public override void OnFlushFinished()
        {
            _fileStream.Flush();
        }

        /// <inheritdoc />
        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (_fileStream != null)
                {
                    _fileStream.Flush();
                    _fileStream.Close();
                    _fileStream.Dispose();
                    _fileStream = null;
                }
            }
        }

        internal override void PrepareLogging(DateTime dateTime)
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
    }
}