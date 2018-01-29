using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Presupuestos.ViewModels;
using Presupuestos.DAL;
using Presupuestos.Services;
using Presupuestos.Repositories;
using Presupuestos.Models;
using System.Globalization;

namespace Presupuestos.Controllers
{
    public class SessionController : Controller
    {

        [HttpGet]
        public ActionResult Imports() 
        {
            SessionViewModel viewModel = new SessionViewModel();
            PipelineRepository pipeline = new PipelineRepository();
            PipelineAbastecimiento check = new PipelineAbastecimiento();
            try
            {
                int? lastDocument = check.GetLastId();
                DateTime? lastUpdate = pipeline.Read().Select(p => p.FechaHora).Max();
                viewModel.DocumentNumber = (lastDocument == null ? (ushort)0 : (ushort)lastDocument);
                viewModel.LastUpdate = lastUpdate;
            }
            catch (Exception)
            {
                viewModel.DocumentNumber = 0;
            }
            finally
            {
                pipeline.Dispose();
            }
            
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Imports(SessionViewModel sessionViewModel)
        {
            SessionViewModel viewShow = new SessionViewModel();
            PipelineRepository pipeline = new PipelineRepository();
            PipelineAbastecimiento check = new PipelineAbastecimiento();
            NumeroSesion numeroSesion = new NumeroSesion();
            try
            {
                List<DetailPipeline> Data = check.NewBudgets(sessionViewModel);
                int lastSession = numeroSesion.ObtenerSesionAbastecimiento();
                check.InsertNewBudgets(Data, (ushort)lastSession, sessionViewModel);
                viewShow.Projections = Data.ToList();
                viewShow.MessageType.Add("Success", "Se cargaron " + Data.Count() + " presupuestos");
                viewShow.DocumentNumber = (ushort)lastSession;
            }
            catch (Exception e)
            {
                viewShow.MessageType.Add("Error", (e.Message ?? e.InnerException.Message));
            }
            finally
            {
                check.Dispose();
                pipeline.Dispose();
            }

            return View(viewShow);
        }

        [HttpGet]
        public ActionResult Comparative() 
        {
            return View();
        }
	}
}