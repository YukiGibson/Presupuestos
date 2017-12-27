namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_new_column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailPipelineTotales", "ItemCodeSustrato", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetailPipelineTotales", "ItemCodeSustrato");
        }
    }
}
