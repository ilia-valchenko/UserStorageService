using System;
using System.Collections.Generic;
using BinarySearchTree;

namespace MyServiceLibrary
{
    public delegate int TransformId(int id);

    /// <summary>
    /// This class represents the simple user storage service which prodives basic operations such as addition, finding and removing users.
    /// </summary>
    public sealed class UserStorageService
    {
        #region Constructors
        public UserStorageService() : this(new List<User>(), (int id) => id + 1) {}

        public UserStorageService(TransformId transformId) : this(new List<User>(), transformId) { }

        public UserStorageService(IEnumerable<User> users, TransformId transformId)
        {
            if (users == null)
                throw new ArgumentNullException(nameof(users));

            if (transformId == null)
                throw new ArgumentNullException(nameof(transformId));

            bst = new BinarySearchTree<User>();
            this.transformId = transformId;

            foreach (var user in users)
                bst.Add(user);
        }
        #endregion

        #region API
        /// <summary>
        /// This method ads users to the storage if this storage doesn't contain the user. 
        /// </summary>
        /// <param name="user">User which must be added.</param>
        /// <returns>Returns the new Id of the user.</returns>
        public int Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // set more info to the exception 
            if (String.IsNullOrEmpty(user.FirstName))
                throw new InvalidUserException();

            if (String.IsNullOrEmpty(user.LastName))
                throw new InvalidUserException();

            if (user.DateOfBirth > DateTime.Now)
                throw new InvalidUserException();

            // should I throw this exception or not?
            if (bst.Contains(user))
                throw new UserAlreadyExistsException();


            // And what about unique Id?
            var userWithId = new User(nextId, user.FirstName, user.LastName, user.DateOfBirth);
            nextId = transformId

            bst.Add(user);
            // stub
            return 1;
        }

        public void Delete(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!bst.Contains(user))
                throw new UserDoesntExistException();

            bst.Remove(user);
        }

        /// <summary>
        /// This method removes user from the storage if it exists.
        /// </summary>
        /// <param name="userId">Id of the user which must be deleted.</param>
        /// <returns>Returns true if the user was successfully deleted.</returns>
        public bool Delete(int userId)
        {
            if (userId < 0)
                throw new ArgumentException(nameof(userId));

            Predicate<User> getUserById = (User u) => { return u.Id == userId; };
            var user = GetUserByPredicate(getUserById);

            if (user != null)
            {
                bst.Remove(user);
                return true;
            }

            return false;
        }

        /// <summary>
        /// This method finds a user by the given predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public User GetUserByPredicate(Predicate<User> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (var user in bst)
                if (predicate(user))
                    return user;

            return null;
        }

        /// <summary>
        /// This method finds an array of users by the given predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public User[] GetUsersByPredicate(Predicate<User> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var users = new List<User>();

            foreach (var user in bst)
                if (predicate(user))
                    users.Add(user);

            return users.ToArray();
        }
        #endregion

        private readonly int nextId = 0;
        private BinarySearchTree<User> bst;
        private TransformId transformId;
    }
}
