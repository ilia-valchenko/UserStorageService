using System;
using System.Collections.Generic;

namespace MyServiceLibrary
{
    /// <summary>
    /// This delegate encapsulates a method which create an Id of a user.
    /// </summary>
    /// <param name="id">An Id of a previous user.</param>
    /// <returns>Returns id of the current user.</returns>
    public delegate void IdentifierChanger(ref int id);

    /// <summary>
    /// This class represents the simple user storage service which prodives basic operations such as addition, finding and removing users.
    /// </summary>
    public sealed class UserStorageService : IUserStorageService
    {
        #region Constructors
        /// <summary>
        /// Default constructor that creates a simple service with empty collection of users and basic autoincrement id.
        /// </summary>
        /// <param name="storage">A storage for users.</param>
        public UserStorageService(IUserStorage storage) : this(storage, new List<User>(), (ref int id) => id++) { }
        /// <summary>
        /// This constructor creates a simple service with empty collection of users and custom identifierChanger.
        /// </summary>
        /// <param name="storage">A storage for users.</param>
        /// <param name="identifierChanger">Delegate which encapsulates method to change user's id.</param>
        public UserStorageService(IUserStorage storage, IdentifierChanger identifierChanger) : this(storage, new List<User>(), identifierChanger) { }
        /// <summary>
        /// This constructor takes an initializes collection of users.
        /// </summary>
        /// <param name="storage">A storage for users.</param>
        /// <param name="users">The collection of users which must be added to the storage for the first time.</param>
        public UserStorageService(IUserStorage storage, IEnumerable<User> users) : this(storage, users, (ref int id) => id++) { }
        /// <summary>
        /// This constructor takes an initializes collection of users and custom identifierChanger.
        /// </summary>
        /// <param name="storage">A storage for users.</param>
        /// <param name="users">The collection of users which must be added to the storage for the first time.</param>
        /// <param name="identifierChanger">Delegate which encapsulates method to change user's id.</param>
        public UserStorageService(IUserStorage storage, IEnumerable<User> users, IdentifierChanger identifierChanger)
        {
            if(storage == null)
                throw new ArgumentNullException(nameof(storage));

            if (users == null)
                throw new ArgumentNullException(nameof(users));

            if (identifierChanger == null)
                throw new ArgumentNullException(nameof(identifierChanger));

            this.storage = storage;
            this.identifierChanger = identifierChanger;

            foreach (var user in users)
                storage.Add(user);
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

            if (string.IsNullOrEmpty(user.FirstName))
                throw new InvalidUserException();

            if (string.IsNullOrEmpty(user.LastName))
                throw new InvalidUserException();

            if (user.DateOfBirth > DateTime.Now)
                throw new InvalidUserException();

            if (storage.Contains(user))
                return;

            identifierChanger(ref id);

            storage.Add(new User(user.FirstName, user.LastName, user.DateOfBirth, id));
        }

        /// <summary>
        /// This method removes user from the storage.
        /// </summary>
        /// <param name="user">A user that must be removed.</param>
        public void Delete(User user)
        { 
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Is it necessary?
            if (!storage.Contains(user))
                throw new UserDoesntExistException();

            storage.Delete(user);
        }

        /// <summary>
        /// This method removes user from the storage if it exists.
        /// </summary>
        /// <param name="userId">Id of the user which must be deleted.</param>
        public void Delete(int userId)
        {
            if (userId < 0)
                throw new ArgumentException(nameof(userId));

            Func<User, bool> getUserById = (User u) => u.Id == userId;
            var user = GetUserByPredicate(getUserById);

            if (user != null)
                storage.Delete(user);
        }

        /// <summary>
        /// This method finds a user by the given predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Returns user which was found by using the predicate.</returns>
        public User GetUserByPredicate(Func<User, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return storage.GetUserByPredicate(predicate);
        }

        /// <summary>
        /// This method finds an array of users by the given predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Returns a collection of users which was found by using predicate.</returns>
        public IEnumerable<User> GetUsersByPredicate(Func<User, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return storage.GetUsersByPredicate(predicate);
        }
        #endregion

        #region Private fields and properties
        private readonly IUserStorage storage;
        private readonly IdentifierChanger identifierChanger;
        private int id;
        #endregion
    }
}
