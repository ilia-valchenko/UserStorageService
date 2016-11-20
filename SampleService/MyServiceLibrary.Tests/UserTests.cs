using System;
using System.Globalization;
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
            var user = new User(null, "Valchenko", new DateTime());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_User_With_Empty_Firstname_ExceptionThrown()
        {
            var user = new User("", "Valchenko", new DateTime());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_User_With_Null_Lastname_ExceptionThrown()
        {
            var user = new User("Ilia", null, new DateTime());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_User_With_Empty_Lastname_ExceptionThrown()
        {
            var user = new User("Ilia", "", new DateTime());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_User_With_Future_DateOfBirth_ExceptionThrown()
        {
            var user = new User("Ilia", "Valchenko", DateTime.ParseExact("12/01/2027", "dd/MM/yyyy", CultureInfo.InvariantCulture));
        }
    }
}
