using System;
using System.Collections.Generic;
using MyServiceLibrary;

namespace ServiceApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var uss = new UserStorageService(new UserStorage());

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

            foreach (var user in users)
                uss.Add(user);

            var result = uss.GetUserByPredicate((User u) => u.FirstName == "Ilia");

            Console.WriteLine($"The result of finding by predicate: {result}");

            //Console.WriteLine($"Are equal: {Object.ReferenceEquals(result, users[1])}");

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }
    }
}
