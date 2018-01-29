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

namespace Presupuestos.Services
{
    public class PipelineVentas : IDisposable
    {
        private PipelineVentasRepository _ventasRepository;


        /*************************************************************************
         * Constructor
         * Initializes a new Repo
         *************************************************************************/
        public PipelineVentas()
        {
            _ventasRepository = new PipelineVentasRepository();
        } // End ctor()

        /// <summary>
        /// Checks if there is any session
        /// </summary>
        /// <returns></returns>
        public bool HasContent()
        {
            return _ventasRepository.GetContext().DetailPipelineVentas.Select(p => p.Sesion).Any();
        }

        /// <summary>
        /// Gets the whole list from the repository
        /// </summary>
        /// <returns></returns>
        public List<DetailPipelineVentas> GetWholeList()
        {
            return _ventasRepository.Read().ToList();
        }

        public int GetLastSession()
        {
            return _ventasRepository.GetContext().DetailPipelineVentas.Select(p => p.Sesion).Max();
        }

        public void SaveColor(int? id, string color)
        {
            DetailPipelineVentas detail = new DetailPipelineVentas();
            detail.ID = id ?? 0;
            _ventasRepository.UpdateColor(detail, color);
            _ventasRepository.Save();
        }

        public Dictionary<string, string> fillDropDownSearchDetalle()
        {
            Dictionary<string, string> temporalList = new Dictionary<string, string>()
            {
                {"Ejecutivo", "Vendedor" },
                {"Mes","Mes"},
                {"Año","Anno"},
                {"Meses Diferencia","MesesDiferencia"},
                {"Presupuesto","Presupuesto"},
                {"Orden de Produccion","OP"},
                {"Producto","TipoProducto"},
                {"Total","CantidadTotal"},
                {"Cantidad","Cantidad"},
                {"Titulo","Titulo"},
                {"Cliente","Cliente"},
                {"Por Facturar","PorFacturar"},
                {"Costo","Costo"},
                {"Rentabilidad","Rentabilidad"},
                {"Prob Venta","ProbabilidadVenta"},
                {"Estimado","Estimado"},
            };
            return temporalList;
        }
        //List<PipelineViewModel> list, string searchWord

        public List<DetailPipelineVentas> Sort(List<DetailPipelineVentas> list, string searchWord)
        {
            switch (searchWord)
            {
                case "Ejecutivo":
                    list = list.OrderBy(o => o.Vendedor).ToList();
                    break;
                case "Mes":
                    list = list.OrderByDescending(o => o.Mes).ToList();
                    break;
                case "Año":
                    list = list.OrderByDescending(o => o.Año).ToList();
                    break;
                case "Meses Diferencia":
                    list = list.OrderByDescending(o => o.MesesDiferencia).ToList();
                    break;
                case "Presupuesto":
                    list = list.OrderByDescending(o => o.Presupuesto).ToList();
                    break;
                case "OP":
                    list = list.OrderByDescending(o => o.OP).ToList();
                    break;
                case "Producto":
                    list = list.OrderBy(o => o.TipoProducto).ToList();
                    break;
                case "Total":
                    list = list.OrderByDescending(o => o.CantidadTotal).ToList();
                    break;
                case "Cantidad":
                    list = list.OrderByDescending(o => o.Cantidad).ToList();
                    break;
                case "Titulo":
                    list = list.OrderBy(o => o.Titulo).ToList();
                    break;
                case "Cliente":
                    list = list.OrderBy(o => o.Cliente).ToList();
                    break;
                case "Por Facturar":
                    list = list.OrderByDescending(o => o.PorFacturar).ToList();
                    break;
                case "Costo":
                    list = list.OrderByDescending(o => o.Costo).ToList();
                    break;
                case "Rentabilidad":
                    list = list.OrderByDescending(o => o.Rentabilidad).ToList();
                    break;
                case "Prob Venta":
                    list = list.OrderByDescending(o => o.ProbabilidadVenta).ToList();
                    break;
                case "Estimado":
                    list = list.OrderByDescending(o => o.Estimado).ToList();
                    break;
                default:
                    break;
            }
            return list;
        }

        public DetalleEjecutivo GetTotalRow(List<DetalleEjecutivo> list)
        {
            DetalleEjecutivo detalleEjecutivo = new DetalleEjecutivo()
            {
                FacturacionEstimado = list.Sum(o => o.FacturacionEstimado),
                FacturacionOP = list.Sum(o => o.FacturacionOP),
                MargenEstimado = list.Sum(o => o.MargenEstimado),
                MargenOP = list.Sum(o => o.MargenOP),
                TotalFacturacion = list.Sum(o => o.TotalFacturacion),
                TotalMargen = list.Sum(o => o.TotalMargen),
                MargenProyectado = list.Sum(o => o.MargenProyectado),
            };
            return detalleEjecutivo;
        }

        /// <summary>
        /// Load a new set of budgets from a stored procedure into a list of type DetailPipelineVentas
        /// </summary>
        /// <returns></returns>
        public List<DetailPipelineVentas> GetNewSales()
        {
            List<DetailPipelineVentas> projectionList = _ventasRepository.GetContext().Database
                .SqlQuery<DetailPipelineVentas>("PCV_PipelineVentas").ToList();
            SetSapClients(ref projectionList);

            return projectionList;
        } // End GetNewSales()

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DetalleEjecutivo> GetExecutiveDetail(DetailViewModel detail)
        {
            SqlParameter estimado = new SqlParameter("@Estimado", detail.estimado ?? "All");
            SqlParameter month = new SqlParameter("@Month", detail.month ?? DateTime.Today.Month.ToString());
            SqlParameter year = new SqlParameter("@Year", detail.year ?? DateTime.Today.Year.ToString());
            List<DetalleEjecutivo> executivesOverview = _ventasRepository.GetContext().Database
                .SqlQuery<DetalleEjecutivo>("PCV_DetalleEjecutivo @Estimado, @Month, @Year", estimado, month, year).ToList();
            if (detail.executive != null)
            {
                executivesOverview = executivesOverview.Where(o => o.Vendedor.Contains(detail.executive)).ToList();
            }
            return executivesOverview;
        }

        private void SetSapClients(ref List<DetailPipelineVentas> list)
        {
            SapDataContext dataContext = new SapDataContext();
            for (int i = 0; i < list.Count(); i++)
            {
                string op = list[i].OP;
                if (!String.IsNullOrEmpty(op))
                {
                    string sapName = (from OWOR in dataContext.OWOR
                                      join OCRD in dataContext.OCRD on OWOR.CardCode equals OCRD.CardCode
                                      join OSLP in dataContext.OSLPs on OCRD.SlpCode equals OSLP.SlpCode
                                      where OWOR.U_OrdenProduccionMet.Contains(op)
                                      select OSLP.SlpName).FirstOrDefault();
                    list[i].Vendedor = sapName ?? list[i].Vendedor;
                }
            }
            dataContext.Dispose();
        }

        /// <summary>
        /// Inserts the list of each new budget
        /// </summary>
        /// <param name="budgets"></param>
        /// <param name="session"></param>
        public void InsertNewSales(List<DetailPipelineVentas> budgets, short session)
        {
            foreach (DetailPipelineVentas budget in budgets)
            {
                budget.Sesion = session;
                if (_ventasRepository.IfExists(budget, session))
                {
                    _ventasRepository.Update(budget, session.ToString());
                }
                else // If it doesn't exists
                {
                    _ventasRepository.Create(budget);
                }
            }
            _ventasRepository.Save();
        } // End InsertNewSales

        /// <summary>
        /// Receives the existing budgets and applies sorting and a search on it
        /// </summary>
        /// <param name="list"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        public List<DetailPipelineVentas> SortSalesPipeline(List<DetailPipelineVentas> list, params PipelineViewModel[] detail)
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

        /// <summary>
        /// Loads data for the meses page dropdowns
        /// </summary>
        /// <param name="list"></param>
        /// <param name="viewModel"></param>
        public void LoadDropDowns(List<DetailPipelineVentas> list, ref PipelineViewModel viewModel)
        {
            List<DetailPipelineVentas> temporalList = list.ToList();
            List<short> annos = temporalList.Select(p => p.Año).Distinct().ToList();
            List<byte> meses = temporalList.OrderBy(o => o.Mes).Select(p => p.Mes).Distinct().ToList();
            List<string> estimados = temporalList.Select(p => p.Estimado).Distinct().ToList();
            List<string> ejecutivos = temporalList.OrderBy(o => o.Vendedor).Select(p => p.Vendedor).Distinct().ToList();

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

        public void LoadDetailDropdown(List<DetailPipelineVentas> list, ref DetailViewModel viewModel)
        {
            List<DetailPipelineVentas> temporalList = list.ToList();
            List<short> annos = temporalList.Select(p => p.Año).Distinct().ToList();
            List<byte> meses = temporalList.OrderBy(o => o.Mes).Select(p => p.Mes).Distinct().ToList();
            List<string> estimados = temporalList.Select(p => p.Estimado).Distinct().ToList();
            List<string> ejecutivos = temporalList.OrderBy(o => o.Vendedor).Select(p => p.Vendedor).Distinct().ToList();

            foreach (var anno in annos)
            {
                viewModel.yearDrop.Add(anno.ToString(), anno.ToString());
            }
            foreach (var mes in meses)
            {
                viewModel.monthDrop.Add(mes.ToString(), mes.ToString());
            }
            foreach (var estimado in estimados)
            {
                viewModel.estimadosDrop.Add(estimado.ToString(), estimado.ToString());
            }
            foreach (var ejecutivo in ejecutivos)
            {
                viewModel.executiveDrop.Add(ejecutivo.ToString(), ejecutivo.ToString());
            }
        }

        public DetailPipelineVentas DetailTotal(List<DetailPipelineVentas> list)
        {
            //TODO prueba unitaria
            decimal cantidad = list.Sum(o => o.Cantidad);
            decimal facturar = list.Sum(o => o.PorFacturar);
            decimal rentabilidad = list.Sum(o => o.Rentabilidad);
            double porcentaje = (double)Decimal.Round(rentabilidad
                / (facturar == 0 ? 1 : facturar) * 100, 2);
            DetailPipelineVentas totals = new DetailPipelineVentas()
            {
                Cantidad = cantidad,
                PorFacturar = facturar,
                Rentabilidad = rentabilidad,
                Porcentaje = porcentaje
            };
            return totals;
        }

        public List<Detail> BuildDetailTable(List<DetailPipelineVentas> list)
        {
            List<Detail> table = new List<Detail>();
            foreach (var client in list.Select(p => p.Cliente).Distinct())
            {
                Detail detail = new Detail();
                detail.client = client;
                
                foreach (var row in list.Where(p => p.Cliente.Equals(client)))
                {
                    detail.sales.Add(
                        new DetailPipelineVentas
                        {
                            ID = row.ID,
                            Cliente = row.Cliente,
                            Titulo = row.Titulo,
                            Presupuesto = row.Presupuesto,
                            Color = row.Color,
                            OP = row.OP,
                            ProbabilidadVenta = row.ProbabilidadVenta,
                            Cantidad = row.Cantidad,
                            PorFacturar = row.PorFacturar,
                            Rentabilidad = row.Rentabilidad,
                            Porcentaje = row.Porcentaje
                        }
                        );
                }
                detail.rowspan = detail.sales.Count() + 1;
                decimal cantidad = detail.sales.Where(o => o.Cliente.Contains(client)).Sum(o => o.Cantidad);
                decimal porFacturar = detail.sales.Where(o => o.Cliente.Contains(client)).Sum(o => o.PorFacturar);
                decimal rentabilidad = detail.sales.Where(o => o.Cliente.Contains(client)).Sum(o => o.Rentabilidad);
                double porcentaje = (double)Decimal.Round(rentabilidad / (porFacturar == 0 ? 1 : porFacturar) * 100, 2);
                detail.totals = new DetailPipelineVentas()
                {
                    Cantidad = cantidad,
                    PorFacturar = porFacturar,
                    Rentabilidad = rentabilidad,
                    Porcentaje = porcentaje
                };
                table.Add(detail);
            }
            return table;
        }


        /// <summary>
        /// Implements an IDisposable interface where it disposes resources such as the data context
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)_ventasRepository).Dispose();
        } // End Dispopse()
    }
}