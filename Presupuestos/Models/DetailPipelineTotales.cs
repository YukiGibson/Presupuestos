namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DetailPipelineTotales
    {
        public int ID { get; set; }

        public int? IdDoc { get; set; }

        public int? IdLine { get; set; }

        public string ItemCodeSustrato { get; set; }

        public int? Mes { get; set; }

        public int? Año { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cantidad { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CantidadKilos { get; set; }

        [StringLength(10)]
        public string Presupuestos { get; set; }
    }
}
