namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFamiliaToDetailPipeline : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailPipeline", "Familia", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetailPipeline", "Familia");
        }
    }
}
