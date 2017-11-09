namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HeaderPipeline")]
    public partial class HeaderPipeline
    {
        [Key]
        public int IdDoc { get; set; }

        public DateTime? FechaDoc { get; set; }

        [StringLength(50)]
        public string Usuario { get; set; }
    }
}
