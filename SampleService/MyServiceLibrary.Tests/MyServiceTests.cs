using System;
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
            var service = new UserStorageService();
            service.Add(null);
        }

        //[TestMethod]
        //[ExpectedException(typeof(In))]

        // http://metanit.com/sharp/mvc5/18.5.php

        // Arrange 
        // Act 
        // Assert

        // test moq
        [TestMethod]
        public void Test()
        {
            var mock = new Mock<>();
        }
    }
}
