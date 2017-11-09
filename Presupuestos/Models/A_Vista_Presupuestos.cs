namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class A_Vista_Presupuestos
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [StringLength(12)]
        public string Presupuesto { get; set; }

        public DateTime? FechaEmisión { get; set; }

        [StringLength(100)]
        public string Título { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(60)]
        public string Cliente { get; set; }

        [StringLength(50)]
        public string TipoProducto { get; set; }

        [StringLength(30)]
        public string CondPago { get; set; }

        [StringLength(12)]
        public string OP { get; set; }

        public double? PrecioTotal { get; set; }

        public double? CostoMateriales { get; set; }

        public double? CostoMO { get; set; }

        public double? CostoTerceros { get; set; }

        public double? CostoComisiones { get; set; }

        public double? CostoFinanciero { get; set; }

        public int? ProbabilidadVenta { get; set; }

        public double? Cantidad { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string Moneda { get; set; }

        public DateTime? usr_FechaEntrega1 { get; set; }

        public int? usr_CantidadEntrega1 { get; set; }

        public DateTime? usr_FechaEntrega2 { get; set; }

        public int? usr_CantidadEntrega2 { get; set; }

        public DateTime? usr_FechaEntrega3 { get; set; }

        public int? usr_CantidadEntrega3 { get; set; }

        [StringLength(40)]
        public string Vendedor { get; set; }

        [StringLength(14)]
        public string T1 { get; set; }

        public int? E1 { get; set; }

        public DateTime? F1 { get; set; }

        [StringLength(14)]
        public string T2 { get; set; }

        public int? E2 { get; set; }

        public DateTime? F2 { get; set; }

        [StringLength(14)]
        public string T3 { get; set; }

        public int? E3 { get; set; }

        public DateTime? F3 { get; set; }

        [StringLength(14)]
        public string T4 { get; set; }

        public int? E4 { get; set; }

        public DateTime? F4 { get; set; }

        [StringLength(14)]
        public string T5 { get; set; }

        public int? E5 { get; set; }

        public DateTime? F5 { get; set; }

        [StringLength(14)]
        public string T6 { get; set; }

        public int? E6 { get; set; }

        public DateTime? F6 { get; set; }

        [StringLength(14)]
        public string T7 { get; set; }

        public int? E7 { get; set; }

        public DateTime? F7 { get; set; }

        [StringLength(14)]
        public string T8 { get; set; }

        public int? E8 { get; set; }

        public DateTime? F8 { get; set; }

        [StringLength(14)]
        public string T9 { get; set; }

        public int? E9 { get; set; }

        public DateTime? F9 { get; set; }

        [StringLength(14)]
        public string T10 { get; set; }

        public int? E10 { get; set; }

        public DateTime? F10 { get; set; }
    }
}
