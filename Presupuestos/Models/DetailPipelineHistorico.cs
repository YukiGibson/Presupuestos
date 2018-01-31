namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetailPipelineHistorico")]
    public partial class DetailPipelineHistorico
    {
        public int ID { get; set; }

        public int? IdDoc { get; set; }

        public int? IdLine { get; set; }

        public int? Mes { get; set; }

        public int? Ano { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cantidad { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CantidadKilos { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CantidadMedida { get; set; }

        [StringLength(10)]
        public string Presupuestos { get; set; }

        public DateTime? FechaHora { get; set; }

        [StringLength(50)]
        public string ItemCodeSustrato { get; set; }
    }
}
