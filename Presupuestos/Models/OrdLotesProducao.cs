namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrdLotesProducao")]
    public partial class OrdLotesProducao
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(14)]
        public string NumOrdem { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string IdLote { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(14)]
        public string IdComponente { get; set; }

        public int? Quantidade { get; set; }

        public DateTime DataTermino { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CMateriais { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CTransformacao { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CTerceiros { get; set; }

        [StringLength(20)]
        public string CodProduto { get; set; }

        public int? UnidadeCalculo { get; set; }

        public double? FatorConvParaUnidade { get; set; }
    }
}
