using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presupuestos.Models
{
    public class DetalleEjecutivo
    {
        public string Vendedor { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0.##}")]
        public decimal? FacturacionEstimado { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0.##}")]
        public decimal? FacturacionOP { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0.##}")]
        public decimal? MargenEstimado { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0.##}")]
        public decimal? MargenOP { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0.##}")]
        public decimal? TotalFacturacion { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0.##}")]
        public decimal? TotalMargen { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0.##}")]
        public decimal MargenProyectado { get; set; }

    }
}