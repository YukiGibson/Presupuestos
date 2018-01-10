namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Lenght : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DetailPipelineVentas", "Estimado", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DetailPipelineVentas", "Estimado", c => c.String());
        }
    }
}
