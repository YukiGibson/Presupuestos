namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrcHdr")]
    public partial class OrcHdr
    {
        [Key]
        public double Objid { get; set; }

        public double? Objid_PedidoVenda { get; set; }

        [StringLength(12)]
        public string NumOrcamento { get; set; }

        public DateTime? DtEmissao { get; set; }

        public double? Objid_PedidoVendaItem { get; set; }

        [StringLength(8)]
        public string CodPlanta { get; set; }

        [StringLength(100)]
        public string Titulo { get; set; }

        [StringLength(32)]
        public string CodGrupoRemun { get; set; }

        public int? Situacao { get; set; }

        public int? CodCliente { get; set; }

        public double? ObjidCliente { get; set; }

        [StringLength(500)]
        public string Observacoes { get; set; }

        [StringLength(32)]
        public string Contato { get; set; }

        [StringLength(32)]
        public string CodVendedor { get; set; }

        public int? Version { get; set; }

        public double? ObjidContato { get; set; }

        [StringLength(100)]
        public string EMail { get; set; }

        [StringLength(60)]
        public string NomeCliente { get; set; }

        [StringLength(32)]
        public string CodUsuarioInc { get; set; }

        [StringLength(32)]
        public string CodTipoProduto { get; set; }

        [StringLength(40)]
        public string Telefone { get; set; }

        [StringLength(40)]
        public string Fax { get; set; }

        public DateTime? DtInclusao { get; set; }

        [StringLength(30)]
        public string CondPagto { get; set; }

        public int? CodSacado { get; set; }

        public DateTime? DtBaseCustos { get; set; }

        [StringLength(12)]
        public string NumOrdem { get; set; }

        public DateTime? DtAlteracao { get; set; }

        [StringLength(32)]
        public string CodUsuarioAlt { get; set; }

        public int? QtdOpcoes { get; set; }

        public int? QtdQuantidades { get; set; }

        public double? PrecoMedio { get; set; }

        public double? SPreco { get; set; }

        public double? SCustosMat { get; set; }

        public double? SCustosMO { get; set; }

        public double? SCustosTerc { get; set; }

        public double? SCustosImpostos { get; set; }

        public double? SCustosComissoes { get; set; }

        public double? SCustosFin { get; set; }

        public double? SCustosVenOutros { get; set; }

        [StringLength(8)]
        public string CodFechamento { get; set; }

        [StringLength(255)]
        public string Obs { get; set; }

        public int? ProbVenda { get; set; }

        public double? SQuantidade { get; set; }

        public double? QtdMedia { get; set; }

        public double? PercTotCustoVenda { get; set; }

        public int? MetCalcMargemFinal { get; set; }

        public int? MetCalcMargemConta { get; set; }

        public int? IdMoeda { get; set; }

        public DateTime? DtRefMoeda { get; set; }

        public double? IdSincOrc { get; set; }

        public double? CodAgencia { get; set; }

        public int? CodCancelamento { get; set; }

        [StringLength(255)]
        public string ObsCancelamento { get; set; }

        [StringLength(20)]
        public string CodConcorrente { get; set; }

        public DateTime? DtCancelamento { get; set; }

        public DateTime? DtLibProducao { get; set; }

        public int? IndUsoTbPreco { get; set; }

        [StringLength(60)]
        public string CodGrupoComissao { get; set; }

        public DateTime? DtValidade { get; set; }

        public DateTime? DtRefACR { get; set; }

        [StringLength(12)]
        public string NumOrcOriginal { get; set; }

        [StringLength(1)]
        public string EhOrcAlternativo { get; set; }

        [StringLength(1)]
        public string UnitXQtdIgualTotal { get; set; }

        [StringLength(13)]
        public string NumPedidoVenda { get; set; }

        [StringLength(150)]
        public string TituloLongo { get; set; }

        public int? NumCasasDecimais { get; set; }

        [StringLength(60)]
        public string NomeAgencia { get; set; }

        public double? ObjIDProjeto { get; set; }

        public int? UnidadeCalculo { get; set; }

        public double? ObjIDTask { get; set; }

        [StringLength(32)]
        public string CodUsuarioAprovInt { get; set; }

        public DateTime? DtAprovacaoInt { get; set; }

        [StringLength(32)]
        public string CodUsuarioSolicAprov { get; set; }

        [StringLength(255)]
        public string ObsRecusado { get; set; }

        public int? SegmentoProduto { get; set; }

        public double? ObjIDUnidadeNegocio { get; set; }

        [StringLength(20)]
        public string CodRoteiro { get; set; }

        public int? RFQNumber { get; set; }

        public int? gerapedidovenda { get; set; }

        [StringLength(1)]
        public string OrigemOrc { get; set; }

        [StringLength(8)]
        public string usr_incoterm { get; set; }

        [StringLength(4)]
        public string usr_int { get; set; }

        [StringLength(16)]
        public string usr_tipo { get; set; }

        [StringLength(16)]
        public string usr_periodo { get; set; }

        [StringLength(20)]
        public string usr_codprod { get; set; }

        [StringLength(50)]
        public string usr_oc { get; set; }

        public DateTime? usr_FechaEntrega1 { get; set; }

        public int? usr_CantidadEntrega1 { get; set; }

        public DateTime? usr_FechaEntrega2 { get; set; }

        public int? usr_CantidadEntrega2 { get; set; }

        public DateTime? usr_FechaEntrega3 { get; set; }

        public int? usr_CantidadEntrega3 { get; set; }

        public DateTime? usr_FechaEntrega4 { get; set; }

        public int? usr_CantidadEntrega4 { get; set; }

        public DateTime? usr_FechaEntrega5 { get; set; }

        public int? usr_CantidadEntrega5 { get; set; }

        [StringLength(50)]
        public string usr_pedv { get; set; }

        public double? usr_variacion { get; set; }

        public int? usr_var { get; set; }

        public DateTime? DtRefEstocagem { get; set; }

        [StringLength(12)]
        public string CodEstruturaOrigem { get; set; }
    }
}
