using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Presupuestos.ViewModels;
using Presupuestos.DAL;
using Presupuestos.cts;
using System.Globalization;

namespace Presupuestos.Controllers
{
    public class CommercialController : Controller
    {
        private ProjectionContext _projectionContext = new ProjectionContext();
        private SapDataContext _sapDataContext = new SapDataContext();
        private static MainViewModel viewModel = new MainViewModel();

        /// <summary>
        /// Main controller method that manages the list with each parameter set in MainViewModel
        /// </summary>
        /// <param name="MainView"></param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult Pipeline(MainViewModel MainView)
        {
            Check checks = new Check(_projectionContext, _sapDataContext);
            viewModel.MessageType = new Dictionary<string, string>();
            try
            {
                checks.PipelineLoad(MainView, ref viewModel); // Pass viewModel by reference, as it works in C++
            }
            catch (Exception e)
            {
                viewModel.MessageType.Add("Error", (String.IsNullOrEmpty(e.InnerException.Message) ? e.Message : e.InnerException.Message) );
                viewModel.SortBy = MainView.SortBy;
                viewModel.Sorts = new Dictionary<string, string>
                {
                    {"Antiguo a nuevo", "oldest"},
                    {"Nuevo a antiguo", "newest"}
                };
                viewModel.Projections = new List<ProjectionViewModel>();
            }
            finally 
            {
                DateTime baseDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 15);
                viewModel.orderedCosts = new SortedSet<CostsViewModel>(new SortCostsById());
                for (short i = 0; i < 7; i++)
                {
                    string spanishMonth = baseDate.AddMonths(i).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    viewModel.orderedCosts.Add(new CostsViewModel { id = i, monthName = spanishMonth, monthValue = (byte)baseDate.AddMonths(i).Month, yearValue = (short)baseDate.AddMonths(i).Year });
                }
                checks.Dispose();
            }
            return View(viewModel);
        }

        /// <summary>
        /// IDispose implementation
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _sapDataContext.Dispose();
                _projectionContext.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}