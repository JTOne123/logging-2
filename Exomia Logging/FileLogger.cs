using System;
using System.IO;
using System.Text;

namespace Exomia.Logging
{
    /// <inheritdoc />
    public sealed class FileLogger : LoggerBase
    {
        private readonly string _currentLogDirectory = string.Empty;
        private string _currentLogFile = string.Empty;

        private FileStream _fileStream = null;

        /// <summary>
        /// 
        /// </summary>
        public string CurrentLogDirectory
        {
            get { return _currentLogDirectory; }
        }

        /// <summary>
        /// 
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
            _fileStream = new FileStream(Path.Combine(_currentLogDirectory, _currentLogFile), FileMode.Append, FileAccess.Write);
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
    }
}
