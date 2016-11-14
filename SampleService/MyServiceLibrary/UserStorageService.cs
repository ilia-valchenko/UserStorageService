using System;
using System.Collections.Generic;
using System.Linq;
using BinarySearchTree;

namespace MyServiceLibrary
{
    /// <summary>
    /// This delegate encapsulates a method which create an Id of a user.
    /// </summary>
    /// <param name="id">An Id of a previous user.</param>
    /// <returns>Returns id of the current user.</returns>
    public delegate int IdentifierChanger(int id);

    /// <summary>
    /// This class represents the simple user storage service which prodives basic operations such as addition, finding and removing users.
    /// </summary>
    public sealed class UserStorageService
    {
        #region Constructors
        public UserStorageService() : this(new List<User>(), id => id++) { }

        public UserStorageService(IEnumerable<User> users) : this(users, id => id++) { }

        public UserStorageService(IEnumerable<User> users, IdentifierChanger identifierChanger)
        {
            if (users == null)
                throw new ArgumentNullException(nameof(users));

            if (identifierChanger == null)
                throw new ArgumentNullException(nameof(identifierChanger));

            bst = new BinarySearchTree<User>();
            this.identifierChanger = identifierChanger;
            this.id = 0;

            foreach (var user in users)
                bst.Add(user);
        }
        #endregion

        #region API
        /// <summary>
        /// This method ads users to the storage if this storage doesn't contain the user. 
        /// </summary>
        /// <param name="user">User which must be added.</param>
        public void Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // set more info to the exception 
            if (string.IsNullOrEmpty(user.FirstName))
                throw new InvalidUserException();

            if (string.IsNullOrEmpty(user.LastName))
                throw new InvalidUserException();

            if (user.DateOfBirth > DateTime.Now)
                throw new InvalidUserException();

            // should I throw this exception or not?
            //if (bst.Contains(user))
            //    throw new UserAlreadyExistsException();

            if (bst.Contains(user))
                return;

            bst.Add(new User(user.FirstName, user.LastName, user.DateOfBirth, identifierChanger(id)));
        }

        /// <summary>
        /// This method removes user from the storage.
        /// </summary>
        /// <param name="user">A user that must be removed.</param>
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
        public void Delete(int userId)
        {
            if (userId < 0)
                throw new ArgumentException(nameof(userId));

            Predicate<User> getUserById = (User u) => u.Id == userId;
            var user = GetUserByPredicate(getUserById);

            if (user != null)
                bst.Remove(user);
        }

        /// <summary>
        /// This method finds a user by the given predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Returns user which was found by using the predicate.</returns>
        public User GetUserByPredicate(Predicate<User> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return bst.FirstOrDefault(user => predicate(user));
        }

        /// <summary>
        /// This method finds an array of users by the given predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Returns a collection of users which was found by using predicate.</returns>
        public IEnumerable<User> GetUsersByPredicate(Predicate<User> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return bst.Where(user => predicate(user));
        }
        #endregion

        #region Private fields and properties
        private readonly BinarySearchTree<User> bst;
        private readonly IdentifierChanger identifierChanger;
        private int id;
        #endregion
    }
}
