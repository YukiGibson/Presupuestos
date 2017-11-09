using System;
using System.Collections.Generic;
using Presupuestos;
using Presupuestos.Controllers;
using Presupuestos.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Presupuestos.Tests.Controllers
{
    [TestClass]
    public class CommercialControllerTest
    {
        [TestMethod]
        public void Dashboard()
        {
            //Arrange
            CommercialController controller = new CommercialController();
            //Act
            MainViewModel main = new MainViewModel();
            main.Projections = new List<ProjectionViewModel>
            {
                new ProjectionViewModel { Ancho_Bobina = 45, Checked = true, Cliente = "Somthing" }
            };

            var result = controller.Dashboard(main);
            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DashboardPost() 
        { 
            //Arrange
            CommercialController controller = new CommercialController();
            //Act
            MainViewModel main = new MainViewModel();
            main.Projections = new List<ProjectionViewModel>
            {
                new ProjectionViewModel { Presupuesto="23456", Ancho_Bobina = 45, Checked = true, Cliente = "Somthing" },
                new ProjectionViewModel { Presupuesto="23457", Ancho_Bobina = 45, Checked = true, Cliente = "Marco" },
                new ProjectionViewModel { Presupuesto="23458", Ancho_Bobina = 45, Checked = true, Cliente = "Bobafeet" },
                new ProjectionViewModel { Presupuesto="23459", Ancho_Bobina = 45, Checked = true, Cliente = "Guillermo" },
                new ProjectionViewModel { Presupuesto="23460", Ancho_Bobina = 45, Checked = true, Cliente = "Meesa" }
            };
            main.Month = new List<MonthViewModel>
            {
                new MonthViewModel { month = 1, value = "0", year = 2017 },
                new MonthViewModel { month = 2, value = "56,000", year = 2017 },
                new MonthViewModel { month = 3, value = "98,765,432", year = 2017 }
            };
            var result = controller.Dashboard(main, "");
            //Assert
            Assert.IsNotNull(result);
           
        }
    }
}
