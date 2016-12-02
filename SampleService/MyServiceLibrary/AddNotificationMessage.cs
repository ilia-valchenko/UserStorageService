using System;

namespace MyServiceLibrary
{
    /// <summary>
    /// This class represents a message that inform services to add a new user.
    /// </summary>
    [Serializable]
    public class AddNotificationMessage : NotificationMessage
    {
        /// <summary>
        /// The instance of user which would be used in Add or Delete methods.
        /// </summary>
        public User UserInstance { get; private set; }

        /// <summary>
        /// Constructor that takes the user which must be added to the service.
        /// </summary>
        /// <param name="user">User that must be added.</param>
        public AddNotificationMessage(User user) : base(Commands.Add)
        {
            UserInstance = user;
        }
    }
}
