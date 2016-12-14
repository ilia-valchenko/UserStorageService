using System;
using UserLibrary;

namespace MessageLibrary
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GetUsersByPredicateNotificationMessage : NotificationMessage
    {
        /// <summary>
        /// This criteria is use to find users.
        /// </summary>
        public Func<User, bool> Criteria { get; private set; }

        /// <summary>
        /// Constructor that takes a predicate.
        /// </summary>
        /// <param name="criteria">Search criteria method.</param>
        public GetUsersByPredicateNotificationMessage(Func<User, bool> criteria) : base(Commands.GetUsersByPredicate)
        {
            Criteria = criteria;
        }
    }
}
