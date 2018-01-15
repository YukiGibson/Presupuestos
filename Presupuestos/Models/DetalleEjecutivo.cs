using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presupuestos.Models
{
    public class DetalleEjecutivo
    {
        public string Vendedor { get; set; }
        public decimal? FacturacionEstimado { get; set; }
        public decimal? FacturacionOP { get; set; }
        public decimal? MargenEstimado { get; set; }
        public decimal? MargenOP { get; set; }
        public decimal? TotalFacturacion { get; set; }
        public decimal? TotalMargen { get; set; }
        
    }
}