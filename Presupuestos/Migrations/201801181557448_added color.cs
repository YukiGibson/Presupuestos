namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedcolor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailPipelineVentas", "Color", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetailPipelineVentas", "Color");
        }
    }
}
