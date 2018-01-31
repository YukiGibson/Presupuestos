using System;
using System.Collections.Generic;
using Presupuestos.Models;
using Presupuestos.Services;
using Presupuestos.ViewModels;
using Presupuestos.Repositories;
using System.Linq;
using System.Web.Mvc;

namespace Presupuestos.Controllers
{
    public class PipelineController : Controller
    {
        /*************************************************************************
         * Controller - Get:
         * Muestra la lista principal que alimenta el pipeline de ventas
         * intranet/Pipeline/detalle
         *************************************************************************/
        [HttpGet]
        public ActionResult detalle(string searchKeyword)
        {
            PipelineVentas venta = new PipelineVentas();
            PipelineViewModel pipeline = new PipelineViewModel();
            try
            {
                if (venta.HasContent())
                {
                    List<DetailPipelineVentas> wholeSales = venta.GetWholeList();
                    pipeline.ventas = venta.SortSalesPipeline(wholeSales);
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
            finally
            {
                venta.Dispose();
            }
            return View(pipeline);
        } // End detalle

        /*************************************************************************
         * Controller - Post:
         * Hace la carga de un nuevo pipeline de ventas, crea una nueva sesion cada
         * lunes de la semana siguiente
         * intranet/Pipeline/detalle?session=#
         *************************************************************************/
        [HttpPost]
        public ActionResult detalle(short session)
        {
            PipelineVentas venta = new PipelineVentas();
            PipelineViewModel pipeline = new PipelineViewModel();
            NumeroSesion numeroSesion = new NumeroSesion();
            try
            {
                session = (short)numeroSesion.ObtenerSesionVentas();
                pipeline.ventas = venta.GetNewSales();
                pipeline.ventas = venta.Sort(pipeline.ventas, "");
                pipeline.searchDropDown = venta.fillDropDownSearchDetalle();
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
            finally
            {
                venta.Dispose();
            }
            return View(pipeline);
        } // End detalle

        /*************************************************************************
         * Controller - Get:
         * Se encarga de mostrar la tabla de datos para el analisis de meses
         * intranet/Pipeline/analisisMeses
         *************************************************************************/
        [HttpGet]
        public ActionResult analisisMeses(PipelineViewModel model)
        {
            PipelineVentas venta = new PipelineVentas();
            PipelineViewModel pipeline = new PipelineViewModel();
            try
            {
                if (venta.HasContent())
                {
                    List<DetailPipelineVentas> wholeSales = venta.GetWholeList();
                    pipeline.ventas = venta.SortSalesPipeline(wholeSales, model);
                    pipeline.venta = venta.DetailTotal(pipeline.ventas);
                    pipeline.estimated = model.estimated;
                    pipeline.executive = model.executive;
                    pipeline.month = model.month;
                    pipeline.year = model.year;
                    pipeline.detailList = venta.BuildDetailTable(pipeline.ventas);
                    venta.LoadDropDowns(wholeSales, ref pipeline);
                    pipeline.session = (short)venta.GetLastSession();
                    pipeline.status = String.Format("Mostrando {0} presupuestos", pipeline.ventas.Count());
                }
            }
            catch (Exception e)
            {
                pipeline.status = "Ocurrió un error en la carga de información, favor contactar al " +
                    "administrador de la página";
            } finally
            {
                venta.Dispose();
            }
            return View(pipeline);
        } // End meses

        /*************************************************************************
         * Controller - Post:
         * Guarda el color de una fila asociado a la orden de produccion
         * intranet/Pipeline/analisisMeses
         *************************************************************************/
        [HttpPost]
        public ActionResult analisisMeses(PipelineViewModel pipelineView, string estimated, string month, 
            string year, string colors, string executive)
        {
            PipelineVentas venta = new PipelineVentas();
            PipelineViewModel pipeline = new PipelineViewModel();
            try
            {
                if (venta.HasContent())
                {
                    pipeline.estimated = estimated;
                    pipeline.executive = executive;
                    pipeline.month = month == null ? (byte)0 : Byte.Parse(month);
                    pipeline.year = year;
                    venta.SaveColor(pipelineView.id, colors);
                    List<DetailPipelineVentas> wholeSales = venta.GetWholeList();
                    pipeline.ventas = venta.SortSalesPipeline(wholeSales, pipeline);
                    pipeline.detailList = venta.BuildDetailTable(pipeline.ventas);
                    venta.LoadDropDowns(wholeSales, ref pipeline);
                    pipeline.session = (short)venta.GetLastSession();
                    pipeline.status = String.Format("Mostrando {0} presupuestos", pipeline.ventas.Count());
                }
            }
            catch (Exception e)
            {
                pipeline.status = "Ocurrió un error en la carga de información, favor contactar al " +
                    "administrador de la página";
            }
            finally
            {
                venta.Dispose();
            }
            return View(pipeline);
        }


        /*************************************************************************
         * Controller - Get:
         * Obtiene los datos para el resumen por ejecutivo además del gráfico
         * intranet/Pipeline/resumenPorEjecutivo
         *************************************************************************/
        [HttpGet]
        public ActionResult resumenPorEjecutivo(DetailViewModel detailView)
        {
            PipelineVentas venta = new PipelineVentas();
            DetailViewModel executivesDetail = new DetailViewModel();
            try
            {
                List<DetailPipelineVentas> wholeSales = venta.GetWholeList();
                executivesDetail.estimado = detailView.estimado;
                executivesDetail.executive = detailView.executive;
                executivesDetail.month = detailView.month ?? DateTime.Today.Month.ToString();
                executivesDetail.year = detailView.year ?? DateTime.Today.Year.ToString();
                executivesDetail.executiveList = venta.GetExecutiveDetail(detailView);
                executivesDetail.totalDetail = venta.GetTotalRow(executivesDetail.executiveList);
                venta.LoadDetailDropdown(wholeSales, ref executivesDetail);
                executivesDetail.session = (short)venta.GetLastSession();
            }
            catch (Exception e)
            {
                //pipeline.status = "Ocurrió un error en la carga de información, favor contactar al " +
                //    "administrador de la página";
                //throw;
            }
            finally
            {
                venta.Dispose();
            }
            return View(executivesDetail);
        } // end resumenporejecutivo

        /*************************************************************************
         * Controller - Post:
         * Guarda la proyeccion con el valor agregado por ejecutivo
         * intranet/Pipeline/resumenPorEjecutivo
         *************************************************************************/
        [HttpPost]
        public ActionResult resumenPorEjecutivo(DetailViewModel detailPipeline, string projectionValue, string executive)
        {
            ProjectionsRepository repository = new ProjectionsRepository();
            PipelineVentas venta = new PipelineVentas();
            DetailViewModel executivesDetail = new DetailViewModel();
            try
            {
                decimal assignable = 0;
                if (Decimal.TryParse(projectionValue, out assignable))
                {
                    DetailPipelineProyeccione detail = new DetailPipelineProyeccione()
                    {
                        Proyeccion = Decimal.Parse(projectionValue),
                        Mes = Byte.Parse(detailPipeline.month),
                        Año = short.Parse(detailPipeline.year),
                        Vendedor = executive
                    };
                    if (repository.ifExists(detail))
                    {
                        repository.Update(detail, projectionValue);
                    }
                    else
                    {
                        repository.Create(detail);
                    }
                    executivesDetail.loadStatus.Add("Success", 
                        String.Format("{0} asignado con exito al ejecutivo {1}", projectionValue, executive));
                }
                else
                {
                    executivesDetail.loadStatus.Add("Alert", "El valor de la proyección no es numérico.");
                }
                repository.Save();
                List<DetailPipelineVentas> wholeSales = venta.GetWholeList();
                detailPipeline.estimado = detailPipeline.executive = null;
                executivesDetail.month = detailPipeline.month ?? DateTime.Today.Month.ToString();
                executivesDetail.year = detailPipeline.year ?? DateTime.Today.Year.ToString();
                executivesDetail.executiveList = venta.GetExecutiveDetail(detailPipeline);
                executivesDetail.totalDetail = venta.GetTotalRow(executivesDetail.executiveList);
                venta.LoadDetailDropdown(wholeSales, ref executivesDetail);
                executivesDetail.session = (short)venta.GetLastSession();
            }
            catch (Exception s)
            {
                executivesDetail.loadStatus.Add("Error", "Ocurrió un error de sistema, favor volver a intentar.");
            }
            finally
            {
                repository.Dispose();
            }
            return View(executivesDetail);
        }
    }
}