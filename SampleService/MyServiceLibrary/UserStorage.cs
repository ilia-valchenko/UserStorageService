using System;
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
            bst = new BinarySearchTree<User>();
        }

        /// <summary>
        /// This method ads users to the storage. 
        /// </summary>
        /// <param name="user">User which must be added.</param>
        public void Add(User user)
        {
            if(user == null)
                throw new ArgumentNullException(nameof(user));

            bst.Add(user);
        }

        /// <summary>
        /// This method removes user from the storage.
        /// </summary>
        /// <param name="user">A user that must be removed.</param>
        public void Delete(User user)
        {
            if(user == null)
                throw new ArgumentNullException(nameof(user));

            bst.Remove(user);
        }

        /// <summary>
        /// This method removes user from the storage by using user's id.
        /// </summary>
        /// <param name="userId">Id of the user which must be deleted.</param>
        public void Delete(int userId)
        {
            if(userId < 0)
                throw new ArgumentException("The id of the user is less than zero.");


        }

        /// <summary>
        /// This method defines if the given user exist into the storage.
        /// </summary>
        /// <returns>Returns true if the given user exists.</returns>
        public bool Contains(User user)
        {
            if(user == null)
                throw new ArgumentNullException(nameof(user));

            return bst.Contains(user);
        }

        /// <summary>
        /// An inner structure for storage.
        /// </summary>
        private readonly BinarySearchTree<User> bst;
    }
}
