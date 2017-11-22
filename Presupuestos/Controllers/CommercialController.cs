/*
 このsystemはラウレアノをしました。
 今は２０１７年１１月八日。
 一番お仕事です、そしてプログラミングはすこしむずかしいですから。
 */
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
        private ProjectionContext db = new ProjectionContext();
        private static MainViewModel viewModel = new MainViewModel();
        
        /// <summary>
        /// Main controller method that manages the list with each parameter set in MainViewModel
        /// </summary>
        /// <param name="MainView"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Dashboard(MainViewModel MainView)
        {
            Check checks = new Check(db);
            viewModel.MessageType = new Dictionary<string, string>();
            try
            {
                checks.DashboardLoad(MainView, ref viewModel); // Pass viewModel by reference, as it works in C++
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
        /// Controller method in charge of setting each Projection value to each chosen line
        /// </summary>
        /// <param name="MainView"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Dashboard(MainViewModel MainView, string Vacio) 
        {
            Check check = new Check(db);
            viewModel.MessageType = new Dictionary<string, string>();
            DashBoardMessage newMessage = new DashBoardMessage(viewModel.MessageType, MainView.month, MainView.Projections);
            try
            {
                for (int i = 0; i < MainView.month.Count(); i++)
                {
                    string monthProjectionValue = MainView.month[i].value.Replace(",", string.Empty);
                    monthProjectionValue = monthProjectionValue.Replace('.', ',');
                    MainView.month[i].value = monthProjectionValue;
                } // End for
                check.InsertNewProjection(MainView.Projections, MainView.month); 
                viewModel.MessageType = newMessage.BuildMessage(true); // The messagge is built here
                check.DashboardLoad(MainView, ref viewModel); // In order to mantain each search and orderby, run this method
            }
            catch (Exception)
            {
                viewModel.MessageType = newMessage.BuildMessage(false);
            }
            finally 
            {
                check.Dispose();
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}