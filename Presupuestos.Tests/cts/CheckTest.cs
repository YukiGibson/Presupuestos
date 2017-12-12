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
        [Description("Checks if the calculation is correct")]
        public void CalculateKGBox()
        {
            //Arrange
            ProjectionContext projectionContext = new ProjectionContext();
            SapDataContext sapDataContext = new SapDataContext();
            Check check = new Check(projectionContext, sapDataContext);
            //Act
            ProjectionViewModel projection = new ProjectionViewModel();
            projection.Ancho_Pliego = 445;
            projection.Largo_Pliego = 838;
            projection.Montaje = 1;
            projection.Pliegos = 2;
            projection.Paginas = 2;
            projection.Gramaje = 44;
            double kgTotal = check.CalculateKG(projection, "5000"); // el segundo valor es la proyección
            //Assert
            Assert.AreEqual(kgTotal, 82.0402);
        }

        [TestMethod]
        [Owner("Laureano")]
        [Priority(5)]
        [Description("Checks if the calculation is correct in the case of GV Medical Books")]
        public void CalculateKGBooks()
        {
            //Arrange
            ProjectionContext projectionContext = new ProjectionContext();
            SapDataContext sapDataContext = new SapDataContext();
            Check check = new Check(projectionContext, sapDataContext);
            //Act
            ProjectionViewModel projection = new ProjectionViewModel();
            projection.Ancho_Pliego = 445;
            projection.Largo_Pliego = 838;
            projection.Montaje = 1;
            projection.Pliegos = 16;
            projection.Paginas = 48;
            projection.Gramaje = 44;
            double kgTotal = check.CalculateKG(projection, "5000"); // el segundo valor es la proyección
            //Assert
            Assert.AreEqual(kgTotal, 246.1206);
        }

        [TestMethod]
        [Owner("Laureano")]
        [Priority(5)]
        [Description("Checks if the sheet calculation is correct")]
        public void CalculateSheetsBooks()
        {
            //Arrange
            ProjectionContext projectionContext = new ProjectionContext();
            SapDataContext sapDataContext = new SapDataContext();
            Check check = new Check(projectionContext, sapDataContext);
            //Act
            ProjectionViewModel projection = new ProjectionViewModel();
            projection.Ancho_Pliego = 445;
            projection.Largo_Pliego = 838;
            projection.Montaje = 1;
            projection.Pliegos = 16;
            projection.Paginas = 48;
            projection.Gramaje = 44;
            double sheetTotal = check.CalculateSheet(projection, "5000"); // el segundo valor es la proyección
            //Assert
            Assert.AreEqual(sheetTotal, 15000);
        }

        [TestMethod]
        [Owner("Laureano")]
        [Priority(5)]
        [Description("Checks if the sheet calculation is correct in the case of Medical")]
        public void CalculateSheetsBox()
        {
            //Arrange
            ProjectionContext projectionContext = new ProjectionContext();
            SapDataContext sapDataContext = new SapDataContext();
            Check check = new Check(projectionContext, sapDataContext);
            //Act
            ProjectionViewModel projection = new ProjectionViewModel();
            projection.Ancho_Pliego = 445;
            projection.Largo_Pliego = 838;
            projection.Montaje = 4;
            projection.Pliegos = 2;
            projection.Paginas = 2;
            projection.Gramaje = 44;
            double sheetTotal = check.CalculateSheet(projection, "5000"); // el segundo valor es la proyección
            //Assert
            Assert.AreEqual(sheetTotal, 1250);
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
