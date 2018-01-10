namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedcolumnPorcentaje : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailPipelineVentas", "Porcentaje", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetailPipelineVentas", "Porcentaje");
        }
    }
}
