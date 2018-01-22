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
using System.Data.SqlClient;

namespace Presupuestos.cts
{
    public class Check : IDisposable
    {
        private ProjectionContext _projectionContext;
        private SapDataContext _sapDataContext;

        public Check(ProjectionContext projectionContext, SapDataContext sapDataContext) 
        {
            _projectionContext = projectionContext;
            _sapDataContext = sapDataContext;
        } // End ctor

        /// <summary>
        /// Main method that brings all new budgets
        /// </summary>
        /// <returns> an IEnumerable<> type holding all ProjectionViewModel </returns>
        public IEnumerable<ProjectionViewModel> NewBudgets(SessionViewModel sessionView)
        {
            //_projectionContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s); // Log in output in order to see the generated sql query
            SqlParameter StartDate = new SqlParameter("@StartDate", sessionView.StartDate.ToString("yyyyMMdd"));
            List<ProjectionViewModel> projectionList = _projectionContext.Database
                .SqlQuery<ProjectionViewModel>("PCV_LoadBudgets @StartDate", StartDate).ToList();
            LoadDimensionsFromSAP(ref projectionList);
            return projectionList;
        } // End newBudgets

        private void LoadDimensionsFromSAP(ref List<ProjectionViewModel> projectionList)
        {
            for (int i = 0; i < projectionList.Count(); i++)
            {
                long test = 0;
                string sustratoCode = projectionList[i].ItemCodeSustrato;
                // Tries to parse the value to integer, if it is a text, then false
                if (Int64.TryParse(sustratoCode, out test))
                {
                    var sap = _sapDataContext.OITM.Where(p => p.ItemCode
                    == sustratoCode).FirstOrDefault();
                    projectionList[i].Sustrato = sap.ItemName;
                    // Esta preguntando si es Bobina, si lo es entonces guardar el ancho de bobina y pliego
                    if (sustratoCode.Substring(10)
                        .Equals("0000"))
                    {
                        projectionList[i].Gramaje = (double?)sap.U_Gramaje;
                        projectionList[i].Ancho_Bobina = projectionList[i].Ancho_Pliego
                            = sap.U_Ancho_Plg;
                    }
                    else // Si es pliego, entonces:
                    {
                        //Agregamos 4 ceros al codigo existente para poder buscar la bobina a la que pertenece
                        string bobinaCode = projectionList[i].ItemCodeSustrato
                            .Substring(0, sustratoCode.Length - 4) + "0000";
                        projectionList[i].Ancho_Bobina = _sapDataContext.OITM
                            .Where(p => p.ItemCode == bobinaCode)
                            .Select(o => o.U_Ancho_Plg).FirstOrDefault();
                        projectionList[i].Gramaje = (double?)sap.U_Gramaje;
                        projectionList[i].Ancho_Pliego = sap.U_Ancho_Plg;
                        projectionList[i].Largo_Pliego = sap.U_Largo_Plg;
                    }
                }
            }
        }

        /// <summary>
        /// Brings a list of the existing budgets in the DataBase
        /// </summary>
        /// <param name="document">integer32 holding the current last document</param>
        /// <returns>IEnumerable type list of ProjectionViewModel containing the current budgets</returns>
        public IEnumerable<ProjectionViewModel> ShowExistingBudgets(int document) 
        {
            List <ProjectionViewModel> Entregas = (from PipeLine in _projectionContext.DetailPipelinePruebas.Where(p => p.IdDoc == document)
                                                  join Projection in _projectionContext.A_Vista_Presupuestos on PipeLine.Presupuesto equals Projection.Presupuesto into PRJ
                                                  from Presupuesto in PRJ.DefaultIfEmpty()
                                                  select new ProjectionViewModel()
                                                  {
                                                      idLinea = (int)PipeLine.IdLinea,
                                                      ID = PipeLine.ID,
                                                      Ejecutivo = PipeLine.Ejecutivo,
                                                      Cliente = PipeLine.Cliente,
                                                      Familia = Presupuesto.TipoProducto,
                                                      Producto = PipeLine.Producto,
                                                      ItemCodeSustrato = PipeLine.ItemCodeSustrato,
                                                      Presupuesto = PipeLine.Presupuesto,
                                                      Sustrato = PipeLine.Sustrato,
                                                      Gramaje = PipeLine.Gramaje,
                                                      Ancho_Bobina = PipeLine.AnchoBobina,
                                                      Ancho_Pliego = PipeLine.AnchoPliego,
                                                      Largo_Pliego = PipeLine.LargoPliego,
                                                      Paginas = (int)PipeLine.Paginas,
                                                      Montaje = PipeLine.Montaje,
                                                      Pliegos = (int)PipeLine.Pliegos,
                                         }).ToList();
            GetProjections(ref Entregas, document);

            return Entregas;
        } // End showExistingBudgets

        /// <summary>
        /// Takes the MainViewModel view model and applies a search and sorting of the data exposed on the DashBoard view
        /// </summary>
        /// <param name="MainView">MainViewModel brought from the view containing the data</param>
        /// <param name="viewModel">Pass-By-Reference from the static MainViewModel</param>
        public void PipelineLoad(MainViewModel MainView, ref MainViewModel viewModel) 
        {
            viewModel.documentNumber = _projectionContext.DetailPipelinePruebas.Count() == 0 ? (ushort)0 : (ushort)_projectionContext.DetailPipelinePruebas.Select(p => p.IdDoc).Max();
            IEnumerable<ProjectionViewModel> list = ShowExistingBudgets(viewModel.documentNumber).ToList();
            list = OrderContentBy(list, ref MainView);
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
            const int PageItems = 20;
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

        private IEnumerable<ProjectionViewModel> OrderContentBy(IEnumerable<ProjectionViewModel> projection, ref MainViewModel main)
        {
            if (!string.IsNullOrEmpty(main.SearchBudget))
            {
                // Look for Budget number and Client
                string SearchBudget = main.SearchBudget;
                projection = projection.Where(p => p.Presupuesto.ToLower().Contains(SearchBudget.Trim().ToLower())
                    || p.Cliente.ToLower().Contains(SearchBudget.Trim().ToLower()));
            }
            else
            {
                main.SearchBudget = String.Empty;
            }
            if (!string.IsNullOrEmpty(main.SearchExecutive))
            {
                // Look for Product and Executive
                string SearchExecutive = main.SearchExecutive;
                projection = projection.Where(p => p.Producto.ToLower().Contains(SearchExecutive.Trim().ToLower()) ||
                    p.Ejecutivo.ToLower().Contains(SearchExecutive.Trim().ToLower()));
            }
            else
            {
                main.SearchExecutive = String.Empty;
            }
            return projection;
        }

        /// <summary>
        /// Void method that connects each projection with the respective budget found in the DataBase 
        /// </summary>
        /// <param name="Entregas">Pass-By-Reference generic list of ProjectionVieModel</param>
        /// <param name="document">The number of the actual document</param>
        private void GetProjections(ref List<ProjectionViewModel> Entregas, int document) 
        {
            byte firstMonth = (byte)DateTime.Today.AddMonths(0).Month;
            byte secondMonth = (byte)DateTime.Today.AddMonths(1).Month;
            byte thirdMonth = (byte)DateTime.Today.AddMonths(2).Month;
            byte fourthMonth = (byte)DateTime.Today.AddMonths(3).Month;
            byte fifthMonth = (byte)DateTime.Today.AddMonths(4).Month;
            byte sixthMonth = (byte)DateTime.Today.AddMonths(5).Month;
            byte seventhMonth = (byte)DateTime.Today.AddMonths(6).Month;
            byte eigthMonth = (byte)DateTime.Today.AddMonths(7).Month;
            byte ninethMonth = (byte)DateTime.Today.AddMonths(8).Month;
            byte tenthMonth = (byte)DateTime.Today.AddMonths(9).Month;
            byte eleventhMonth = (byte)DateTime.Today.AddMonths(10).Month;
            foreach (var item in Entregas)
            {
                if (_projectionContext.DetailPipelineEntregasPruebas.Where(p => p.Presupuestos.Equals(item.Presupuesto) && p.IdDoc == document && p.IdLine == item.idLinea).Any())
                {
                    //LINQ query that creates a list of MonthViewModel data based on Presupuesto, IdLinea and between the range specified
                    Entregas[Entregas.FindIndex(c => c.Presupuesto == item.Presupuesto && c.idLinea == item.idLinea)].Month =
                        (from s in _projectionContext.DetailPipelineEntregasPruebas.Where(x => x.Presupuestos.Equals(item.Presupuesto) && x.IdDoc == document
                        && x.IdLine == item.idLinea && 
                        (x.Mes == firstMonth || x.Mes == secondMonth || x.Mes == thirdMonth
                        || x.Mes == fourthMonth || x.Mes == fifthMonth || x.Mes == sixthMonth 
                        || x.Mes == seventhMonth || x.Mes == eigthMonth || x.Mes == ninethMonth
                        || x.Mes == tenthMonth || x.Mes == eleventhMonth))
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
            DetailPipelinePruebas pipelineModel = new DetailPipelinePruebas();
            var codigoCliente = _projectionContext.Vista_SAP.Where(s => s.U_OrdenProduccionMet == row.ItemCodeSustrato).Select(p => p.CardCode).SingleOrDefault();
            pipelineModel.Ejecutivo = row.Ejecutivo;
            pipelineModel.Cliente = row.Cliente;
            pipelineModel.Producto = row.Producto;
            pipelineModel.Presupuesto = row.Presupuesto;
            pipelineModel.ItemCodeSustrato = row.ItemCodeSustrato;
            pipelineModel.Sustrato = row.Sustrato;
            pipelineModel.Gramaje = row.Gramaje == null ? 0 : (int)row.Gramaje;
            pipelineModel.AnchoBobina = row.Ancho_Bobina == null ? 0 : (int?)row.Ancho_Bobina;
            pipelineModel.AnchoPliego = row.Ancho_Pliego == null ? 0 : (int?)row.Ancho_Pliego;
            pipelineModel.LargoPliego = row.Largo_Pliego == null ? 0 : (int?)row.Largo_Pliego;
            pipelineModel.Paginas = row.Paginas == null ? 0 : row.Paginas;
            pipelineModel.Montaje = row.Montaje == null ? 0 : (int?)row.Montaje;
            pipelineModel.Pliegos = row.Pliegos == null ? 0 : row.Pliegos;
            pipelineModel.IdDoc = lastDocument + 1;
            pipelineModel.IdLinea = (int?)lineNumber;
            pipelineModel.FechaHora = DateTime.Now;
            pipelineModel.CardName = codigoCliente;
            _projectionContext.Entry(pipelineModel).State = EntityState.Added;
        } // End InsertSingelBudget

        /// <summary>
        /// Inserts a new line of projections with a new document number meanwhile holding the value from past documents/budgets
        /// </summary>
        /// <param name="item">Contains the data of each budgets</param>
        /// <param name="lineNumber">The line number of the main query</param>
        /// <param name="lastDocument">Last document number</param>
        public void InsertProjections(ProjectionViewModel row, uint lineNumber, ushort lastDocument, SessionViewModel session)
        {
            ConversionProcess listaReserva = (from Reserva in _projectionContext.A_Vista_OConversion_Reserva
                                              where Reserva.Presupuesto == row.Presupuesto && Reserva.Material 
                                              == row.ItemCodeSustrato
                                              join kgPapel in _projectionContext.VU_ACR_DON_012_OrcPapel on new
                                              {
                                                  CodigoPapel = Reserva.Material,
                                                  OP = Reserva.NumOrdem
                                              }
                                              equals new
                                              {
                                                  CodigoPapel = kgPapel.CodSubConta,
                                                  OP = kgPapel.NumOrdem
                                              }
                                              select new ConversionProcess
                                              {
                                                  TotalKilogramosPorCodigo = kgPapel.totKgCot
                                              }).FirstOrDefault();

            List<ProjectionKg> cantidadEnUnidades = 
                (from Lote in _projectionContext.OrdLotesProducao
                 where Lote.DataTermino >= session.StartDate && Lote.NumOrdem == row.NumOrdem
                 select new ProjectionKg
                 {
                     NumOrdem = Lote.NumOrdem,
                     IdLote = Lote.IdLote,
                     Lote = Lote.IdComponente,
                     CantidadUnidades = Lote.Quantidade,
                     FechaDeTermino = Lote.DataTermino
                 }).ToList();

            //DetailPipelineTotales pipelineTotales = new DetailPipelineTotales();
            //pipelineTotales.IdDoc = lastDocument + 1;
            //pipelineTotales.IdLine = (int?)lineNumber;
            //pipelineTotales.Mes = cantidadEnUnidades.Select(p => p.FechaDeTermino).Min().Month;
            //pipelineTotales.Año = cantidadEnUnidades.Select(p => p.FechaDeTermino).Min().Year;
            //pipelineTotales.Cantidad = row.Quantidade;
            //pipelineTotales.CantidadKilos = (decimal)listaReserva.TotalKilogramosPorCodigo;
            //pipelineTotales.Presupuestos = row.Presupuesto;
            //pipelineTotales.ItemCodeSustrato = row.ItemCodeSustrato;
            //pipelineTotales.NumeroPresupuesto = row.NumOrdem;
            //_projectionContext.Entry(pipelineTotales).State = EntityState.Added;

            //TODO foreach (ProjectionKg kgItem in cantidadEnUnidades.Where(p => p.Lote.Contains(row.Lote)))

            foreach (ProjectionKg kgItem in cantidadEnUnidades)
            {
                DetailPipelineEntregasPruebas month = new DetailPipelineEntregasPruebas();
                month.Cantidad = kgItem.CantidadUnidades;
                double unidadesEntreTotalPorLote = (double)kgItem.CantidadUnidades / (double)row.Quantidade;
                month.CantidadKilos = (decimal?)(unidadesEntreTotalPorLote * listaReserva.TotalKilogramosPorCodigo);
                month.IdDoc = lastDocument + 1;
                month.IdLine = (int?)lineNumber;
                month.Mes = kgItem.FechaDeTermino.Month;
                month.Año = kgItem.FechaDeTermino.Year;
                month.Presupuestos = row.Presupuesto;
                _projectionContext.Entry(month).State = EntityState.Added;
            }
        } // End GetProjections
        
        /// <summary>
        /// Inserts a new document number containing the list of new budgets, assigning projections from last documents 
        /// </summary>
        /// <param name="newBudgets">IEnumerable type that brings all the new budgets</param>
        public void InsertNewBudgets(IEnumerable<ProjectionViewModel> newBudgets, ushort lastDocument, SessionViewModel session) 
        {
            uint lineNumber = 0;
            foreach (var item in newBudgets)
            {
                if (!item.ItemCodeSustrato.Equals("PAPELES"))
                {
                    lineNumber++;
                    InsertSingleBudget(item, lineNumber, lastDocument);
                    InsertProjections(item, lineNumber, lastDocument, session);
                }
            } // End foreach
            HeaderPipelinePruebas headerPipeline = new HeaderPipelinePruebas();
            headerPipeline.FechaDoc = DateTime.Now;
            headerPipeline.IdDoc = lastDocument + 1;
            headerPipeline.Usuario = "rcast";
            _projectionContext.Entry(headerPipeline).State = EntityState.Added;
            _projectionContext.SaveChanges();
        } // End InsertNewBudgets

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
            double cantidadRequerida = Projection.Paginas > 2 ? CalculateSheet(Projection, Month) : double.Parse(Month);
            double montaje = Projection.Montaje == null ? 0 : (double)Projection.Montaje;
            double pliegos = Projection.Pliegos == null ? 0 : (double)Projection.Pliegos;
            double gramaje = Projection.Gramaje == null ? 0 : (double)Projection.Gramaje;

            if (Projection.Paginas > 2)
            {
                double result = (((anchoPliego * largoPliego) / 1000000) *
                    (cantidadRequerida)) * (gramaje / 1000);
                return Double.IsInfinity(result) || Double.IsNaN(result) ? 0 : Math.Round(result, 4);
            }
            else
            {
                double result = (((anchoPliego * largoPliego) / 1000000) *
                    (cantidadRequerida / montaje)) * (gramaje / 1000);
                return Double.IsInfinity(result) || Double.IsNaN(result) ? 0 : Math.Round(result, 4);
            }
        } // End calculateKG

        /// <summary>
        /// Calculates the amount of sheets
        /// </summary>
        /// <param name="Projection"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public double CalculateSheet(ProjectionViewModel Projection, string Month) 
        {
            double cantidadProyectada = Month == null ? 0 : Double.Parse(Month);
            double montaje = Projection.Montaje == null ? 0 : (double)Projection.Montaje;
            double pliegos = Projection.Pliegos == null ? 0 : (double)Projection.Pliegos;
            double paginas = Projection.Paginas == null ? 0 : (double)Projection.Paginas;
            double result = (cantidadProyectada / montaje) * (paginas/pliegos);
            return Double.IsInfinity(result) || Double.IsNaN(result) ? 0 : Math.Round(result, 4);
        } // End calculateSheet

        public void Dispose()
        {
            _projectionContext.Dispose();
            _sapDataContext.Dispose();
        }
    }
}