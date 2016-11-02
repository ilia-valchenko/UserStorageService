using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
