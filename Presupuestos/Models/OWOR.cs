namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OWOR")]
    public partial class OWOR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DocEntry { get; set; }

        public int DocNum { get; set; }

        public short? Series { get; set; }

        [StringLength(20)]
        public string ItemCode { get; set; }

        [StringLength(1)]
        public string Status { get; set; }

        [StringLength(1)]
        public string Type { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PlannedQty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CmpltQty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? RjctQty { get; set; }

        public DateTime? PostDate { get; set; }

        public DateTime? DueDate { get; set; }

        public int? OriginAbs { get; set; }

        public int? OriginNum { get; set; }

        [StringLength(1)]
        public string OriginType { get; set; }

        public short? UserSign { get; set; }

        [StringLength(254)]
        public string Comments { get; set; }

        public DateTime? CloseDate { get; set; }

        public DateTime? RlsDate { get; set; }

        [StringLength(15)]
        public string CardCode { get; set; }

        [StringLength(8)]
        public string Warehouse { get; set; }

        [StringLength(20)]
        public string Uom { get; set; }

        public int? LineDirty { get; set; }

        public int? WOR1Count { get; set; }

        [StringLength(50)]
        public string JrnlMemo { get; set; }

        public int? TransId { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(1)]
        public string Printed { get; set; }

        [StringLength(8)]
        public string OcrCode { get; set; }

        [Required]
        [StringLength(10)]
        public string PIndicator { get; set; }

        [StringLength(8)]
        public string OcrCode2 { get; set; }

        [StringLength(8)]
        public string OcrCode3 { get; set; }

        [StringLength(8)]
        public string OcrCode4 { get; set; }

        [StringLength(8)]
        public string OcrCode5 { get; set; }

        public short? SeqCode { get; set; }

        public int? Serial { get; set; }

        [StringLength(3)]
        public string SeriesStr { get; set; }

        [StringLength(3)]
        public string SubStr { get; set; }

        public short? LogInstanc { get; set; }

        public short? UserSign2 { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(20)]
        public string Project { get; set; }

        [Column(TypeName = "ntext")]
        public string U_Cotizacion { get; set; }

        [Column(TypeName = "ntext")]
        public string U_DocAnexo { get; set; }

        public string U_Consecutivo { get; set; }

        public string U_Sufijo { get; set; }

        public string U_Tipo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? U_NoTiro { get; set; }

        public string U_TipoTinta { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? U_NoRetiro { get; set; }

        public string U_TipoRetiro { get; set; }

        public string U_Barniz { get; set; }

        public int? U_Anilox { get; set; }

        [Required]
        public string U_DirFibra { get; set; }

        public string U_Sustrato { get; set; }

        public string U_Calibre { get; set; }

        public string U_Ancho { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? U_UnidadesMontaje { get; set; }

        public string U_MontajeA { get; set; }

        public string U_MontajeB { get; set; }

        public string U_Empaque { get; set; }

        public string U_ConFajilla { get; set; }

        public string U_Exportacion { get; set; }

        public string U_Conformidad { get; set; }

        public string U_AcabadosEsp { get; set; }

        public string U_UV { get; set; }

        [Required]
        public string U_Estampado { get; set; }

        [Required]
        public string U_VentanaPlas { get; set; }

        [Required]
        public string U_Laminado { get; set; }

        [Required]
        public string U_PegadoMan { get; set; }

        public string U_Observaciones { get; set; }

        public string U_Status { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? U_TotalPresup { get; set; }

        public DateTime? U_FechaImpresion { get; set; }

        public DateTime? U_FechaFin { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? U_Metros { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? U_Margen { get; set; }

        [Required]
        public string U_PermitirExceso { get; set; }

        public string U_Observaciones2 { get; set; }

        public string U_DetalleFacturacion { get; set; }

        public string U_Tiraje { get; set; }

        public string U_Monteje { get; set; }

        [Required]
        public string U_OrdenProduccionMet { get; set; }

        public string U_Sincronizado { get; set; }

        public string U_DetallesConversion { get; set; }

        [Required]
        public string U_StatusNuevo { get; set; }
    }
}
