namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VU_ACR_DON_012_OrcPapel
    {
        [StringLength(12)]
        public string NumOrdem { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(1)]
        public string TipoLancamento { get; set; }

        [StringLength(1)]
        public string DebCred { get; set; }

        [StringLength(1)]
        public string Natureza { get; set; }

        [StringLength(30)]
        public string CodSubConta { get; set; }

        public double? totKgCot { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(16)]
        public string CodConta { get; set; }
    }
}
