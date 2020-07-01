using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCGrid.Controllers;


namespace MVCGridTests
{
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController controller;
        private ViewResult result;

        [TestInitialize]
        public void SetupContext()
        {
            controller = new HomeController();
            result = controller.Index() as ViewResult;
        }

        [TestMethod]
        public void IndexViewResultNotNull()
        {
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexViewEqualIndexCshtml()
        {
            Assert.AreEqual("Index", result.ViewName);
        }
        public void GetDataTest()
        {
           Assert.IsInstanceOfType(GetData Type expectedType)
        }


    }
}
