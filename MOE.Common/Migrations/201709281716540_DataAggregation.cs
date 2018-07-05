using System;
using System.Data.Entity.Migrations;

namespace MOE.Common.Migrations
{
    public partial class DataAggregation : DbMigration
    {
        public override void Up()
        {
            //Historical Configuration Migration
            DropForeignKey("dbo.ActionLogs", "SignalId", "dbo.Signals");
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
            DropPrimaryKey("Signals");

            //}


            CreateTable(
                    "dbo.VersionActions",
                    c => new
                    {
                        ID = c.Int(false),
                        Description = c.String()
                    })
                .PrimaryKey(t => t.ID);

            Sql("Insert into VersionActions(ID, Description) values (1, 'New')");
            Sql("Insert into VersionActions(ID, Description) values (2, 'Edit')");
            Sql("Insert into VersionActions(ID, Description) values (3, 'Delete')");
            Sql("Insert into VersionActions(ID, Description) values (4, 'New Version')");
            Sql("Insert into VersionActions(ID, Description) values (10, 'Initial')");


            AddColumn("dbo.MetricComments", "VersionID", c => c.Int(nullable: false));
            AddColumn("dbo.Signals", "VersionID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Signals", "VersionActionId", c => c.Int(nullable: false, defaultValue:10));
            AddColumn("dbo.Signals", "Note", c => c.String(nullable: false, defaultValue: "Initial"));
            AddColumn("dbo.Signals", "Start", c => c.DateTime(nullable: false ));
            AddColumn("dbo.Approaches", "VersionID", c => c.Int(nullable: false));
            AlterColumn("dbo.ActionLogs", "SignalID", c => c.String(nullable: false));
            AlterColumn("dbo.MetricComments", "SignalID", c => c.String());
            AlterColumn("dbo.Approaches", "SignalID", c => c.String());
            AlterColumn("dbo.SPMWatchDogErrorEvents", "SignalID", c => c.String(nullable: false));
            AddPrimaryKey("dbo.Signals", "VersionID");
            CreateIndex("dbo.MetricComments", "VersionID");
            CreateIndex("dbo.Signals", "VersionActionId");
            CreateIndex("dbo.Approaches", "VersionID");
            //AddForeignKey("dbo.Signals", "VersionActionId", "dbo.VersionActions", "ID", cascadeDelete: true);
            //AddForeignKey("dbo.MetricComments", "VersionID", "dbo.Signals", "VersionID", cascadeDelete: true);
            //AddForeignKey("dbo.Approaches", "VersionID", "dbo.Signals", "VersionID", cascadeDelete: true);
            DropColumn("dbo.ApproachRouteDetail", "ApproachOrder");
            DropColumn("dbo.ApproachRouteDetail", "ApproachID");
            DropTable("dbo.SignalWithDetections");
            //This only exists on UDOT Database
            //DropTable("dbo.ApproachRoute");
            DropTable("dbo.ApproachRouteDetail");

            //Original Data Aggregation Migration
            CreateTable(
                    "dbo.ApproachCycleAggregations",
                    c => new
                    {
                        Id = c.Int(false, true),
                        BinStartTime = c.DateTime(false),
                        ApproachId = c.Int(false),
                        RedTime = c.Double(false),
                        YellowTime = c.Double(false),
                        GreenTime = c.Double(false),
                        TotalCycles = c.Int(false),
                        PedActuations = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, false)
                .Index(t => t.ApproachId);

            CreateTable(
                    "dbo.ApproachPcdAggregations",
                    c => new
                    {
                        Id = c.Int(false, true),
                        BinStartTime = c.DateTime(false),
                        ApproachId = c.Int(false),
                        ArrivalsOnGreen = c.Int(false),
                        ArrivalsOnRed = c.Int(false),
                        ArrivalsOnYellow = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, false)
                .Index(t => t.ApproachId);

            CreateTable(
                    "dbo.ApproachSpeedAggregations",
                    c => new
                    {
                        Id = c.Int(false, true),
                        BinStartTime = c.DateTime(false),
                        ApproachId = c.Int(false),
                        SummedSpeed = c.Double(false),
                        SpeedVolume = c.Double(false),
                        Speed85Th = c.Double(false),
                        Speed15Th = c.Double(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, false)
                .Index(t => t.ApproachId);

            CreateTable(
                    "dbo.ApproachSplitFailAggregations",
                    c => new
                    {
                        Id = c.Int(false, true),
                        BinStartTime = c.DateTime(false),
                        ApproachId = c.Int(false),
                        SplitFailures = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, false)
                .Index(t => t.ApproachId);

            CreateTable(
                    "dbo.ApproachYellowRedActivationAggregations",
                    c => new
                    {
                        Id = c.Int(false, true),
                        BinStartTime = c.DateTime(false),
                        ApproachId = c.Int(false),
                        SevereRedLightViolations = c.Int(false),
                        TotalRedLightViolations = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, false)
                .Index(t => t.ApproachId);

            CreateTable(
                    "dbo.DetectorAggregations",
                    c => new
                    {
                        Id = c.Int(false, true),
                        BinStartTime = c.DateTime(false),
                        DetectorId = c.String(false, 10),
                        Volume = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Detectors", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                    "dbo.PreemptionAggregations",
                    c => new
                    {
                        ID = c.Int(false, true),
                        BinStartTime = c.DateTime(false),
                        SignalID = c.String(false, 10),
                        PreemptNumber = c.Int(false),
                        PreemptRequests = c.Int(false),
                        PreemptServices = c.Int(false),
                        Signal_VersionID = c.Int()
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.Signal_VersionID)
                .Index(t => t.Signal_VersionID);

            CreateTable(
                    "dbo.PriorityAggregations",
                    c => new
                    {
                        ID = c.Int(false, true),
                        BinStartTime = c.DateTime(false),
                        SignalID = c.String(false, 10),
                        PriorityNumber = c.Int(false),
                        TotalCycles = c.Int(false),
                        PriorityRequests = c.Int(false),
                        PriorityServiceEarlyGreen = c.Int(false),
                        PriorityServiceExtendedGreen = c.Int(false),
                        Signal_VersionID = c.Int()
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.Signal_VersionID)
                .Index(t => t.Signal_VersionID);

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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.VersionlID, false)
                .Index(t => t.VersionlID);

            DropTable("dbo.Archived_Metrics");
        }

        public override void Down()
        {
            CreateTable(
                    "dbo.Archived_Metrics",
                    c => new
                    {
                        Timestamp = c.DateTime(false),
                        DetectorID = c.String(false, 50, unicode: false),
                        BinSize = c.Int(false),
                        Volume = c.Int(),
                        speed = c.Int(),
                        delay = c.Int(),
                        AoR = c.Int(),
                        SpeedHits = c.Int(),
                        BinGreenTime = c.Int()
                    })
                .PrimaryKey(t => new {t.Timestamp, t.DetectorID, t.BinSize});

            DropForeignKey("dbo.SignalAggregations", "VersionlID", "dbo.Signals");
            DropForeignKey("dbo.PriorityAggregations", "Signal_VersionID", "dbo.Signals");
            DropForeignKey("dbo.PreemptionAggregations", "Signal_VersionID", "dbo.Signals");
            DropForeignKey("dbo.DetectorAggregations", "Id", "dbo.Detectors");
            DropForeignKey("dbo.ApproachYellowRedActivationAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachSplitFailAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachSpeedAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachPcdAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachCycleAggregations", "ApproachId", "dbo.Approaches");
            DropIndex("dbo.SignalAggregations", new[] {"VersionlID"});
            DropIndex("dbo.PriorityAggregations", new[] {"Signal_VersionID"});
            DropIndex("dbo.PreemptionAggregations", new[] {"Signal_VersionID"});
            DropIndex("dbo.DetectorAggregations", new[] {"Id"});
            DropIndex("dbo.ApproachYellowRedActivationAggregations", new[] {"ApproachId"});
            DropIndex("dbo.ApproachSplitFailAggregations", new[] {"ApproachId"});
            DropIndex("dbo.ApproachSpeedAggregations", new[] {"ApproachId"});
            DropIndex("dbo.ApproachPcdAggregations", new[] {"ApproachId"});
            DropIndex("dbo.ApproachCycleAggregations", new[] {"ApproachId"});
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
                        SignalID = c.String(false, 10),
                        DetectionTypeID = c.Int(false),
                        PrimaryName = c.String(),
                        Secondary_Name = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        Region = c.String()
                    })
                .PrimaryKey(t => new {t.SignalID, t.DetectionTypeID});

            AddColumn("dbo.ApproachRouteDetail", "ApproachID", c => c.Int(false));
            AddColumn("dbo.ApproachRouteDetail", "ApproachOrder", c => c.Int(false));
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
            AlterColumn("dbo.SPMWatchDogErrorEvents", "SignalID", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Approaches", "SignalID", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.MetricComments", "SignalID", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.ActionLogs", "SignalID", c => c.String(nullable: false, maxLength: 10));
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
            DropForeignKey("dbo.SPMWatchDogErrorEvents", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.MetricComments", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.Approaches", "SignalID", "dbo.Signals");
            DropIndex("dbo.ActionLogs", new[] {"SignalID"});
            DropIndex("dbo.MetricComments", new[] {"SignalID"});
            DropIndex("dbo.Approaches", new[] {"SignalID"});
            DropIndex("dbo.ApproachRouteDetail", new[] {"ApproachID"});
            DropIndex("dbo.SPMWatchDogErrorEvents", new[] {"SignalID"});
            DropPrimaryKey("dbo.Signals");
            //try
            //{
            //    DropPrimaryKey("dbo.Signals", "PK_Signals");
            //}
            //catch (Exception e)
            //{
            //    DropPrimaryKey("dbo.Signals", "PK_dbo._Signals");
            //}
            //finally
            //{
            Sql("Insert into VersionActions(ID, Description) values (10, 'Initial')");


            AddColumn("dbo.MetricComments", "VersionID", c => c.Int(false));
            AddColumn("dbo.Signals", "VersionID", c => c.Int(false, true));
            AddColumn("dbo.Signals", "VersionActionId", c => c.Int(false, defaultValue: 10));
            AddColumn("dbo.Signals", "Note", c => c.String(false, defaultValue: "Initial"));
            AddColumn("dbo.Signals", "Start", c => c.DateTime(false));
            AddColumn("dbo.Approaches", "VersionID", c => c.Int(false));
            AlterColumn("dbo.ActionLogs", "SignalID", c => c.String(false));
            AlterColumn("dbo.MetricComments", "SignalID", c => c.String());
            AlterColumn("dbo.Approaches", "SignalID", c => c.String());
            AlterColumn("dbo.SPMWatchDogErrorEvents", "SignalID", c => c.String(false));
            DropIndex("dbo.Approaches", new[] {"VersionID"});
            DropIndex("dbo.Signals", new[] {"VersionActionId"});
            DropIndex("dbo.MetricComments", new[] {"VersionID"});
            DropIndex("dbo.ApproachRouteDetail", new[] {"DirectionType2_DirectionTypeID"});
            DropIndex("dbo.ApproachRouteDetail", new[] {"DirectionType1_DirectionTypeID"});
            AlterColumn("dbo.SPMWatchDogErrorEvents", "SignalID", c => c.String(false, 10));
            AlterColumn("dbo.Approaches", "SignalID", c => c.String(false, 10));
            AlterColumn("dbo.MetricComments", "SignalID", c => c.String(false, 10));
            AlterColumn("dbo.ActionLogs", "SignalID", c => c.String(false, 10));
            AddPrimaryKey("dbo.Signals", "SignalID");
            CreateIndex("dbo.SPMWatchDogErrorEvents", "SignalID");
            CreateIndex("dbo.Detectors", "DetectorID", true, "IX_DetectorIDUnique");
            CreateIndex("dbo.Approaches", "SignalID");
            CreateIndex("dbo.MetricComments", "SignalID");
            CreateIndex("dbo.ActionLogs", "SignalID");
            AddForeignKey("dbo.Approaches", "SignalID", "dbo.Signals", "SignalID", true);
            AddForeignKey("dbo.MetricComments", "SignalID", "dbo.Signals", "SignalID", true);
            AddForeignKey("dbo.SPMWatchDogErrorEvents", "SignalID", "dbo.Signals", "SignalID", true);
            AddForeignKey("dbo.ApproachRouteDetail", "ApproachID", "dbo.Approaches", "ApproachID", true);
            AddForeignKey("dbo.ActionLogs", "SignalID", "dbo.Signals", "SignalID");