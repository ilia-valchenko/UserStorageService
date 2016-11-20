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

            var bst = new BinarySearchTree<int>();

            //int[] arr = {8,3,10,1,6,14,4,7,13};
            int[] arr = {4,2,5,1,3,7,6,8};
            //int[] arr = {5, 2, 18, -4, 3, 21, 19, 25};

            foreach (var VARIABLE in arr)
            {
                bst.Add(VARIABLE);
            }

            // Test the remove method
            bst.Remove(5);

            foreach (var VARIABLE in bst)
            {
                Console.Write(VARIABLE + " ");
            }

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }
    }
}
