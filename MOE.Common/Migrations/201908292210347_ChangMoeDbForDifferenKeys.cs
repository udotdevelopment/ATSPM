namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangMoeDbForDifferenKeys : DbMigration
    {
        public override void Up()
        {
            AddPrimaryKey("dbo.SignalEventCountAggregations", "Id");
            AddPrimaryKey("dbo.PriorityAggregations", "Id");
            AddPrimaryKey("dbo.PreemptionAggregations", "Id");
            AddPrimaryKey("dbo.PhaseTerminationAggregations", "Id");
            AddPrimaryKey("dbo.PhasePedAggregations", "Id");
            AddPrimaryKey("dbo.DetectorEventCountAggregations", "Id");
            AddPrimaryKey("dbo.DetectorAggregations", "Id");
            AddPrimaryKey("dbo.ApproachYellowRedActivationAggregations", "Id");
            AddPrimaryKey("dbo.ApproachSplitFailAggregations", "Id");
            AddPrimaryKey("dbo.ApproachSpeedAggregations", "Id");
            AddPrimaryKey("dbo.ApproachPcdAggregations", "Id");
            AddPrimaryKey("dbo.ApproachEventCountAggregations", "Id");
            AddPrimaryKey("dbo.ApproachCycleAggregations", "Id");
            DropForeignKey("dbo.DetectorAggregations", "DetectorPrimaryId", "dbo.Detectors");

            DropIndex("dbo.DetectorAggregations", new[] { "DetectorPrimaryId" });

            DropPrimaryKey("dbo.ApproachCycleAggregations");

            DropPrimaryKey("dbo.ApproachEventCountAggregations");

            DropPrimaryKey("dbo.ApproachPcdAggregations");

            DropPrimaryKey("dbo.ApproachSpeedAggregations");

            DropPrimaryKey("dbo.ApproachSplitFailAggregations");

            DropPrimaryKey("dbo.ApproachYellowRedActivationAggregations");

            DropPrimaryKey("dbo.DetectorAggregations");

            DropPrimaryKey("dbo.DetectorEventCountAggregations");

            DropPrimaryKey("dbo.PhasePedAggregations");

            DropPrimaryKey("dbo.PhaseTerminationAggregations");

            DropPrimaryKey("dbo.PreemptionAggregations");

            DropPrimaryKey("dbo.PriorityAggregations");

            DropPrimaryKey("dbo.SignalEventCountAggregations");

            AddColumn("dbo.MetricTypes", "DisplayOrder", c => c.Int(nullable: false));

            AddColumn("dbo.DetectorEventCountAggregations", "DetectorPrimaryId", c => c.Int(nullable: false));

            AlterColumn("dbo.PhaseTerminationAggregations", "Id", c => c.Int(nullable: false, identity: true));

            AlterColumn("dbo.PreemptionAggregations", "Id", c => c.Int(nullable: false, identity: true));

            AlterColumn("dbo.PriorityAggregations", "Id", c => c.Int(nullable: false, identity: true));

            AlterColumn("dbo.SignalEventCountAggregations", "Id", c => c.Int(nullable: false, identity: true));

            AddPrimaryKey("dbo.ApproachCycleAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase" });


            AddPrimaryKey("dbo.ApproachEventCountAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase" });

            AddPrimaryKey("dbo.ApproachPcdAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase" });

            AddPrimaryKey("dbo.ApproachSpeedAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase" });

            AddPrimaryKey("dbo.ApproachSplitFailAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase" });

            AddPrimaryKey("dbo.ApproachYellowRedActivationAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase" });

            AddPrimaryKey("dbo.DetectorAggregations", new[] { "BinStartTime", "DetectorPrimaryId" });

            AddPrimaryKey("dbo.DetectorEventCountAggregations", new[] { "BinStartTime", "DetectorPrimaryId" });

            AddPrimaryKey("dbo.PhasePedAggregations", new[] { "BinStartTime", "Id" });

            AddPrimaryKey("dbo.PhaseTerminationAggregations", new[] { "BinStartTime", "PhaseNumber", "Id" });

            AddPrimaryKey("dbo.PreemptionAggregations", new[] { "BinStartTime", "Id" });

            AddPrimaryKey("dbo.PriorityAggregations", new[] { "BinStartTime", "VersionId", "Id" });

            AddPrimaryKey("dbo.SignalEventCountAggregations", new[] { "BinStartTime", "Id" });

            DropColumn("dbo.ApproachCycleAggregations", "Id");

            DropColumn("dbo.ApproachEventCountAggregations", "Id");

            DropColumn("dbo.ApproachPcdAggregations", "Id");

            DropColumn("dbo.ApproachSpeedAggregations", "Id");

            DropColumn("dbo.ApproachSplitFailAggregations", "Id");

            DropColumn("dbo.ApproachYellowRedActivationAggregations", "Id");

            DropColumn("dbo.DetectorAggregations", "Id");

            DropColumn("dbo.DetectorEventCountAggregations", "Id");

            DropColumn("dbo.DetectorEventCountAggregations", "DetectorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DetectorEventCountAggregations", "DetectorId", c => c.String(nullable: false));
            AddColumn("dbo.DetectorEventCountAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.DetectorAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.ApproachYellowRedActivationAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.ApproachSplitFailAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.ApproachSpeedAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.ApproachPcdAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.ApproachEventCountAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.ApproachCycleAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.SignalEventCountAggregations");
            DropPrimaryKey("dbo.PriorityAggregations");
            DropPrimaryKey("dbo.PreemptionAggregations");
            DropPrimaryKey("dbo.PhaseTerminationAggregations");
            DropPrimaryKey("dbo.PhasePedAggregations");
            DropPrimaryKey("dbo.DetectorEventCountAggregations");
            DropPrimaryKey("dbo.DetectorAggregations");
            DropPrimaryKey("dbo.ApproachYellowRedActivationAggregations");
            DropPrimaryKey("dbo.ApproachSplitFailAggregations");
            DropPrimaryKey("dbo.ApproachSpeedAggregations");
            DropPrimaryKey("dbo.ApproachPcdAggregations");
            DropPrimaryKey("dbo.ApproachEventCountAggregations");
            DropPrimaryKey("dbo.ApproachCycleAggregations");
            AlterColumn("dbo.SignalEventCountAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.PriorityAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.PreemptionAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.PhaseTerminationAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropColumn("dbo.DetectorEventCountAggregations", "DetectorPrimaryId");
            DropColumn("dbo.MetricTypes", "DisplayOrder");
            AddPrimaryKey("dbo.SignalEventCountAggregations", "Id");
            AddPrimaryKey("dbo.PriorityAggregations", "Id");
            AddPrimaryKey("dbo.PreemptionAggregations", "Id");
            AddPrimaryKey("dbo.PhaseTerminationAggregations", "Id");
            AddPrimaryKey("dbo.PhasePedAggregations", "Id");
            AddPrimaryKey("dbo.DetectorEventCountAggregations", "Id");
            AddPrimaryKey("dbo.DetectorAggregations", "Id");
            AddPrimaryKey("dbo.ApproachYellowRedActivationAggregations", "Id");
            AddPrimaryKey("dbo.ApproachSplitFailAggregations", "Id");
            AddPrimaryKey("dbo.ApproachSpeedAggregations", "Id");
            AddPrimaryKey("dbo.ApproachPcdAggregations", "Id");
            AddPrimaryKey("dbo.ApproachEventCountAggregations", "Id");
            AddPrimaryKey("dbo.ApproachCycleAggregations", "Id");
            CreateIndex("dbo.DetectorAggregations", "DetectorPrimaryId");
            AddForeignKey("dbo.DetectorAggregations", "DetectorPrimaryId", "dbo.Detectors", "ID", cascadeDelete: true);
        }
    }
}
