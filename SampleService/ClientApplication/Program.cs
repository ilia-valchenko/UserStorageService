using System;
using MyServiceLibrary;
using UserLibrary;
using SearchCriteriaLibrary;
using System.Threading.Tasks;

namespace ClientApplication
{
    class Program
    {
        public static Slave slave = new Slave(2030, "localhost", 3000);

        //public static string hostName = ConfigurationSettings.AppSettings.Get("HostName");

        //int masterPort;
        //Int32.TryParse(ConfigurationSettings.AppSettings.Get("MasterPort"), out masterPort);

        //    int numberOfSlaves;
        //Int32.TryParse(ConfigurationSettings.AppSettings.Get("NumberOfSlaves"), out numberOfSlaves);

        //    int[] portsOfSlaves = new int[numberOfSlaves];
        //    for(int i = 0; i<numberOfSlaves; i++)
        //        Int32.TryParse(ConfigurationSettings.AppSettings.Get("SlavePort" + i), out portsOfSlaves[i]);

        static void Main(string[] args)
        {
            //Slave slave = new Slave(2030, "localhost", 3000);

            Task listenerTask = new Task(slave.StartListen);
            listenerTask.Start();

            
            //User user = slave.GetUserByPredicate(Criteria.GetUserByName);
            //Console.WriteLine($"RESULT: {user}");

            Task searchingTask = new Task(Search);
            searchingTask.Wait(10000);
            searchingTask.Start();
            

            //User anotherUser = slave.GetUserByPredicate(Criteria.GetUserByName);
            //Console.WriteLine($"RESULT: {user}");

            listenerTask.Wait();

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }

        public static void Search()
        {
            slave.GetUserByPredicate(Criteria.GetUserByName);
        }
    }
}
