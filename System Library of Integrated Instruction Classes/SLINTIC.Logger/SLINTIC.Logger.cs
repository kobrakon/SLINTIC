// INCOMPLETE!!!
// will get to later

using System;
using Microsoft.Extensions.Logging;

namespace SLINTIC.Logger
{
    public class Logger : ILogger
    {
        /// <summary>
        /// Creates a new instance of a Logger
        /// </summary>
        public Logger(string tag)
        {
            this.tag = tag;
        }

        private

        public bool IsEnabled(LogLevel log) => true;

        public void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            throw new NotImplementedException();
        }

        public bool ILogger.IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public string tag { get; protected set; }
    }

    internal class LocalConsole : ILogger
    {
        /// <summary>
        /// Outputs a general message to logger
        /// </summary>
        internal static void Log(object message)
        {
            this.LogInformation(message);
        }

        /// <summary>
        /// Outputs an error message to logger
        /// </summary>
        internal static void LogError(object message)
        {
            this.LogError(message);
        }

        // Overload 1: log exception
        /// <summary>
        /// Outputs an error message and exception body to logger
        /// </summary>
        internal static void LogError(object message, Exception e)
        {
            this.LogError(message, e);
        }

        internal static void LogInternal(object message)
        {
            #if DEBUG

            #else
            return;

            #endif
        }
    }
}