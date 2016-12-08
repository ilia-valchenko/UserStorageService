using System;
using System.Globalization;
using System.IO;
using System.Reflection;

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

            int numberOfSlaves;
            Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["NumberOfSlaves"], out numberOfSlaves);
            AppDomain masterDomain = AppDomain.CreateDomain("masterdomain");
            AppDomain[] domainsOfSlaves = new AppDomain[numberOfSlaves];
            string serviceLibraryPath = @"G:\GitEpam\UserStorageService\SampleService\MyServiceLibrary\bin\Debug\MyServiceLibrary.dll";
            Assembly serviceAssembly = Assembly.LoadFrom(serviceLibraryPath);

            for (int i = 0; i < numberOfSlaves; i++)
                domainsOfSlaves[i] = AppDomain.CreateDomain("slavedomain" + i);

            Type userType = serviceAssembly.GetType("MyServiceLibrary.User");
            Type storageType = serviceAssembly.GetType("MyServiceLibrary.UserStorage");
            Type userStorageServiceType = serviceAssembly.GetType("MyServiceLibrary.UserStorageService");

            var storage = masterDomain.CreateInstanceFromAndUnwrap(serviceLibraryPath, storageType.FullName);

            Console.WriteLine("Type of storage: " + storage.GetType());

            var userStorageService = masterDomain.CreateInstanceFromAndUnwrap(
                    serviceLibraryPath, // assemblyFile 
                    userStorageServiceType.FullName, // typeName                                                           
                    false, // ignoreCase                                                        
                    BindingFlags.CreateInstance, // bindingAttr                                                                
                    default(Binder), // binder                                                            
                    new object[] {storage}, // args                                                                 
                    CultureInfo.CurrentCulture, // culture                                                                 
                    new object[] {} // activationAttributes
            );

            var toshiro = masterDomain.CreateInstanceFromAndUnwrap(
                    serviceLibraryPath, // assemblyFile 
                    userType.FullName, // typeName                                                           
                    false, // ignoreCase                                                        
                    BindingFlags.CreateInstance, // bindingAttr                                                                
                    default(Binder), // binder                                                            
                    new object[] { "Toshiro", "Mifune", "Male", new DateTime(1920, 4, 1) }, // args                                                                 
                    CultureInfo.CurrentCulture, // culture                                                                 
                    new object[] { } // activationAttributes
            );

            Proxy.Call(masterDomain, serviceLibraryPath, userStorageServiceType.FullName, userStorageService, "Add",
                new object[] {toshiro});

            Console.WriteLine("\nTap to continue...");
            Console.ReadKey(true);
        }
    }
}





