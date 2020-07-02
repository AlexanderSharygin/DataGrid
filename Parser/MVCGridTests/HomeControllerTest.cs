using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCGrid.Controllers;
using MVCGrid.Models;
using Moq;
using System.Collections.Generic;
using MVCGrid;
using System.Linq;

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
        [TestMethod]
        public void GetDataTest()
        {
            Assert.IsInstanceOfType(controller.GetData(), typeof(string));
        }
        [TestMethod]
        public void CreatePostAction_ModelError()
        {
            // arrange
            string expected = "Create";
            var mock = new Mock<Repository>();
            WorkersSmall w = new WorkersSmall();
            HomeController controller = new HomeController(mock.Object);
            controller.ModelState.AddModelError("Name", "Имя не указано");
            // act
            ViewResult result = controller.Create(w) as ViewResult;
            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }
        [TestMethod]
        public void CreatePostAction_RedirectToIndexView()
        {
            // arrange
            string expected = "Index";
            var mock = new Mock<Repository>();
            WorkersSmall w = new WorkersSmall { Address = "a", BirthDate = DateTime.Parse("01/01/2001"), FirstName = "a", Id = 11111, IsAlcoholic = false, LastName = "b", Notes = "", Position = "s", Prefix = "c", Salary = 111, StateID = 122321 };
            

            HomeController controller = new HomeController(mock.Object);
            // act
            RedirectToRouteResult result = controller.Create(w) as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
         
        }
      
        

    }
}
