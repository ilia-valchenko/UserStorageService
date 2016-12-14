namespace MyServiceLibrary
{
    /// <summary>
    /// This interface defines basic methods for logging messages.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Designates informational messages that highlight the progress of the application at coarse-grained level.
        /// </summary>
        /// <param name="message">Text of message.</param>
        void WriteInfo(string message);
        /// <summary>
        /// Designates fine-grained informational events that are most useful to debug an application.
        /// </summary>
        /// <param name="message"></param>
        void WriteDebug(string message);
        /// <summary>
        /// Designates potentially harmful situations.
        /// </summary>
        /// <param name="message"></param>
        void WriteWarning(string message);
        /// <summary>
        /// Designates very severe error events that will presumably lead the application to abort.
        /// </summary>
        /// <param name="message"></param>
        void WriteError(string message);
    }
}
