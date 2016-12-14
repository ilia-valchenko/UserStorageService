using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using MessageLibrary;
using UserLibrary;

namespace MyServiceLibrary
{
    /// <summary>
    /// This class represents the main server that executes CRUD and seeking operations for user storage.
    /// </summary>
    public class Master : MarshalByRefObject, IMaster
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
        /// <param name="portsOfSlaves">Numbers of ports of slaves.</param>
        public Master(int port, string hostName, int[] portsOfSlaves)
        {
            Port = port;
            HostName = hostName;
            IpHost = Dns.GetHostEntry(HostName);
            IpAddress = IpHost.AddressList[0];
            EndPoint = new IPEndPoint(IpAddress, Port);
            SocketListener = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.portsOfSlaves = portsOfSlaves;
            uss = new UserStorageService();
        }

        #region API
        /// <summary>
        /// This method ads user to the storage.
        /// </summary>
        /// <param name="user">User which must be added.</param>
        public void Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            uss.Add(user);

            // set not msg to slaves
        }

        /// <summary>
        /// This method removes user from the storage.
        /// </summary>
        /// <param name="user">A user that must be removed.</param>
        public bool Delete(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return uss.Delete(user);

            // set not msg to slaves
        }

        /// <summary>
        /// This method removes user from the storage if it exists.
        /// </summary>
        /// <param name="userId">Id of the user which must be deleted.</param>
        public bool Delete(int userId)
        {
            if (userId < 0)
                throw new ArgumentException(nameof(userId));

            return uss.Delete(userId);

            // set not msg to slaves
        }

        /// <summary>
        /// This method finds a user by the given criteria.
        /// </summary>
        /// <param name="criteria">Represents the method for searching a specific user by given criterion.</param>
        /// <returns>Returns user which was found by using the criteria function.</returns>
        public User GetUserByPredicate(Func<User, bool> criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            return uss.GetUserByPredicate(criteria);
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
                    Console.WriteLine("Master is ready to accept a new client...");

                    Socket handler = SocketListener.Accept();
                    byte[] bytes = new byte[2048];
                    int numberOfReceivedBytes = handler.Receive(bytes);

                    NotificationMessage receivedMessage = null;

                    try
                    {
                        receivedMessage = NotificationMessage.TransfromBytesToNotificationMessage(bytes, 0, numberOfReceivedBytes);
                        Console.WriteLine("Message was received.");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }

                    if(receivedMessage is StopNotificationMessage)
                    {
                        isWorkFinished = true;
                        Console.WriteLine("Stop message was received. The master is stoping.");
                    }
                    else
                    {
                        ExecuteCommand(receivedMessage, handler);
                    }
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
        #endregion

        /// <summary>
        /// This method executes command which was been received from the notification message.
        /// </summary>
        /// <param name="message">The given notification message.</param>
        /// <param name="handler">Socket for sending a response to a slave.</param>
        private void ExecuteCommand(NotificationMessage message, Socket handler)
        {
            Console.WriteLine("Into ExecuteCommand method.");

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
                    /*GetUserByPredicateNotificationMessage msg = message as GetUserByPredicateNotificationMessage;
                    User user = uss.GetUserByPredicate(msg.Criteria);
                    AddNotificationMessage response = new AddNotificationMessage(user);
                    byte[] bytesOfResponse = NotificationMessage.TransformMessageToBytes(response);
                    handler.Send(bytesOfResponse);
                    Console.WriteLine("The answer from master was sent.");*/
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
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            handler.Send(bytesOfMessage);

            Console.WriteLine("The message was sent.");
        }

        private UserStorageService uss;
        private int[] portsOfSlaves;
    }
}
