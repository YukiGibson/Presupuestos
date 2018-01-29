using System;
using System.Linq;
using Presupuestos.Services;
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
            PipelineAbastecimiento check = new PipelineAbastecimiento();
            SessionViewModel sessionView = new SessionViewModel();
            //Act
            //IEnumerable<ProjectionViewModel> newBudgets = check.NewBudgets(sessionView);
            //if (String.IsNullOrEmpty(newBudgets.ToString()))
            //{
            //    //Assert
            //    Assert.Inconclusive("Inconcluso: La lista no pudo traer ningun dato");
            //}
            //else
            //{
            //    //Assert
            //    Assert.IsNotNull(newBudgets, "La lista falló en traer datos");
            //}
            //check.Dispose();
        }

        [TestMethod]
        [Owner("Laureano")]
        [Priority(2)]
        [Description("Checks month name")]
        public void GetMonthName()
        {
            //Arrange
            PipelineAbastecimiento check = new PipelineAbastecimiento();
            //Act
            string monthName = check.GetMonthName(2017, 12);
            //Assert
            Assert.AreEqual("diciembre", monthName);
            check.Dispose();
        }

    }
}
