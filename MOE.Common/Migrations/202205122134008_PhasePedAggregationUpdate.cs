namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhasePedAggregationUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PhasePedAggregations", "ImputedPedCallsRegistered", c => c.Int(nullable: false));
            AddColumn("dbo.PhasePedAggregations", "UniquePedDetections", c => c.Int(nullable: false));
            AddColumn("dbo.PhasePedAggregations", "PedBeginWalkCount", c => c.Int(nullable: false));
            AddColumn("dbo.PhasePedAggregations", "PedCallsRegisteredCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PhasePedAggregations", "PedCallsRegisteredCount");
            DropColumn("dbo.PhasePedAggregations", "PedBeginWalkCount");
            DropColumn("dbo.PhasePedAggregations", "UniquePedDetections");
            DropColumn("dbo.PhasePedAggregations", "ImputedPedCallsRegistered");
        }
    }
}
