namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggregationData : DbMigration
    {
        public override void Up()
        {
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
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.SignalID, cascadeDelete: true)
                .Index(t => t.SignalID);
            
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
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.SignalID, cascadeDelete: true)
                .Index(t => t.SignalID);
            
            CreateTable(
                "dbo.SignalAggregations",
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
            
            DropForeignKey("dbo.SignalAggregations", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.PriorityAggregations", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.PreemptionAggregations", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.DetectorAggregations", "Id", "dbo.Detectors");
            DropForeignKey("dbo.ApproachYellowRedActivationAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachSplitFailAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachSpeedAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachPcdAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachCycleAggregations", "ApproachId", "dbo.Approaches");
            DropIndex("dbo.SignalAggregations", new[] { "SignalID" });
            DropIndex("dbo.PriorityAggregations", new[] { "SignalID" });
            DropIndex("dbo.PreemptionAggregations", new[] { "SignalID" });
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
        }
    }
}
