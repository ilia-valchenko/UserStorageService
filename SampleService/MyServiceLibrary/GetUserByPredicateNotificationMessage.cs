using System;

namespace MyServiceLibrary
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    class GetUserByPredicateNotificationMessage : NotificationMessage
    {
        /// <summary>
        /// This criteria is use to find a concrete user.
        /// </summary>
        public Func<User, bool> Criteria { get; private set; }

        /// <summary>
        /// Constructor that takes a predicate.
        /// </summary>
        /// <param name="criteria">Search criteria method.</param>
        public GetUserByPredicateNotificationMessage(Func<User, bool> criteria) : base(Commands.GetUserByPredicate)
        {
            Criteria = criteria;
        }
    }
}
