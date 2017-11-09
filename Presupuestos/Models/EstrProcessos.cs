namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EstrProcessos
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(12)]
        public string CodEstrutura { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(12)]
        public string IdProcesso { get; set; }

        [Required]
        [StringLength(250)]
        public string Descricao { get; set; }

        public int QtdBase { get; set; }

        public int? QtdRepeticoes { get; set; }

        [StringLength(1)]
        public string DividirPorLote { get; set; }

        [StringLength(1)]
        public string DividirPorRepet { get; set; }

        [StringLength(16)]
        public string PIdClasse { get; set; }

        public int? PQtdPagCad { get; set; }

        public int? PQtdPagTot { get; set; }

        public int? PFmtPagL { get; set; }

        public int? PFmtPagA { get; set; }

        public int? PFmtAbertoL { get; set; }

        public int? PFmtAbertoA { get; set; }

        public int? PFmtCadL { get; set; }

        public int? PFmtCadA { get; set; }

        public int? PFmtFabL { get; set; }

        public int? PFmtFabA { get; set; }

        public int? PFmtCorteL { get; set; }

        public int? PFmtCorteA { get; set; }

        public int? PFmtCorteMaxL { get; set; }

        public int? PFmtCorteMaxA { get; set; }

        public int? PQtdPedacos { get; set; }

        public int? PCadPorGiro { get; set; }

        public int? PCoresF { get; set; }

        public int? PCoresV { get; set; }

        public int? PSistImpr { get; set; }

        public int? PQtdBob { get; set; }

        public double? PGramatura { get; set; }

        public int? PFmtCortePedacoL { get; set; }

        public int? PFmtCortePedacoA { get; set; }

        public int? PFmtCorteMaxPedacoA { get; set; }

        [StringLength(12)]
        public string PCodModeloTracado { get; set; }

        [StringLength(100)]
        public string PGruposTracado { get; set; }

        public int? PFmtCorteMaxPedacoL { get; set; }
    }
}
