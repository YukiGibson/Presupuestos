using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Presupuestos.ViewModels;
using Presupuestos.Models;
using Presupuestos.DAL;
using Presupuestos.Services;
using System.Globalization;

namespace Presupuestos.Controllers
{
    public class CommercialController : Controller
    {

        [HttpGet]
        public ActionResult Pipeline(MainViewModel MainView)
        {
            MainViewModel viewModel = new MainViewModel();
            PipelineAbastecimiento checks = new PipelineAbastecimiento();
            viewModel.MessageType = new Dictionary<string, string>();
            try
            {
                viewModel.documentNumber = checks.GetLastId();
                List<ProjectionViewModel> tableData = checks.PipelineLoad(MainView);
                checks.LoadDropDowns(tableData, ref viewModel);
                tableData = checks.SortSalesPipeline(tableData, MainView);
                viewModel.Projections = checks.BuildDetailTable(tableData);
            }
            catch (Exception e)
            {
                viewModel.MessageType.Add("Error", (e.Message ?? e.InnerException.Message));
            }
            finally 
            {
                checks.Dispose();
            }
            return View(viewModel);
        }
	}
}