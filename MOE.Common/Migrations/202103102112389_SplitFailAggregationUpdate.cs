namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SplitFailAggregationUpdate : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ApproachSplitFailAggregations");
            AddColumn("dbo.ApproachSplitFailAggregations", "SignalId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.ApproachSplitFailAggregations", "GreenOccupancySum", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "RedOccupancySum", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "GreenTimeSum", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "RedTimeSum", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "Cycles", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "PhaseNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.ApproachPcdAggregations", "TotalDelay", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ApproachSplitFailAggregations", new[] { "BinStartTime", "SignalId", "ApproachId", "IsProtectedPhase" });
            DropColumn("dbo.ApproachSplitFailAggregations", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApproachSplitFailAggregations", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.ApproachSplitFailAggregations");
            AlterColumn("dbo.ApproachPcdAggregations", "TotalDelay", c => c.Double(nullable: false));
            DropColumn("dbo.ApproachSplitFailAggregations", "PhaseNumber");
            DropColumn("dbo.ApproachSplitFailAggregations", "Cycles");
            DropColumn("dbo.ApproachSplitFailAggregations", "RedTimeSum");
            DropColumn("dbo.ApproachSplitFailAggregations", "GreenTimeSum");
            DropColumn("dbo.ApproachSplitFailAggregations", "RedOccupancySum");
            DropColumn("dbo.ApproachSplitFailAggregations", "GreenOccupancySum");
            DropColumn("dbo.ApproachSplitFailAggregations", "SignalId");
            AddPrimaryKey("dbo.ApproachSplitFailAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase", "Id" });
        }
    }
}
