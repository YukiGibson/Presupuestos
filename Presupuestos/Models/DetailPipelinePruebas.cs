namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DetailPipelinePruebas
    {
        [StringLength(100)]
        public string Ejecutivo { get; set; }

        [StringLength(200)]
        public string Cliente { get; set; }

        [StringLength(200)]
        public string Producto { get; set; }

        [StringLength(10)]
        public string Presupuesto { get; set; }

        [StringLength(20)]
        public string ItemCodeSustrato { get; set; }

        [StringLength(200)]
        public string Sustrato { get; set; }

        public int? Calibre { get; set; }

        public int? Gramaje { get; set; }

        public int? AnchoBobina { get; set; }

        public int? AnchoPliego { get; set; }

        public int? LargoPliego { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Paginas { get; set; }

        public int? Montaje { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Pliegos { get; set; }

        public int? IdLinea { get; set; }

        public int? IdDoc { get; set; }

        public int? CantidadMP { get; set; }

        public int? CantidadPT { get; set; }

        [StringLength(30)]
        public string Estatus { get; set; }

        [StringLength(4)]
        public string UoM { get; set; }

        public int ID { get; set; }

        public DateTime? FechaHora { get; set; }

        [StringLength(15)]
        public string CardName { get; set; }
    }
}
