namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vista_SAP2
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(255)]
        public string Recurso { get; set; }

        [StringLength(255)]
        public string Orden_Produccion_Metrics { get; set; }

        [StringLength(255)]
        public string Orden_Produccion_Vadi { get; set; }

        [StringLength(255)]
        public string Tipo_Recurso { get; set; }

        [StringLength(255)]
        public string Cliente { get; set; }

        [StringLength(255)]
        public string Codigo_Producto { get; set; }

        [StringLength(255)]
        public string Producto { get; set; }

        [StringLength(255)]
        public string Codigo_Sustrato { get; set; }

        [StringLength(255)]
        public string Sustrato { get; set; }

        public double? Ancho { get; set; }

        public double? Largo { get; set; }

        public double? Ancho_Pulgadas { get; set; }

        public double? Largo_Pulgadas { get; set; }

        [StringLength(255)]
        public string Fecha_Requerida { get; set; }

        [StringLength(255)]
        public string Pliego { get; set; }

        [StringLength(255)]
        public string Pliego_Impresion { get; set; }

        public int? Tiros_Por_Hoja { get; set; }

        public double? Tiros { get; set; }

        [StringLength(255)]
        public string Solicitante { get; set; }

        [StringLength(255)]
        public string Empleado_Solicitante { get; set; }

        [StringLength(255)]
        public string Estado_Produccion { get; set; }

        [StringLength(255)]
        public string Estado_Envio { get; set; }

        public double? Cantidad_Kilos { get; set; }

        public double? Kilos_Bobina_Utilizados { get; set; }

        public double? Total_Kilos { get; set; }

        public double? Kilos_Bobina_Faltantes { get; set; }

        public double? Ultima_Cantidad_ProducidaK { get; set; }

        public double? Desviacion_Kilos { get; set; }

        public double? Tarifa_Conversion { get; set; }

        public double? Corte_Parcial_Bobina { get; set; }

        public double? Cantidad_Pliegos { get; set; }

        public double? Produccion { get; set; }

        public double? Producido_Pliegos { get; set; }

        public double? Faltante_Pliegos_Producir { get; set; }

        public double? Ultima_Cantidad_Producida { get; set; }

        public double? Desviacion_Producido { get; set; }

        public double? Enviar_Kilos { get; set; }

        public double? Faltante_Kilos_Enviados { get; set; }

        public double? Ultima_Kilos_Enviados { get; set; }

        public double? Desviacion_Kilos_Enviados { get; set; }

        public double? Total_Kilos_Enviados { get; set; }

        public double? Enviar_Pliegos { get; set; }

        public double? Faltante_Pliegos_Enviados { get; set; }

        public double? Ultima_Pliegos_Enviados { get; set; }

        public double? Desviacion_Pliegos_Enviados { get; set; }

        public double? Total_Pliegos_Enviados { get; set; }

        public double? Ultima_Cantidad_Devuelta { get; set; }

        public double? Total_Devuelto { get; set; }

        [StringLength(255)]
        public string Fecha_Ultima_Entrega { get; set; }

        [StringLength(255)]
        public string Fecha_Ultima_Modificacion { get; set; }

        public string Lotes { get; set; }

        public int? Id_Orden_Reestablecida { get; set; }

        [StringLength(255)]
        public string Fecha_Produccion_Vadi { get; set; }

        [StringLength(255)]
        public string Estado_Produccion_Vadi { get; set; }

        [StringLength(253)]
        public string OC_SAP { get; set; }
    }
}
