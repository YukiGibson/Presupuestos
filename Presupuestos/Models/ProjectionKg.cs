using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presupuestos.Models
{
    public class ProjectionKg
    {
        public string NumOrdem { get; set; }
        public string IdLote { get; set; }
        public string Lote { get; set; }
        public int? CantidadUnidades { get; set; }
        public DateTime FechaDeTermino { get; set; }
    }
    public class ConversionProcess
    {
        public string CodeSustrato { get; set; }
        public double? TotalKilogramosPorCodigo { get; set; }
    }    
}