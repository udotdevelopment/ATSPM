namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAggregationIds : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ApproachCycleAggregations");
            DropColumn("dbo.ApproachCycleAggregations", "Id");
            AddPrimaryKey("dbo.ApproachCycleAggregations", new[] { "BinStartTime", "ApproachID" });

            DropPrimaryKey("dbo.ApproachEventCountAggregations");
            DropColumn("dbo.ApproachEventCountAggregations", "Id");
            AddPrimaryKey("dbo.ApproachEventCountAggregations", new[] { "BinStartTime", "ApproachID" });

            DropPrimaryKey("dbo.ApproachPcdAggregations");
            DropColumn("dbo.ApproachPcdAggregations", "Id");
            AddPrimaryKey("dbo.ApproachPcdAggregations", new[] { "BinStartTime", "ApproachID" });

            DropPrimaryKey("dbo.ApproachSpeedAggregations");
            DropColumn("dbo.ApproachSpeedAggregations", "Id");
            AddPrimaryKey("dbo.ApproachSpeedAggregations", new[] { "BinStartTime", "ApproachID" });

            DropPrimaryKey("dbo.ApproachSplitFailAggregations");
            DropColumn("dbo.ApproachSplitFailAggregations", "Id");
            AddPrimaryKey("dbo.ApproachSplitFailAggregations", new[] { "BinStartTime", "ApproachID" });

            DropPrimaryKey("dbo.ApproachYellowRedActivationAggregations");
            DropColumn("dbo.ApproachYellowRedActivationAggregations", "Id");
            AddPrimaryKey("dbo.ApproachYellowRedActivationAggregations", new[] { "BinStartTime", "ApproachID" });

            DropPrimaryKey("dbo.DetectorAggregations");
            DropColumn("dbo.DetectorAggregations", "Id");
            AddPrimaryKey("dbo.DetectorAggregations", new[] { "BinStartTime", "DetectorPrimaryId" });

            DropPrimaryKey("dbo.DetectorEventCountAggregations");
            DropColumn("dbo.DetectorEventCountAggregations", "Id");
            DropColumn("dbo.DetectorEventCountAggregations", "DetectorId");
            AddColumn("dbo.DetectorEventCountAggregations", "DetectorId", s => s.String(false, 10));
            AddPrimaryKey("dbo.DetectorEventCountAggregations", new[] { "BinStartTime", "DetectorId" });

            DropPrimaryKey("dbo.PhasePedAggregations");
            DropColumn("dbo.PhasePedAggregations", "Id");
            DropColumn("dbo.PhasePedAggregations", "SignalId");
            AddColumn("dbo.PhasePedAggregations", "SignalId", s => s.String(false, 10));
            AddPrimaryKey("dbo.PhasePedAggregations", new[] { "BinStartTime", "SignalID" });

            DropPrimaryKey("dbo.PhaseTerminationAggregations");
            DropColumn("dbo.PhaseTerminationAggregations", "Id");
            DropColumn("dbo.PhaseTerminationAggregations", "SignalId");
            AddColumn("dbo.PhaseTerminationAggregations", "SignalId", s => s.String(false, 10));
            AddPrimaryKey("dbo.PhaseTerminationAggregations", new[] { "BinStartTime", "SignalId" });

            DropPrimaryKey("dbo.PreemptionAggregations");
            DropColumn("dbo.PreemptionAggregations", "Id");
            DropColumn("dbo.PreemptionAggregations", "SignalId");
            AddColumn("dbo.PreemptionAggregations", "SignalId", s => s.String(false, 10));
            AddPrimaryKey("dbo.PreemptionAggregations", new[] { "BinStartTime", "SignalId" });

            DropPrimaryKey("dbo.PriorityAggregations");
            DropColumn("dbo.PriorityAggregations", "Id");
            DropColumn("dbo.PriorityAggregations", "SignalId");
            AddColumn("dbo.PriorityAggregations", "SignalId", s => s.String(false, 10));
            AddPrimaryKey("dbo.PriorityAggregations", new[] { "BinStartTime", "SignalId" });

            DropPrimaryKey("dbo.SignalEventCountAggregations");
            DropColumn("dbo.SignalEventCountAggregations", "Id");
            DropColumn("dbo.SignalEventCountAggregations", "SignalId");
            AddColumn("dbo.SignalEventCountAggregations", "SignalId", s => s.String(false, 10));
            AddPrimaryKey("dbo.SignalEventCountAggregations", new[] { "BinStartTime", "SignalId" });
        }

        public override void Down()
        {
            AddColumn("dbo.ApproachCycleAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.ApproachCycleAggregations");
            AddPrimaryKey("dbo.ApproachCycleAggregations", "Id");

            AddColumn("dbo.ApproachEventCountAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.ApproachEventCountAggregations");
            AddPrimaryKey("dbo.ApproachEventCountAggregations", "Id");

            AddColumn("dbo. ApproachCycleAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo. ApproachCycleAggregations");
            AddPrimaryKey("dbo. ApproachCycleAggregations", "Id");

            AddColumn("dbo.ApproachPcdAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.ApproachPcdAggregations");
            AddPrimaryKey("dbo.ApproachPcdAggregations", "Id");

            AddColumn("dbo.ApproachSpeedAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.ApproachSpeedAggregations");
            AddPrimaryKey("dbo.ApproachSpeedAggregations", "Id");
            
            AddColumn("dbo.ApproachSplitFailAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.ApproachSplitFailAggregations");
            AddPrimaryKey("dbo.ApproachSplitFailAggregations", "Id");

            AddColumn("dbo.ApproachYellowRedActivationAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.ApproachYellowRedActivationAggregations");
            AddPrimaryKey("dbo.ApproachYellowRedActivationAggregations", "Id");

            AddColumn("dbo.DetectorEventCountAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.DetectorEventCountAggregations");
            AddPrimaryKey("dbo.DetectorEventCountAggregations", "Id");

            AddColumn("dbo.PhasePedAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.PhasePedAggregations");
            AddPrimaryKey("dbo.PhasePedAggregations", "Id");

            AddColumn("dbo.PhaseTerminationAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.PhaseTerminationAggregations");
            AddPrimaryKey("dbo.PhaseTerminationAggregations", "Id");

            AddColumn("dbo.PreemptionAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.PreemptionAggregations");
            AddPrimaryKey("dbo.PreemptionAggregations", "Id");

            AddColumn("dbo.PriorityAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.PriorityAggregations");
            AddPrimaryKey("dbo.PriorityAggregations", "Id");

            AddColumn("dbo.SignalEventCountAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.SignalEventCountAggregations");
            AddPrimaryKey("dbo.SignalEventCountAggregations", "Id");
        }
    }
}
