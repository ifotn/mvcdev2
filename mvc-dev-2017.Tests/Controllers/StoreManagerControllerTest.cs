using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mvc_dev_2017.Controllers;
using Moq;


namespace mvc_dev_2017.Tests.Controllers
{
    [TestClass]
    public class StoreManagerControllerTest
    {
        
        [TestMethod]
        public void Index()
        {
            // Arrange
            StoreManagerController controller = new StoreManagerController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Details_Found()
        {
            // Arrange
            StoreManagerController controller = new StoreManagerController();

            // Act
            ViewResult result = controller.Details(388) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Details Passed");
        }

        [TestMethod]
        public void Details_NotFound()
        {
            StoreManagerController controller = new StoreManagerController();
            ViewResult result = controller.Details(999) as ViewResult;
            Assert.IsNull(result);

        }

        [TestMethod]
        public void Create_Fail()
        {
            
        }
}
}
