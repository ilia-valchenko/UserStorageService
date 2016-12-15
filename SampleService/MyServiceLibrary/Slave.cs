using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
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

            // If we can't find user in a local storage
            if (user == null)
            {
                Console.Write("User wasn't found in a local storage. Start to asking the master.");

                GetUserByPredicateNotificationMessage message = new GetUserByPredicateNotificationMessage(criteria);
                AddNotificationMessage receivedAnswer = SendMessageTo(masterPort, message) as AddNotificationMessage;

                if (receivedAnswer.User != null)
                {
                    uss.Add(receivedAnswer.User);
                    return receivedAnswer.User;
                }     
                else
                {
                    return null;
                } 
            }
            else
            {
                Console.WriteLine("User was found in a local storage.");
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
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            return uss.GetUsersByPredicate(criteria);
        }

        /// <summary>
        /// This method starts to listen an incomming messages.
        /// </summary>
        public void StartListen()
        {
            try
            {
                SocketListener.Bind(EndPoint);
                SocketListener.Listen(10);
                bool isWorkFinished = false;

                while (!isWorkFinished)
                {
                    Console.WriteLine("\nSlave is ready to accept a new client...");

                    Socket handler = SocketListener.Accept();
                    byte[] bytes = new byte[2048];
                    int numberOfReceivedBytes = handler.Receive(bytes);

                    NotificationMessage receivedMessage = null;

                    try
                    {
                        receivedMessage = NotificationMessage.TransfromBytesToNotificationMessage(bytes, 0, numberOfReceivedBytes);
                        Console.WriteLine("Slave received an answer from the master.");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }

                    if (receivedMessage is StopNotificationMessage)
                    {
                        isWorkFinished = true;
                        Console.WriteLine("Stop message was received. The master is stoping.");
                    }
                    else
                    {
                        ExecuteCommand(receivedMessage);
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
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
                    AddNotificationMessage addNotificationMessage = message as AddNotificationMessage;
                    Console.WriteLine($"Master said that slave must add a new user to inner storage. The user is: {addNotificationMessage.User}");
                    uss.Add(addNotificationMessage.User);
                    break;

                case Commands.Delete:
                    Console.WriteLine("Master said that slave should delete the user from inner storage.");
                    DeleteNotificationMessage deleteNotificationMessage = message as DeleteNotificationMessage;
                    User user = uss.GetUserByPredicate((User u) => u.Id == deleteNotificationMessage.UserId);

                    if(user == null)
                    {
                        Console.WriteLine("The local storage doesn't contains the user with given Id.");
                    }
                    else
                    {
                        Console.WriteLine($"The user which will be removed is: {user}");
                        uss.Delete(user.Id);
                    }
                   
                    break;

                default:
                    Console.WriteLine("The command is undefined");
                    break;
            }
        }

        /// <summary>
        /// This method sends a notification message to the service which works on the given port.
        /// </summary>
        /// <param name="port">Number of a port.</param>
        /// <param name="message">Notification message that must be sent.</param>
        private NotificationMessage SendMessageTo(int port, NotificationMessage message)
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

            Console.WriteLine("Slave sent message to the master.");
            Console.WriteLine("Wait an answer from the master.");

            byte[] bytesOfReceivedMessage = new byte[2048];
            int numberOfReceivedBytes = sender.Receive(bytesOfReceivedMessage);
            NotificationMessage msg = NotificationMessage.TransfromBytesToNotificationMessage(bytesOfReceivedMessage, 0, numberOfReceivedBytes);

            Console.WriteLine($"The received command from the master is {msg.Command}.");

            return msg;
        }

        private UserStorageService uss;
        private int masterPort;
    }
}
