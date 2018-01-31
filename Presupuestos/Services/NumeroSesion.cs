using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presupuestos.Repositories;
using System.Globalization;

namespace Presupuestos.Services
{
    public class NumeroSesion
    {
        /*************************************************************************
         * Method
         * Funcion principal de manejar la carga de sesion por semana, el cambio ocurre
         * en el momento que nos encontramos en el lunes de la semana siguiente, sumando un
         * 1 a la sesion y creado otra lista de presupuestos en el pipeline de abastecimiento
         *************************************************************************/
        public int ObtenerSesionAbastecimiento()
        {
            PipelineRepository pipeline = new PipelineRepository();
            int? lastDocument = pipeline.Read().Select(p => p.IdDoc).Max() == null ? (ushort)1 :
                (ushort)pipeline.Read().Select(p => p.IdDoc).Max();
            var lastSession = pipeline.GetProjectionContext.HeaderPipeline
                .Where(p => p.IdDoc == lastDocument).FirstOrDefault();

            if (lastSession != null)
            {
                DateTime lastSessionDate = (DateTime)lastSession.FechaDoc;
                int week = CultureInfo.InvariantCulture.Calendar
                    .GetWeekOfYear(lastSessionDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                int currentWeek = CultureInfo.InvariantCulture.Calendar
                    .GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                if (currentWeek > week)
                {
                    lastDocument += 1;
                }
            }
            pipeline.Dispose();
            return (int)lastDocument;
        }

        /*************************************************************************
         * Method
         * Funcion principal de manejar la carga de sesion por semana, el cambio ocurre
         * en el momento que nos encontramos en el lunes de la semana siguiente, sumando un
         * 1 a la sesion y creado otra lista de presupuestos en el pipeline de ventas
         *************************************************************************/
        public int ObtenerSesionVentas()
        {
            PipelineVentasRepository pipeline = new PipelineVentasRepository();
            int? lastDocument = pipeline.Read().Select(p => p.Sesion).Max() == null ? (ushort)1 :
                (ushort)pipeline.Read().Select(p => p.Sesion).Max();
            var lastSession = pipeline.GetContext().DetailPipelineVentas
                .Where(p => p.Sesion == lastDocument).FirstOrDefault();

            if (lastSession != null)
            {
                DateTime lastSessionDate = (DateTime)lastSession.FechaSesion;
                int week = CultureInfo.InvariantCulture.Calendar
                    .GetWeekOfYear(lastSessionDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                int currentWeek = CultureInfo.InvariantCulture.Calendar
                    .GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                if (currentWeek > week)
                {
                    lastDocument += 1;
                }
            }
            pipeline.Dispose();
            return (int)lastDocument;
        }
    }
}