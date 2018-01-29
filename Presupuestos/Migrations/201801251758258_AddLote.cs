namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailPipeline", "Lote", c => c.String(maxLength: 30));
            AddColumn("dbo.DetailPipelinePruebas", "Lote", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetailPipelinePruebas", "Lote");
            DropColumn("dbo.DetailPipeline", "Lote");
        }
    }
}
