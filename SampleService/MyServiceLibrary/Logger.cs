using NLog;

namespace MyServiceLibrary
{
    /// <summary>
    /// This class implements ILogger interface by using NLog.
    /// </summary>
    public sealed class Logger : ILogger
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Logger()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        /// <summary>
        /// Designates informational messages that highlight the progress of the application at coarse-grained level.
        /// </summary>
        /// <param name="message">Text of message.</param>
        public void WriteInfo(string message)
        {
            logger.Info(message);
        }
        /// <summary>
        /// Designates fine-grained informational events that are most useful to debug an application.
        /// </summary>
        /// <param name="message"></param>
        public void WriteDebug(string message)
        {
            logger.Debug(message);
        }
        /// <summary>
        /// Designates potentially harmful situations.
        /// </summary>
        /// <param name="message"></param>
        public void WriteWarning(string message)
        {
            logger.Warn(message);
        }
        /// <summary>
        /// Designates very severe error events that will presumably lead the application to abort.
        /// </summary>
        /// <param name="message"></param>
        public void WriteError(string message)
        {
            logger.Error(message);
        }

        private static NLog.Logger logger;
    }
}
