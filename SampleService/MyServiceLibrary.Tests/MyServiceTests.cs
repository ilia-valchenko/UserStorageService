using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MyServiceLibrary.Tests
{
    [TestClass]
    public class MyServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_NullUser_ExceptionThrown()
        {
            var service = new UserStorageService(new UserStorage());
            service.Add(null);
        }

        // Test Moq framework
        [TestMethod]
        public void GetUsersByPredicate_Where_Name_Ilia()
        {
            // Arrange 
            var mock = new Mock<IUserStorage>();
            mock.Setup(service => service.GetUsersByPredicate((User user) => user.FirstName == "Ilia"))
                .Returns(new List<User>() { new User("Ilia", "Valchenko", Gender.Male, new DateTime(1995, 8, 2), null),
                                            new User("Ilia", "Codogno", Gender.Male, new DateTime(1938, 1, 6), null),
                                           });
            var uss = new UserStorageService(mock.Object);

            // Act 
            var result = uss.GetUserByPredicate((User user) => user.FirstName == "Ilia");

            // Assert
            Assert.AreEqual(result, new List<User>() { new User("Ilia", "Valchenko", Gender.Male, new DateTime(1995, 8, 2), null),
                                                       new User("Ilia", "Codogno", Gender.Male, new DateTime(1938, 1, 6), null),
                                                     });
        }

        // http://metanit.com/sharp/mvc5/18.5.php

        [TestMethod]
        public void GetUserByPredicate_Where_Name_Is_Ilia()
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
                         },
                         1),

                new User("Ilia",
                         "Valchenko",
                         Gender.Male,
                         new DateTime(1995, 8, 2),
                         new List<VisaRecord>
                         {
                             new VisaRecord("Ukraine", new DateTime(2003, 8, 2), new DateTime(2003, 8, 29)),
                             new VisaRecord("Mexico", new DateTime(2016, 9, 7), new DateTime(2016, 10, 10))
                         },
                         2),

                new User("Tim",
                         "Berners-Lee",
                         Gender.Male,
                         new DateTime(1955, 6, 8),
                         new List<VisaRecord>(),
                         3)
            };

            foreach (var user in users)
                uss.Add(user);

            var result = uss.GetUserByPredicate((User u) => u.FirstName == "Ilia");

            Assert.AreEqual(result, users[1]);
        }
    }
}
