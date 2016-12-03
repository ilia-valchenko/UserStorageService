using System;
using System.Collections.Generic;
using System.Xml.Linq;
using NLog.Internal;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;

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
        public UserStorageService(IUserStorage storage) : this(storage, new List<User>(), (ref int id) => id++, new Logger()) { }
        /// <summary>
        /// This constructor creates a simple service with empty collection of users and custom identifierChanger.
        /// </summary>
        /// <param name="storage">A storage for users.</param>
        /// <param name="identifierChanger">Delegate which encapsulates method to change user's id.</param>
        public UserStorageService(IUserStorage storage, IdentifierChanger identifierChanger) : this(storage, new List<User>(), identifierChanger, new Logger()) { }
        /// <summary>
        /// This constructor takes an initializes collection of users.
        /// </summary>
        /// <param name="storage">A storage for users.</param>
        /// <param name="users">The collection of users which must be added to the storage for the first time.</param>
        public UserStorageService(IUserStorage storage, IEnumerable<User> users) : this(storage, users, (ref int id) => id++, new Logger()) { }
        /// <summary>
        /// This constructor takes an initializes collection of users and custom identifierChanger.
        /// </summary>
        /// <param name="storage">A storage for users.</param>
        /// <param name="users">The collection of users which must be added to the storage for the first time.</param>
        /// <param name="identifierChanger">Delegate which encapsulates method to change user's id.</param>
        /// <param name="logger">The instance of class which implements ILogger interface.</param>
        public UserStorageService(IUserStorage storage, IEnumerable<User> users, IdentifierChanger identifierChanger, ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            logger.WriteInfo("Start UserStorageService.");

            if (storage == null)
            {
                logger.WriteError("The given storage is null.");
                throw new ArgumentNullException(nameof(storage));
            }

            if (users == null)
            {
                logger.WriteError("The given collection of users is null.");
                throw new ArgumentNullException(nameof(users));
            }

            if (identifierChanger == null)
            {
                logger.WriteError("The given identifierChanger is null.");
                throw new ArgumentNullException(nameof(identifierChanger));
            }
                
            this.storage = storage;
            this.identifierChanger = identifierChanger;
            this.logger = logger;

            // Check if we have the previous state of our service
            string statefile = System.Configuration.ConfigurationManager.AppSettings["StateFileName"];

            if (File.Exists(statefile))
            {
                XDocument xDoc = null;

                try
                {
                    xDoc = XDocument.Load(statefile);
                }
                catch (Exception exc)
                {
                    logger.WriteError(exc.Message);
                }

                Int32.TryParse(xDoc.Root.Element("lastGeneratedId").Value, out this.id);
                var xmlUsers = xDoc.Root.Element("users").Elements("user");

                if (xmlUsers.Count() > 0)
                {
                    foreach (var xUser in xmlUsers)
                    {
                        int userId = 0;
                        Int32.TryParse(xUser.Element("id").Value, out userId);
                        string firstName = xUser.Element("firstName").Value;
                        string lastName = xUser.Element("lastName").Value;
                        Gender gender = (Gender)Enum.Parse(typeof(Gender), xUser.Element("gender").Value);
                        DateTime dateOfBirth = Convert.ToDateTime(xUser.Element("dateOfBirth").Value);

                        List<VisaRecord> visaRecords = new List<VisaRecord>();

                        var xmlVisaRecords = xUser.Element("visaRecords").Elements("visa");

                        foreach (var xVisa in xmlVisaRecords)
                        {
                            string country = xVisa.Element("country").Value;
                            DateTime start = Convert.ToDateTime(xVisa.Element("start").Value);
                            DateTime end = Convert.ToDateTime(xVisa.Element("end").Value);

                            visaRecords.Add(new VisaRecord(country, start, end));
                        }

                        this.storage.Add(new User(firstName, lastName, gender, dateOfBirth, visaRecords, userId));
                    }
                }
                
                logger.WriteInfo($"Service has state file. The ID which will be use is: {id}");
            }

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
            logger.WriteInfo("Start Add user method.");

            if (user == null)
            {
                logger.WriteError("The user is null.");
                throw new ArgumentNullException(nameof(user));
            }

            if (storage.Contains(user))
            {
                logger.WriteWarning($"The service already contains this user. {user}");
                return;
            }

            identifierChanger(ref id);
            logger.WriteInfo($"Generated ID is: {id}");
            
            storage.Add(new User(user.FirstName, user.LastName, user.Gender, user.DateOfBirth, user.VisaRecords, id));

            logger.WriteInfo($"New user was successfully added. {user}");

            SaveState();

            logger.WriteInfo("The state of the service was saved.");
        }

        /// <summary>
        /// This method removes user from the storage.
        /// </summary>
        /// <param name="user">A user that must be removed.</param>
        public bool Delete(User user)
        {
            logger.WriteInfo("Start Delete method which uses an instance of User as input parameter.");

            if (user == null)
            {
                logger.WriteError("The given user is null.");
                throw new ArgumentNullException(nameof(user));
            }

            if (storage.Delete(user))
            {
                logger.WriteInfo($"The user was successfully deleted. {user}");
                SaveState();
                logger.WriteInfo("The state of the service was saved.");
                return true;
            }

            logger.WriteWarning($"The user wasn't deleted. {user}");
            return false;
        }

        /// <summary>
        /// This method removes user from the storage if it exists.
        /// </summary>
        /// <param name="userId">Id of the user which must be deleted.</param>
        public bool Delete(int userId)
        {
            logger.WriteInfo("Start Delete method which uses an id of user as input parameter.");

            if (userId < 0)
            {
                logger.WriteError("The ID of the user is less then zero.");
                throw new ArgumentException(nameof(userId));
            }
                
            logger.WriteInfo($"ID of the user which must be deleted: {userId}");

            if (storage.Delete(userId))
            {
                logger.WriteInfo("User was successfully deleted.");
                SaveState();
                logger.WriteInfo("The state of the service was saved.");
                return true;
            }

            logger.WriteWarning("User wasn't deleted.");
            return false;
        }

        /// <summary>
        /// This method finds a user by the given criteria.
        /// </summary>
        /// <param name="criteria">Represents the method for searching a specific user by given criterion.</param>
        /// <returns>Returns user which was found by using the criteria function.</returns>
        public User GetUserByPredicate(Func<User, bool> criteria)
        {
            logger.WriteInfo("Start GetUserByPredicate method.");

            if (criteria == null)
            {
                logger.WriteError("The given search criteria is null.");
                throw new ArgumentNullException(nameof(criteria));
            }

            User user = storage.GetUserByPredicate(criteria);

            if (user != null)
            {
                logger.WriteInfo($"User was successfully found. {user}");
                return user;
            }

            logger.WriteInfo("User wasn't found. The returned result is null.");
            return null;
        }

        /// <summary>
        /// This method finds an array of users by the given criteria.
        /// </summary>
        /// <param name="criteria">Represents the method for searching users by given criteria.</param>
        /// <returns>Returns a collection of users which was found by using the criteria function.</returns>
        public IEnumerable<User> GetUsersByPredicate(Func<User, bool> criteria)
        {
            logger.WriteInfo("Start GetUsersByPredicate method.");

            if (criteria == null)
            {
                logger.WriteError("The given search criteria is null.");
                throw new ArgumentNullException(nameof(criteria));
            }
                
            var users = storage.GetUsersByPredicate(criteria);

            if (users != null)
            {
                logger.WriteInfo($"Users were successfully found.");
                return users;
            }

            logger.WriteInfo("Users weren't found. The returned result is null.");
            return null;
        }
        #endregion

        /// <summary>
        /// This method saves a state of the service to xml file. File will contain the last generaed id and the number of users in the storage.
        /// </summary>
        private void SaveState()
        {
            logger.WriteInfo("Start SaveState method.");

            string fileName = System.Configuration.ConfigurationManager.AppSettings["StateFileName"];

            logger.WriteInfo($"The name of the future state file: {fileName}");

            XDocument xDoc = new XDocument();
            XElement userStorageService = new XElement("userStorageService");
            XElement lastGeneratedId = new XElement("lastGeneratedId", id.ToString());
            XElement numberOfUsers = new XElement("numberOfUsers", storage.Count.ToString());
            XElement users = new XElement("users");

            logger.WriteInfo($"The number of users which will be written into the xml file: {storage.Count}");
            logger.WriteInfo($"The last generated user's id: {id}");

            userStorageService.Add(lastGeneratedId);
            userStorageService.Add(numberOfUsers);
            

            foreach (var item in storage)
            {
                XElement user = new XElement("user");
                XElement userId = new XElement("id", item.Id);
                XElement firstName = new XElement("firstName", item.FirstName);
                XElement lastName = new XElement("lastName", item.LastName);
                XElement gender = new XElement("gender", item.Gender);
                XElement dateOfBirth = new XElement("dateOfBirth", item.DateOfBirth);
                XElement visaRecords = new XElement("visaRecords");

                foreach (var visaItem in item.VisaRecords)
                {
                    XElement visa = new XElement("visa");
                    XElement country = new XElement("country", visaItem.Country);
                    XElement start = new XElement("start", visaItem.Start);
                    XElement end = new XElement("end", visaItem.End);

                    visa.Add(country, start, end);
                    visaRecords.Add(visa);
                }

                user.Add(userId, firstName, lastName, gender, dateOfBirth, visaRecords);
                users.Add(user);
            }

            userStorageService.Add(users);
            xDoc.Add(userStorageService);

            try
            {
                xDoc.Save(fileName);
            }
            catch (Exception exc)
            {
                logger.WriteError(exc.Message);
            }
        }

        public void PrintUsersToConsole()
        {
            Console.WriteLine("All users:" + Environment.NewLine);

            foreach (var user in storage)
                Console.WriteLine(user + Environment.NewLine);
        }

        #region Private fields
        private readonly IUserStorage storage;
        private readonly IdentifierChanger identifierChanger;
        private readonly ILogger logger;
        private int id;     
        #endregion
    }
}
