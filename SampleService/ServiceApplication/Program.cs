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

            BinarySearchTree.BinarySearchTree<int> bst =new BinarySearchTree<int>();

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }
    }
}
