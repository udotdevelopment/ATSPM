namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReCreateAggTables : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ApproachEventCountAggregations");
            DropTable("dbo.ApproachCycleAggregations");
            DropTable("dbo.DetectorAggregations");
            //DropTable("dbo.PhaseCycleAggregations");


            CreateTable(
                "dbo.PhaseCycleAggregations",
                c => new
                {
                    BinStartTime = c.DateTime(false),
                    SignalId = c.String(false, maxLength: 10),
                    ApproachId = c.Int(false),
                    PhaseNumber = c.Int(false),
                    RedTime = c.Int(false),
                    YellowTime = c.Int(false),
                    GreenTime = c.Int(false),
                    TotalRedToRedCycles = c.Int(false),
                    TotalGreenToGreenCycles = c.Int(false)
                });
            AddPrimaryKey("dbo.PhaseCycleAggregations", new[] { "BinStartTime", "SignalId", "PhaseNumber" });


            DropTable("dbo.ApproachPcdAggregations");
            CreateTable(
                "dbo.ApproachPcdAggregations",
                c => new
                {
                    BinStartTime = c.DateTime(false),
                    SignalId = c.String(false, maxLength: 10),
                    ApproachId = c.Int(false),
                    PhaseNumber = c.Int(false),
                    IsProtectedPhase = c.Boolean(false),
                    ArrivalsOnGreen = c.Int(false),
                    ArrivalsOnRed = c.Int(false),
                    ArrivalsOnYellow = c.Int(false),
                    Volume = c.Int(false),
                    TotalDelay = c.Int(false)
                });
            AddPrimaryKey("dbo.ApproachPcdAggregations", new[] { "BinStartTime", "SignalId", "PhaseNumber", "IsProtectedPhase" });

            DropTable("dbo.ApproachSpeedAggregations");
            CreateTable(
                "dbo.ApproachSpeedAggregations",
                c => new
                {
                    BinStartTime = c.DateTime(false),
                    SignalId = c.String(false, maxLength: 10),
                    ApproachId = c.Int(false),
                    SummedSpeed = c.Int(false),
                    SpeedVolume = c.Int(false),
                    Speed85th = c.Int(false),
                    Speed15th = c.Int(false)
                });
            AddPrimaryKey("dbo.ApproachSpeedAggregations", new[] { "BinStartTime", "SignalId", "ApproachId" });

            DropTable("dbo.ApproachSplitFailAggregations");
            CreateTable(
                "dbo.ApproachSplitFailAggregations",
                c => new
                {
                    BinStartTime = c.DateTime(false),
                    SignalId = c.String(false, maxLength: 10),
                    ApproachId = c.Int(false),
                    PhaseNumber = c.Int(false),
                    IsProtectedPhase = c.Boolean(false),
                    SplitFailures = c.Int(false),
                    GreenOccupancySum = c.Int(false),
                    RedOccupancySum = c.Int(false),
                    GreenTimeSum = c.Int(false),
                    RedTimeSum = c.Int(false),
                    Cycles = c.Int(false)
                });
            AddPrimaryKey("dbo.ApproachSplitFailAggregations", new[] { "BinStartTime", "SignalId", "ApproachId", "PhaseNumber", "IsProtectedPhase" });

            DropTable("dbo.ApproachYellowRedActivationAggregations");
            CreateTable(
                "dbo.ApproachYellowRedActivationAggregations",
                c => new
                {
                    BinStartTime = c.DateTime(false),
                    SignalId = c.String(false, maxLength: 10),
                    ApproachId = c.Int(false),
                    PhaseNumber = c.Int(false),
                    IsProtectedPhase = c.Boolean(false),
                    SevereRedLightViolations = c.Int(false),
                    TotalRedLightViolations = c.Int(false),
                    YellowActivations = c.Int(false),
                    ViolationTime = c.Int(false),
                    Cycles = c.Int(false)
                });
            AddPrimaryKey("dbo.ApproachYellowRedActivationAggregations", new[] { "BinStartTime", "SignalId", "PhaseNumber", "IsProtectedPhase" });

            DropTable("dbo.DetectorEventCountAggregations");
            CreateTable(
                "dbo.DetectorEventCountAggregations",
                c => new
                {
                    BinStartTime = c.DateTime(false),
                    SignalId = c.String(false, maxLength: 10),
                    ApproachId = c.Int(false),
                    DetectorPrimaryId = c.Int(false),
                    EventCount = c.Int(false)
                });
            AddPrimaryKey("dbo.DetectorEventCountAggregations", new[] { "BinStartTime", "DetectorPrimaryId" });

            DropTable("dbo.PhasePedAggregations");
            CreateTable(
                "dbo.PhasePedAggregations",
                c => new
                {
                    BinStartTime = c.DateTime(false),
                    SignalId = c.String(false, maxLength: 10),
                    PhaseNumber = c.Int(false),
                    PedCycles = c.Int(false),
                    PedDelaySum = c.Int(false),
                    MinPedDelay = c.Int(false),
                    MaxPedDelay = c.Int(false),
                    PedActuations = c.Int(false)
                });
            AddPrimaryKey("dbo.PhasePedAggregations", new[] { "BinStartTime", "SignalId", "PhaseNumber" });

            DropTable("dbo.PhaseTerminationAggregations");
            CreateTable(
                "dbo.PhaseTerminationAggregations",
                c => new
                {
                    BinStartTime = c.DateTime(false),
                    SignalId = c.String(false, maxLength: 10),
                    PhaseNumber = c.Int(false),
                    GapOuts = c.Int(false),
                    ForceOffs = c.Int(false),
                    MaxOuts = c.Int(false),
                    Unknown = c.Int(false)
                });
            AddPrimaryKey("dbo.PhaseTerminationAggregations", new[] { "BinStartTime", "SignalId", "PhaseNumber" });


            DropTable("dbo.PreemptionAggregations");
            CreateTable(
                "dbo.PreemptionAggregations",
                c => new
                {
                    BinStartTime = c.DateTime(false),
                    SignalId = c.String(false, maxLength: 10),
                    PreemptNumber = c.Int(false),
                    PreemptRequests = c.Int(false),
                    PreemptServices = c.Int(false)
                });
            AddPrimaryKey("dbo.PreemptionAggregations", new[] { "BinStartTime", "SignalId", "PreemptNumber" });

            DropTable("dbo.PriorityAggregations");
            CreateTable(
                "dbo.PriorityAggregations",
                c => new
                {
                    BinStartTime = c.DateTime(false),
                    SignalId = c.String(false, maxLength: 10),
                    PriorityNumber = c.Int(false),
                    PriorityRequests = c.Int(false),
                    PriorityServiceEarlyGreen = c.Int(false),
                    PriorityServiceExtendedGreen = c.Int(false)
                });
            AddPrimaryKey("dbo.PriorityAggregations", new[] { "BinStartTime", "SignalId", "PriorityNumber" });

            DropTable("dbo.SignalEventCountAggregations");
            CreateTable(
                "dbo.SignalEventCountAggregations",
                c => new
                {
                    BinStartTime = c.DateTime(false),
                    SignalId = c.String(false, maxLength: 10),
                    EventCount = c.Int(false)
                });
            AddPrimaryKey("dbo.SignalEventCountAggregations", new[] { "BinStartTime", "SignalId" });

            DropTable("dbo.PhaseLeftTurnGapAggregations");
            CreateTable(
                    "dbo.PhaseLeftTurnGapAggregations",
                    c => new
                    {
                        BinStartTime = c.DateTime(nullable: false),
                        SignalId = c.String(nullable: false, maxLength: 10),
                        PhaseNumber = c.Int(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        GapCount1 = c.Int(nullable: false),
                        GapCount2 = c.Int(nullable: false),
                        GapCount3 = c.Int(nullable: false),
                        GapCount4 = c.Int(nullable: false),
                        GapCount5 = c.Int(nullable: false),
                        GapCount6 = c.Int(nullable: false),
                        GapCount7 = c.Int(nullable: false),
                        GapCount8 = c.Int(nullable: false),
                        GapCount9 = c.Int(nullable: false),
                        GapCount10 = c.Int(nullable: false),
                        GapCount11 = c.Int(nullable: false),
                        SumGapDuration1 = c.Double(nullable: false),
                        SumGapDuration2 = c.Double(nullable: false),
                        SumGapDuration3 = c.Double(nullable: false),
                        SumGreenTime = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.BinStartTime, t.SignalId, t.PhaseNumber });
            DropTable("dbo.PhaseSplitMonitorAggregations");
            CreateTable(
                    "dbo.PhaseSplitMonitorAggregations",
                    c => new
                    {
                        BinStartTime = c.DateTime(nullable: false),
                        SignalId = c.String(nullable: false, maxLength: 128),
                        PhaseNumber = c.Int(nullable: false),
                        EightyFifthPercentileSplit = c.Int(nullable: false),
                        SkippedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BinStartTime, t.SignalId, t.PhaseNumber });
            DropTable("dbo.SignalPlanAggregations");
            CreateTable(
                    "dbo.SignalPlanAggregations",
                    c => new
                    {
                        SignalId = c.String(nullable: false, maxLength: 128),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        PlanNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SignalId, t.Start, t.End });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.DetectorAggregations",
                c => new
                    {
                        SignalId = c.String(nullable: false, maxLength: 128),
                        ApproachId = c.Int(nullable: false),
                        BinStartTime = c.DateTime(nullable: false),
                        DetectorPrimaryId = c.Int(nullable: false),
                        Volume = c.Int(nullable: false),
                        MovementTypeId = c.Int(nullable: false),
                        DirectionTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SignalId, t.ApproachId, t.BinStartTime, t.DetectorPrimaryId });
            
        }
    }
}
