using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UserLibrary;
using MessageLibrary;

namespace MyServiceLibrary
{
    /// <summary>
    /// This class represent a slave. It'a a master with limited functionality. 
    /// </summary>
    public class Slave : MarshalByRefObject, IMaster
    {
        #region Public properties
        /// <summary>
        /// The name of a host.
        /// </summary>
        public string HostName { get; private set; }

        /// <summary>
        /// Number of port.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Ip host.
        /// </summary>
        public IPHostEntry IpHost { get; private set; }

        /// <summary>
        /// Ip address.
        /// </summary>
        public IPAddress IpAddress { get; private set; }

        /// <summary>
        /// Represents a network endpoint as an IP address and a port number.
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }

        /// <summary>
        /// Socket is for listening incomming messages.
        /// </summary>
        public Socket SocketListener { get; private set; }
        #endregion

        /// <summary>
        /// Constructor that takes the port which will be used to start the master.
        /// </summary>
        /// <param name="port">Number of port.</param>
        /// <param name="hostName">Name of a host.</param>
        /// <param name="masterPort">Number of master port.</param>
        public Slave(int port, string hostName, int masterPort)
        {
            Port = port;
            HostName = hostName;
            IpHost = Dns.GetHostEntry(HostName);
            IpAddress = IpHost.AddressList[0];
            EndPoint = new IPEndPoint(IpAddress, Port);
            SocketListener = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.masterPort = masterPort;
            uss = new UserStorageService();
        }

        #region API
        /// <summary>
        /// This method finds a user by the given criteria.
        /// </summary>
        /// <param name="criteria">Represents the method for searching a specific user by given criterion.</param>
        /// <returns>Returns user which was found by using the criteria function.</returns>
        public User GetUserByPredicate(Func<User, bool> criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            var user = uss.GetUserByPredicate(criteria);

            // If we can't find user in local storage
            if (user == null)
            {
                GetUserByPredicateNotificationMessage message = new GetUserByPredicateNotificationMessage(criteria);
                SendMessageTo(masterPort, message);
                return null;
            }
            else
            {
                return user;
            }
        }

        /// <summary>
        /// This method finds an array of users by the given criteria.
        /// </summary>
        /// <param name="criteria">Represents the method for searching users by given criteria.</param>
        /// <returns>Returns a collection of users which was found by using the criteria function.</returns>
        public IEnumerable<User> GetUsersByPredicate(Func<User, bool> criteria)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method starts to listen an incomming messages.
        /// </summary>
        public void StartListen()
        {
            throw new NotImplementedException();
        }

        #region Invalid slave's operation
        /// <summary>
        /// This method ads user to the storage.
        /// </summary>
        /// <param name="user">User which must be added.</param>
        public void Add(User user)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// This method removes user from the storage if it exists.
        /// </summary>
        /// <param name="userId">Id of the user which must be deleted.</param>
        public bool Delete(int userId)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// This method removes user from the storage.
        /// </summary>
        /// <param name="user">A user that must be removed.</param>
        public bool Delete(User user)
        {
            throw new InvalidOperationException();
        }
        #endregion 
        #endregion

        /// <summary>
        /// This method executes command which was been received from the notification message.
        /// </summary>
        /// <param name="message">The given notification message.</param>
        private void ExecuteCommand(NotificationMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            switch (message.Command)
            {
                case Commands.Add:
                    Console.WriteLine("The client is calling Add method.");
                    break;

                case Commands.Delete:
                    Console.WriteLine("The client is calling Delete method.");
                    break;

                case Commands.GetUserByPredicate:
                    Console.WriteLine("The client is calling GetUserByPredicate method.");
                    break;

                case Commands.GetUsersByPredicate:
                    Console.WriteLine("The client is calling GetUsersByPredicate method.");
                    break;

                default:
                    Console.WriteLine("The command is undefined");
                    break;
            }
        }

        /// <summary>
        /// This method sends a notification message to service that works on the given number of port.
        /// </summary>
        /// <param name="handler">The socket which was received from Accept method.</param>
        /// <param name="message">The message that must be sent.</param>
        private void SendMessageTo(Socket handler, NotificationMessage message)
        {
            byte[] bytesOfMessage = null;

            try
            {
                bytesOfMessage = NotificationMessage.TransformMessageToBytes(message);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            handler.Send(bytesOfMessage);

            Console.WriteLine("The message was sent.");
        }

        private void SendMessageTo(int port, NotificationMessage message)
        {
            byte[] bytesOfMessage = null;
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                bytesOfMessage = NotificationMessage.TransformMessageToBytes(message);
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            sender.Connect(endPoint);

            sender.Send(bytesOfMessage);

            Console.WriteLine("The message was sent.");
        }

        private UserStorageService uss;
        private int masterPort;
    }
}
