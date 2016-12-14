using System;
using MyServiceLibrary;
using UserLibrary;
using SearchCriteriaLibrary;
using System.Threading.Tasks;

namespace ClientApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Slave slave = new Slave(2030, "localhost", 3000);

            Task listenerTask = new Task(slave.StartListen);
            listenerTask.Start();

            User user = slave.GetUserByPredicate(Criteria.GetUserByName);
            Console.WriteLine($"RESULT: {user}");

            listenerTask.Wait();

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }
    }
}
