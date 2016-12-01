using System;
using System.Collections.Generic;
using System.IO;
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

            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 83);
            Socket socketListener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);           

            try
            {
                socketListener.Bind(endPoint);
                socketListener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Wait a connection...");

                    Socket handler = socketListener.Accept();

                    byte[] data = new byte[1024];
                    int numberOfReceivedBytes = handler.Receive(data);

                    NotificationMessage nMsg = TransfromToNotificationMessage(data, 0, numberOfReceivedBytes);

                    Console.WriteLine("Notification command is: " + nMsg.Command);

                    if (nMsg.Command == Commands.Stop)
                    {
                        Console.WriteLine("Service was stopped.");
                        break;
                    }
                }
            }
            catch (SocketException exc)
            {
                Console.WriteLine(exc.Message);
            }

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }

        public static NotificationMessage TransfromToNotificationMessage(byte[] array, int offset, int count)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter binFormatter = new BinaryFormatter();
            ms.Write(array, offset, count);
            ms.Seek(0, SeekOrigin.Begin);
            return binFormatter.Deserialize(ms) as NotificationMessage;
        }
    }
}
