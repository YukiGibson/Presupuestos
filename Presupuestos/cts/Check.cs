/*
 このsystemはラウレアノをしました。
 今は２０１７年１１月八日。
 一番お仕事です、そしてプログラミングはすこしむずかしいですから。
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Presupuestos.ViewModels;
using Presupuestos.Models;
using Presupuestos.DAL;
using System.Data.Entity.SqlServer;
using System.Data.Entity;
using PagedList;
using System.Globalization;

namespace Presupuestos.cts
{
    public class Check : IDisposable
    {
        private ProjectionContext _db;

        public Check(ProjectionContext db) 
        {
            _db = db;
        } // End ctor

        /// <summary>
        /// Main method that brings all new budgets
        /// </summary>
        /// <returns> an IEnumerable<> type holding all ProjectionViewModel </returns>
        public IEnumerable<ProjectionViewModel> NewBudgets() 
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

        /// <summary>
        /// Brings a list of the existing budgets in the DataBase
        /// </summary>
        /// <param name="document">integer32 holding the current last document</param>
        /// <returns>IEnumerable type list of ProjectionViewModel containing the current budgets</returns>
        public IEnumerable<ProjectionViewModel> ShowExistingBudgets(int document) 
        {
            string firstName = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            string secondName = DateTime.Now.AddMonths(1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            string thirdName = DateTime.Now.AddMonths(2).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            //TODO se tiene que agregar las fechas para que asi sean mostradas en la vista
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
                                                         new MonthViewModel{ month=0, value="0", year=0, monthName = firstName },
                                                         new MonthViewModel{ month=0, value="0", year=0, monthName = secondName },
                                                         new MonthViewModel{ month=0, value="0", year=0, monthName = thirdName }
                                                     }
                                         }).ToList();
            GetProjections(ref Entregas, document);
            return Entregas;
        } // End showExistingBudgets

        /// <summary>
        /// Takes the MainViewModel view model and applies a search and sorting of the data exposed on the DashBoard view
        /// </summary>
        /// <param name="MainView">MainViewModel brought from the view containing the data</param>
        /// <param name="viewModel">Pass-By-Reference from the static MainViewModel</param>
        public void DashboardLoad(MainViewModel MainView, ref MainViewModel viewModel) 
        {
            int last = _db.DetailPipeline.Count() == 0 ? (ushort)0 : (ushort)_db.DetailPipeline.Select(p => p.IdDoc).Max();
            viewModel.documentNumber = (ushort)last;
            //Si se ocupa traer las proyecciones por meses, entonces se deben enviar por referencia en showExistingBudgets()
            IEnumerable<ProjectionViewModel> list = ShowExistingBudgets(last).ToList();
            
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

        /// <summary>
        /// Void method that connects each projection with the respective budget found in the DataBase 
        /// </summary>
        /// <param name="Entregas">Pass-By-Reference generic list of ProjectionVieModel</param>
        /// <param name="document">The number of the actual document</param>
        private void GetProjections(ref List<ProjectionViewModel> Entregas, int document) 
        {
            DateTime firstMonth = DateTime.Now;
            DateTime secondMonth = DateTime.Now.AddMonths(1);
            DateTime thirdMonth = DateTime.Now.AddMonths(2);
            foreach (var item in Entregas)
            {
                if (_db.DetailPipelineEntregas.Where(p => p.Presupuestos.Equals(item.Presupuesto) && p.IdDoc == document && p.IdLine == item.idLinea).Any())
                {
                    //LINQ query that creates a list of MonthViewModel data based on Presupuesto, IdLinea and betweet the range specified
                    Entregas[Entregas.FindIndex(c => c.Presupuesto == item.Presupuesto && c.idLinea == item.idLinea)].Month =
                        (from s in _db.DetailPipelineEntregas.Where(x => x.Presupuestos.Equals(item.Presupuesto) && x.IdDoc == document
                        && x.IdLine == item.idLinea && (x.Mes == firstMonth.Month || x.Mes == secondMonth.Month || x.Mes == thirdMonth.Month))
                         select new MonthViewModel
                         {
                             id = Math.Round((((double)s.Mes / 12) + (double)s.Año), 6),
                             month = s.Mes == null ? 0 : (int)s.Mes,
                             year = s.Año == null ? 0 : (int)s.Año,
                             value = s.Cantidad == null ? "0" : SqlFunctions.StringConvert((double)s.Cantidad).Trim()
                         }).OrderBy(o => o.id).ToList();
                    //Foreach that saves the name of the month in spanish into the monthName property
                    Entregas[Entregas.FindIndex(c => c.Presupuesto == item.Presupuesto && c.idLinea == item.idLinea)].
                        Month.ForEach(x => x.monthName = GetMonthName(x.year, x.month));
                } // End if
            } // End foreach
        } // End getProjections

        /// <summary>
        /// Takes the year and month of the projection, then it returns the month in spanish
        /// </summary>
        /// <param name="date">A params vector containing the year int type and month int type</param>
        /// <returns>A string type containing a month name in Spanish</returns>
        public string GetMonthName(params int[] date)
        {
            DateTime fecha = new DateTime(date[0], date[1], 15);
            return fecha.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
        } // End GetMonthName

        /// <summary>
        /// Line by line, inserts the new budget with all the properties
        /// </summary>
        /// <param name="row"></param>
        /// <param name="lineNumber"></param>
        /// <param name="lastDocument"></param>
        private void InsertSingleBudget(ProjectionViewModel row, uint lineNumber, ushort lastDocument) 
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
        } // End InsertSingelBudget

        /// <summary>
        /// Inserts a new line of projections with a new document number meanwhile holding the value from past documents/budgets
        /// </summary>
        /// <param name="item">Contains the data of each budgets</param>
        /// <param name="lineNumber">The line number of the main query</param>
        /// <param name="lastDocument">Last document number</param>
        private void CreateNewProjectionLine(ProjectionViewModel item, uint lineNumber, int lastDocument)
        {
            if (_db.DetailPipeline.Where(p => p.IdDoc == lastDocument &&
                           p.Presupuesto == item.Presupuesto && p.Sustrato == item.Sustrato && p.ItemCodeSustrato == item.ItemCodeSustrato).Any())
            {
                //When dealing with budgets holding the same budget number, I checked with LINQ everytime by Sustrato and Paper code
                int idLine = (int)_db.DetailPipeline.Where(p => p.IdDoc == lastDocument &&
                       p.Presupuesto == item.Presupuesto && p.Sustrato == item.Sustrato && p.ItemCodeSustrato == item.ItemCodeSustrato).
                       Select(s => s.IdLinea).FirstOrDefault();

                if (_db.DetailPipelineEntregas.Where(p => p.Presupuestos == item.Presupuesto
                && p.IdDoc == lastDocument && p.IdLine == idLine).Any())
                {
                    List<DetailPipelineEntregas> existingBudgets = _db.DetailPipelineEntregas.Where(p => p.Presupuestos == item.Presupuesto
                    && p.IdDoc == lastDocument && p.IdLine == idLine).ToList();
                    foreach (var Month in existingBudgets)
                    {
                        DetailPipelineEntregas month = new DetailPipelineEntregas();
                        month.IdDoc = lastDocument + 1;
                        month.IdLine = (int?)lineNumber;
                        month.Mes = Month.Mes;
                        month.Año = Month.Año;
                        month.Cantidad = Month.Cantidad;
                        month.CantidadKilos = (decimal)CalculateKG(item, Month.Cantidad.ToString());
                        month.CantidadMedida = (decimal)CalculateSheet(item, Month.CantidadMedida.ToString());
                        month.Presupuestos = item.Presupuesto;
                        _db.Entry(month).State = EntityState.Added;
                    } // End foreach
                } // End if
            }
        } // End CreateNewProjectionLine

        /// <summary>
        /// When inserting a new Projection on Checked budgets:
        /// if it has values
        ///     Writes in a Log called Historico the old data, and updates each projection in DetailPipelineEntregas
        /// if it does exist and is not empty
        ///     Inserts a new projection in PipelineEntregas
        /// </summary>
        /// <param name="Projection"></param>
        /// <param name="Month"></param>
        /// <param name="lastDocument"></param>
        private void InsertUpdateEntregas(ProjectionViewModel Projection, List<MonthViewModel> Month, ushort lastDocument) 
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
                    updateRow.CantidadKilos = (decimal)CalculateKG(Projection, item.value);
                    updateRow.CantidadMedida = (decimal)CalculateSheet(Projection, item.value);
                }
                else if (item.value != "0")
                {
                    DetailPipelineEntregas entregas = new DetailPipelineEntregas();
                    entregas.Año = item.year;
                    entregas.Mes = item.month;
                    entregas.Cantidad = decimal.Parse(item.value);
                    entregas.Presupuestos = Projection.Presupuesto;
                    entregas.CantidadKilos = (decimal)CalculateKG(Projection, item.value);
                    entregas.CantidadMedida = (decimal)CalculateSheet(Projection, item.value);
                    entregas.IdDoc = lastDocument;
                    entregas.IdLine = (int)Projection.idLinea;
                    _db.Entry(entregas).State = EntityState.Added;
                } // End if/else        
            } // End foreach
        } // End insertUpdateEntregas
        
        /// <summary>
        /// Inserts a new document number containing the list of new budgets, assigning projections from last documents 
        /// </summary>
        /// <param name="newBudgets">IEnumerable type that brings all the new budgets</param>
        public void InsertNewBudgets(IEnumerable<ProjectionViewModel> newBudgets) 
        {
            ushort lastDocument = _db.DetailPipeline.Count() == 0 ? (ushort)0 : (ushort)_db.DetailPipeline.Select(p => p.IdDoc).Max();
            uint lineNumber = 0;
            foreach (var item in newBudgets)
            {
                lineNumber++;
                InsertSingleBudget(item, lineNumber, lastDocument);
                CreateNewProjectionLine(item, lineNumber, lastDocument);
            } // End foreach
            HeaderPipeline headerPipeline = new HeaderPipeline();
            headerPipeline.FechaDoc = DateTime.Now;
            headerPipeline.IdDoc = lastDocument + 1;
            headerPipeline.Usuario = "rcast";
            _db.Entry(headerPipeline).State = EntityState.Added;
            _db.SaveChanges();
        } // End InsertNewBudgets

        /// <summary>
        /// Using the values from the budgets that have the Checked Boolean on true, assign those value to each budgets
        /// </summary>
        /// <param name="projection">New projection</param>
        /// <param name="month"></param>
        public void InsertNewProjection(List<ProjectionViewModel> projection, List<MonthViewModel> month) 
        {
            ushort lastDocument = _db.DetailPipeline.Count() == 0 ? (ushort)0 : (ushort)_db.DetailPipeline.Select(p => p.IdDoc).Max();
            foreach (var item in projection.Where(p => p.Checked)) // Where the item is checked
            {
                if (month != null)
                {
                    InsertUpdateEntregas(item, month, lastDocument);
                }
            } // End foreach
            _db.SaveChanges();
        } // End InsertNewProjection

        /// <summary>
        /// Calculates the KG of each month
        /// </summary>
        /// <param name="Projection"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public double CalculateKG(ProjectionViewModel Projection, string Month)
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

        /// <summary>
        /// Calculates the amount of sheets
        /// </summary>
        /// <param name="Projection"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public double CalculateSheet(ProjectionViewModel Projection, string Month) 
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