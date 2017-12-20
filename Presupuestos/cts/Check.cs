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
            _projectionContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s); // Log in output in order to see the generated sql query
            var list = (from Budget in _projectionContext.A_Vista_Presupuestos.Where(o => o.FechaEmisión >= sessionView.StartDate
                        && o.FechaEmisión <= sessionView.EndDate)
                       join Reserva in _projectionContext.A_Vista_OConversion_Reserva on Budget.Presupuesto equals Reserva.Presupuesto
                       join Process in _projectionContext.EstrProcessos on new
                       {
                           name = SqlFunctions.CharIndex("P", Budget.Presupuesto) != 0 ?
                           Budget.Presupuesto.Substring(0, Budget.Presupuesto.Length - 1) : Budget.Presupuesto,
                           name1 = Reserva.IDProcessoOrigem
                       } equals new
                       {
                           name = Process.CodEstrutura,
                           name1 = Process.IdProcesso
                       } into Processos
                       from Proc in Processos.DefaultIfEmpty()
                       join Mater in _projectionContext.View_USR_OrdMateriaisM3 on new
                       {
                           first = Budget.OP,
                           second = Reserva.IDProcessoOrigem,
                           third = Reserva.Material
                       } equals new
                       {
                           first = Mater.NumOrdem,
                           second = Mater.IdProcessoUso,
                           third = Mater.CodItem
                       } into Materiais
                        where Budget.Presupuesto.Contains("P") && !Reserva.Material.Equals("PAPELES") && Reserva.ProbVenta > 60
                        select new ProjectionViewModel
                       {
                           ID = Budget.ID,
                           Ejecutivo = Budget.Vendedor, // Vendedor
                           Cliente = Budget.Cliente, // Cliente
                           Familia = Budget.TipoProducto, // 
                           Producto = Reserva.Titulo, // Producto
                           Presupuesto = Budget.Presupuesto,
                           ItemCodeSustrato = Reserva.Material,
                           Sustrato = Reserva.Descripcion,
                           Gramaje = 0, // SAP
                           Ancho_Bobina = 0, // SAP
                           Ancho_Pliego = 0, // SAP
                           Largo_Pliego = 0, // SAP
                           Paginas = Proc.PQtdPagTot,
                           Montaje = (int?)Materiais.FirstOrDefault().FatorUnidades, //Cambiar ese 
                           Pliegos = Proc.QtdRepeticoes,
                       }).ToList();

            for (int i = 0; i < list.Count(); i++)
            {
                long test = 0;
                string sustratoCode = list[i].ItemCodeSustrato;
                if (Int64.TryParse(sustratoCode, out test)) // Tries to parse the value to integer, if it is a text, then false
                {
                    var sap = _sapDataContext.OITM.Where(p => p.ItemCode == sustratoCode).FirstOrDefault();
                    list[i].Sustrato = sap.ItemName;
                    if (sustratoCode.Substring(10)
                        .Equals("0000")) // Esta preguntando si es Bobina, si lo es entonces guardar el ancho de bobina y pliego
                    {
                        list[i].Gramaje = (double?)sap.U_Gramaje;
                        list[i].Ancho_Bobina = list[i].Ancho_Pliego = sap.U_Ancho_Plg;
                    }
                    else // Si es pliego, entonces:
                    {
                        //Agregamos 4 ceros al codigo existente para poder buscar la bobina a la que pertenece
                        string bobinaCode = list[i].ItemCodeSustrato.Substring(0 , sustratoCode.Length - 4) + "0000";
                        list[i].Ancho_Bobina = _sapDataContext.OITM.Where(p => p.ItemCode == bobinaCode).Select(o => o.U_Ancho_Plg).FirstOrDefault();
                        list[i].Gramaje = (double?)sap.U_Gramaje;
                        list[i].Ancho_Pliego = sap.U_Ancho_Plg;
                        list[i].Largo_Pliego = sap.U_Largo_Plg;
                    }
                }
            }

            return list;
        } // End newBudgets

        /// <summary>
        /// Brings a list of the existing budgets in the DataBase
        /// </summary>
        /// <param name="document">integer32 holding the current last document</param>
        /// <returns>IEnumerable type list of ProjectionViewModel containing the current budgets</returns>
        public IEnumerable<ProjectionViewModel> ShowExistingBudgets(int document) 
        {
            List <ProjectionViewModel> Entregas = (from PipeLine in _projectionContext.DetailPipeline.Where(p => p.IdDoc == document)
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
        public void DashboardLoad(MainViewModel MainView, ref MainViewModel viewModel) 
        {
            int last = _projectionContext.DetailPipeline.Count() == 0 ? (ushort)0 : (ushort)_projectionContext.DetailPipeline.Select(p => p.IdDoc).Max();
            viewModel.documentNumber = (ushort)last;
            //Si se ocupa traer las proyecciones por meses, entonces se deben enviar por referencia en showExistingBudgets()
            IEnumerable<ProjectionViewModel> list = ShowExistingBudgets(last).ToList();

            if (!string.IsNullOrEmpty(MainView.SearchBudget))
            {
                // Look for Budget number and Client
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
                // Look for Product and Executive
                list = list.Where(p => p.Producto.ToLower().Contains(MainView.SearchExecutive.Trim().ToLower()) ||
                    p.Ejecutivo.ToLower().Contains(MainView.SearchExecutive.Trim().ToLower()));
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
            byte firstMonth = (byte)DateTime.Today.AddMonths(0).Month;
            byte secondMonth = (byte)DateTime.Today.AddMonths(1).Month;
            byte thirdMonth = (byte)DateTime.Today.AddMonths(2).Month;
            byte fourthMonth = (byte)DateTime.Today.AddMonths(3).Month;
            byte fifthMonth = (byte)DateTime.Today.AddMonths(4).Month;
            byte sixthMonth = (byte)DateTime.Today.AddMonths(5).Month;
            byte seventhMonth = (byte)DateTime.Today.AddMonths(6).Month;
            foreach (var item in Entregas)
            {
                if (_projectionContext.DetailPipelineEntregas.Where(p => p.Presupuestos.Equals(item.Presupuesto) && p.IdDoc == document && p.IdLine == item.idLinea).Any())
                {
                    //LINQ query that creates a list of MonthViewModel data based on Presupuesto, IdLinea and between the range specified
                    Entregas[Entregas.FindIndex(c => c.Presupuesto == item.Presupuesto && c.idLinea == item.idLinea)].Month =
                        (from s in _projectionContext.DetailPipelineEntregas.Where(x => x.Presupuestos.Equals(item.Presupuesto) && x.IdDoc == document
                        && x.IdLine == item.idLinea && 
                        (x.Mes == firstMonth || x.Mes == secondMonth || x.Mes == thirdMonth
                        || x.Mes == fourthMonth || x.Mes == fifthMonth || x.Mes == sixthMonth || x.Mes == seventhMonth))
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
            string nombreCliente = GetSearchName(row.Cliente);
            var codigoCliente = _projectionContext.Vista_SAP.Where(s => s.U_OrdenProduccionMet == row.ItemCodeSustrato).Select(p => p.CardCode).SingleOrDefault();
            //TODO hacer por el codigo de sustrato, no por la OP porque no es fiable
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
            pipelineModel.Montaje = row.Montaje == null ? 0 : row.Montaje;
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
        private void CreateNewProjectionLine(ProjectionViewModel item, uint lineNumber, int lastDocument)
        {
            //TODO aca traer la data necesaria 
            //A_Vista_OConversion_Reserva 
            //[NumOrdem] = OP
            //[Material] = Codigo de material
            //[IdProcessOrigem] = codigo del proceso
            //OrdLotesProducao 
            //[NumOrdem] = OP
            //[IdComponente] = Codigo de proceso
            //[IdLote] = A, B, C, D, E, F, G
            //[DataTermino] = Fechas
            //VU_ACR_DON_012_OrcPapel
            //[NumOrdem] = Número de orden de producción
            //[totKgCot] = el total de los kilos
            //[CodSubConta] = Código de papel

            ConversionProcess listaReserva = (from Reserva in _projectionContext.A_Vista_OConversion_Reserva
                    where Reserva.Presupuesto == item.Presupuesto && Reserva.Material == item.ItemCodeSustrato
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
                    { Lote = Reserva.IDProcessoOrigem, OP = Reserva.NumOrdem,
                        TotalKilogramosPorCodigo = kgPapel.totKgCot }).FirstOrDefault();

            string OP = listaReserva.OP;
            string NombreProceso = listaReserva.Lote.Substring(0, listaReserva.Lote.IndexOf('-') == -1 ? 
                listaReserva.Lote.Length : listaReserva.Lote.IndexOf('-'));

            List<ProjectionKg> cantidadEnUnidades = (from Lote in _projectionContext.OrdLotesProducao
                                                        where Lote.NumOrdem == OP
                                                        select new ProjectionKg
                                                        {
                                                            IdLote = Lote.IdLote,
                                                            Lote = Lote.IdComponente,
                                                            CantidadUnidades = Lote.Quantidade,
                                                            FechaDeTermino = Lote.DataTermino
                                                        }).ToList();

            foreach (ProjectionKg kgItem in cantidadEnUnidades.Where(p => p.Lote.Substring(0, p.Lote.IndexOf
                ('-') == -1 ? p.Lote.Length : p.Lote.IndexOf('-')).Equals(NombreProceso)))
            {
                int totalUnidadesPorLote = (int)cantidadEnUnidades.Where(p => p.Lote.Substring(0, p.Lote.IndexOf('-') == -1 ?
                    p.Lote.Length : p.Lote.IndexOf('-')).Equals(kgItem.Lote.Substring(0, kgItem.Lote.IndexOf('-') == -1 ?
                    kgItem.Lote.Length : kgItem.Lote.IndexOf('-')))).Sum(o => o.CantidadUnidades);//Sum(p => p.CantidadUnidades);
                DetailPipelineEntregas month = new DetailPipelineEntregas();
                month.Cantidad = kgItem.CantidadUnidades;
                //TODO revisar el calculo ya que no lo está realizando
                double unidadesEntreTotalPorLote = (double)kgItem.CantidadUnidades / (double)totalUnidadesPorLote;
                month.CantidadKilos = (decimal?)(unidadesEntreTotalPorLote * listaReserva.TotalKilogramosPorCodigo);
                month.IdDoc = lastDocument + 1;
                month.IdLine = (int?)lineNumber;
                month.Mes = kgItem.FechaDeTermino.Month;
                month.Año = kgItem.FechaDeTermino.Year;
                month.Presupuestos = item.Presupuesto;
                _projectionContext.Entry(month).State = EntityState.Added;
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
                List<DetailPipelineEntregas> list = _projectionContext.DetailPipelineEntregas.Where(x => x.Presupuestos == Projection.Presupuesto
                    && x.Mes == item.month && x.Año == item.year && x.IdDoc == lastDocument && x.IdLine == Projection.idLinea).ToList();

                if (list.Count != 0)
                {
                    //TODO Comentar si se va a publicar a la página de pruebas
                    //DetailPipelineHistorico historico = new DetailPipelineHistorico();
                    //historico.IdDoc = lastDocument;
                    //historico.IdLine = (int)Projection.idLinea;
                    //historico.Ano = item.year;
                    //historico.Cantidad = list.Select(p => p.Cantidad).SingleOrDefault();
                    //historico.CantidadKilos = list.Select(p => p.CantidadKilos).SingleOrDefault();
                    //historico.CantidadMedida = list.Select(p => p.CantidadMedida).SingleOrDefault();
                    //historico.Presupuestos = Projection.Presupuesto;
                    //historico.Mes = item.month;
                    //historico.FechaHora = DateTime.Now;

                    ////TO DO
                    ////Ingresar el nombre de la persona que este autenticada hasta ese momento
                    //_projectionContext.Entry(historico).State = EntityState.Added;

                    var updateRow = _projectionContext.DetailPipelineEntregas.Where(p => p.Presupuestos == Projection.Presupuesto
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
                    _projectionContext.Entry(entregas).State = EntityState.Added;
                } // End if/else        
            } // End foreach
        } // End insertUpdateEntregas
        
        /// <summary>
        /// Inserts a new document number containing the list of new budgets, assigning projections from last documents 
        /// </summary>
        /// <param name="newBudgets">IEnumerable type that brings all the new budgets</param>
        public void InsertNewBudgets(IEnumerable<ProjectionViewModel> newBudgets, ushort lastDocument) 
        {
            uint lineNumber = 0;
            foreach (var item in newBudgets)
            {
                //TODO ahora que las proyecciones vienen de la base de datos de Metrics, entonces cambiar CreateNewProjectionLine
                lineNumber++;
                InsertSingleBudget(item, lineNumber, lastDocument);
                CreateNewProjectionLine(item, lineNumber, lastDocument);
            } // End foreach
            HeaderPipeline headerPipeline = new HeaderPipeline();
            headerPipeline.FechaDoc = DateTime.Now;
            headerPipeline.IdDoc = lastDocument + 1;
            headerPipeline.Usuario = "rcast";
            _projectionContext.Entry(headerPipeline).State = EntityState.Added;
            _projectionContext.SaveChanges();
        } // End InsertNewBudgets

        /// <summary>
        /// Using the values from the budgets that have the Checked Boolean on true, assign those value to each budgets
        /// </summary>
        /// <param name="projection">New projection</param>
        /// <param name="month"></param>
        public void InsertNewProjection(List<ProjectionViewModel> projection, List<MonthViewModel> month) 
        {
            ushort lastDocument = _projectionContext.DetailPipeline.Count() == 0 ? (ushort)0 : (ushort)_projectionContext.DetailPipeline.Select(p => p.IdDoc).Max();
            foreach (var item in projection.Where(p => p.Checked)) // Where the item is checked
            {
                if (month != null)
                {
                    InsertUpdateEntregas(item, month, lastDocument);
                }
            } // End foreach
            _projectionContext.SaveChanges();
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
        }
        
         private string GetSearchName(String clientName){
            String[] arrayCliente = new String[2];
            if (clientName.Contains((char)44))
            {
                clientName = clientName.Remove(clientName.IndexOf((char)44));
            }
            // End if, remueve la coma de cualquier nombre
            for (int i = 0; i < arrayCliente.Length; i++)
            {
                if (clientName.IndexOf(" ") != 0 && clientName.IndexOf(" ") != -1)
                {
                    arrayCliente[i] = clientName.Substring(0, clientName.IndexOf(" "));
                    clientName = clientName.Remove(0, clientName.IndexOf(" ") + 1);
                }
                else if (arrayCliente[0] != clientName)
                {
                    arrayCliente[i] = clientName;
                } // End if/else
            } // End for
            if (arrayCliente[0] != null)
            {
                return arrayCliente[0] + " " + arrayCliente[1];
            }// End If

            return "";
        } // End dividirClienteNombre
          /*List<ProjectionViewModel> list = (from Presupuestos in _projectionContext.A_Vista_Presupuestos
                    join leftReserva in _projectionContext.A_Vista_OConversion_Reserva on Presupuestos.Presupuesto equals leftReserva.Presupuesto 
                    into Res
                    from Reserva in Res.DefaultIfEmpty()
                    from Process in _projectionContext.EstrProcessos.Where(p => p.CodEstrutura == 
                    (SqlFunctions.CharIndex("P", Presupuestos.Presupuesto) != 0 ? 
                    Presupuestos.Presupuesto.Substring(0, Presupuestos.Presupuesto.Length - 1) : Presupuestos.Presupuesto)
                    && p.IdProcesso == Reserva.IDProcessoOrigem).Take(1).DefaultIfEmpty() // Outer Apply
                    //&& p.IdProcesso == Reserva.IDProcessoOrigem agregados el 13/12/2017 a las 11:55 am
                    join Orc in _projectionContext.OrcHdr on Presupuestos.Presupuesto equals Orc.NumOrcamento
                    where Orc.Fax.Contains("PREV") && Process.PIdClasse != "ACBMT" && Presupuestos.Presupuesto.Contains("P")
                    select new ProjectionViewModel()
                    {
                        ID = Presupuestos.ID,
                        Ejecutivo = Presupuestos.Vendedor,
                        Cliente = Presupuestos.Cliente,
                        Familia = Presupuestos.TipoProducto,
                        Producto = Reserva.Titulo, // Sustrato => SAP
                        Presupuesto = Presupuestos.Presupuesto,
                        ItemCodeSustrato = Reserva.Material,
                        Sustrato = "",
                        Gramaje = 0, // SAP
                        Ancho_Bobina = 0, // SAP
                        Ancho_Pliego = 0, // SAP
                        Largo_Pliego = 0, // SAP
                        Paginas = Process.PQtdPagTot,  
                        Montaje = Process.PQtdPagCad, //Cambiar ese 
                        Pliegos = Process.QtdRepeticoes, 
                    }).ToList();*/
    }
}