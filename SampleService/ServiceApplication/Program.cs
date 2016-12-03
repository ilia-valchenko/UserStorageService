using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using MyServiceLibrary;

namespace ServiceApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var uss = new UserStorageService(new UserStorage());

            #region Input
            var users = new User[]
                {
                new User("Bobby",
                         "McFerrin",
                         Gender.Male,
                         new DateTime(1950, 3, 11),
                         new List<VisaRecord>
                         {
                             new VisaRecord("USA", new DateTime(1969, 4, 2), new DateTime(1969, 5, 16)),
                             new VisaRecord("Mexico", new DateTime(1973, 3, 10), new DateTime(1973, 4, 10))
                         }),

                new User("Ilia",
                         "Valchenko",
                         Gender.Male,
                         new DateTime(1995, 8, 2),
                         new List<VisaRecord>
                         {
                             new VisaRecord("Ukraine", new DateTime(2003, 8, 2), new DateTime(2003, 8, 29)),
                             new VisaRecord("Mexico", new DateTime(2016, 9, 7), new DateTime(2016, 10, 10))
                         }),

                new User("Tim",
                         "Berners-Lee",
                         Gender.Male,
                         new DateTime(1955, 6, 8),
                         new List<VisaRecord>())
                };
            #endregion

            foreach (var user in users)
                uss.Add(user);

            //User toshiro = new User("Toshiro", "Mifune", Gender.Male, new DateTime(1920, 4, 1));
            //uss.Add(toshiro);

            uss.PrintUsersToConsole();

            #region Server emulator
            /*IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 83);
            Socket socketListener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socketListener.Bind(endPoint);
                socketListener.Listen(10);
                bool isWorkFinished = false;

                while (!isWorkFinished)
                {
                    Console.WriteLine("Server is ready to accept a new client.");

                    Socket handler = socketListener.Accept();
                    byte[] bytes = new byte[1024];

                    // Get data from connected socket to buffer
                    int numberOfReceivedBytes = handler.Receive(bytes);

                    NotificationMessage nMsg = null;

                    try
                    {
                        nMsg = TransfromBytesToNotificationMessage(bytes, 0, numberOfReceivedBytes);
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }

                    switch (nMsg.Command)
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

                        case Commands.Stop:
                            Console.WriteLine("The client is calling Stop method.");
                            isWorkFinished = true;
                            break;
                    }

                    //using (NetworkStream networkStream = new NetworkStream(handler))
                    //{
                    //    byte[] data = new byte[1024];
                    //    using (MemoryStream ms = new MemoryStream())
                    //    {

                    //        int numBytesRead;
                    //        while ((numBytesRead = networkStream.Read(data, 0, data.Length)) > 0)
                    //        {
                    //            Console.WriteLine("Current number of bytes: " + numBytesRead);
                    //            ms.Write(data, 0, numBytesRead);
                    //            ms.Seek(0, SeekOrigin.Begin);
                    //        }
                    //        str = Encoding.ASCII.GetString(ms.ToArray(), 0, (int)ms.Length);
                    //    }
                    //}
                }
            }
            catch (SocketException exc)
            {
                Console.WriteLine(exc.Message);
            }*/
            #endregion

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }

        public static NotificationMessage TransfromBytesToNotificationMessage(byte[] array, int offset, int count)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binFormatter = new BinaryFormatter();
                stream.Write(array, offset, count);
                stream.Seek(0, SeekOrigin.Begin);
                return binFormatter.Deserialize(stream) as NotificationMessage;
            }
        }
    }
}
