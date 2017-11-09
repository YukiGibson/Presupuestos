/*
 このsystemはラウレアノをしました。
 今は２０１７年１１月八日。
 一番お仕事です、そしてプログラミングはすこしむずかしいですから。
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presupuestos.ViewModels;
using Presupuestos.Models;
using Presupuestos.DAL;
using System.Data.Entity.SqlServer;
using System.Data.Entity;
using PagedList;

namespace Presupuestos.cts
{
    public class Check : IDisposable
    {
        private ProjectionContext _db;

        public Check(ProjectionContext db) 
        {
            _db = db;
        } // End ctor

        private bool ifExist(DetailPipeline existingRow, ProjectionViewModel newRow) 
        {
            if (existingRow.Cliente.Equals(newRow.Cliente) && existingRow.Gramaje.Equals(newRow.Gramaje == null ? 0 : newRow.Gramaje) &&
                existingRow.AnchoBobina.Equals(newRow.Ancho_Bobina) && 
                existingRow.AnchoPliego.Equals(newRow.Ancho_Pliego) &&
                existingRow.LargoPliego.Equals(newRow.Largo_Pliego) &&
                    existingRow.Paginas.Equals(newRow.Paginas) &&
                existingRow.Montaje.Equals(newRow.Montaje) &&
                (existingRow.Pliegos.Equals(newRow.Pliegos))
                )
	        {
                return true; // gg
	        } else
            return true; // ggez
        }


        /// <summary>
        /// Main method that brings all new budgets
        /// </summary>
        /// <returns> an IEnumerable<> type holding all ProjectionViewModel </returns>
        public IEnumerable<ProjectionViewModel> newBudgets() 
        {
            return from Presupuestos in _db.A_Vista_Presupuestos
                   join leftReserva in _db.A_Vista_OConversion_Reserva on Presupuestos.Presupuesto equals leftReserva.Presupuesto into Res
                   from Reserva in Res.DefaultIfEmpty()
                   from Process in _db.EstrProcessos.Where(p => p.CodEstrutura == (SqlFunctions.CharIndex("P", Presupuestos.Presupuesto) != 0 ? 
                   Presupuestos.Presupuesto.Substring(0, Presupuestos.Presupuesto.Length - 1) : Presupuestos.Presupuesto)).Take(1).DefaultIfEmpty() // Outer Apply
                   join Orc in _db.OrcHdr on Presupuestos.Presupuesto equals Orc.NumOrcamento
                   where Orc.Fax.Contains("PREV") && Process.PIdClasse != "ACBMT" 
                   select new ProjectionViewModel()
                   {
                       ID = Presupuestos.ID,
                       Ejecutivo = Presupuestos.Vendedor,
                       Cliente = Presupuestos.Cliente,
                       Familia = Presupuestos.TipoProducto,
                       Producto = Presupuestos.Título,
                       Presupuesto = Presupuestos.Presupuesto,
                       ItemCodeSustrato = Reserva.Material,
                       Sustrato = Reserva.Apelido,
                       Gramaje = Process.PGramatura, // This
                       Ancho_Bobina = Process.PFmtFabL, // This
                       Ancho_Pliego = Process.PFmtCadA, // This
                       Largo_Pliego = Process.PFmtFabA, // This
                       Paginas = Process.PQtdPagTot,  //This
                       Montaje = Process.PQtdPagCad, // This
                       Pliegos = Process.QtdRepeticoes, // This
                   };
        } // End newBudgets

        public IEnumerable<ProjectionViewModel> showExistingBudgets(int document) 
        {
            List<ProjectionViewModel> Entregas = (from PipeLine in _db.DetailPipeline.Where(p => p.IdDoc == document)
                                                  join Projection in _db.A_Vista_Presupuestos on PipeLine.Presupuesto equals Projection.Presupuesto into PRJ
                                                  from Presupuesto in PRJ.DefaultIfEmpty()
                                                  select new ProjectionViewModel()
                                                  {
                                                      idLinea = (int)PipeLine.IdLinea,
                                                      ID = PipeLine.ID,
                                                      Ejecutivo = PipeLine.Ejecutivo,
                                                      Cliente = PipeLine.Cliente,
                                                      Familia = Presupuesto.TipoProducto,
                                                      Producto = PipeLine.Producto,
                                                      Presupuesto = PipeLine.Presupuesto,
                                                      Sustrato = PipeLine.Sustrato,
                                                      Gramaje = PipeLine.Gramaje,
                                                      Ancho_Bobina = PipeLine.AnchoBobina,
                                                      Ancho_Pliego = PipeLine.AnchoPliego,
                                                      Largo_Pliego = PipeLine.LargoPliego,
                                                      Paginas = (int)PipeLine.Paginas,
                                                      Montaje = PipeLine.Montaje,
                                                      Pliegos = (int)PipeLine.Pliegos,
                                                      Month = new List<MonthViewModel>
                                                     {
                                                         new MonthViewModel{ month=0, value="0", year=0 },
                                                         new MonthViewModel{ month=0, value="0", year=0 },
                                                         new MonthViewModel{ month=0, value="0", year=0 }
                                                     }
                                         }).ToList();
            getProjections(ref Entregas, document);
            return Entregas;
        } // End showExistingBudgets

        public void dashboardLoad(MainViewModel MainView, ref MainViewModel viewModel) 
        {
            int last = _db.DetailPipeline.Count() == 0 ? (ushort)0 : (ushort)_db.DetailPipeline.Select(p => p.IdDoc).Max();
            viewModel.documentNumber = (ushort)last;
            //Si se ocupa traer las proyecciones por meses, entonces se deben enviar por referencia en showExistingBudgets()
            IEnumerable<ProjectionViewModel> list = showExistingBudgets(last).ToList();
            
            if (!string.IsNullOrEmpty(MainView.SearchBudget))
            {
                list = list.Where(p => p.Presupuesto.ToLower().Contains(MainView.SearchBudget.Trim().ToLower())
                    || p.Cliente.ToLower().Contains(MainView.SearchBudget.Trim().ToLower()));
                viewModel.SearchBudget = MainView.SearchBudget.Trim();
            }
            else
            {
                viewModel.SearchBudget = string.Empty;
            }
            if (!string.IsNullOrEmpty(MainView.SearchExecutive))
            {
                list = list.Where(p => p.Presupuesto.ToLower().Contains(MainView.SearchExecutive.Trim().ToLower()) ||
                    p.Ejecutivo.ToLower().Contains(MainView.SearchExecutive.Trim().ToLower()) || p.Cliente.ToLower().Contains(MainView.SearchExecutive.Trim().ToLower()));
                viewModel.SearchExecutive = MainView.SearchExecutive.Trim();
            }
            else
            {
                viewModel.SearchExecutive = string.Empty;
            }

            switch (MainView.SortBy)
            {
                case "oldest":
                    list = list.OrderBy(p => p.Presupuesto);
                    break;
                case "newest":
                    list = list.OrderByDescending(p => p.Presupuesto);
                    break;
                default:
                    list = list.OrderBy(p => p.Cliente);
                    break;
            }

            const int PageItems = 15;
            int currentPage = (MainView.Page ?? 1);
            viewModel.Projections = list.Skip((currentPage - 1) * PageItems).Take(PageItems).ToList();
            viewModel.PagingMetaData = new StaticPagedList<ProjectionViewModel>(viewModel.Projections, currentPage, PageItems, list.Count()).GetMetaData();
            viewModel.SortBy = MainView.SortBy;
            viewModel.Sorts = new Dictionary<string, string> 
            {
                {"Antiguo a nuevo", "oldest"},
                {"Nuevo a antiguo", "newest"}
            };
        } // End dashboardLoad

        private void getProjections(ref List<ProjectionViewModel> Entregas, int document) 
        {
            byte firstMonth = (byte)DateTime.Now.Month;
            byte secondMonth = (byte)DateTime.Now.AddMonths(1).Month;
            byte thirdMonth = (byte)DateTime.Now.AddMonths(2).Month;
            foreach (var item in Entregas)
            {
                if (_db.DetailPipelineEntregas.Where(p => p.Presupuestos.Equals(item.Presupuesto) && p.IdDoc == document).Any())
                {
                    Entregas[Entregas.FindIndex(c => c.Presupuesto == item.Presupuesto && c.idLinea == item.idLinea)].Month = 
                        (from s in _db.DetailPipelineEntregas.Where(x => x.Presupuestos.Equals(item.Presupuesto) && x.IdDoc == document
                        && x.IdLine == item.idLinea && (x.Mes == firstMonth || x.Mes == secondMonth || x.Mes == thirdMonth))
                         select new MonthViewModel
                         {
                             month = s.Mes == null ? 0 : (int)s.Mes,
                             year = s.Año == null ? 0 : (int)s.Año,
                             value = s.Cantidad == null ? "0" : SqlFunctions.StringConvert((double)s.Cantidad).Trim()
                         }).ToList();
                } // End if
            } // End foreach
        } // End getProjections

        private void insertToPipeLine(ProjectionViewModel row, uint lineNumber, ushort lastDocument) 
        {
            DetailPipeline pipelineModel = new DetailPipeline();
            var ordenDeProduccion = _db.A_Vista_Presupuestos.Where(p => p.Presupuesto == row.Presupuesto).Select(s => s.OP).SingleOrDefault();
            var codigoCliente = _db.Vista_SAP.Where(s => s.U_OrdenProduccionMet == ordenDeProduccion).Select(p => p.CardCode).SingleOrDefault();

            pipelineModel.Ejecutivo = row.Ejecutivo;
            pipelineModel.Cliente = row.Cliente;
            pipelineModel.Producto = row.Producto;
            pipelineModel.Presupuesto = row.Presupuesto;
            pipelineModel.ItemCodeSustrato = row.ItemCodeSustrato;
            pipelineModel.Sustrato = row.Sustrato;
            pipelineModel.Gramaje = row.Gramaje == null ? 0 : (int)row.Gramaje;
            pipelineModel.AnchoBobina = row.Ancho_Bobina == null ? 0 : row.Ancho_Bobina;
            pipelineModel.AnchoPliego = row.Ancho_Pliego == null ? 0 : row.Ancho_Pliego;
            pipelineModel.LargoPliego = row.Largo_Pliego == null ? 0 : row.Largo_Pliego;
            pipelineModel.Paginas = row.Paginas == null ? 0 : row.Paginas;
            pipelineModel.Montaje = row.Montaje == null ? 0 : row.Montaje;
            pipelineModel.Pliegos = row.Pliegos == null ? 0 : row.Pliegos;
            pipelineModel.IdDoc = lastDocument + 1;
            pipelineModel.IdLinea = (int?)lineNumber;
            pipelineModel.FechaHora = DateTime.Now;
            pipelineModel.CardName = codigoCliente;
            _db.Entry(pipelineModel).State = EntityState.Added;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Projection"></param>
        /// <param name="Month"></param>
        /// <param name="lastDocument"></param>
        private void insertUpdateEntregas(ProjectionViewModel Projection, List<MonthViewModel> Month, ushort lastDocument) 
        {
            foreach (var item in Month)
            {
                List<DetailPipelineEntregas> porLinea = _db.DetailPipelineEntregas.Where(x => x.Presupuestos == Projection.Presupuesto
                    && x.Mes == item.month && x.Año == item.year && x.IdDoc == lastDocument && x.IdLine == Projection.idLinea).ToList();

                if (porLinea.Count != 0)
                {
                    DetailPipelineHistorico historico = new DetailPipelineHistorico();
                    historico.IdDoc = lastDocument;
                    historico.IdLine = (int)Projection.idLinea;
                    historico.Ano = item.year;
                    historico.Cantidad = porLinea.Select(p => p.Cantidad).SingleOrDefault();
                    historico.CantidadKilos = porLinea.Select(p => p.CantidadKilos).SingleOrDefault();
                    historico.CantidadMedida = porLinea.Select(p => p.CantidadMedida).SingleOrDefault();
                    historico.Presupuestos = Projection.Presupuesto;
                    historico.Mes = item.month;
                    historico.FechaHora = DateTime.Now;
                    _db.Entry(historico).State = EntityState.Added;
                    var updateRow = _db.DetailPipelineEntregas.Where(p => p.Presupuestos == Projection.Presupuesto
                        && p.Mes == item.month && p.Año == item.year && p.IdDoc == lastDocument && p.IdLine == Projection.idLinea).SingleOrDefault();
                    updateRow.Cantidad = decimal.Parse(item.value);
                    updateRow.CantidadKilos = (decimal)calculateKG(Projection, item.value);
                    updateRow.CantidadMedida = (decimal)calculateSheet(Projection, item.value);
                }
                else if (item.value != "0")
                {
                    DetailPipelineEntregas entregas = new DetailPipelineEntregas();
                    entregas.Año = item.year;
                    entregas.Mes = item.month;
                    entregas.Cantidad = decimal.Parse(item.value);
                    entregas.Presupuestos = Projection.Presupuesto;
                    entregas.CantidadKilos = (decimal)calculateKG(Projection, item.value);
                    entregas.CantidadMedida = (decimal)calculateSheet(Projection, item.value);
                    entregas.IdDoc = lastDocument;
                    entregas.IdLine = (int)Projection.idLinea;
                    _db.Entry(entregas).State = EntityState.Added;
                } // End if/else        
            } // End foreach
        } // End insertUpdateEntregas

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newBudgets">List of new budgets</param>
        public void InsertNewDocument(IEnumerable<ProjectionViewModel> newBudgets) 
        {
            ushort lastDocument = _db.DetailPipeline.Count() == 0 ? (ushort)0 : (ushort)_db.DetailPipeline.Select(p => p.IdDoc).Max();
            uint lineNumber = 0;
            foreach (var item in newBudgets)
            {
                lineNumber++;
                insertToPipeLine(item, lineNumber, lastDocument);
                if (_db.DetailPipelineEntregas.Where(p => p.Presupuestos == item.Presupuesto
                    && p.IdDoc == lastDocument).Any())
                {
                    List<DetailPipelineEntregas> existingBudgets = _db.DetailPipelineEntregas.Where(p => p.Presupuestos == item.Presupuesto
                    && p.IdDoc == lastDocument).ToList();
                    foreach (var Month in existingBudgets)
                    {
                        DetailPipelineEntregas month = new DetailPipelineEntregas();
                        month.IdDoc = lastDocument + 1;
                        month.IdLine = Month.IdLine;
                        month.Mes = Month.Mes;
                        month.Año = Month.Año;
                        month.Cantidad = Month.Cantidad;
                        month.CantidadKilos = (decimal)calculateKG(item, Month.Cantidad.ToString());
                        month.CantidadMedida = (decimal)calculateSheet(item, Month.CantidadMedida.ToString());
                        month.Presupuestos = item.Presupuesto;
                        _db.Entry(month).State = EntityState.Added;
                        //_db.SaveChanges();
                    } // End foreach
                } // End if
            } // End foreach
            HeaderPipeline headerPipeline = new HeaderPipeline();
            headerPipeline.FechaDoc = DateTime.Now;
            headerPipeline.IdDoc = lastDocument + 1;
            headerPipeline.Usuario = "rcast";
            _db.Entry(headerPipeline).State = EntityState.Added;
            _db.SaveChanges(); 
        } // End InserNewDocument

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projection"></param>
        /// <param name="month"></param>
        public void InsertNewProjection(List<ProjectionViewModel> projection, List<MonthViewModel> month) 
        {
            ushort lastDocument = _db.DetailPipeline.Count() == 0 ? (ushort)0 : (ushort)_db.DetailPipeline.Select(p => p.IdDoc).Max();
            foreach (var item in projection.Where(p => p.Checked)) // Where the item is checked
            {
                if (month != null)
                {
                    insertUpdateEntregas(item, month, lastDocument);
                }
            } // End foreach
            _db.SaveChanges();
        } // End InsertNewProjection

        private double calculateKG(ProjectionViewModel Projection, string Month)
        {
            double anchoPliego = Projection.Ancho_Pliego == null ? 0 : (double)Projection.Ancho_Pliego;
            double largoPliego = Projection.Largo_Pliego == null ? 0 : (double)Projection.Largo_Pliego;
            double month = Month == null ? 0 : Double.Parse(Month);
            double montaje = Projection.Montaje == null ? 0 : (double)Projection.Montaje;
            double pliegos = Projection.Pliegos == null ? 0 : (double)Projection.Pliegos;
            double gramaje = Projection.Gramaje == null ? 0 : (double)Projection.Gramaje;
            if (Projection.Paginas != 0)
            {
                double result = (((anchoPliego * largoPliego) / 1000000) *
                    (month * pliegos)) * (gramaje / 1000);
                return Double.IsInfinity(result) || Double.IsNaN(result) ? 0 : Math.Round(result, 4);
            }
            else
            {
                double result = (((anchoPliego * largoPliego) / 1000000) *
                    (month / montaje)) * (gramaje / 1000);
                return Double.IsInfinity(result) || Double.IsNaN(result) ? 0 : Math.Round(result, 4);
            }
            /* Fórmulas para el cálculo de los KG y la cantidad de pliegos
             if (dcPaginas != "0") // Es la cantidad de paginas, se refiere al campo Quantidade
                {
                       Kilos = (((double.Parse(intAnchoPliego) * double.Parse(intLargoPliego)) / 1000000) * 
             * (double.Parse(intCantidad) * double.Parse(dcPliegos))) * (double.Parse(intGramaje) / 1000);
             * 
                       Pliegos = double.Parse(dcPliegos) * double.Parse(intCantidad);
                               }
                               else
                               {
                       Kilos = (((double.Parse(intAnchoPliego) * double.Parse(intLargoPliego)) / 1000000) * 
             * (double.Parse(intCantidad) / double.Parse(strMontaje))) * (double.Parse(intGramaje) / 1000);
             * 
                       Pliegos = (1 / double.Parse(strMontaje)) * double.Parse(intCantidad);
                  }                                
             */
        
        } // End calculateKG

        private double calculateSheet(ProjectionViewModel Projection, string Month) 
        {
            double month = Month == null ? 0 : Double.Parse(Month);
            double montaje = Projection.Montaje == null ? 0 : (double)Projection.Montaje;
            double pliegos = Projection.Pliegos == null ? 0 : (double)Projection.Pliegos;
            if (Projection.Paginas != 0)
            {
                double result = pliegos * month;
                return Double.IsInfinity(result) || Double.IsNaN(result) ? 0 : Math.Round(result, 4);
            }
            else
            {
                double result = (1 / montaje) * month;
                return Double.IsInfinity(result) || Double.IsNaN(result) ? 0 : Math.Round(result, 4);
            }
        } // End calculateSheet

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}