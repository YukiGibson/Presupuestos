using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presupuestos.Models;
using Presupuestos.Repositories;
using Presupuestos.DAL;
using System.Data.SqlClient;
using Presupuestos.ViewModels;
using System.Data.Entity.SqlServer;

namespace Presupuestos.Ventas
{
    public class VentaPipe : IDisposable
    {
        private PipelineVentasRepository _ventasRepository;

        /*************************************************************************
         * Constructor
         * Initializes a new Repo
         *************************************************************************/
        public VentaPipe()
        {
            _ventasRepository = new PipelineVentasRepository();
        } // End ctor()

        public bool IsEmpty()
        {
            return _ventasRepository.GetContext().DetailPipelineVentas.Select(p => p.Sesion).Any();
        }

        public List<DetailPipelineVentas> GetWholeList()
        {
            return _ventasRepository.Read().ToList();
        }

        /*************************************************************************
         * New Sales
         * Load a new set of budgets from a stored procedure into a list of type DetailPipelineVentas
         *************************************************************************/
        public List<DetailPipelineVentas> GetNewSales()
        {
            SqlParameter fechaActual = new SqlParameter("@fechaActual", DateTime.Today.ToString("yyyyMMdd"));
            List<DetailPipelineVentas> projectionList = _ventasRepository.GetContext().Database
                .SqlQuery<DetailPipelineVentas>("PipelineVentas @fechaActual", fechaActual).ToList();
            //SetSapClients(ref projectionList);
            //TODO cambiar el nombre del vendedor por el de SAP
            /*
              --SELECT [OWOR].[CardCode], [OSLP].[SlpName]
                --FROM [SBO_LITOGRAFIA].[dbo].[OWOR] AS [OWOR]
                --INNER JOIN [SBO_LITOGRAFIA].[dbo].[OCRD] AS [OCRD] 
                --ON [OWOR].[CardCode] = [OCRD].[CardCode]
                --INNER JOIN [SBO_LITOGRAFIA].[dbo].[OSLP] AS [OSLP]
                --ON [OCRD].[SlpCode] = [OSLP].[SlpCode]
                --WHERE U_OrdenProduccionMet = '11200'
             */
            return projectionList;
        } // End GetNewSales()

        private void SetSapClients(ref List<DetailPipelineVentas> list)
        {
            SapDataContext dataContext = new SapDataContext();
            for (int i = 0; i < list.Count(); i++)
            {
                //list[i].Vendedor = dataContext.;
            }
        }

        /*************************************************************************
         * Insert new budgets
         * Inserts the list of each new budget
         * *************************************************************************/
        public void InsertNewSales(List<DetailPipelineVentas> budgets, short session)
        {
            foreach (DetailPipelineVentas budget in budgets)
            {
                budget.Sesion = session + 1;
                _ventasRepository.Create(budget);
            }
            _ventasRepository.Save();
        } // End InsertNewSales

        /*************************************************************************
         * Gets and sort existing budgets
         * Receives the existing budgets and applies sorting and a search on it
         *************************************************************************/
        public List<DetailPipelineVentas> OrderSalesPipeline(List<DetailPipelineVentas> list, params PipelineViewModel[] detail)
        {
            if (detail.Count() != 0)
            {
                if (!String.IsNullOrEmpty(detail[0].executive))
                {
                    list = list.Where(o => o.Vendedor.Contains(detail[0].executive)).ToList();
                }
                if (detail[0].month != 0)
                {
                    list = list.Where(o => o.Mes == detail[0].month).ToList();
                }
                if (!String.IsNullOrEmpty(detail[0].year))
                {
                    list = list.Where(o => o.Año == short.Parse(detail[0].year)).ToList();
                }
                if (!String.IsNullOrEmpty(detail[0].estimated))
                {
                    list = list.Where(o => o.Estimado.Contains(detail[0].estimated)).ToList();
                }
            }
            return list.Count == 0 ? list : list.OrderByDescending(o => o.Cliente).ToList();
        } // End OrderSalesPipeline()

        /*************************************************************************
         * Load Dropdowns
         * Loads data for the meses page dropdowns
         *************************************************************************/
        public void LoadDropDowns(List<DetailPipelineVentas> list, ref PipelineViewModel viewModel)
        {
            List<DetailPipelineVentas> temporalList = list.ToList();
            List<short> annos = temporalList.Select(p => p.Año).Distinct().ToList();
            List<byte> meses = temporalList.Select(p => p.Mes).Distinct().ToList();
            List<string> estimados = temporalList.Select(p => p.Estimado).Distinct().ToList();
            List<string> ejecutivos = temporalList.Select(p => p.Vendedor).Distinct().ToList();

            foreach (var anno in annos)
            {
                viewModel.yearDropDown.Add(anno.ToString(), anno.ToString());
            }
            foreach (var mes in meses)
            {
                viewModel.monthDropDown.Add(mes.ToString(), mes.ToString());
            }
            foreach (var estimado in estimados)
            {
                viewModel.estimatedDropDown.Add(estimado.ToString(), estimado.ToString());
            }
            foreach (var ejecutivo in ejecutivos)
            {
                viewModel.executiveDropDown.Add(ejecutivo.ToString(), ejecutivo.ToString());
            }
        }

        public void Dispose()
        {
            ((IDisposable)_ventasRepository).Dispose();
        } // End Dispopse()
    }
}