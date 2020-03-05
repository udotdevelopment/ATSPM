namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeKeysOnAggregationTables : DbMigration
    {
        public override void Up()
        {
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
            AddColumn("dbo.DetectorAggregations", "Detector_ID", c => c.Int());
            AlterColumn("dbo.ApproachCycleAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ApproachEventCountAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ApproachPcdAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ApproachSpeedAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ApproachSplitFailAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ApproachYellowRedActivationAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.DetectorAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.DetectorEventCountAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.PhasePedAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.PhaseTerminationAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.PreemptionAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.PriorityAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.SignalEventCountAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ApproachCycleAggregations", "Id");
            AddPrimaryKey("dbo.ApproachEventCountAggregations", "Id");
            AddPrimaryKey("dbo.ApproachPcdAggregations", "Id");
            AddPrimaryKey("dbo.ApproachSpeedAggregations", "Id");
            AddPrimaryKey("dbo.ApproachSplitFailAggregations", "Id");
            AddPrimaryKey("dbo.ApproachYellowRedActivationAggregations", "Id");
            AddPrimaryKey("dbo.DetectorAggregations", "Id");
            AddPrimaryKey("dbo.DetectorEventCountAggregations", "Id");
            AddPrimaryKey("dbo.PhasePedAggregations", "Id");
            AddPrimaryKey("dbo.PhaseTerminationAggregations", "Id");
            AddPrimaryKey("dbo.PreemptionAggregations", "Id");
            AddPrimaryKey("dbo.PriorityAggregations", "Id");
            AddPrimaryKey("dbo.SignalEventCountAggregations", "Id");
            CreateIndex("dbo.DetectorAggregations", "Detector_ID");
            AddForeignKey("dbo.DetectorAggregations", "Detector_ID", "dbo.Detectors", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DetectorAggregations", "Detector_ID", "dbo.Detectors");
            DropIndex("dbo.DetectorAggregations", new[] { "Detector_ID" });
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
            AlterColumn("dbo.PhasePedAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.DetectorEventCountAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.DetectorAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.ApproachYellowRedActivationAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.ApproachSplitFailAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.ApproachSpeedAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.ApproachPcdAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.ApproachEventCountAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.ApproachCycleAggregations", "Id", c => c.Long(nullable: false, identity: true));
            DropColumn("dbo.DetectorAggregations", "Detector_ID");
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
