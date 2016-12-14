using System;
using System.Collections.Generic;
using UserLibrary;

namespace MyServiceLibrary
{
    /// <summary>
    /// This interface provides an API of CRUD and seeking operations for storage of users.
    /// </summary>
    public interface IUserStorageService
    {
        /// <summary>
        /// This method ads users to the storage if this storage doesn't contain the user. 
        /// </summary>
        /// <param name="user">User which must be added.</param>
        void Add(User user);
        /// <summary>
        /// This method removes user from the storage.
        /// </summary>
        /// <param name="user">A user that must be removed.</param>
        bool Delete(User user);
        /// <summary>
        /// This method removes user from the storage if it exists.
        /// </summary>
        /// <param name="userId">Id of the user which must be deleted.</param>
        bool Delete(int userId);
        /// <summary>
        /// This method finds a user by the given criteria.
        /// </summary>
        /// <param name="criteria">Represents the method for searching a specific user by given criterion.</param>
        /// <returns>Returns user which was found by using the criteria function.</returns>
        User GetUserByPredicate(Func<User, bool> criteria);
        /// <summary>
        /// This method finds an array of users by the given predicate.
        /// </summary>
        /// <param name="criteria">Represents the method for searching users by given criterion.</param>
        /// <returns>Returns a collection of users which was found by using the criteria function.</returns>
        IEnumerable<User> GetUsersByPredicate(Func<User, bool> criteria);
    }
}
