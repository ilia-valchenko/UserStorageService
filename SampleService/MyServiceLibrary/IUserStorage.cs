using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace MyServiceLibrary
{
    /// <summary>
    /// This interface prodives basic operations for storage such as CRUD and seeking operations.
    /// </summary>
    public interface IUserStorage
    {
        /// <summary>
        /// This property returns the number of users in a storage.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// This method ads the given user to the storage if it doesn't exist.
        /// </summary>
        /// <param name="user">A user which must be added to the storage.</param>
        void Add(User user);

        /// <summary>
        /// This method removes the given user from the storage if it exists.
        /// </summary>
        /// <param name="user">A user which must be removed.</param>
        bool Delete(User user);

        /// <summary>
        /// This method removes a user from the storage by using user's id.
        /// </summary>
        /// <param name="userId">An Id of the user which must be removed.</param>
        bool Delete(int userId);

        /// <summary>
        /// This method defines if the given user exist into the storage.
        /// </summary>
        /// <returns>Returns true if the given user exists.</returns>
        bool Contains(User user);

        /// <summary>
        /// This method finds a user by the given criteria.
        /// </summary>
        /// <param name="criteria">Represents the method for searching a specific user by given criterion.</param>
        /// <returns>Returns user which was found by using the criteria function.</returns>
        User GetUserByPredicate(Func<User, bool> criteria);

        /// <summary>
        /// This method finds a collection of users by the given criteria.
        /// </summary>
        /// <param name="criteria">Represents the method for searching users by given criteria.</param>
        /// <returns>Returns a collection of users which was found by using the criteria function.</returns>
        IEnumerable<User> GetUsersByPredicate(Func<User, bool> criteria);
    }
}
