using System;
using System.Collections.Generic;
using Presupuestos.Models;
using Presupuestos.Ventas;
using Presupuestos.ViewModels;
using System.Linq;
using System.Web;
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
        public ActionResult detalle()
        {
            VentaPipe venta = new VentaPipe();
            PipelineViewModel pipeline = new PipelineViewModel();
            try
            {
                if (venta.IsEmpty())
                {
                    List<DetailPipelineVentas> wholeSales = venta.GetWholeList();
                    pipeline.ventas = venta.OrderSalesPipeline(wholeSales);
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
         * Meses
         * Get: 
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
        public ActionResult resumenPorEjecutivo()
        {
            return View();
        } // end resumenporejecutivo
    }
}