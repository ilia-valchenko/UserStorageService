using System;
using MyServiceLibrary;
using System.Configuration;
using UserLibrary;

namespace ServiceApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region This part of code may be used to work with remote library, but I can't deserialized an unknown object.
            //int numberOfSlaves;
            //Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["NumberOfSlaves"], out numberOfSlaves);
            //string serviceLibraryPath = @"G:\GitEpam\UserStorageService\SampleService\MyServiceLibrary\bin\Debug\MyServiceLibrary.dll";

            //Assembly serviceAssembly = Assembly.LoadFrom(serviceLibraryPath);

            //Type userType = serviceAssembly.GetType("MyServiceLibrary.User");
            //Type storageType = serviceAssembly.GetType("MyServiceLibrary.UserStorage");
            //Type userStorageServiceType = serviceAssembly.GetType("MyServiceLibrary.UserStorageService");
            //Type notificatiomMessageType = serviceAssembly.GetType("MyServiceLibrary.NotificationMessage");

            //var storage = Activator.CreateInstance(storageType);
            //var userStorageService = Activator.CreateInstance(userStorageServiceType, new object[] { storage });

            //IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            //IPAddress ipAddr = ipHost.AddressList[0];
            //IPEndPoint endPoint = new IPEndPoint(ipAddr, 83);
            //Socket socketListener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //try
            //{
            //    socketListener.Bind(endPoint);
            //    socketListener.Listen(10);
            //    bool isWorkFinished = false;

            //    while (!isWorkFinished)
            //    {
            //        Console.WriteLine("Server is ready to accept a new client...");

            //        Socket handler = socketListener.Accept();
            //        byte[] bytes = new byte[2064];

            //        // Get data from connected socket to buffer
            //        int numberOfReceivedBytes = handler.Receive(bytes);
            //        byte[] bytesOfMessage = new byte[numberOfReceivedBytes];

            //        for (int i = 0; i < numberOfReceivedBytes; i++)
            //            bytesOfMessage[i] = bytes[i];

            //        MethodInfo transfromMethod = notificatiomMessageType.GetMethod("TransfromBytesToNotificationMessage");
            //        // We can't deserialize an unknown object
            //        var receivedMessage = transfromMethod.Invoke(null, new object[] {bytes, 0, numberOfReceivedBytes});

            //        /*switch (receivedMessage.Command)
            //        {
            //            case Commands.Add:
            //                Console.WriteLine("The client is calling Add method.");
            //                //MethodInfo add = userStorageServiceType.GetMethod("Add");
            //                //var res = add.Invoke(userStorageService, new object[] { addNotificatiomMessageType.GetProperty("User").GetValue(result, null) });
            //                break;

            //            case Commands.Delete:
            //                Console.WriteLine("The client is calling Delete method.");
            //                break;

            //            case "GetUserByPredicate":
            //                Console.WriteLine("The client is calling GetUserByPredicate method.");
            //                break;

            //            case "GetUsersByPredicate":
            //                Console.WriteLine("The client is calling GetUsersByPredicate method.");
            //                break;

            //            case Commands.Stop:
            //                Console.WriteLine("The client is calling Stop method.");
            //                isWorkFinished = true;
            //                break;

            //            default:
            //                Console.WriteLine("The command is undefined.");
            //                isWorkFinished = true;
            //                break;
            //        }*/
            //    }
            //}
            //catch (SocketException exc)
            //{
            //    Console.WriteLine(exc.Message);
            //} 
            #endregion

            string hostName = "localhost";

            int masterPort;
            Int32.TryParse(ConfigurationSettings.AppSettings.Get("MasterPort"), out masterPort);

            int numberOfSlaves;
            Int32.TryParse(ConfigurationSettings.AppSettings.Get("NumberOfSlaves"), out numberOfSlaves);

            int[] portsOfSlaves = new int[numberOfSlaves];
            for(int i = 0; i < numberOfSlaves; i++)
                Int32.TryParse(ConfigurationSettings.AppSettings.Get("SlavePort" + i), out portsOfSlaves[i]);

            /*AppDomain masterDomain = AppDomain.CreateDomain("masterdomain");

            var master = masterDomain.CreateInstance(
                    "MyServiceLibrary", // assemblyFile 
                    "MyServiceLibrary.Master", // typeName                                                           
                    false, // ignoreCase                                                        
                    BindingFlags.CreateInstance, // bindingAttr                                                                
                    default(Binder), // binder                                                            
                    new object[] { masterPort, hostName, portsOfSlaves }, // args                                                                 
                    CultureInfo.CurrentCulture, // culture                                                                 
                    new object[] { } // activationAttributes
            );*/

            Master master = new Master(masterPort, hostName, portsOfSlaves);

            User ilia = new User("Ilia", "Valchenko", "Male", new DateTime(1995, 8, 2));
            User toshiro = new User("Toshiro", "Mifune", "Male", new DateTime(1920, 4, 1));

            master.Add(ilia);
            master.Add(toshiro);

            master.StartListen();

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }
    }
}





