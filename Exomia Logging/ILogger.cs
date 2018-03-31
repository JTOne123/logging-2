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

namespace Exomia.Logging
{
    /// <summary>
    ///     LogType
    /// </summary>
    public enum LogType
    {
        /// <summary>
        ///     Info
        /// </summary>
        Info,

        /// <summary>
        ///     Warning
        /// </summary>
        Warning,

        /// <summary>
        ///     Error
        /// </summary>
        Error
    }

    /// <summary>
    ///     LogMethod
    /// </summary>
    [Flags]
    public enum LogMethod
    {
        /// <summary>
        /// </summary>
        Default = 1 << 0,

        /// <summary>
        /// </summary>
        Console = 1 << 1,

        /// <summary>
        /// </summary>
        Both = Default | Console
    }

    /// <inheritdoc />
    public interface ILogger : IDisposable
    {
        #region Properties

        /// <summary>
        ///     get or set the log method
        /// </summary>
        LogMethod LogMethod { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     a info log
        /// </summary>
        /// <param name="message">Message</param>
        void Info(string message);

        /// <summary>
        ///     a info log
        /// </summary>
        /// <param name="format">Format</param>
        /// <param name="args">Arguments</param>
        void Info(string format, params object[] args);

        /// <summary>
        ///     a info log
        /// </summary>
        /// <param name="ex">Exception</param>
        void Info(Exception ex);

        /// <summary>
        ///     a warning log
        /// </summary>
        /// <param name="message">Message</param>
        void Warning(string message);

        /// <summary>
        ///     a warning log
        /// </summary>
        /// <param name="format">Format</param>
        /// <param name="args">Arguments</param>
        void Warning(string format, params object[] args);

        /// <summary>
        ///     a warning log
        /// </summary>
        /// <param name="ex">Exception</param>
        void Warning(Exception ex);

        /// <summary>
        ///     a error log
        /// </summary>
        /// <param name="message">Message</param>
        void Error(string message);

        /// <summary>
        ///     a error log
        /// </summary>
        /// <param name="format">Format</param>
        /// <param name="args">Arguments</param>
        void Error(string format, params object[] args);

        /// <summary>
        ///     a error log
        /// </summary>
        /// <param name="ex">Exception</param>
        void Error(Exception ex);

        /// <summary>
        ///     Flushes the Queue to the physical log file
        /// </summary>
        void Flush();

        #endregion
    }
}