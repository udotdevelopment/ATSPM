namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInAggregationTablsV3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ApproachSpeedAggregations");
            DropPrimaryKey("dbo.ApproachSplitFailAggregations");
            DropPrimaryKey("dbo.DetectorAggregations");
            DropPrimaryKey("dbo.PhaseTerminationAggregations");
            DropPrimaryKey("dbo.PreemptionAggregations");
            DropPrimaryKey("dbo.PriorityAggregations");
            DropPrimaryKey("dbo.SignalEventCountAggregations");
            AddColumn("dbo.ApproachCycleAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.ApproachSpeedAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.ApproachSplitFailAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.DetectorAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ApproachSpeedAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase", "Id" });
            AddPrimaryKey("dbo.ApproachSplitFailAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase", "Id" });
            AddPrimaryKey("dbo.DetectorAggregations", new[] { "BinStartTime", "DetectorPrimaryId", "Id" });
            AddPrimaryKey("dbo.PhaseTerminationAggregations", new[] { "BinStartTime", "SignalId", "PhaseNumber", "Id" });
            AddPrimaryKey("dbo.PreemptionAggregations", new[] { "BinStartTime", "SignalId", "VersionId", "Id" });
            AddPrimaryKey("dbo.PriorityAggregations", new[] { "BinStartTime", "SignalID", "VersionId", "Id" });
            AddPrimaryKey("dbo.SignalEventCountAggregations", new[] { "BinStartTime", "SignalId", "Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.SignalEventCountAggregations");
            DropPrimaryKey("dbo.PriorityAggregations");
            DropPrimaryKey("dbo.PreemptionAggregations");
            DropPrimaryKey("dbo.PhaseTerminationAggregations");
            DropPrimaryKey("dbo.DetectorAggregations");
            DropPrimaryKey("dbo.ApproachSplitFailAggregations");
            DropPrimaryKey("dbo.ApproachSpeedAggregations");
            DropColumn("dbo.DetectorAggregations", "Id");
            DropColumn("dbo.ApproachSplitFailAggregations", "Id");
            DropColumn("dbo.ApproachSpeedAggregations", "Id");
            DropColumn("dbo.ApproachCycleAggregations", "Id");
            AddPrimaryKey("dbo.SignalEventCountAggregations", new[] { "BinStartTime", "Id" });
            AddPrimaryKey("dbo.PriorityAggregations", new[] { "BinStartTime", "VersionId", "Id" });
            AddPrimaryKey("dbo.PreemptionAggregations", new[] { "BinStartTime", "Id" });
            AddPrimaryKey("dbo.PhaseTerminationAggregations", new[] { "BinStartTime", "PhaseNumber", "Id" });
            AddPrimaryKey("dbo.DetectorAggregations", new[] { "BinStartTime", "DetectorPrimaryId" });
            AddPrimaryKey("dbo.ApproachSplitFailAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase" });
            AddPrimaryKey("dbo.ApproachSpeedAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase" });
        }
    }
}
