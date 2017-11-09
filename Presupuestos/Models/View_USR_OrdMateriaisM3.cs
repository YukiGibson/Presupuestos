namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_USR_OrdMateriaisM3
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(14)]
        public string NumOrdem { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdAtivUso { get; set; }

        [StringLength(20)]
        public string CodItem { get; set; }

        [StringLength(12)]
        public string IdProcessoUso { get; set; }

        [StringLength(60)]
        public string DescMaterial { get; set; }

        [StringLength(4)]
        public string Tipo { get; set; }

        public double? Quantidade { get; set; }

        [StringLength(4)]
        public string Unidade { get; set; }

        public int? QtdPlanejada { get; set; }

        public int? QtdRepeticoes { get; set; }

        public int? QtdEntradas { get; set; }

        public double? QuantidadeUnid2 { get; set; }

        [StringLength(4)]
        public string Unidade2 { get; set; }

        public double? QtdLiquida { get; set; }

        public double? FatorUnidades { get; set; }
    }
}
