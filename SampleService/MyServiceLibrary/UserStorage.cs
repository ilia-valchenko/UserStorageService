using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyServiceLibrary
{
    /// <summary>
    /// This class represents a simple storage for users with basic operations.
    /// </summary>
    public sealed class UserStorage : IUserStorage
    {
        /// <summary>
        /// This property returns the number of users in a storage.
        /// </summary>
        public int Count => storage.Count;

        /// <summary>
        /// A default constructor that creates an empty storage.
        /// </summary>
        public UserStorage()
        {
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
        public bool Delete(User user)
        {
            if(user == null)
                throw new ArgumentNullException(nameof(user));

            return storage.Remove(user.Id);
        }

        /// <summary>
        /// This method removes user from the storage by using user's id.
        /// </summary>
        /// <param name="userId">Id of the user which must be deleted.</param>
        public bool Delete(int userId) => storage.Remove(userId);

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
        /// This method finds a user by the given criteria. 
        /// </summary>
        /// <param name="criteria">Represents the method for searching a specific user by given criterion.</param>
        /// <returns>Returns the seeking user.</returns>
        public User GetUserByPredicate(Func<User, bool> criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            return storage.FirstOrDefault(item => criteria(item.Value)).Value;
        }

        /// <summary>
        /// This method finds a collection of users by the given criteria.
        /// </summary>
        /// <param name="criteria">Represents the method for searching users by the given criteria.</param>
        /// <returns>Returns a collection of users which was found by using the criteria function.</returns>
        public IEnumerable<User> GetUsersByPredicate(Func<User, bool> criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            return storage.Where(item => criteria(item.Value)).Select(item => item.Value);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a storage.
        /// </summary>
        public IEnumerator<User> GetEnumerator()
        {
            foreach (var item in storage)
                yield return item.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// An inner structure for storage.
        /// </summary>
        private readonly Dictionary<int, User> storage;
    }
}
