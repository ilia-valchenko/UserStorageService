using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Security.Policy;
using System.Threading;
using NLog;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServiceApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Input data
            //var users = new User[]
            //    {
            //    new User("Bobby",
            //             "McFerrin",
            //             Gender.Male,
            //             new DateTime(1950, 3, 11),
            //             new List<VisaRecord>
            //             {
            //                 new VisaRecord("USA", new DateTime(1969, 4, 2), new DateTime(1969, 5, 16)),
            //                 new VisaRecord("Mexico", new DateTime(1973, 3, 10), new DateTime(1973, 4, 10))
            //             }),

            //    new User("Ilia",
            //             "Valchenko",
            //             Gender.Male,
            //             new DateTime(1995, 8, 2),
            //             new List<VisaRecord>
            //             {
            //                 new VisaRecord("Ukraine", new DateTime(2003, 8, 2), new DateTime(2003, 8, 29)),
            //                 new VisaRecord("Mexico", new DateTime(2016, 9, 7), new DateTime(2016, 10, 10))
            //             }),

            //    new User("Tim",
            //             "Berners-Lee",
            //             Gender.Male,
            //             new DateTime(1955, 6, 8),
            //             new List<VisaRecord>()),

            //    new User("Toshiro",
            //             "Mifune",
            //             Gender.Male,
            //             new DateTime(1920, 4, 1))
            //    };
            #endregion

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

            int numberOfSlaves;
            Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["NumberOfSlaves"], out numberOfSlaves);
            string assemblyPath = @"G:\GitEpam\UserStorageService\SampleService\MyServiceLibrary\bin\Debug\MyServiceLibrary.dll";
            AppDomain masterDomain = AppDomain.CreateDomain("masterdomain");
            AppDomain[] domainsOfSlaves = new AppDomain[numberOfSlaves];

            Console.WriteLine("Number of slaves: " + numberOfSlaves);

            for (int i = 0; i < numberOfSlaves; i++)
                domainsOfSlaves[i] = AppDomain.CreateDomain("slavedomain" + i);

            masterDomain.DoCallBack(() =>
                {
                    Console.WriteLine("Start master DoCallBack method.");

                    Assembly serviceAssembly = null;

                    try
                    {
                        serviceAssembly = Assembly.LoadFrom(@"G:\GitEpam\UserStorageService\SampleService\MyServiceLibrary\bin\Debug\MyServiceLibrary.dll");
                    }
                    catch (Exception exc)
                    {
                        //logger.WriteError($"Unable to load assembly by the given path: {assemblyPath}");
                        //logger.WriteError(exc.Message);
                        Console.WriteLine(exc.Message);
                    }

                    Type ussType = serviceAssembly.GetType("MyServiceLibrary.UserStorageService");
                    var storageInstance = serviceAssembly.CreateInstance("MyServiceLibrary.UserStorage");
                    var masterServiceInstance = serviceAssembly.CreateInstance("MyServiceLibrary.UserStorageService", false, BindingFlags.CreateInstance, default(Binder), new object[] { storageInstance }, CultureInfo.CurrentCulture, new object[] { });

                    IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                    IPAddress ipAddr = ipHost.AddressList[0];
                    IPEndPoint endPoint = new IPEndPoint(ipAddr, 83);
                    Socket socketListener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    //logger.WriteInfo($"Master's ipHost: {ipHost.HostName}");
                    Console.WriteLine($"Master's ipHost: {ipHost.HostName}");
                    //logger.WriteInfo($"Master's IP address: {ipAddr}");
                    Console.WriteLine($"Master's IP address: {ipAddr}");

                    try
                    {
                        socketListener.Bind(endPoint);
                        socketListener.Listen(10);
                    }
                    catch (SocketException exc)
                    {
                        //logger.WriteError(exc.Message);
                        Console.WriteLine(exc.Message);
                    }

                    

                    ////Thread thread = new Thread(() =>
                    ////    {   
                    ////        Type userType = servicAssembly.GetType("MyServiceLibrary.User");      
                    ////        var user = servicAssembly.CreateInstance("MyServiceLibrary.User");
                    ////        MethodInfo mi = ussType.GetMethod("Add");
                    ////        mi.Invoke(masterServiceInstance, new object[] { user });
                    ////    }
                    ////);

                    ////    thread.Start();

                    bool isWorkFinished = false;

                    Thread thread = new Thread(() => {

                    
                    while (!isWorkFinished)
                    { 
                        Socket handler = socketListener.Accept();

                        byte[] bytes = new byte[1024];

                        // Get data from connected socket to buffer
                        int numberOfReceivedBytes = handler.Receive(bytes);

                        // test
                        Type commandsEnum = serviceAssembly.GetType("MyServiceLibrary.Commands");

                        Type notificationMessageType = serviceAssembly.GetType("MyServiceLibrary.NotificationMessage");
                        Type addNotificationMessageType = serviceAssembly.GetType("MyServiceLibrary.AddNotificationMessage");
                        Type deleteNotificationMessageType = serviceAssembly.GetType("MyServiceLibrary.DeleteNotificationMessage");
                        Type getUserByPredicateNotificationMessageType = serviceAssembly.GetType("MyServiceLibrary.AddNotificationMessage");
                        Type getUsersByPredicateNotificationMessageType = serviceAssembly.GetType("MyServiceLibrary.AddNotificationMessage");
                        Type stopNotificationMessageType = serviceAssembly.GetType("MyServiceLibrary.AddNotificationMessage");
                        //object currentMessage = null;
                        dynamic currentMessage = null;

                        //var testMessage = serviceAssembly.CreateInstance("MyServiceLibrary.StopNotificationMessage");

                        try
                        {
                            MethodInfo transform = notificationMessageType.GetMethod("TransfromBytesToNotificationMessage");

                            try
                            {
                                Console.WriteLine("Master domain before trahsform to NotMsg.");
                                currentMessage = transform.Invoke(null, new object[] {bytes, 0, numberOfReceivedBytes});
                                Console.WriteLine("CURRENT MESSAGE IS: " + currentMessage);
                            }
                            catch (Exception exc)
                            {
                                Console.WriteLine("Error with invoke transform method." + exc.Message);
                            }
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine(exc.Message);
                        }

                       //string command = currentMessage.Command.ToString();                     

                       // switch (command.ToString())
                       // {
                       //     case "Add":
                       //         Console.WriteLine("The client is calling Add method.");
                       //         break;

                       //     case "Delete":
                       //         Console.WriteLine("The client is calling Delete method.");
                       //         break;

                       //     case "GetUserByPredicate":
                       //         Console.WriteLine("The client is calling GetUserByPredicate method.");
                       //         break;

                       //     case "GetUsersByPredicate":
                       //         Console.WriteLine("The client is calling GetUsersByPredicate method.");
                       //         break;

                       //     case "Stop":
                       //         Console.WriteLine("The client is calling Stop method.");
                       //         isWorkFinished = true;
                       //         Thread.CurrentThread.Abort();
                       //         break;
                       // }

                        //Thread thread = new Thread(() =>
                        //    {
                        //        //logger.WriteInfo("New thread started.");
                        //        Console.WriteLine("New thread started.");

                        //        Socket handler = socketListener.Accept();

                        //        byte[] bytes = new byte[1024];

                        //        // Get data from connected socket to buffer
                        //        int numberOfReceivedBytes = handler.Receive(bytes);

                        //        // test
                        //        Type commandsEnum = serviceAssembly.GetType("MyServiceLibrary.Commands");

                        //        Type notificationMessageType = serviceAssembly.GetType("MyServiceLibrary.NotificationMessage");
                        //        Type addNotificationMessageType = serviceAssembly.GetType("MyServiceLibrary.AddNotificationMessage");
                        //        Type deleteNotificationMessageType = serviceAssembly.GetType("MyServiceLibrary.DeleteNotificationMessage");
                        //        Type getUserByPredicateNotificationMessageType = serviceAssembly.GetType("MyServiceLibrary.AddNotificationMessage");
                        //        Type getUsersByPredicateNotificationMessageType = serviceAssembly.GetType("MyServiceLibrary.AddNotificationMessage");
                        //        Type stopNotificationMessageType = serviceAssembly.GetType("MyServiceLibrary.AddNotificationMessage");
                        //        //object currentMessage = null;
                        //        dynamic currentMessage = null;

                        //        try
                        //        {
                        //            MethodInfo transform = ussType.GetMethod("TransfromBytesToNotificationMessage");
                        //            currentMessage = transform.Invoke(masterServiceInstance, new object[] { bytes, 0, numberOfReceivedBytes });
                        //        }
                        //        catch (Exception exc)
                        //        {
                        //            Console.WriteLine(exc.Message);
                        //        }

                        //        string command = currentMessage.Command;

                        //        switch (command)
                        //        {
                        //            case "Add":
                        //                Console.WriteLine("The client is calling Add method.");
                        //                break;

                        //            case "Delete":
                        //                Console.WriteLine("The client is calling Delete method.");
                        //                break;

                        //            case "GetUserByPredicate":
                        //                Console.WriteLine("The client is calling GetUserByPredicate method.");
                        //                break;

                        //            case "GetUsersByPredicate":
                        //                Console.WriteLine("The client is calling GetUsersByPredicate method.");
                        //                break;

                        //            case "Stop":
                        //                Console.WriteLine("The client is calling Stop method.");
                        //                isWorkFinished = true;
                        //                break;
                        //        }

                        //        // -------------------------------------------- //


                        //        //Type userType = servicAssembly.GetType("MyServiceLibrary.User");
                        //        //var user = servicAssembly.CreateInstance("MyServiceLibrary.User");
                        //        //MethodInfo mi = ussType.GetMethod("Add");
                        //        //mi.Invoke(masterServiceInstance, new object[] { user });

                        //        Thread.CurrentThread.Abort();

                        //    }
                        //);



                        //thread.Start();


                        // Get data from connected socket to buffer
                        //int numberOfReceivedBytes = handler.Receive(bytes);

                        //NotificationMessage nMsg = null;
                    }


                    });
                    //end

                    thread.Start();

                }
            );


            // --------------------------------------------- //

            slaveDomain.DoCallBack(() =>
            {
                Assembly serviceAssembly = Assembly.LoadFrom(@"G:\GitEpam\UserStorageService\SampleService\MyServiceLibrary\bin\Debug\MyServiceLibrary.dll");
                // Type of abstract class
                Type notificationMessageType = serviceAssembly.GetType("MyServiceLibrary.NotificationMessage");
                // This message is a stub
                dynamic stopNotificationMessage = serviceAssembly.CreateInstance("MyServiceLibrary.StopNotificationMessage");

                int port = 83;
                string hostName = "localhost";

                IPHostEntry ipHost = Dns.GetHostEntry(hostName);
                IPAddress ipAddress = ipHost.AddressList[0];
                IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

                Socket sender = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                sender.Connect(endPoint);

                MethodInfo transform = notificationMessageType.GetMethod("TransformMessageToBytes");
                var convertedMessage = (byte[])transform.Invoke(null, new object[] { stopNotificationMessage });

                sender.Send(convertedMessage);
            }
            );

            // ---------------------------------------------------- //

            domainsOfSlaves[0].DoCallBack(() =>
                {
                    Console.WriteLine("Start slave DoCallBack method.");

                    Assembly serviceAssembly = null;

                    try
                    {
                        serviceAssembly = Assembly.LoadFrom(@"G:\GitEpam\UserStorageService\SampleService\MyServiceLibrary\bin\Debug\MyServiceLibrary.dll");
                    }
                    catch (Exception exc)
                    {
                        //logger.WriteError($"Unable to load assembly by the given path: {assemblyPath}");
                        //logger.WriteError(exc.Message);
                        Console.WriteLine(exc.Message);
                    }

                    // Type of abstract class
                    Type notificationMessageType = serviceAssembly.GetType("MyServiceLibrary.NotificationMessage");
                    dynamic stopNotificationMessage = serviceAssembly.CreateInstance("MyServiceLibrary.StopNotificationMessage");

                    Console.WriteLine("The command from message is: " + stopNotificationMessage.Command);

                    int port = 83;
                    string hostName = "localhost";

                    IPHostEntry ipHost = Dns.GetHostEntry(hostName);
                    IPAddress ipAddress = ipHost.AddressList[0];
                    IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

                    Socket sender = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    try
                    {
                        sender.Connect(endPoint);

                        MethodInfo transform = notificationMessageType.GetMethod("TransformMessageToBytes");
                        var convertedMessage = (byte[])transform.Invoke(null, new object[] {stopNotificationMessage});

                        // test
                        //var transform2 = notificationMessageType.GetMethod("TransfromBytesToNotificationMessage");
                        //dynamic msg = transform2.Invoke(null, new object[] { convertedMessage, 0, convertedMessage.Length });

                        //Console.WriteLine("Test output with vice versa transfrom method: " + msg.Command.ToString());
                        // end of the test


                        sender.Send(convertedMessage);
                    }
                    catch (SocketException exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
            );

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }
    }
}





