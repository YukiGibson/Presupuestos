namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DetailPipelineVentas
    {
        public int ID { get; set; }

        public int Sesion { get; set; }

        public byte MesSesion { get; set; }

        public short YearSesion { get; set; }

        [Required]
        [StringLength(50)]
        public string Pipeline { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Ejecutivo")]
        public string Vendedor { get; set; }

        public byte Mes { get; set; }

        public short Año { get; set; }

        [Display(Name = "Mes Diferencia")]
        public byte MesesDiferencia { get; set; }

        [Required]
        [StringLength(10)]
        public string Presupuesto { get; set; }

        [StringLength(20)]
        public string OP { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Producto")]
        public string TipoProducto { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:0,0.##}")]
        public decimal CantidadTotal { get; set; }

        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:0,0.##}")]
        public decimal Cantidad { get; set; }

        [Required]
        [StringLength(150)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(50)]
        public string Cliente { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Por Facturar")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0.##}")]
        public decimal PorFacturar { get; set; }

        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0.##}")]
        public decimal Costo { get; set; }

        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0.##}")]
        public decimal Rentabilidad { get; set; }

        [Display(Name = "Prob Venta")]
        public byte ProbabilidadVenta { get; set; }

        [StringLength(10)]
        public string Estimado { get; set; }

        public double Porcentaje { get; set; }

        public string Color { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? FechaSesion { get; set; }
    }
}
