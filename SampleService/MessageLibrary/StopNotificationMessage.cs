using System;

namespace MessageLibrary
{
    /// <summary>
    /// This message says that service should stop his work. 
    /// </summary>
    [Serializable]
    public class StopNotificationMessage : NotificationMessage
    {
        public StopNotificationMessage() : base(Commands.Stop) {}
    }
}
