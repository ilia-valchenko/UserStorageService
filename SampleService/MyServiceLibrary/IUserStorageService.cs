using System;
using System.Collections.Generic;

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
        void Delete(User user);
        /// <summary>
        /// This method removes user from the storage if it exists.
        /// </summary>
        /// <param name="userId">Id of the user which must be deleted.</param>
        void Delete(int userId);
        /// <summary>
        /// This method finds a user by the given predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Returns user which was found by using the predicate.</returns>
        User GetUserByPredicate(Predicate<User> predicate);
        /// <summary>
        /// This method finds an array of users by the given predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Returns a collection of users which was found by using predicate.</returns>
        IEnumerable<User> GetUsersByPredicate(Predicate<User> predicate);
    }
}
