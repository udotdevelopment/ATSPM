namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApproachToPhasePedAgg : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.PhasePedAggregations");
            AddColumn("dbo.PhasePedAggregations", "ApproachId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.PhasePedAggregations", new[] { "BinStartTime", "SignalId", "ApproachId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.PhasePedAggregations");
            DropColumn("dbo.PhasePedAggregations", "ApproachId");
            AddPrimaryKey("dbo.PhasePedAggregations", new[] { "BinStartTime", "SignalId", "PhaseNumber" });
        }
    }
}
