namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class A_Vista_OConversion_Reserva
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [StringLength(12)]
        public string Presupuesto { get; set; }

        public DateTime? Emision { get; set; }

        [StringLength(100)]
        public string Titulo { get; set; }

        [StringLength(60)]
        public string Cliente { get; set; }

        public int? ProbVenta { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(30)]
        public string Material { get; set; }

        [StringLength(90)]
        public string Descripcion { get; set; }

        [StringLength(20)]
        public string ComplementoDesc { get; set; }

        public double? Cantidad { get; set; }

        [StringLength(8)]
        public string UM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Valor { get; set; }

        [StringLength(1)]
        public string Suministrado { get; set; }

        [StringLength(30)]
        public string Lote { get; set; }

        [StringLength(32)]
        public string IDProcessoOrigem { get; set; }

        public double? Gramaje { get; set; }

        public double? Ancho { get; set; }

        public double? Largo { get; set; }

        [StringLength(12)]
        public string NumOrdem { get; set; }

        public DateTime? usr_FechaEntrega1 { get; set; }

        public int? usr_CantidadEntrega1 { get; set; }

        public DateTime? usr_FechaEntrega2 { get; set; }

        public int? usr_CantidadEntrega2 { get; set; }

        public DateTime? usr_FechaEntrega3 { get; set; }

        public int? usr_CantidadEntrega3 { get; set; }

        public double? Expr1 { get; set; }

        [StringLength(20)]
        public string Apelido { get; set; }

        [StringLength(50)]
        public string Recurso { get; set; }

        public DateTime? FechaRequerida { get; set; }

        [StringLength(12)]
        public string CodEstructura { get; set; }
    }
}
