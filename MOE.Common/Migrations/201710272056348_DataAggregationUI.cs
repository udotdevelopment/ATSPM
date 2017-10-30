namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAggregationUI : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SignalAggregations", "VersionlID", "dbo.Signals");
            DropIndex("dbo.SignalAggregations", new[] { "VersionlID" });
            //RenameColumn(table: "dbo.Signals", name: "VersionAction_ID", newName: "VersionActionId");
            //RenameIndex(table: "dbo.Signals", name: "IX_VersionAction_ID", newName: "IX_VersionActionId");
            //AddColumn("dbo.Signals", "Start", c => c.DateTime(nullable: false));
            AddColumn("dbo.ApproachCycleAggregations", "IsProtectedPhase", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApproachPcdAggregations", "IsProtectedPhase", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApproachSpeedAggregations", "IsProtectedPhase", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "IsProtectedPhase", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApproachYellowRedActivationAggregations", "IsProtectedPhase", c => c.Boolean(nullable: false));
            //DropColumn("dbo.Signals", "End");
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
            DropColumn("dbo.ApproachYellowRedActivationAggregations", "IsProtectedPhase");
            DropColumn("dbo.ApproachSplitFailAggregations", "IsProtectedPhase");
            DropColumn("dbo.ApproachSpeedAggregations", "IsProtectedPhase");
            DropColumn("dbo.ApproachPcdAggregations", "IsProtectedPhase");
            DropColumn("dbo.ApproachCycleAggregations", "IsProtectedPhase");
            DropColumn("dbo.Signals", "Start");
            RenameIndex(table: "dbo.Signals", name: "IX_VersionActionId", newName: "IX_VersionAction_ID");
            RenameColumn(table: "dbo.Signals", name: "VersionActionId", newName: "VersionAction_ID");
            CreateIndex("dbo.SignalAggregations", "VersionlID");
            AddForeignKey("dbo.SignalAggregations", "VersionlID", "dbo.Signals", "VersionID", cascadeDelete: true);
        }
    }
}
