namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class A_Vista_Papel
    {
        [StringLength(12)]
        public string NumeroOrden { get; set; }

        [StringLength(60)]
        public string Cliente { get; set; }

        [StringLength(105)]
        public string Proyecto { get; set; }

        [StringLength(20)]
        public string Cod_Papel_Etiqueta { get; set; }

        [StringLength(100)]
        public string Nombre_Papel_Etiq { get; set; }

        public double? Cantidad { get; set; }

        [StringLength(8)]
        public string UMedida { get; set; }

        public double? Pliegos { get; set; }

        public int? Gramaje { get; set; }

        public int? Largo { get; set; }

        public int? Ancho { get; set; }

        public DateTime? FechaEntrega { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }
    }
}
