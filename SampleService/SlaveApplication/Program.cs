using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using MyServiceLibrary;

namespace SlaveApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //SlaveService slave = new SlaveService(new UserStorage(), new List<User>(), new Logger());

            //Console.WriteLine("Current slave's domain: " + AppDomain.CurrentDomain.FriendlyName);

            int port = 83;
            string hostName = "localhost";
            NotificationMessage msg = new NotificationMessage(Commands.Add, new User(), 0);

            IPHostEntry ipHost = Dns.GetHostEntry(hostName);
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

            Socket sender = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sender.Connect(endPoint);
                sender.Send(TransformMessageToByteArray(msg));
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }

        public static byte[] TransformMessageToByteArray(NotificationMessage msg)
        {
            if (msg == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, msg);

            return ms.ToArray();
        }
    }
}
