using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServiceLibrary
{
    /// <summary>
    /// This class represents a slave service that executes only search operation. 
    /// </summary>
    public class SlaveService : MarshalByRefObject, IUserStorageService
    {
        #region Constructors
        /// <summary>
        /// Default constructor that creates a simple slave sservice with empty collection of users.
        /// </summary>
        /// <param name="storage">A storage for users.</param>
        //public SlaveService(IUserStorage storage) : this(storage, new List<User>(), new Logger()) { }

        /// <summary>
        /// This constructor takes an initializes collection of users.
        /// </summary>
        /// <param name="storage">A storage for users.</param>
        /// <param name="users">The collection of users which must be added to the storage for the first time.</param>
        /// <param name="logger">The instance of class which implements ILogger interface.</param>
        public SlaveService(IUserStorage storage, ILogger logger)
        {
            if (storage == null)
                throw new ArgumentNullException(nameof(storage));

            /*if (users == null)
                throw new ArgumentNullException(nameof(users));*/

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            this.storage = storage;
            this.logger = logger;

            /*foreach (var user in users)
                this.storage.Add(user);*/
        } 
        #endregion

        /// <summary>
        /// This method finds a user by the given criteria.
        /// </summary>
        /// <param name="criteria">Represents the method for searching a specific user by given criterion.</param>
        /// <returns>Returns user which was found by using the criteria function.</returns>
        public User GetUserByPredicate(Func<User, bool> criteria)
        {
            logger.WriteInfo("Start slave's GetUserByPredicate method.");

            if (criteria == null)
            {
                logger.WriteError("The given criteria is null.");
                throw new ArgumentNullException(nameof(criteria));
            }
                
            return storage.GetUserByPredicate(criteria);
        }

        /// <summary>
        /// This method finds an array of users by the given criteria.
        /// </summary>
        /// <param name="criteria">Represents the method for searching users by given criteria.</param>
        /// <returns>Returns a collection of users which was found by using the criteria function.</returns>
        public IEnumerable<User> GetUsersByPredicate(Func<User, bool> criteria)
        {
            logger.WriteInfo("Start slave's GetUsersByPredicate method.");

            if (criteria == null)
            {
                logger.WriteError("The given criteria is null.");
                throw new ArgumentNullException(nameof(criteria));
            }

            return storage.GetUsersByPredicate(criteria);
        }

        #region Not allowed operations
        public void Add(User user)
        {
            throw new InvalidOperationException("This operation isn't allowed for this service.");
        }

        public bool Delete(User user)
        {
            throw new InvalidOperationException("This operation isn't allowed for this service.");
        }

        public bool Delete(int userId)
        {
            throw new InvalidOperationException("This operation isn't allowed for this service.");
        }
        #endregion

        private readonly IUserStorage storage;
        private readonly ILogger logger;
    }
}
