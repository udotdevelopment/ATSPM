namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Web.Mvc;

    public partial class PuttigTimeIndexsOnAggregationTables : DbMigration
    {
        public override void Up()
        {

            Filter TABLE[dbo].[ApproachCycleAggregations]
            DROP CONSTRAINT[FK_dbo.ApproachCycleAggregations_dbo.Approaches_ApproachId]

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

            DropColumn("dbo.ApproachCycleAggregations", "Id");
            DropColumn("dbo.ApproachEventCountAggregations", "Id");
            DropColumn("dbo.ApproachPcdAggregations", "Id");
            DropColumn("dbo.ApproachSpeedAggregations", "Id");
            DropColumn("dbo.ApproachSplitFailAggregations", "Id");
            DropColumn("dbo.ApproachYellowRedActivationAggregations", "Id");
            DropColumn("dbo.DetectorAggregations", "Id");
            DropColumn("dbo.DetectorEventCountAggregations", "Id");
            DropColumn("dbo.PhasePedAggregations", "Id");
            DropColumn("dbo.PhaseTerminationAggregations", "Id");
            DropColumn("dbo.PreemptionAggregations", "Id");
            DropColumn("dbo.PriorityAggregations", "Id");
            DropColumn("dbo.SignalEventCountAggregations", "Id");
        }

        public override void Down()
        {
            AddColumn("dbo.ApproachCycleAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.ApproachEventCountAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.ApproachPcdAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.ApproachSpeedAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.ApproachSplitFailAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.ApproachYellowRedActivationAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.DetectorAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.DetectorEventCountAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.PhasePedAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.PhaseTerminationAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.PreemptionAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.PriorityAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.SignalEventCountAggregations", "Id", c => c.Long(nullable: false, identity: true));

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
        }
    }
}



AddColumn("dbo.Detectors", "DetectionHardwareID", c => c.Int(false));
AddColumn("dbo.MovementTypes", "DisplayOrder", c => c.Int(false));
AddColumn("dbo.DirectionTypes", "DisplayOrder", c => c.Int(false));
AddColumn("dbo.FAQs", "OrderNumber", c => c.Int(false));
CreateIndex("dbo.Detectors", "DetectionHardwareID");
AddForeignKey("dbo.Detectors", "DetectionHardwareID", "dbo.DetectionHardwares", "ID", true);
DropColumn("dbo.MetricTypes", "DetectionType_DetectionTypeID");