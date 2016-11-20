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
                .Returns(new List<User>() { new User("Ilia", "Valchenko", new DateTime(1995, 8, 2)),
                                            new User("Ilia", "Codogno", new DateTime(1938, 1, 6)),
                                           });
            var uss = new UserStorageService(mock.Object);

            // Act 
            var result = uss.GetUserByPredicate((User user) => user.FirstName == "Ilia");

            // Assert
            Assert.AreEqual(result, new List<User>() { new User("Ilia", "Valchenko", new DateTime(1995, 8, 2)),
                                                       new User("Ilia", "Codogno", new DateTime(1938, 1, 6)),
                                                     });
        }

        // http://metanit.com/sharp/mvc5/18.5.php

        [TestMethod]
        public void GetUserByPredicate_Where_Name_Is_Ilia()
        {
            var uss = new UserStorageService(new UserStorage());

            var users = new User[]
            {
                new User("Bobby", "McFerrin", new DateTime(1950, 3, 11), 1),
                new User("Ilia", "Valchenko", new DateTime(1995, 8, 2), 2),
                new User("Tim", "Berners-Lee", new DateTime(1955, 6, 8), 3)
            };

            foreach (var user in users)
                uss.Add(user);

            var result = uss.GetUserByPredicate((User u) => u.FirstName == "Ilia");

            Assert.AreEqual(result, users[1]);
        }
    }
}
