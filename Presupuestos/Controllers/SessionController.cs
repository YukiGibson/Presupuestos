﻿/*
 このsystemはラウレアノをしました。
 今は２０１７年１１月八日。
 一番お仕事です、そしてプログラミングはすこしむずかしいですから。
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Presupuestos.Models;
using Presupuestos.ViewModels;
using Presupuestos.DAL;
using Presupuestos.cts;
using System.Data.Entity.SqlServer;
using System.Globalization;
using PagedList;

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
            return View();
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
                IEnumerable<ProjectionViewModel> Data = check.newBudgets(); // LINQ that brings the latest Presupuestos
                check.InsertNewBudgets(Data);
                viewShow.Projections = Data.ToList();
                viewShow.MessageType.Add("Success", "Se cargaron " + Data.Count() + " presupuestos");
            }
            catch (Exception e)
            {
                viewShow.MessageType.Add("Error", "Ocurrió un fallo en la carga de los presupuestos, favor volver a intentar");
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