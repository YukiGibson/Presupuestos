namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedestimado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailPipelineVentas", "Estimado", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetailPipelineVentas", "Estimado");
        }
    }
}
