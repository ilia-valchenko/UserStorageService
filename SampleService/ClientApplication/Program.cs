using System;
using MyServiceLibrary;
using UserLibrary;
using SearchCriteriaLibrary;

namespace ClientApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Slave slave = new Slave(2030, "localhost", 3000);
            //slave.GetUserByPredicate((User u) => u.FirstName == "Ilia");
            slave.GetUserByPredicate(Criteria.GetUserByName);

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }
    }
}
