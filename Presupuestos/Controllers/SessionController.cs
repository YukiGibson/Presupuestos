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

namespace Presupuestos.Controllers
{
    public class SessionController : Controller
    {
        private ProjectionContext db = new ProjectionContext();
        static MainViewModel viewModel = new MainViewModel();

        /// <summary>
        /// Returns the imports page 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Imports() 
        {
            try
            {
                int? lastDocument = db.DetailPipeline.Select(p => p.IdDoc).Max();
                viewModel.documentNumber = (lastDocument == null ? (ushort)0 : (ushort)lastDocument);
            }
            catch (Exception)
            {
                viewModel.documentNumber = 0;
            }
            
            return View(viewModel);
        }

        /// <summary>
        /// Brings the budgets based on the parameters set
        /// </summary>
        /// <returns>Result from the import</returns>
        [HttpPost]
        public ActionResult Imports(string a)
        {
            MainViewModel viewShow = new MainViewModel();
            viewShow.MessageType = new Dictionary<string, string>();
            Check check = new Check(db);
            viewShow.Projections = new List<ProjectionViewModel>();
            //viewModel.Data
            try
            {
                IEnumerable<ProjectionViewModel> Data = check.NewBudgets(); // LINQ that brings the latest Presupuestos
                check.InsertNewBudgets(Data);
                viewShow.Projections = Data.ToList();
                viewShow.MessageType.Add("Success", "Se cargaron " + Data.Count() + " presupuestos");
                int? lastDocument = db.DetailPipeline.Select(p => p.IdDoc).Max();
                viewShow.documentNumber = (lastDocument == null ? (ushort)0 : (ushort)lastDocument);
            }
            catch (Exception e)
            {
                viewShow.MessageType.Add("Error", (String.IsNullOrEmpty(e.InnerException.Message) 
                    ? e.Message : e.InnerException.Message));
            }
            finally // To finally free resources
            {
                check.Dispose();
            }

            return View(viewShow);
        }

        /// <summary>
        /// Returns the Comparative page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Comparative() 
        {
            return View();
        }
	}
}