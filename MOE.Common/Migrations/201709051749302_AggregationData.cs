namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggregationData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApproachAggregationDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachID = c.Int(nullable: false),
                        RedTime = c.Double(nullable: false),
                        YellowTime = c.Double(nullable: false),
                        GreenTime = c.Double(nullable: false),
                        TotalCycles = c.Int(nullable: false),
                        PedActuations = c.Int(nullable: false),
                        SplitFailures = c.Int(nullable: false),
                        ArrivalsOnGreen = c.Int(nullable: false),
                        ArrivalsOnRed = c.Int(nullable: false),
                        ArrivalsOnYellow = c.Int(nullable: false),
                        SevereRedLightViolations = c.Int(nullable: false),
                        TotalRedLightViolations = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Approaches", t => t.ApproachID, cascadeDelete: true)
                .Index(t => t.ApproachID);
            
            CreateTable(
                "dbo.ApproachSpeedAggregationDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachID = c.Int(nullable: false),
                        SummedSpeed = c.Double(nullable: false),
                        SpeedVolume = c.Double(nullable: false),
                        Speed85th = c.Double(nullable: false),
                        Speed15th = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Approaches", t => t.ApproachID, cascadeDelete: true)
                .Index(t => t.ApproachID);
            
            CreateTable(
                "dbo.DetectorAggregationDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        DetectorID = c.String(nullable: false, maxLength: 10),
                        Volume = c.Int(nullable: false),
                        Detector_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Detectors", t => t.Detector_ID)
                .Index(t => t.Detector_ID);
            
            CreateTable(
                "dbo.PreemptionAggregationDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        PreemptNumber = c.Int(nullable: false),
                        PreemptRequests = c.Int(nullable: false),
                        PreemptServices = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.SignalID, cascadeDelete: true)
                .Index(t => t.SignalID);
            
            CreateTable(
                "dbo.PriorityAggregationDatas",
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
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.SignalID, cascadeDelete: true)
                .Index(t => t.SignalID);
            
            CreateTable(
                "dbo.SignalAggregationDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        TotalCycles = c.Int(nullable: false),
                        AddCyclesInTransition = c.Int(nullable: false),
                        SubtractCyclesInTransition = c.Int(nullable: false),
                        DwellCyclesInTransition = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.SignalID, cascadeDelete: true)
                .Index(t => t.SignalID);
            
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
            
            DropForeignKey("dbo.SignalAggregationDatas", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.PriorityAggregationDatas", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.PreemptionAggregationDatas", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.DetectorAggregationDatas", "Detector_ID", "dbo.Detectors");
            DropForeignKey("dbo.ApproachSpeedAggregationDatas", "ApproachID", "dbo.Approaches");
            DropForeignKey("dbo.ApproachAggregationDatas", "ApproachID", "dbo.Approaches");
            DropIndex("dbo.SignalAggregationDatas", new[] { "SignalID" });
            DropIndex("dbo.PriorityAggregationDatas", new[] { "SignalID" });
            DropIndex("dbo.PreemptionAggregationDatas", new[] { "SignalID" });
            DropIndex("dbo.DetectorAggregationDatas", new[] { "Detector_ID" });
            DropIndex("dbo.ApproachSpeedAggregationDatas", new[] { "ApproachID" });
            DropIndex("dbo.ApproachAggregationDatas", new[] { "ApproachID" });
            DropTable("dbo.SignalAggregationDatas");
            DropTable("dbo.PriorityAggregationDatas");
            DropTable("dbo.PreemptionAggregationDatas");
            DropTable("dbo.DetectorAggregationDatas");
            DropTable("dbo.ApproachSpeedAggregationDatas");
            DropTable("dbo.ApproachAggregationDatas");
        }
    }
}
