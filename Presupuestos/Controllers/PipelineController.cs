using System;
using System.Collections.Generic;
using Presupuestos.Models;
using Presupuestos.Ventas;
using Presupuestos.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace Presupuestos.Controllers
{
    public class PipelineController : Controller
    {
        /************************************************************************
         * Detalle
         * Get: When the page is loaded, a list of the actual session is loaded
         ***********************************************************************/
        [HttpGet]
        public ActionResult detalle(string searchKeyword)
        {
            VentaPipe venta = new VentaPipe();
            PipelineViewModel pipeline = new PipelineViewModel();
            try
            {
                if (venta.IsEmpty())
                {
                    List<DetailPipelineVentas> wholeSales = venta.GetWholeList();
                    pipeline.ventas = venta.OrderSalesPipeline(wholeSales);
                    pipeline.ventas = venta.Sort(pipeline.ventas, searchKeyword);
                    pipeline.searchDropDown = venta.fillDropDownSearchDetalle();
                    pipeline.session = pipeline.ventas.Count() == 0 ? (short)0 : (short)pipeline.ventas.Select(o => o.Sesion).Max();
                    pipeline.status = "Viendo " + (pipeline.ventas.Count())
                        + " presupuestos";
                }
            }
            catch (Exception e)
            {
                pipeline.status = "Ocurrió un error en la carga de información, favor contactar al " +
                    "administrador de la página";
            }
            return View(pipeline);
        } // End detalle

        /************************************************************************
         * Detalle
         * Post: Creates a new session which inserts a whole new set of budgets
         ************************************************************************/
        [HttpPost]
        public ActionResult detalle(short session)
        {
            VentaPipe venta = new VentaPipe();
            PipelineViewModel pipeline = new PipelineViewModel();
            try
            {
                pipeline.ventas = venta.GetNewSales();
                pipeline.ventas = venta.Sort(pipeline.ventas, "");
                pipeline.searchDropDown = venta.fillDropDownSearchDetalle();
                //TODO metodo que sortee la lista, enviar como parametro solo la lista pipeline.ventas 
                venta.InsertNewSales(pipeline.ventas, session);
                pipeline.loadStatus.Add("Success",
                    String.Format("Se cargaron {0} nuevos presupuestos de forma correcta",
                    pipeline.ventas.Count()));
            }
            catch (Exception e)
            {
                pipeline.loadStatus.Add("Error", "Ocurrió un error en la carga " +
                    "de nuevos presupuestos, favor volver a intentar");
            }
            return View(pipeline);
        } // End detalle

        /*************************************************************************
         * Analisis Meses
         * Get: Presents an analisis of the data inserted in detalles
         *************************************************************************/
        [HttpGet]
        public ActionResult analisisMeses(PipelineViewModel model)
        {
            VentaPipe venta = new VentaPipe();
            PipelineViewModel pipeline = new PipelineViewModel();
            try
            {
                if (venta.IsEmpty())
                {
                    List<DetailPipelineVentas> wholeSales = venta.GetWholeList();
                    pipeline.ventas = venta.OrderSalesPipeline(wholeSales, model);
                    pipeline.estimated = model.estimated;
                    pipeline.executive = model.executive;
                    pipeline.month = model.month;
                    pipeline.year = model.year;
                    venta.LoadDropDowns(wholeSales, ref pipeline);
                    pipeline.session = pipeline.ventas.Count() == 0 ? (short)0 : (short)pipeline.ventas.Select(o => o.Sesion).Max();
                }
            }
            catch (Exception e)
            {
                pipeline.status = "Ocurrió un error en la carga de información, favor contactar al " +
                    "administrador de la página";
            }
            return View(pipeline);
        } // End meses

        /************************************************************************
         * Resumen Por Ejecutivo
          ************************************************************************/
        [HttpGet]
        public ActionResult resumenPorEjecutivo(DetailViewModel detailView)
        {
            VentaPipe venta = new VentaPipe();
            DetailViewModel executivesDetail = new DetailViewModel();
            try
            {
                //Enviar parametro de carga del stored procedure
                List<DetailPipelineVentas> wholeSales = venta.GetWholeList();
                executivesDetail.estimado = detailView.estimado;
                executivesDetail.executive = detailView.executive;
                executivesDetail.month = detailView.month;
                executivesDetail.year = detailView.year;
                executivesDetail.executiveList = venta.GetExecutiveDetail(detailView);
                executivesDetail.totalDetail = venta.GetTotalRow(executivesDetail.executiveList);
                venta.LoadDetailDropdown(wholeSales, ref executivesDetail);
            }
            catch (Exception e)
            {
                //pipeline.status = "Ocurrió un error en la carga de información, favor contactar al " +
                //    "administrador de la página";
                //throw;
            }
            return View(executivesDetail);
        } // end resumenporejecutivo
    }
}