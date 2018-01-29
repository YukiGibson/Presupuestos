namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProbVenta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailPipeline", "ProbVenta", c => c.Int(nullable: false));
            AddColumn("dbo.DetailPipelinePruebas", "ProbVenta", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetailPipelinePruebas", "ProbVenta");
            DropColumn("dbo.DetailPipeline", "ProbVenta");
        }
    }
}
