namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAggregationUI : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SignalAggregations", "VersionlID", "dbo.Signals");
            DropForeignKey("dbo.PreemptionAggregations", "Signal_VersionID", "dbo.Signals");
            DropForeignKey("dbo.PriorityAggregations", "Signal_VersionID", "dbo.Signals");
            DropIndex("dbo.PreemptionAggregations", new[] { "Signal_VersionID" });
            DropIndex("dbo.PriorityAggregations", new[] { "Signal_VersionID" });
            DropIndex("dbo.SignalAggregations", new[] { "VersionlID" });
            RenameColumn(table: "dbo.Signals", name: "VersionAction_ID", newName: "VersionActionId");
            RenameColumn(table: "dbo.PreemptionAggregations", name: "Signal_VersionID", newName: "VersionId");
            RenameColumn(table: "dbo.PriorityAggregations", name: "Signal_VersionID", newName: "VersionId");
            RenameIndex(table: "dbo.Signals", name: "IX_VersionAction_ID", newName: "IX_VersionActionId");
            AddColumn("dbo.Signals", "Start", c => c.DateTime(nullable: false));
            AddColumn("dbo.ApproachCycleAggregations", "IsProtectedPhase", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApproachPcdAggregations", "IsProtectedPhase", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApproachSpeedAggregations", "IsProtectedPhase", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "GapOuts", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "ForceOffs", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "MaxOuts", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "UnknownTerminationTypes", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "IsProtectedPhase", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApproachYellowRedActivationAggregations", "IsProtectedPhase", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ApplicationEvents", "Class", c => c.String());
            AlterColumn("dbo.ApplicationEvents", "Function", c => c.String());
            AlterColumn("dbo.PreemptionAggregations", "VersionId", c => c.Int(nullable: false));
            AlterColumn("dbo.PriorityAggregations", "VersionId", c => c.Int(nullable: false));
            CreateIndex("dbo.PreemptionAggregations", "VersionId");
            CreateIndex("dbo.PriorityAggregations", "VersionId");
            AddForeignKey("dbo.PreemptionAggregations", "VersionId", "dbo.Signals", "VersionID", cascadeDelete: true);
            AddForeignKey("dbo.PriorityAggregations", "VersionId", "dbo.Signals", "VersionID", cascadeDelete: true);
            DropColumn("dbo.Signals", "End");
            DropTable("dbo.SignalAggregations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SignalAggregations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        VersionlID = c.Int(nullable: false),
                        TotalCycles = c.Int(nullable: false),
                        AddCyclesInTransition = c.Int(nullable: false),
                        SubtractCyclesInTransition = c.Int(nullable: false),
                        DwellCyclesInTransition = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Signals", "End", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.PriorityAggregations", "VersionId", "dbo.Signals");
            DropForeignKey("dbo.PreemptionAggregations", "VersionId", "dbo.Signals");
            DropIndex("dbo.PriorityAggregations", new[] { "VersionId" });
            DropIndex("dbo.PreemptionAggregations", new[] { "VersionId" });
            AlterColumn("dbo.PriorityAggregations", "VersionId", c => c.Int());
            AlterColumn("dbo.PreemptionAggregations", "VersionId", c => c.Int());
            AlterColumn("dbo.ApplicationEvents", "Function", c => c.String(maxLength: 50));
            AlterColumn("dbo.ApplicationEvents", "Class", c => c.String(maxLength: 50));
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
            RenameIndex(table: "dbo.Signals", name: "IX_VersionActionId", newName: "IX_VersionAction_ID");
            RenameColumn(table: "dbo.PriorityAggregations", name: "VersionId", newName: "Signal_VersionID");
            RenameColumn(table: "dbo.PreemptionAggregations", name: "VersionId", newName: "Signal_VersionID");
            RenameColumn(table: "dbo.Signals", name: "VersionActionId", newName: "VersionAction_ID");
            CreateIndex("dbo.SignalAggregations", "VersionlID");
            CreateIndex("dbo.PriorityAggregations", "Signal_VersionID");
            CreateIndex("dbo.PreemptionAggregations", "Signal_VersionID");
            AddForeignKey("dbo.PriorityAggregations", "Signal_VersionID", "dbo.Signals", "VersionID");
            AddForeignKey("dbo.PreemptionAggregations", "Signal_VersionID", "dbo.Signals", "VersionID");
            AddForeignKey("dbo.SignalAggregations", "VersionlID", "dbo.Signals", "VersionID", cascadeDelete: true);
        }
    }
}
