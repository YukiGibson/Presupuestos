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
using Presupuestos.Repositories;

namespace Presupuestos.Services
{
    public class PipelineAbastecimiento : IDisposable
    {

        private PipelineRepository _pipeline;
        private SapRepository _sapRepository;

        public PipelineAbastecimiento()
        {
            this._pipeline = new PipelineRepository();
            this._sapRepository = new SapRepository();
        }

        public List<DetailPipeline> NewBudgets(SessionViewModel sessionView)
        {
            SqlParameter StartDate = new SqlParameter("@StartDate", sessionView.StartDate.ToString("yyyyMMdd"));
            List<DetailPipeline> projectionList = _pipeline.GetProjectionContext.Database
                .SqlQuery<DetailPipeline>("PCV_LoadBudgets @StartDate", StartDate).ToList();
            //LoadDimensionsFromSAP(ref projectionList); NO BORRAR
            return projectionList;
        } // End newBudgets

        //Si necesita cargar las dimensiones de SAP, descomentar la función LeadDimensionsFromSap
        //y en NewBudgets descomentar la llamada a la funcion
        #region LoanDimendionsfromSAP
        private void LoadDimensionsFromSAP(ref List<DetailPipeline> projectionList)
        {
            for (int i = 0; i < projectionList.Count(); i++)
            {
                long test = 0;
                string sustratoCode = projectionList[i].ItemCodeSustrato;
                // Tries to parse the value to integer, if it is a text, then false
                if (Int64.TryParse(sustratoCode, out test))
                {
                    var sap = _sapRepository.Read().Where(p => p.ItemCode
                    == sustratoCode).FirstOrDefault();
                    projectionList[i].Sustrato = sap.ItemName;
                    // Esta preguntando si es Bobina, si lo es entonces guardar el ancho de bobina y pliego
                    if (sustratoCode.Substring(10)
                        .Equals("0000"))
                    {
                        projectionList[i].Gramaje = (int?)sap.U_Gramaje;
                        projectionList[i].AnchoBobina = projectionList[i].AnchoPliego
                            = (int?)sap.U_Ancho_Plg;
                    }
                    else // Si es pliego, entonces:
                    {
                        //Agregamos 4 ceros al codigo existente para poder buscar la bobina a la que pertenece
                        string bobinaCode = projectionList[i].ItemCodeSustrato
                            .Substring(0, sustratoCode.Length - 4) + "0000";
                        projectionList[i].AnchoBobina = (int)_sapRepository.Read()
                            .Where(p => p.ItemCode == bobinaCode)
                            .Select(o => o.U_Ancho_Plg).FirstOrDefault();
                        projectionList[i].Gramaje = (int?)sap.U_Gramaje;
                        projectionList[i].AnchoPliego = (int?)sap.U_Ancho_Plg;
                        projectionList[i].LargoPliego = (int?)sap.U_Largo_Plg;
                    }
                }
            }
        }
        #endregion
        public ushort GetLastId()
        {
            if (_pipeline.Read().Count() == 0)
            {
                return (ushort)0;
            }
            else
            {
                return (ushort)_pipeline.GetProjectionContext.HeaderPipeline.Select(o => o.IdDoc).Max();
            }
        }

        public List<ProjectionViewModel> ShowExistingBudgets(int document) 
        {
            List <ProjectionViewModel> Entregas = 
                (from PipeLine in _pipeline.GetProjectionContext.DetailPipeline.Where(p => p.IdDoc == document)
                join Projection in _pipeline.GetProjectionContext.A_Vista_Presupuestos on 
                PipeLine.Presupuesto equals Projection.Presupuesto into PRJ
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
                    NumOrdem = PipeLine.NumOrdem,
                    ProbVenta = PipeLine.ProbVenta
                }).ToList();
            GetProjections(ref Entregas, document);
            return Entregas;
        } // End showExistingBudgets

        public List<Abastecimiento> BuildDetailTable(List<ProjectionViewModel> list)
        {
            List<Abastecimiento> table = new List<Abastecimiento>();
            foreach (var client in list.Select(p => p.Cliente).Distinct())
            {
                Abastecimiento detail = new Abastecimiento();
                detail.client = client;
                decimal? totalQuantity = 0;
                foreach (var row in list.Where(p => p.Cliente.Equals(client)))
                {
                    detail.sales.Add(
                        new ProjectionViewModel
                        {
                            ID = row.ID,
                            Cliente = row.Cliente,
                            Producto = row.Producto,
                            Presupuesto = row.Presupuesto,
                            ItemCodeSustrato = row.ItemCodeSustrato,
                            Sustrato = row.Sustrato,
                            NumOrdem = row.NumOrdem,
                            Month = row.Month,
                            kg = row.kg,
                            ProbVenta = row.ProbVenta
                        }
                        );
                    foreach (var mes in row.Month)
                    {
                        totalQuantity = totalQuantity + (decimal?)mes.value;
                    }
                }
                detail.rowspan = detail.sales.Count() + 1;
 
                decimal? kg = detail.sales.Where(o => o.Cliente.Contains(client)).Sum(o => o.kg);

                detail.totals = new ProjectionViewModel()
                {
                    cantidad = totalQuantity,
                    kg = kg
                };
                table.Add(detail);
            }
            return table;
        }

        public List<ProjectionViewModel> PipelineLoad(MainViewModel viewModel) 
        {
            int document = _pipeline.Read().Count() == 0 
                ? (ushort)0 : (ushort)_pipeline.Read().Select(p => p.IdDoc).Max();
            
            List<ProjectionViewModel> list = ShowExistingBudgets(document).ToList();
            for (int i = 0; i < list.Count(); i++)
            {
                decimal? kg = 0;
                foreach (var month in list[i].Month)
                {
                    kg = kg + month.kg;
                }
                list[i].kg = kg;
            }
            return list;
        } // End dashboardLoad

        public void LoadDropDowns(List<ProjectionViewModel> list, ref MainViewModel viewModel)
        {
            List<ProjectionViewModel> temporalList = list.ToList();
            List<string> ejecutivos = temporalList.OrderBy(o => o.Ejecutivo).Select(p => p.Ejecutivo).Distinct().ToList();
            foreach (var ejecutivo in ejecutivos)
            {
                viewModel.executiveDropDown.Add(ejecutivo.ToString(), ejecutivo.ToString());
            }
        } // End LoadDropDowns

        public List<ProjectionViewModel> SortSalesPipeline(List<ProjectionViewModel> list, MainViewModel detail)
        {
            if (detail != null)
            {
                if (!String.IsNullOrEmpty(detail.executive))
                {
                    list = list.Where(o => o.Ejecutivo.Contains(detail.executive)).ToList();
                }

            }
            return list.Count == 0 ? list : list.OrderByDescending(o => o.Cliente).ToList();
        } // End OrderSalesPipeline()

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
                if (_pipeline.GetProjectionContext.DetailPipelineEntregas.Where(p => p.Presupuestos.Equals(item.Presupuesto) && p.IdDoc == document && p.IdLine == item.idLinea).Any())
                {
                    //LINQ query that creates a list of MonthViewModel data based on Presupuesto, IdLinea and between the range specified
                    Entregas[Entregas.FindIndex(c => c.Presupuesto == item.Presupuesto && c.idLinea == item.idLinea)].Month =
                        (from s in _pipeline.GetProjectionContext.DetailPipelineEntregas.Where(x => x.Presupuestos.Equals(item.Presupuesto) && x.IdDoc == document
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
                             value = s.Cantidad == null ? 0 : (double)s.Cantidad,
                             kg = s.CantidadKilos
                         }).OrderBy(o => o.id).ToList();
                    //Foreach that saves the name of the month in spanish into the monthName property
                    Entregas[Entregas.FindIndex(c => c.Presupuesto == item.Presupuesto && c.idLinea == item.idLinea)].
                        Month.ForEach(x => x.monthName = GetMonthName(x.year, x.month));
                } // End if
            } // End foreach
        } // End getProjections

        public string GetMonthName(params int[] date)
        {
            DateTime fecha = new DateTime(date[0], date[1], 15);
            return fecha.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
        } // End GetMonthName

        private void InsertSingleBudget(DetailPipeline row, uint lineNumber, ushort lastDocument) 
        {
            var oldList = _pipeline.Read().Where(p => p.IdDoc == lastDocument).ToList();
            var editable = oldList.Where(p => p.Presupuesto.Contains(row.Presupuesto)
            && p.NumOrdem.Contains(row.NumOrdem)
            && p.ItemCodeSustrato.Contains(row.ItemCodeSustrato)
            && p.Sustrato.Contains(row.Sustrato)).FirstOrDefault();

            if (editable != null)
            {
                editable.ProbVenta = row.ProbVenta;
                editable.Quantidade = row.Quantidade;
                editable.cantidad = row.cantidad;
                editable.Producto = row.Producto;
                editable.FechaHora = row.FechaHora;
            }
            else
            {
                row.IdDoc = lastDocument;
                row.IdLinea = (int?)lineNumber;
                _pipeline.Create(row);
            }
        } // End InsertSingelBudget

        public void InsertProjections(DetailPipeline row, uint lineNumber, ushort lastDocument, SessionViewModel session)
        {
            ConversionProcess listaReserva = (from Reserva in _pipeline.GetProjectionContext.A_Vista_OConversion_Reserva
                                              where Reserva.Presupuesto == row.Presupuesto && Reserva.Material 
                                              == row.ItemCodeSustrato
                                              join kgPapel in _pipeline.GetProjectionContext.VU_ACR_DON_012_OrcPapel on new
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
                (from Lote in _pipeline.GetProjectionContext.OrdLotesProducao
                 where Lote.DataTermino >= session.StartDate && Lote.NumOrdem == row.NumOrdem
                 select new ProjectionKg
                 {
                     NumOrdem = Lote.NumOrdem,
                     IdLote = Lote.IdLote,
                     Lote = Lote.IdComponente,
                     CantidadUnidades = Lote.Quantidade,
                     FechaDeTermino = Lote.DataTermino
                 }).ToList();
            InsertTotales(row, lastDocument, cantidadEnUnidades, listaReserva, lineNumber);
            InsertEntregas(row, lastDocument, cantidadEnUnidades, listaReserva, lineNumber);
        } // End GetProjections

        private void InsertTotales(DetailPipeline row, 
            ushort lastDocument, List<ProjectionKg> cantidadEnUnidades, 
            ConversionProcess listaReserva, uint lineNumber)
        {
            int minMonth = cantidadEnUnidades.Select(p => p.FechaDeTermino).Min().Month;
            int minYear = cantidadEnUnidades.Select(p => p.FechaDeTermino).Min().Year;

            DetailPipelineTotales pipelineTotales = _pipeline.GetProjectionContext
                .DetailPipelineTotales.Where(p => p.IdDoc == lastDocument
                && p.Presupuestos.Contains(row.Presupuesto) && p.NumeroPresupuesto.Contains(row.NumOrdem)
                && p.ItemCodeSustrato.Contains(row.ItemCodeSustrato) && p.Mes == minMonth && p.Año == minYear).FirstOrDefault();
            if (pipelineTotales != null)
            {
                pipelineTotales.Cantidad = row.Quantidade;
                pipelineTotales.CantidadKilos = (decimal)listaReserva.TotalKilogramosPorCodigo;
                //pipelineTotales.IdLine = (int?)lineNumber;
            }
            else
            {
                DetailPipelineTotales totales = new DetailPipelineTotales();
                totales.IdDoc = lastDocument;
                totales.IdLine = (int?)lineNumber;
                totales.Mes = cantidadEnUnidades.Select(p => p.FechaDeTermino).Min().Month;
                totales.Año = cantidadEnUnidades.Select(p => p.FechaDeTermino).Min().Year;
                totales.Cantidad = row.Quantidade;
                totales.CantidadKilos = (decimal)listaReserva.TotalKilogramosPorCodigo;
                totales.Presupuestos = row.Presupuesto;
                totales.ItemCodeSustrato = row.ItemCodeSustrato;
                totales.NumeroPresupuesto = row.NumOrdem;
                _pipeline.GetProjectionContext.Entry(totales).State = EntityState.Added;
            }
        }

        private void InsertEntregas(DetailPipeline row,
            ushort lastDocument, List<ProjectionKg> cantidadEnUnidades,
            ConversionProcess listaReserva, uint lineNumber)
        {

            foreach (ProjectionKg kgItem in cantidadEnUnidades.Where(p => p.Lote.Contains(row.Lote)))
            {
                DetailPipelineEntregas existing = _pipeline.GetProjectionContext.DetailPipelineEntregas
                .Where(p => p.Presupuestos.Contains(row.Presupuesto) && p.IdDoc == lastDocument
                && p.Mes == kgItem.FechaDeTermino.Month && p.Año == kgItem.FechaDeTermino.Year 
                && p.ItemCodeSustrato.Contains(row.ItemCodeSustrato)).FirstOrDefault();
                if (existing != null)
                {
                    double unidadesEntreTotalPorLote = (double)kgItem.CantidadUnidades / (double)row.Quantidade;
                    decimal? cantidadKilos = (decimal?)(unidadesEntreTotalPorLote * listaReserva.TotalKilogramosPorCodigo);
                    if (existing.Cantidad != kgItem.CantidadUnidades &&
                        existing.CantidadKilos != cantidadKilos)
                    {
                        DetailPipelineHistorico historico = new DetailPipelineHistorico()
                        {
                            IdDoc = lastDocument,
                            IdLine = (int?)lineNumber,
                            Mes = kgItem.FechaDeTermino.Month,
                            Ano = kgItem.FechaDeTermino.Year,
                            Cantidad = existing.Cantidad,
                            CantidadKilos = existing.CantidadKilos,
                            FechaHora = DateTime.Now,
                            Presupuestos = existing.Presupuestos
                        };
                        _pipeline.GetProjectionContext.Entry(historico).State = EntityState.Added;
                        existing.Cantidad = 0;//kgItem.CantidadUnidades;
                        existing.CantidadKilos = (decimal?)(unidadesEntreTotalPorLote * listaReserva.TotalKilogramosPorCodigo);
                        existing.IdLine = (int?)lineNumber;
                    }
                }
                else
                {
                    DetailPipelineEntregas month = new DetailPipelineEntregas();
                    month.Cantidad = kgItem.CantidadUnidades;
                    double unidadesEntreTotalPorLote = (double)kgItem.CantidadUnidades / (double)row.Quantidade;
                    month.CantidadKilos = (decimal?)(unidadesEntreTotalPorLote * listaReserva.TotalKilogramosPorCodigo);
                    month.IdDoc = lastDocument;
                    month.IdLine = (int?)lineNumber;
                    month.Mes = kgItem.FechaDeTermino.Month;
                    month.Año = kgItem.FechaDeTermino.Year;
                    month.Presupuestos = row.Presupuesto;
                    month.ItemCodeSustrato = row.ItemCodeSustrato;
                    _pipeline.GetProjectionContext.Entry(month).State = EntityState.Added;
                }
            }
        }

        public void InsertNewBudgets(List<DetailPipeline> newBudgets, ushort lastDocument, SessionViewModel session) 
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
            HeaderPipeline headerPipeline = _pipeline.GetProjectionContext
                .HeaderPipeline.Where(p => p.IdDoc == lastDocument).FirstOrDefault();
            if (headerPipeline != null)
            {
                headerPipeline.FechaDoc = DateTime.Now;
            }
            else
            {
                HeaderPipeline headerNew = new HeaderPipeline();
                headerNew.FechaDoc = DateTime.Now;
                headerNew.IdDoc = lastDocument;
                headerNew.Usuario = "rcast";
                _pipeline.GetProjectionContext.Entry(headerNew).State = EntityState.Added;
            }
            _pipeline.Save();
        } // End InsertNewBudgets


        public void Dispose()
        {
            _pipeline.Dispose();
            _sapRepository.Dispose();
        }
    }
}