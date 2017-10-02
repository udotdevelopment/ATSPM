namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HistoricalConfiguration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActionLogs", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.ApproachRouteDetail", "ApproachID", "dbo.Approaches");
            DropForeignKey("dbo.SPMWatchDogErrorEvents", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.MetricComments", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.Approaches", "SignalID", "dbo.Signals");
            DropIndex("dbo.ActionLogs", new[] { "SignalID" });
            DropIndex("dbo.MetricComments", new[] { "SignalID" });
            DropIndex("dbo.Approaches", new[] { "SignalID" });
            DropIndex("dbo.ApproachRouteDetail", new[] { "ApproachID" });
            DropIndex("dbo.SPMWatchDogErrorEvents", new[] { "SignalID" });
            DropPrimaryKey("dbo.Signals");
            CreateTable(
                "dbo.VersionActions",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ApproachRouteMetricTypes",
                c => new
                    {
                        ApproachRoute_ApproachRouteId = c.Int(nullable: false),
                        MetricType_MetricID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApproachRoute_ApproachRouteId, t.MetricType_MetricID })
                .ForeignKey("dbo.ApproachRoute", t => t.ApproachRoute_ApproachRouteId, cascadeDelete: true)
                .ForeignKey("dbo.MetricTypes", t => t.MetricType_MetricID, cascadeDelete: true)
                .Index(t => t.ApproachRoute_ApproachRouteId)
                .Index(t => t.MetricType_MetricID);
            
            AddColumn("dbo.MetricComments", "VersionID", c => c.Int(nullable: false));
            AddColumn("dbo.Signals", "VersionID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Signals", "Note", c => c.String(nullable: false));
            AddColumn("dbo.Signals", "End", c => c.DateTime(nullable: false));
            AddColumn("dbo.Signals", "VersionAction_ID", c => c.Int(nullable: false));
            AddColumn("dbo.Approaches", "VersionID", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachRouteDetail", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachRouteDetail", "SignalId", c => c.String(nullable: false));
            AddColumn("dbo.ApproachRouteDetail", "PhaseDirection1", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachRouteDetail", "IsPhaseDirection1Overlap", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApproachRouteDetail", "PhaseDirection2", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachRouteDetail", "IsPhaseDirection2Overlap", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApproachRouteDetail", "DirectionType1_DirectionTypeID", c => c.Int());
            AddColumn("dbo.ApproachRouteDetail", "DirectionType2_DirectionTypeID", c => c.Int());
            AlterColumn("dbo.ActionLogs", "SignalID", c => c.String(nullable: false));
            AlterColumn("dbo.MetricComments", "SignalID", c => c.String());
            AlterColumn("dbo.Approaches", "SignalID", c => c.String());
            AlterColumn("dbo.SPMWatchDogErrorEvents", "SignalID", c => c.String(nullable: false));
            AddPrimaryKey("dbo.Signals", "VersionID");
            CreateIndex("dbo.ApproachRouteDetail", "DirectionType1_DirectionTypeID");
            CreateIndex("dbo.ApproachRouteDetail", "DirectionType2_DirectionTypeID");
            CreateIndex("dbo.MetricComments", "VersionID");
            CreateIndex("dbo.Signals", "VersionAction_ID");
            CreateIndex("dbo.Approaches", "VersionID");
            //AddForeignKey("dbo.ApproachRouteDetail", "DirectionType1_DirectionTypeID", "dbo.DirectionTypes", "DirectionTypeID");
            //AddForeignKey("dbo.ApproachRouteDetail", "DirectionType2_DirectionTypeID", "dbo.DirectionTypes", "DirectionTypeID");
            //AddForeignKey("dbo.Signals", "VersionAction_ID", "dbo.VersionActions", "ID", cascadeDelete: true);
            //AddForeignKey("dbo.MetricComments", "VersionID", "dbo.Signals", "VersionID", cascadeDelete: true);
            //AddForeignKey("dbo.Approaches", "VersionID", "dbo.Signals", "VersionID", cascadeDelete: true);
            DropColumn("dbo.ApproachRouteDetail", "ApproachOrder");
            DropColumn("dbo.ApproachRouteDetail", "ApproachID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApproachRouteDetail", "ApproachID", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachRouteDetail", "ApproachOrder", c => c.Int(nullable: false));
            DropForeignKey("dbo.Approaches", "VersionID", "dbo.Signals");
            DropForeignKey("dbo.MetricComments", "VersionID", "dbo.Signals");
            DropForeignKey("dbo.Signals", "VersionAction_ID", "dbo.VersionActions");
            DropForeignKey("dbo.ApproachRouteMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropForeignKey("dbo.ApproachRouteMetricTypes", "ApproachRoute_ApproachRouteId", "dbo.ApproachRoute");
            DropForeignKey("dbo.ApproachRouteDetail", "DirectionType2_DirectionTypeID", "dbo.DirectionTypes");
            DropForeignKey("dbo.ApproachRouteDetail", "DirectionType1_DirectionTypeID", "dbo.DirectionTypes");
            DropIndex("dbo.ApproachRouteMetricTypes", new[] { "MetricType_MetricID" });
            DropIndex("dbo.ApproachRouteMetricTypes", new[] { "ApproachRoute_ApproachRouteId" });
            DropIndex("dbo.Approaches", new[] { "VersionID" });
            DropIndex("dbo.Signals", new[] { "VersionAction_ID" });
            DropIndex("dbo.MetricComments", new[] { "VersionID" });
            DropIndex("dbo.ApproachRouteDetail", new[] { "DirectionType2_DirectionTypeID" });
            DropIndex("dbo.ApproachRouteDetail", new[] { "DirectionType1_DirectionTypeID" });
            DropPrimaryKey("dbo.Signals");
            AlterColumn("dbo.SPMWatchDogErrorEvents", "SignalID", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Approaches", "SignalID", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.MetricComments", "SignalID", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.ActionLogs", "SignalID", c => c.String(nullable: false, maxLength: 10));
            DropColumn("dbo.ApproachRouteDetail", "DirectionType2_DirectionTypeID");
            DropColumn("dbo.ApproachRouteDetail", "DirectionType1_DirectionTypeID");
            DropColumn("dbo.ApproachRouteDetail", "IsPhaseDirection2Overlap");
            DropColumn("dbo.ApproachRouteDetail", "PhaseDirection2");
            DropColumn("dbo.ApproachRouteDetail", "IsPhaseDirection1Overlap");
            DropColumn("dbo.ApproachRouteDetail", "PhaseDirection1");
            DropColumn("dbo.ApproachRouteDetail", "SignalId");
            DropColumn("dbo.ApproachRouteDetail", "Order");
            DropColumn("dbo.Approaches", "VersionID");
            DropColumn("dbo.Signals", "VersionAction_ID");
            DropColumn("dbo.Signals", "End");
            DropColumn("dbo.Signals", "Note");
            DropColumn("dbo.Signals", "VersionID");
            DropColumn("dbo.MetricComments", "VersionID");
            DropTable("dbo.ApproachRouteMetricTypes");
            DropTable("dbo.VersionActions");
            AddPrimaryKey("dbo.Signals", "SignalID");
            CreateIndex("dbo.SPMWatchDogErrorEvents", "SignalID");
            CreateIndex("dbo.ApproachRouteDetail", "ApproachID");
            CreateIndex("dbo.Approaches", "SignalID");
            CreateIndex("dbo.MetricComments", "SignalID");
            CreateIndex("dbo.ActionLogs", "SignalID");
            AddForeignKey("dbo.Approaches", "SignalID", "dbo.Signals", "SignalID", cascadeDelete: true);
            AddForeignKey("dbo.MetricComments", "SignalID", "dbo.Signals", "SignalID", cascadeDelete: true);
            AddForeignKey("dbo.SPMWatchDogErrorEvents", "SignalID", "dbo.Signals", "SignalID", cascadeDelete: true);
            AddForeignKey("dbo.ApproachRouteDetail", "ApproachID", "dbo.Approaches", "ApproachID", cascadeDelete: true);
            AddForeignKey("dbo.ActionLogs", "SignalID", "dbo.Signals", "SignalID");
        }
    }
}
