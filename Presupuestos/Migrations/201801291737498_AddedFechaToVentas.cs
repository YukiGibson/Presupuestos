namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFechaToVentas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailPipelineVentas", "FechaSesion", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetailPipelineVentas", "FechaSesion");
        }
    }
}
