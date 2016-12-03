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
            AddNotificationMessage msg = new AddNotificationMessage(new User());

            #region Client emulator
            int port = 83;
            string hostName = "localhost";

            IPHostEntry ipHost = Dns.GetHostEntry(hostName);
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

            Socket sender = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sender.Connect(endPoint);
                //NetworkStream stream = new NetworkStream(sender);
                sender.Send(TransformMessageToBytes(msg));
            }
            catch (SocketException exc)
            {
                Console.WriteLine(exc.Message);
            } 
            #endregion

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }

        public static byte[] TransformMessageToBytes(NotificationMessage msg)
        {
            if (msg == null)
                return null;

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binFormatter = new BinaryFormatter();
                binFormatter.Serialize(stream, msg);
                return stream.ToArray();
            }
        }
    }
}
