using Presupuestos.Repositories;
using Presupuestos.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Presupuestos.Services
{
    public class ResumenService : IDisposable
    {
        private PipelineRepository pipelineRepository = new PipelineRepository();


        /*************************************************************************
         * Method
         * Query que obtiene los datos para el grafico de kilos por cliente
         *************************************************************************/
        public List<ClienteKilosGrafico> GetKilosForGrafico(ResumenViewModel resumen)
        {
            return (from Detalle in pipelineRepository.Read()
                    join Entregas in pipelineRepository.GetProjectionContext.DetailPipelineEntregas on
                    new
                    {
                        presupuesto = Detalle.Presupuesto,
                        sustrato = Detalle.ItemCodeSustrato
                    } equals new
                    {
                        presupuesto = Entregas.Presupuestos,
                        sustrato = Entregas.ItemCodeSustrato
                    }
                    where Detalle.IdDoc == 1 && Entregas.Mes == resumen.mes && Entregas.Año == resumen.anno
                    group Entregas by new { Detalle.Cliente } into g
                    select new ClienteKilosGrafico
                    {
                        cliente = g.Key.Cliente,
                        kilogramos = g.Sum(o => o.CantidadKilos)
                    }).ToList();
        }

        /*************************************************************************
         * Method
         * Query que obtiene los datos para el grafico de consumos por cliente
         *************************************************************************/
        public List<ClienteConsumosMesesGrafico> GetConsumosForGrafico()
        {
            return (from Detalle in pipelineRepository.Read()
                    join Entregas in pipelineRepository.GetProjectionContext.DetailPipelineEntregas on
                    new
                    {
                        presupuesto = Detalle.Presupuesto,
                        sustrato = Detalle.ItemCodeSustrato
                    } equals new
                    {
                        presupuesto = Entregas.Presupuestos,
                        sustrato = Entregas.ItemCodeSustrato
                    }
                    group Entregas by new { Detalle.Cliente, Entregas.Mes, Entregas.Año, Detalle.Familia } into g
                    select new ClienteConsumosMesesGrafico
                    {
                        cliente = g.Key.Cliente,
                        anno = g.Key.Año,
                        mes = g.Key.Mes,
                        consumos = g.Sum(o => o.Cantidad),
                        familia = g.Key.Familia
                    }).ToList();
        }

        /*************************************************************************
         * Method
         * Borra la puntuacion del valor de clientes por temas de mal encoding en la 
         * vista
         *************************************************************************/
        public List<ClienteConsumosMesesGrafico> DeletePunctuationConsumos(List<ClienteConsumosMesesGrafico> consumos)
        {
            for (int i = 0; i < consumos.Count(); i++)
            {
                consumos[i].cliente = consumos[i].cliente.Replace('ñ', 'n');
                Regex regex = new Regex(@"[\,\.\'\!\?\;\:\-\(\)]+");
                string result = regex.Replace(consumos[i].cliente, "");
                consumos[i].cliente = result;
            }
            return consumos;
        }

        /*************************************************************************
         * Method
         * Borra la puntuacion del valor de clientes por temas de mal encoding en la 
         * vista
         *************************************************************************/
        public List<ClienteKilosGrafico> DeletePunctuationKilos(List<ClienteKilosGrafico> kilos)
        {
            for (int i = 0; i < kilos.Count(); i++)
            {
                kilos[i].cliente = kilos[i].cliente.Replace('ñ', 'n');
                Regex regex = new Regex(@"[\,\.\'\!\?\;\:\-\(\)]+");
                string result = regex.Replace(kilos[i].cliente, "");
                kilos[i].cliente = result;
            }
            return kilos;
        }

        public void Dispose()
        {
            ((IDisposable)pipelineRepository).Dispose();
        }
    }
}