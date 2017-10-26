namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAggregation : DbMigration
    {
        public override void Up()
        {
            //Historical Configuration Migration
            DropForeignKey("dbo.ActionLogs", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.ApproachRouteDetail", "ApproachID", "dbo.Approaches");
            DropForeignKey("dbo.SPMWatchDogErrorEvents", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.MetricComments", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.Approaches", "SignalID", "dbo.Signals");
            DropIndex("dbo.ActionLogs", new[] { "SignalID" });
            DropIndex("dbo.MetricComments", new[] { "SignalID" });
            DropIndex("dbo.Approaches", new[] { "SignalID" });
            DropIndex("dbo.ApproachRouteDetail", new[] { "ApproachID" });
            DropIndex("dbo.Detectors", "IX_DetectorIDUnique");
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

            

            AddColumn("dbo.MetricComments", "VersionID", c => c.Int(nullable: false));
            AddColumn("dbo.Signals", "VersionID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Signals", "VersionActionId", c => c.Int(nullable: false));
            AddColumn("dbo.Signals", "Note", c => c.String(nullable: false));
            AddColumn("dbo.Signals", "Start", c => c.DateTime(nullable: false));
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
            CreateIndex("dbo.Signals", "VersionActionId");
            CreateIndex("dbo.Approaches", "VersionID");
            AddForeignKey("dbo.ApproachRouteDetail", "DirectionType1_DirectionTypeID", "dbo.DirectionTypes", "DirectionTypeID");
            AddForeignKey("dbo.ApproachRouteDetail", "DirectionType2_DirectionTypeID", "dbo.DirectionTypes", "DirectionTypeID");
            AddForeignKey("dbo.Signals", "VersionActionId", "dbo.VersionActions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.MetricComments", "VersionID", "dbo.Signals", "VersionID", cascadeDelete: true);
            AddForeignKey("dbo.Approaches", "VersionID", "dbo.Signals", "VersionID", cascadeDelete: true);
            DropColumn("dbo.ApproachRouteDetail", "ApproachOrder");
            DropColumn("dbo.ApproachRouteDetail", "ApproachID");
            DropTable("dbo.SignalWithDetections");
            //Original Data Aggragation Migration
            CreateTable(
                "dbo.ApproachCycleAggregations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        RedTime = c.Double(nullable: false),
                        YellowTime = c.Double(nullable: false),
                        GreenTime = c.Double(nullable: false),
                        TotalCycles = c.Int(nullable: false),
                        PedActuations = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, cascadeDelete: true)
                .Index(t => t.ApproachId);
            
            CreateTable(
                "dbo.ApproachPcdAggregations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        ArrivalsOnGreen = c.Int(nullable: false),
                        ArrivalsOnRed = c.Int(nullable: false),
                        ArrivalsOnYellow = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, cascadeDelete: true)
                .Index(t => t.ApproachId);
            
            CreateTable(
                "dbo.ApproachSpeedAggregations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        SummedSpeed = c.Double(nullable: false),
                        SpeedVolume = c.Double(nullable: false),
                        Speed85Th = c.Double(nullable: false),
                        Speed15Th = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, cascadeDelete: true)
                .Index(t => t.ApproachId);
            
            CreateTable(
                "dbo.ApproachSplitFailAggregations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        SplitFailures = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, cascadeDelete: true)
                .Index(t => t.ApproachId);
            
            CreateTable(
                "dbo.ApproachYellowRedActivationAggregations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        SevereRedLightViolations = c.Int(nullable: false),
                        TotalRedLightViolations = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, cascadeDelete: true)
                .Index(t => t.ApproachId);
            
            CreateTable(
                "dbo.DetectorAggregations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        DetectorId = c.String(nullable: false, maxLength: 10),
                        Volume = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Detectors", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.PreemptionAggregations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        PreemptNumber = c.Int(nullable: false),
                        PreemptRequests = c.Int(nullable: false),
                        PreemptServices = c.Int(nullable: false),
                        Signal_VersionID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.Signal_VersionID)
                .Index(t => t.Signal_VersionID);
            
            CreateTable(
                "dbo.PriorityAggregations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        PriorityNumber = c.Int(nullable: false),
                        TotalCycles = c.Int(nullable: false),
                        PriorityRequests = c.Int(nullable: false),
                        PriorityServiceEarlyGreen = c.Int(nullable: false),
                        PriorityServiceExtendedGreen = c.Int(nullable: false),
                        Signal_VersionID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.Signal_VersionID)
                .Index(t => t.Signal_VersionID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.VersionlID, cascadeDelete: true)
                .Index(t => t.VersionlID);
            
            DropTable("dbo.Archived_Metrics");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Archived_Metrics",
                c => new
                    {
                        Timestamp = c.DateTime(nullable: false),
                        DetectorID = c.String(nullable: false, maxLength: 50, unicode: false),
                        BinSize = c.Int(nullable: false),
                        Volume = c.Int(),
                        speed = c.Int(),
                        delay = c.Int(),
                        AoR = c.Int(),
                        SpeedHits = c.Int(),
                        BinGreenTime = c.Int(),
                    })
                .PrimaryKey(t => new { t.Timestamp, t.DetectorID, t.BinSize });
            
            DropForeignKey("dbo.SignalAggregations", "VersionlID", "dbo.Signals");
            DropForeignKey("dbo.PriorityAggregations", "Signal_VersionID", "dbo.Signals");
            DropForeignKey("dbo.PreemptionAggregations", "Signal_VersionID", "dbo.Signals");
            DropForeignKey("dbo.DetectorAggregations", "Id", "dbo.Detectors");
            DropForeignKey("dbo.ApproachYellowRedActivationAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachSplitFailAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachSpeedAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachPcdAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachCycleAggregations", "ApproachId", "dbo.Approaches");
            DropIndex("dbo.SignalAggregations", new[] { "VersionlID" });
            DropIndex("dbo.PriorityAggregations", new[] { "Signal_VersionID" });
            DropIndex("dbo.PreemptionAggregations", new[] { "Signal_VersionID" });
            DropIndex("dbo.DetectorAggregations", new[] { "Id" });
            DropIndex("dbo.ApproachYellowRedActivationAggregations", new[] { "ApproachId" });
            DropIndex("dbo.ApproachSplitFailAggregations", new[] { "ApproachId" });
            DropIndex("dbo.ApproachSpeedAggregations", new[] { "ApproachId" });
            DropIndex("dbo.ApproachPcdAggregations", new[] { "ApproachId" });
            DropIndex("dbo.ApproachCycleAggregations", new[] { "ApproachId" });
            DropTable("dbo.SignalAggregations");
            DropTable("dbo.PriorityAggregations");
            DropTable("dbo.PreemptionAggregations");
            DropTable("dbo.DetectorAggregations");
            DropTable("dbo.ApproachYellowRedActivationAggregations");
            DropTable("dbo.ApproachSplitFailAggregations");
            DropTable("dbo.ApproachSpeedAggregations");
            DropTable("dbo.ApproachPcdAggregations");
            DropTable("dbo.ApproachCycleAggregations");

            //Historical Configuration Migration
            CreateTable(
                "dbo.SignalWithDetections",
                c => new
                {
                    SignalID = c.String(nullable: false, maxLength: 10),
                    DetectionTypeID = c.Int(nullable: false),
                    PrimaryName = c.String(),
                    Secondary_Name = c.String(),
                    Latitude = c.String(),
                    Longitude = c.String(),
                    Region = c.String(),
                })
                .PrimaryKey(t => new { t.SignalID, t.DetectionTypeID });

            AddColumn("dbo.ApproachRouteDetail", "ApproachID", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachRouteDetail", "ApproachOrder", c => c.Int(nullable: false));
            DropForeignKey("dbo.Approaches", "VersionID", "dbo.Signals");
            DropForeignKey("dbo.MetricComments", "VersionID", "dbo.Signals");
            DropForeignKey("dbo.Signals", "VersionActionId", "dbo.VersionActions");
            DropForeignKey("dbo.ApproachRouteDetail", "DirectionType2_DirectionTypeID", "dbo.DirectionTypes");
            DropForeignKey("dbo.ApproachRouteDetail", "DirectionType1_DirectionTypeID", "dbo.DirectionTypes");
            DropIndex("dbo.Approaches", new[] { "VersionID" });
            DropIndex("dbo.Signals", new[] { "VersionActionId" });
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
            DropColumn("dbo.Signals", "Start");
            DropColumn("dbo.Signals", "Note");
            DropColumn("dbo.Signals", "VersionActionId");
            DropColumn("dbo.Signals", "VersionID");
            DropColumn("dbo.MetricComments", "VersionID");
            DropTable("dbo.VersionActions");
            AddPrimaryKey("dbo.Signals", "SignalID");
            CreateIndex("dbo.SPMWatchDogErrorEvents", "SignalID");
            CreateIndex("dbo.Detectors", "DetectorID", unique: true, name: "IX_DetectorIDUnique");
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
