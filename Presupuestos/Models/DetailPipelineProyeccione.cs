namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DetailPipelineProyeccione
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Vendedor { get; set; }

        public byte Mes { get; set; }

        public short Año { get; set; }

        [Column(TypeName = "money")]
        public decimal Proyeccion { get; set; }

        [StringLength(10)]
        public string Estimado { get; set; }
    }
}
