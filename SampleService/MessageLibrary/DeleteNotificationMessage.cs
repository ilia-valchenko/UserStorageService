using System;
using UserLibrary;

namespace MessageLibrary
{

    /// <summary>
    /// This class represents a message that inform services to delete a user.
    /// </summary>
    [Serializable]
    public class DeleteNotificationMessage : NotificationMessage
    {
        /// <summary>
        /// The id of the user which must be deleted.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Constructor that takes the id of the user which must be deleted.
        /// </summary>
        /// <param name="userId">The id of the user which must be deleted.</param>
        public DeleteNotificationMessage(int userId) : base(Commands.Delete)
        {
            UserId = userId;
        }
    }
}
