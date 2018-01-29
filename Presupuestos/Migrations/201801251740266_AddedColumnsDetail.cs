namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedColumnsDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailPipeline", "kg", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.DetailPipeline", "cantidad", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.DetailPipeline", "Quantidade", c => c.Int());
            AddColumn("dbo.DetailPipeline", "NumOrdem", c => c.String(maxLength: 12));
            AddColumn("dbo.DetailPipelinePruebas", "kg", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.DetailPipelinePruebas", "cantidad", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.DetailPipelinePruebas", "Quantidade", c => c.Int());
            AddColumn("dbo.DetailPipelinePruebas", "NumOrdem", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetailPipelinePruebas", "NumOrdem");
            DropColumn("dbo.DetailPipelinePruebas", "Quantidade");
            DropColumn("dbo.DetailPipelinePruebas", "cantidad");
            DropColumn("dbo.DetailPipelinePruebas", "kg");
            DropColumn("dbo.DetailPipeline", "NumOrdem");
            DropColumn("dbo.DetailPipeline", "Quantidade");
            DropColumn("dbo.DetailPipeline", "cantidad");
            DropColumn("dbo.DetailPipeline", "kg");
        }
    }
}
