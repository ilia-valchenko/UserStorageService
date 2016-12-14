using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UserLibrary;

namespace MyServiceLibrary
{
    /// <summary>
    /// This interface provides basic user service API.
    /// </summary>
    public interface IMaster
    {
        /// <summary>
        /// The name of a host.
        /// </summary>
        string HostName { get; }

        /// <summary>
        /// Number of port.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Ip host.
        /// </summary>
        IPHostEntry IpHost { get; }

        /// <summary>
        /// Ip address.
        /// </summary>
        IPAddress IpAddress { get; }

        /// <summary>
        /// Represents a network endpoint as an IP address and a port number.
        /// </summary>
        IPEndPoint EndPoint { get; }

        /// <summary>
        /// Socket is for listening incomming messages.
        /// </summary>
        Socket SocketListener { get; }

        /// <summary>
        /// This method ads user to the storage.
        /// </summary>
        /// <param name="user">User which must be added.</param>
        void Add(User user);

        /// <summary>
        /// This method removes user from the storage.
        /// </summary>
        /// <param name="user">A user that must be removed.</param>
        bool Delete(User user);

        /// <summary>
        /// This method removes user from the storage if it exists.
        /// </summary>
        /// <param name="userId">Id of the user which must be deleted.</param>
        bool Delete(int userId);

        /// <summary>
        /// This method finds a user by the given criteria.
        /// </summary>
        /// <param name="criteria">Represents the method for searching a specific user by given criterion.</param>
        /// <returns>Returns user which was found by using the criteria function.</returns>
        User GetUserByPredicate(Func<User, bool> criteria);

        /// <summary>
        /// This method finds an array of users by the given criteria.
        /// </summary>
        /// <param name="criteria">Represents the method for searching users by given criteria.</param>
        /// <returns>Returns a collection of users which was found by using the criteria function.</returns>
        IEnumerable<User> GetUsersByPredicate(Func<User, bool> criteria);

        /// <summary>
        /// This method made to start listen a port of master or slave.
        /// </summary>
        void StartListen();
    }
}
