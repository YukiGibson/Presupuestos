using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Presupuestos.ViewModels;
using Presupuestos.Models;
using Presupuestos.DAL;
using Presupuestos.Services;
using System.Globalization;
using Presupuestos.Repositories;
using System.Text.RegularExpressions;

namespace Presupuestos.Controllers
{
    public class CommercialController : Controller
    {

        /*************************************************************************
         * Controller - Get:
         * Carga la lista de consulta en la pagina 
         * intranet/Commercial/Pipeline
         *************************************************************************/
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

        /*************************************************************************
         * Controller - Get:
         * Carga los datos necesarios para los gráficos en la página
         * intranet/Commercial/Resumen
         *************************************************************************/
        [HttpGet]
        public ActionResult Resumen(ResumenViewModel resumenView)
        {
            PipelineRepository pipelineRepository = new PipelineRepository();
            ResumenViewModel resumenViewModel = new ResumenViewModel();
            ResumenService resumen = new ResumenService();
            try
            {
                resumenView.mes = resumenView.mes == 0 ? DateTime.Today.AddMonths(1).Month : resumenView.mes;
                resumenView.anno = resumenView.anno == 0 ? DateTime.Today.AddMonths(1).Year : resumenView.anno;
                var kilos = resumen.GetKilosForGrafico(resumenView);
                kilos = resumen.DeletePunctuationKilos(kilos);
                var consumos = resumen.GetConsumosForGrafico();
                consumos = resumen.DeletePunctuationConsumos(consumos);

                if (!String.IsNullOrEmpty(resumenView.familia))
                {
                    consumos = consumos.Where(p => p.familia.Contains(resumenView.familia)).ToList();
                }

                foreach (var item in consumos.Select(o => o.mes).OrderBy(p => p.Value).Distinct())
                {
                    resumenViewModel.mesDropDown.Add(item.ToString(), item.ToString());
                }

                foreach (var item in consumos.Select(p => p.anno).OrderBy(p => p.Value).Distinct())
                {
                    resumenViewModel.annoDropDrown.Add(item.ToString(), item.ToString());
                }

                List<string> familias = pipelineRepository.Read().Select(pipeline => pipeline.Familia).Distinct().ToList();

                foreach (var item in familias)
                {
                    resumenViewModel.familiaDropDown.Add(item, item);
                }

                resumenViewModel.graficoKilos = kilos;
                resumenViewModel.graficoConsumos = consumos;
            }
            catch (Exception e)
            {
                resumenViewModel.requestStatus.Add("Error", "Ocurrió un error en la carga de los datos, favor volver a intentar");
            }
            finally
            {
                pipelineRepository.Dispose();
                resumen.Dispose();
            }

            return View(resumenViewModel);
        }
	}
}