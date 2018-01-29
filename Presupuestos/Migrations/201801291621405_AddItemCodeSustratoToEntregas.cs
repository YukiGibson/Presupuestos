namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItemCodeSustratoToEntregas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailPipelineEntregas", "ItemCodeSustrato", c => c.String(maxLength: 40));
            AddColumn("dbo.DetailPipelineEntregasPruebas", "ItemCodeSustrato", c => c.String(maxLength: 40));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetailPipelineEntregasPruebas", "ItemCodeSustrato");
            DropColumn("dbo.DetailPipelineEntregas", "ItemCodeSustrato");
        }
    }
}
