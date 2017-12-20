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
using Presupuestos.Repositories;

namespace Presupuestos.Controllers
{
    public class SessionController : Controller
    {

        [HttpGet]
        public ActionResult Imports() 
        {
            SessionViewModel viewModel = new SessionViewModel();
            PipelineRepository pipeline = new PipelineRepository();
            try
            {
                int? lastDocument = pipeline.Read().Select(p => p.IdDoc).Max();
                viewModel.DocumentNumber = (lastDocument == null ? (ushort)0 : (ushort)lastDocument);
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
            viewShow.MessageType = new Dictionary<string, string>();
            SapDataContext _sapDataContext = new SapDataContext();
            
            viewShow.Projections = new List<ProjectionViewModel>();
            PipelineRepository pipeline = new PipelineRepository();
            Check check = new Check(pipeline.GetProjectionContext, _sapDataContext);
            try
            {
                IEnumerable<ProjectionViewModel> Data = check.NewBudgets(sessionViewModel);
                ushort? lastDocument = pipeline.Read().Select(p => p.IdDoc).Max() == null ? (ushort)0 :
                (ushort)pipeline.Read().Select(p => p.IdDoc).Max();
                check.InsertNewBudgets(Data, (ushort)lastDocument);
                viewShow.Projections = Data.ToList();
                viewShow.MessageType.Add("Success", "Se cargaron " + Data.Count() + " presupuestos");
                viewShow.DocumentNumber = (ushort)lastDocument;
            }
            catch (Exception e)
            {
                viewShow.MessageType.Add("Error", (!String.IsNullOrEmpty(e.InnerException.Message) 
                    ? e.Message : e.InnerException.Message));
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