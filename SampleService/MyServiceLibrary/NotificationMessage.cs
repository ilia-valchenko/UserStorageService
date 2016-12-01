using System;

namespace MyServiceLibrary
{
    [Serializable]
    public class NotificationMessage
    {
        public Commands Command { get; set; }
        public User UserInstance { get; set; }
        public int UserId { get; set; }

        public NotificationMessage(Commands command, User user, int id)
        {
            Command = command;
            UserInstance = user;
            UserId = id;
        }
    }

    public enum Commands
    {
        Add,
        Delete,
        GetUserByPredicate,
        GetUsersByPredicate,
        Stop
    };
}
