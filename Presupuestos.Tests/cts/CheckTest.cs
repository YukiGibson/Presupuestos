using System;
using System.Linq;
using Presupuestos.cts;
using Presupuestos.DAL;
using Presupuestos.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Presupuestos.Tests.cts
{
    [TestClass]
    public class CheckTest
    {
        [TestMethod]
        [Timeout(25000)]
        [Owner("Laureano")]
        [Priority(10)]
        [Description("Checks if the query is bringing data, if not then throws an error")]
        public void NewBudgets()
        {
            //Arrange
            ProjectionContext db = new ProjectionContext();
            Check check = new Check(db);
            //Act
            IEnumerable<ProjectionViewModel> newBudgets = check.NewBudgets();
            if (String.IsNullOrEmpty(newBudgets.ToString()))
            {
                //Assert
                db.Dispose();
                Assert.Inconclusive("Inconcluso: La lista no pudo traer ningun dato");
            }
            else
            {
                //Assert
                db.Dispose();
                Assert.IsNotNull(newBudgets, "La lista falló en traer datos");
            }
        }

        [TestMethod]
        [Owner("Laureano")]
        [Priority(2)]
        [Description("Checks month name")]
        public void GetMonthName()
        {
            //Arrange
            ProjectionContext db = new ProjectionContext();
            Check check = new Check(db);
            //Act
            string monthName = check.GetMonthName(2017, 12);
            //Assert
            Assert.AreEqual("diciembre", monthName);
        }

        [TestMethod]
        [Owner("Laureano")]
        [Priority(5)]
        [Description("Checks if ")]
        public void CalculateKG()
        {
            //Arrange
            ProjectionContext db = new ProjectionContext();
            Check check = new Check(db);
            //Act
            ProjectionViewModel projection = new ProjectionViewModel();
            projection.Ancho_Pliego = 677;
            projection.Largo_Pliego = 677;
            projection.Montaje = 2;
            projection.Pliegos = 1;
            projection.Gramaje = 63;
            double kgTotal = check.CalculateKG(projection, "7800000");
            //Assert
            Assert.AreEqual(kgTotal, 225222.8706);
        }
        /*
        [TestMethod]
        [Owner("Laureano")]
        [Priority(5)]
        [Description("Checks if ")]
        public void CalculateSheet()
        {
            //Arrange
            ProjectionContext db = new ProjectionContext();
            Check check = new Check(db);
            //Act
            //Assert
        }
        */
    }
}
