using System;
using System.Collections.Generic;
using System.Linq;
using BinarySearchTree;

namespace MyServiceLibrary
{
    /// <summary>
    /// This class represents a simple storage for users with basic operations.
    /// </summary>
    public class UserStorage : IUserStorage
    {
        /// <summary>
        /// A default constructor that creates an empty storage.
        /// </summary>
        public UserStorage()
        {
            //bst = new BinarySearchTree<User>();
            storage = new Dictionary<int, User>();
        }

        /// <summary>
        /// This method ads users to the storage. 
        /// </summary>
        /// <param name="user">User which must be added.</param>
        public void Add(User user)
        {
            if(user == null)
                throw new ArgumentNullException(nameof(user));

            storage.Add(user.Id, user);
        }

        /// <summary>
        /// This method removes user from the storage.
        /// </summary>
        /// <param name="user">A user that must be removed.</param>
        public void Delete(User user)
        {
            if(user == null)
                throw new ArgumentNullException(nameof(user));

            storage.Remove(user.Id);
        }

        /// <summary>
        /// This method removes user from the storage by using user's id.
        /// </summary>
        /// <param name="userId">Id of the user which must be deleted.</param>
        public void Delete(int userId) => storage.Remove(userId);

        /// <summary>
        /// This method defines if the given user exist into the storage.
        /// </summary>
        /// <returns>Returns true if the given user exists.</returns>
        public bool Contains(User user)
        {
            if(user == null)
                throw new ArgumentNullException(nameof(user));

            return storage.ContainsValue(user);
        }

        /// <summary>
        /// This method finds a user by the given predicate. 
        /// </summary>
        /// <param name="predicate">Represents the method for searching a specific user by given criterion.</param>
        /// <returns>Returns the seeking user.</returns>
        public User GetUserByPredicate(Predicate<User> predicate)
        {
            if(predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return storage.First(item => predicate(item.Value)).Value;
        }

        /// <summary>
        /// This method finds a collection of users by the given predicate.
        /// </summary>
        /// <param name="predicate">Represents the method for searching users by given criterion.</param>
        /// <returns></returns>
        public IEnumerable<User> GetUsersByPredicate(Predicate<User> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return storage.Where(item => predicate(item.Value)).Select(item => item.Value);
        }

        /// <summary>
        /// An inner structure for storage.
        /// </summary>
        private readonly Dictionary<int, User> storage;
    }
}
