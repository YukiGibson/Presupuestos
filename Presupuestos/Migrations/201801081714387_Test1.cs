namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DetailPipelineVentas", "Vendedor", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.DetailPipelineVentas", "TipoProducto", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DetailPipelineVentas", "TipoProducto", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.DetailPipelineVentas", "Vendedor", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
    }
}
