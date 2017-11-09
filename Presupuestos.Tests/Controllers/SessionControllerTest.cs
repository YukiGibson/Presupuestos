using System;
using Presupuestos.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Presupuestos.Tests.Controllers
{
    [TestClass]
    public class SessionControllerTest
    {
        [TestMethod]
        public void Imports()
        {
            //Arrange
            SessionController controller = new SessionController();
            var a = controller.Imports("");
            //Act
            //Assert
        } // End Import
    }
}
