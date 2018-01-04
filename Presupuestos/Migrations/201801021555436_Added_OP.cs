namespace Presupuestos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_OP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailPipelineTotales", "NumeroPresupuesto", c => c.String(maxLength: 13));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetailPipelineTotales", "NumeroPresupuesto");
        }
    }
}
