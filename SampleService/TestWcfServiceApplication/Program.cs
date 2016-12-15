using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWcfServiceApplication.ServiceReference1;

namespace TestWcfServiceApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceClient client = new ServiceClient();
            UserLibrary.User leonard = new UserLibrary.User("Leonard", "Cohen", "Male", new DateTime(1934, 9, 21));

            Console.WriteLine($"Test connection: {client.TestConnection()}");

            try
            {
                client.Add(leonard);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Error message: {exc.Message}\n\nStack trace: {exc.StackTrace}");
            }

            client.Close();

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }
    }
}
