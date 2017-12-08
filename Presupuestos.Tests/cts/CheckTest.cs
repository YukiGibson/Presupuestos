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
            ProjectionContext projectionContext = new ProjectionContext();
            SapDataContext sapDataContext = new SapDataContext();
            Check check = new Check(projectionContext, sapDataContext);
            //Act
            IEnumerable<ProjectionViewModel> newBudgets = check.NewBudgets();
            if (String.IsNullOrEmpty(newBudgets.ToString()))
            {
                //Assert
                projectionContext.Dispose();
                sapDataContext.Dispose();
                Assert.Inconclusive("Inconcluso: La lista no pudo traer ningun dato");
            }
            else
            {
                //Assert
                projectionContext.Dispose();
                sapDataContext.Dispose();
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
            ProjectionContext projectionContext = new ProjectionContext();
            SapDataContext sapDataContext = new SapDataContext();
            Check check = new Check(projectionContext, sapDataContext);
            //Act
            string monthName = check.GetMonthName(2017, 12);
            //Assert
            Assert.AreEqual("diciembre", monthName);
        }

        [TestMethod]
        [Owner("Laureano")]
        [Priority(5)]
        [Description("Checks if the calculation is correct ")]
        public void CalculateKG()
        {
            //Arrange
            ProjectionContext projectionContext = new ProjectionContext();
            SapDataContext sapDataContext = new SapDataContext();
            Check check = new Check(projectionContext, sapDataContext);
            //Act
            ProjectionViewModel projection = new ProjectionViewModel();
            projection.Ancho_Pliego = 677;
            projection.Largo_Pliego = 677;
            projection.Montaje = 2;
            projection.Pliegos = 1;
            projection.Gramaje = 63;
            double kgTotal = check.CalculateKG(projection, "7800000");
            //Assert
            Assert.AreEqual(kgTotal, 112611.4353);
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
