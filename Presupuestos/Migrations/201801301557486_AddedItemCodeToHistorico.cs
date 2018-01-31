namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedItemCodeToHistorico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailPipelineHistorico", "ItemCodeSustrato", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetailPipelineHistorico", "ItemCodeSustrato");
        }
    }
}
