using System;
using BinarySearchTree;
using MyServiceLibrary;

namespace ServiceApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var service = new UserStorageService();

            // 1. Add a new user to the storage.
            // 2. Remove an user from the storage.
            // 3. Search for an user by the first name.
            // 4. Search for an user by the last name.

            //var bst = new BinarySearchTree<int>();

            ////int[] arr = {8,3,10,1,6,14,4,7,13};
            //int[] arr = {4,2,5,1,3,7,6,8};
            ////int[] arr = {5, 2, 18, -4, 3, 21, 19, 25};

            //foreach (var VARIABLE in arr)
            //{
            //    bst.Add(VARIABLE);
            //}

            //// Test the remove method
            //bst.Remove(5);

            //foreach (var VARIABLE in bst)
            //{
            //    Console.Write(VARIABLE + " ");
            //}





            //var storage = new UserStorage();
            //var uss = new UserStorageService(storage);

            //var users = new User[]
            //{
            //    new User("Bobby", "McFerrin", new DateTime(1950, 3, 11)),
            //    new User("Ilia", "Valchenko", new DateTime(1995, 8, 2)),
            //    new User("Tim", "Berners-Lee", new DateTime(1955, 6, 8))
            //};

            //foreach (var user in users)
            //    uss.Add(user);

            //Console.WriteLine("Print all users in the storage:");
            //foreach(var item in storage.storage)
            //    Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");



            var uss = new UserStorageService(new UserStorage());

            var users = new User[]
            {
                new User("Bobby", "McFerrin", new DateTime(1950, 3, 11), 1),
                new User("Ilia", "Valchenko", new DateTime(1995, 8, 2), 2),
                new User("Tim", "Berners-Lee", new DateTime(1955, 6, 8), 3)
            };

            foreach (var user in users)
                uss.Add(user);

            //Console.WriteLine("TypeOf: " + users[1].GetType());
            Console.WriteLine("Test ReferenceEquals method: " + Object.ReferenceEquals(users[1], users[1]));

            var result = uss.GetUserByPredicate((User u) => u.FirstName == "Ilia");

            Console.WriteLine($"Are equal: {Object.ReferenceEquals(result, users[1])}");

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }
    }
}
