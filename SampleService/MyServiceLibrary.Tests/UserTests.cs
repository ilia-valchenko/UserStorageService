using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyServiceLibrary.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_User_With_Null_Firstname_ExceptionThrown()
        {
            var user = new User(null, "Valchenko", Gender.Male, new DateTime(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_User_With_Empty_Firstname_ExceptionThrown()
        {
            var user = new User("", "Valchenko", Gender.Male, new DateTime(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_User_With_Null_Lastname_ExceptionThrown()
        {
            var user = new User("Ilia", null, Gender.Male, new DateTime(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_User_With_Empty_Lastname_ExceptionThrown()
        {
            var user = new User("Ilia", "", Gender.Male, new DateTime(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_User_With_Future_DateOfBirth_ExceptionThrown()
        {
            var user = new User("Ilia", "Valchenko", Gender.Male, new DateTime(2027, 8, 2), null);
        }
    }
}
