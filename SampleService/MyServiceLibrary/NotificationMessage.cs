using System;

namespace MyServiceLibrary
{
    /// <summary>
    /// This is the class that provides communications between services.
    /// </summary>
    [Serializable]
    public abstract class NotificationMessage
    {
        /// <summary>
        /// Command.
        /// </summary>
        public Commands Command { get; private set; }

        /// <summary>
        /// Constructor that takes a command.
        /// </summary>
        /// <param name="command">Command.</param>
        protected NotificationMessage(Commands command)
        {
            Command = command;
        }
    }

    /// <summary>
    /// There are commands which our service will take from client.
    /// </summary>
    public enum Commands
    {
        Add,
        Delete,
        GetUserByPredicate,
        GetUsersByPredicate,
        Stop
    };
}
