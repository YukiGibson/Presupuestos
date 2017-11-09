namespace Presupuestos.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Presupuestos.Models;

    public partial class ProjectionContext : DbContext
    {
        public ProjectionContext()
            : base("name=ProjectionContext")
        {
        }

        public virtual DbSet<DetailPipeline> DetailPipeline { get; set; }
        public virtual DbSet<DetailPipelineEntregas> DetailPipelineEntregas { get; set; }
        public virtual DbSet<DetailPipelinePruebas> DetailPipelinePruebas { get; set; }
        public virtual DbSet<A_Vista_OConversion> A_Vista_OConversion { get; set; }
        public virtual DbSet<A_Vista_OConversion_Reserva> A_Vista_OConversion_Reserva { get; set; }
        public virtual DbSet<A_Vista_Papel> A_Vista_Papel { get; set; }
        public virtual DbSet<A_Vista_Presupuestos> A_Vista_Presupuestos { get; set; }
        public virtual DbSet<View_USR_OrdMateriaisM3> View_USR_OrdMateriaisM3 { get; set; }
        public virtual DbSet<Vista_SAP2> Vista_SAP2 { get; set; }
        public virtual DbSet<DetailPipelineHistorico> DetailPipelineHistorico { get; set; }
        public virtual DbSet<DetailPipelineEntregasPruebas> DetailPipelineEntregasPruebas { get; set; }
        public virtual DbSet<HeaderPipeline> HeaderPipeline { get; set; }
        public virtual DbSet<OrcHdr> OrcHdr { get; set; }
        public virtual DbSet<Vista_SAP> Vista_SAP { get; set; }
        public virtual DbSet<EstrProcessos> EstrProcessos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.Presupuesto)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.Cliente)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.Material)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.ComplementoDesc)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.UM)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.Valor)
                .HasPrecision(20, 5);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.Suministrado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.Lote)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.IDProcessoOrigem)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.NumOrdem)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.Apelido)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.Recurso)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion>()
                .Property(e => e.CodEstructura)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.Presupuesto)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.Cliente)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.Material)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.ComplementoDesc)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.UM)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.Valor)
                .HasPrecision(20, 5);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.Suministrado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.Lote)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.IDProcessoOrigem)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.NumOrdem)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.Apelido)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.Recurso)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_OConversion_Reserva>()
                .Property(e => e.CodEstructura)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Papel>()
                .Property(e => e.NumeroOrden)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Papel>()
                .Property(e => e.Cliente)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Papel>()
                .Property(e => e.Proyecto)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Papel>()
                .Property(e => e.Cod_Papel_Etiqueta)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Papel>()
                .Property(e => e.Nombre_Papel_Etiq)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Papel>()
                .Property(e => e.UMedida)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.Presupuesto)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.Título)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.Cliente)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.TipoProducto)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.CondPago)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.OP)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.Moneda)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.Vendedor)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.T1)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.T2)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.T3)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.T4)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.T5)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.T6)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.T7)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.T8)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.T9)
                .IsUnicode(false);

            modelBuilder.Entity<A_Vista_Presupuestos>()
                .Property(e => e.T10)
                .IsUnicode(false);

            modelBuilder.Entity<View_USR_OrdMateriaisM3>()
                .Property(e => e.NumOrdem)
                .IsUnicode(false);

            modelBuilder.Entity<View_USR_OrdMateriaisM3>()
                .Property(e => e.CodItem)
                .IsUnicode(false);

            modelBuilder.Entity<View_USR_OrdMateriaisM3>()
                .Property(e => e.IdProcessoUso)
                .IsUnicode(false);

            modelBuilder.Entity<View_USR_OrdMateriaisM3>()
                .Property(e => e.DescMaterial)
                .IsUnicode(false);

            modelBuilder.Entity<View_USR_OrdMateriaisM3>()
                .Property(e => e.Tipo)
                .IsUnicode(false);

            modelBuilder.Entity<View_USR_OrdMateriaisM3>()
                .Property(e => e.Unidade)
                .IsUnicode(false);

            modelBuilder.Entity<View_USR_OrdMateriaisM3>()
                .Property(e => e.Unidade2)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Recurso)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Orden_Produccion_Metrics)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Orden_Produccion_Vadi)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Tipo_Recurso)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Codigo_Producto)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Producto)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Codigo_Sustrato)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Sustrato)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Fecha_Requerida)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Pliego)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Pliego_Impresion)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Solicitante)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Empleado_Solicitante)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Estado_Produccion)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Estado_Envio)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Fecha_Ultima_Entrega)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Fecha_Ultima_Modificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Lotes)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Fecha_Produccion_Vadi)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.Estado_Produccion_Vadi)
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP2>()
                .Property(e => e.OC_SAP)
                .IsUnicode(false);
            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.NumOrcamento)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CodPlanta)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CodGrupoRemun)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.Observacoes)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.Contato)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CodVendedor)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.EMail)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.NomeCliente)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CodUsuarioInc)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CodTipoProduto)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.Telefone)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.Fax)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CondPagto)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.NumOrdem)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CodUsuarioAlt)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CodFechamento)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.Obs)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.ObsCancelamento)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CodConcorrente)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CodGrupoComissao)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.NumOrcOriginal)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.EhOrcAlternativo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.UnitXQtdIgualTotal)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.NumPedidoVenda)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.TituloLongo)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.NomeAgencia)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CodUsuarioAprovInt)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CodUsuarioSolicAprov)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.ObsRecusado)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CodRoteiro)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.OrigemOrc)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.usr_incoterm)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.usr_int)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.usr_tipo)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.usr_periodo)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.usr_codprod)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.usr_oc)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.usr_pedv)
                .IsUnicode(false);

            modelBuilder.Entity<OrcHdr>()
                .Property(e => e.CodEstruturaOrigem)
                .IsUnicode(false);
            modelBuilder.Entity<Vista_SAP>()
               .Property(e => e.Status)
               .IsFixedLength()
               .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP>()
                .Property(e => e.Type)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP>()
                .Property(e => e.PlannedQty)
                .HasPrecision(19, 6);

            modelBuilder.Entity<Vista_SAP>()
                .Property(e => e.CmpltQty)
                .HasPrecision(19, 6);

            modelBuilder.Entity<Vista_SAP>()
                .Property(e => e.RjctQty)
                .HasPrecision(19, 6);

            modelBuilder.Entity<Vista_SAP>()
                .Property(e => e.OriginType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP>()
                .Property(e => e.Printed)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Vista_SAP>()
                .Property(e => e.U_NoTiro)
                .HasPrecision(19, 6);

            modelBuilder.Entity<Vista_SAP>()
                .Property(e => e.U_NoRetiro)
                .HasPrecision(19, 6);

            modelBuilder.Entity<Vista_SAP>()
                .Property(e => e.U_UnidadesMontaje)
                .HasPrecision(19, 6);

            modelBuilder.Entity<Vista_SAP>()
                .Property(e => e.U_TotalPresup)
                .HasPrecision(19, 6);

            modelBuilder.Entity<Vista_SAP>()
                .Property(e => e.U_Metros)
                .HasPrecision(19, 6);

            modelBuilder.Entity<Vista_SAP>()
                .Property(e => e.U_Margen)
                .HasPrecision(19, 6);
            modelBuilder.Entity<EstrProcessos>()
                .Property(e => e.CodEstrutura)
                .IsUnicode(false);

            modelBuilder.Entity<EstrProcessos>()
                .Property(e => e.IdProcesso)
                .IsUnicode(false);

            modelBuilder.Entity<EstrProcessos>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<EstrProcessos>()
                .Property(e => e.DividirPorLote)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EstrProcessos>()
                .Property(e => e.DividirPorRepet)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EstrProcessos>()
                .Property(e => e.PIdClasse)
                .IsUnicode(false);

            modelBuilder.Entity<EstrProcessos>()
                .Property(e => e.PCodModeloTracado)
                .IsUnicode(false);

            modelBuilder.Entity<EstrProcessos>()
                .Property(e => e.PGruposTracado)
                .IsUnicode(false);
        }
    }
}
