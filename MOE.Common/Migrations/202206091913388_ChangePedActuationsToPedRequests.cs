namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePedActuationsToPedRequests : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PhasePedAggregations", "PedRequests", c => c.Int(nullable: false));
            DropColumn("dbo.PhasePedAggregations", "PedActuations");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PhasePedAggregations", "PedActuations", c => c.Int(nullable: false));
            DropColumn("dbo.PhasePedAggregations", "PedRequests");
        }
    }
}
