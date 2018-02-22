using System.Data.Entity.Migrations;

namespace MOE.Common.Migrations
{
    public partial class DataAggregationUI : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RouteSignals", "Signal_VersionID", "dbo.Signals");
            DropForeignKey("dbo.SignalAggregations", "VersionlID", "dbo.Signals");
            DropForeignKey("dbo.DetectorAggregations", "Id", "dbo.Detectors");
            DropForeignKey("dbo.PreemptionAggregations", "Signal_VersionID", "dbo.Signals");
            DropForeignKey("dbo.PriorityAggregations", "Signal_VersionID", "dbo.Signals");
            DropIndex("dbo.DetectorAggregations", new[] {"Id"});
            DropIndex("dbo.PreemptionAggregations", new[] {"Signal_VersionID"});
            DropIndex("dbo.PriorityAggregations", new[] {"Signal_VersionID"});
            DropIndex("dbo.RouteSignals", new[] {"Signal_VersionID"});
            DropIndex("dbo.SignalAggregations", new[] {"VersionlID"});
            //RenameColumn(table: "dbo.Signals", name: "VersionAction_ID", newName: "VersionActionId");
            RenameColumn("dbo.PreemptionAggregations", "Signal_VersionID", "VersionId");
            RenameColumn("dbo.PriorityAggregations", "Signal_VersionID", "VersionId");
            //RenameIndex(table: "dbo.Signals", name: "IX_VersionAction_ID", newName: "IX_VersionActionId");
            //AddColumn("dbo.Signals", "Start", c => c.DateTime(nullable: false));
            AddColumn("dbo.ApproachCycleAggregations", "IsProtectedPhase", c => c.Boolean(false));
            AddColumn("dbo.ApproachPcdAggregations", "IsProtectedPhase", c => c.Boolean(false));
            AddColumn("dbo.ApproachSpeedAggregations", "IsProtectedPhase", c => c.Boolean(false));
            AddColumn("dbo.ApproachSplitFailAggregations", "GapOuts", c => c.Int(false));
            AddColumn("dbo.ApproachSplitFailAggregations", "ForceOffs", c => c.Int(false));
            AddColumn("dbo.ApproachSplitFailAggregations", "MaxOuts", c => c.Int(false));
            AddColumn("dbo.ApproachSplitFailAggregations", "UnknownTerminationTypes", c => c.Int(false));
            AddColumn("dbo.ApproachSplitFailAggregations", "IsProtectedPhase", c => c.Boolean(false));
            AddColumn("dbo.ApproachYellowRedActivationAggregations", "IsProtectedPhase", c => c.Boolean(false));
            AddColumn("dbo.DetectorAggregations", "DetectorPrimaryId", c => c.Int(false));
            AlterColumn("dbo.ApplicationEvents", "Class", c => c.String());
            AlterColumn("dbo.ApplicationEvents", "Function", c => c.String());
            AlterColumn("dbo.PreemptionAggregations", "VersionId", c => c.Int(false));
            AlterColumn("dbo.PriorityAggregations", "VersionId", c => c.Int(false));
            CreateIndex("dbo.DetectorAggregations", "DetectorPrimaryId");
            CreateIndex("dbo.PreemptionAggregations", "VersionId");
            CreateIndex("dbo.PriorityAggregations", "VersionId");
            AddForeignKey("dbo.DetectorAggregations", "DetectorPrimaryId", "dbo.Detectors", "ID", true);
            AddForeignKey("dbo.PreemptionAggregations", "VersionId", "dbo.Signals", "VersionID", true);
            AddForeignKey("dbo.PriorityAggregations", "VersionId", "dbo.Signals", "VersionID", true);
            //DropColumn("dbo.Signals", "End");
            DropColumn("dbo.DetectorAggregations", "DetectorId");
            DropColumn("dbo.RouteSignals", "Signal_VersionID");
            DropTable("dbo.SignalAggregations");
        }

        public override void Down()
        {
            CreateTable(
                    "dbo.SignalAggregations",
                    c => new
                    {
                        ID = c.Int(false, true),
                        BinStartTime = c.DateTime(false),
                        VersionlID = c.Int(false),
                        TotalCycles = c.Int(false),
                        AddCyclesInTransition = c.Int(false),
                        SubtractCyclesInTransition = c.Int(false),
                        DwellCyclesInTransition = c.Int(false)
                    })
                .PrimaryKey(t => t.ID);

            AddColumn("dbo.RouteSignals", "Signal_VersionID", c => c.Int());
            AddColumn("dbo.DetectorAggregations", "DetectorId", c => c.String(false, 10));
            AddColumn("dbo.Signals", "End", c => c.DateTime(false));
            DropForeignKey("dbo.PriorityAggregations", "VersionId", "dbo.Signals");
            DropForeignKey("dbo.PreemptionAggregations", "VersionId", "dbo.Signals");
            DropForeignKey("dbo.DetectorAggregations", "DetectorPrimaryId", "dbo.Detectors");
            DropIndex("dbo.PriorityAggregations", new[] {"VersionId"});
            DropIndex("dbo.PreemptionAggregations", new[] {"VersionId"});
            DropIndex("dbo.DetectorAggregations", new[] {"DetectorPrimaryId"});
            AlterColumn("dbo.PriorityAggregations", "VersionId", c => c.Int());
            AlterColumn("dbo.PreemptionAggregations", "VersionId", c => c.Int());
            AlterColumn("dbo.ApplicationEvents", "Function", c => c.String(maxLength: 50));
            AlterColumn("dbo.ApplicationEvents", "Class", c => c.String(maxLength: 50));
            DropColumn("dbo.DetectorAggregations", "DetectorPrimaryId");
            DropColumn("dbo.ApproachYellowRedActivationAggregations", "IsProtectedPhase");
            DropColumn("dbo.ApproachSplitFailAggregations", "IsProtectedPhase");
            DropColumn("dbo.ApproachSplitFailAggregations", "UnknownTerminationTypes");
            DropColumn("dbo.ApproachSplitFailAggregations", "MaxOuts");
            DropColumn("dbo.ApproachSplitFailAggregations", "ForceOffs");
            DropColumn("dbo.ApproachSplitFailAggregations", "GapOuts");
            DropColumn("dbo.ApproachSpeedAggregations", "IsProtectedPhase");
            DropColumn("dbo.ApproachPcdAggregations", "IsProtectedPhase");
            DropColumn("dbo.ApproachCycleAggregations", "IsProtectedPhase");
            DropColumn("dbo.Signals", "Start");
            RenameIndex("dbo.Signals", "IX_VersionActionId", "IX_VersionAction_ID");
            RenameColumn("dbo.PriorityAggregations", "VersionId", "Signal_VersionID");
            RenameColumn("dbo.PreemptionAggregations", "VersionId", "Signal_VersionID");
            RenameColumn("dbo.Signals", "VersionActionId", "VersionAction_ID");
            CreateIndex("dbo.SignalAggregations", "VersionlID");
            CreateIndex("dbo.RouteSignals", "Signal_VersionID");
            CreateIndex("dbo.PriorityAggregations", "Signal_VersionID");
            CreateIndex("dbo.PreemptionAggregations", "Signal_VersionID");
            CreateIndex("dbo.DetectorAggregations", "Id");
            AddForeignKey("dbo.PriorityAggregations", "Signal_VersionID", "dbo.Signals", "VersionID");
            AddForeignKey("dbo.PreemptionAggregations", "Signal_VersionID", "dbo.Signals", "VersionID");
            AddForeignKey("dbo.DetectorAggregations", "Id", "dbo.Detectors", "ID");
            AddForeignKey("dbo.SignalAggregations", "VersionlID", "dbo.Signals", "VersionID", true);
            AddForeignKey("dbo.RouteSignals", "Signal_VersionID", "dbo.Signals", "VersionID");
        }
    }
}