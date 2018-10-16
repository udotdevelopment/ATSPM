namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inital : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionLogs",
                c => new
                    {
                        ActionLogID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        AgencyID = c.Int(nullable: false),
                        Comment = c.String(maxLength: 255),
                        SignalID = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ActionLogID)
                .ForeignKey("dbo.ATSPM_Agency", t => t.AgencyID, cascadeDelete: true)
                .Index(t => t.AgencyID);
            
            CreateTable(
                "dbo.Actions",
                c => new
                    {
                        ActionID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ActionID);
            
            CreateTable(
                "dbo.ATSPM_Agency",
                c => new
                    {
                        AgencyID = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.AgencyID);
            
            CreateTable(
                "dbo.MetricTypes",
                c => new
                    {
                        MetricID = c.Int(nullable: false),
                        ChartName = c.String(nullable: false),
                        Abbreviation = c.String(nullable: false),
                        ShowOnWebsite = c.Boolean(nullable: false),
                        ShowOnAggregationSite = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MetricID);
            
            CreateTable(
                "dbo.MetricComments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        VersionID = c.Int(nullable: false),
                        SignalID = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        CommentText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Signals", t => t.VersionID, cascadeDelete: true)
                .Index(t => t.VersionID);
            
            CreateTable(
                "dbo.Signals",
                c => new
                    {
                        VersionID = c.Int(nullable: false, identity: true),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        VersionActionId = c.Int(nullable: false),
                        Note = c.String(nullable: false),
                        Start = c.DateTime(nullable: false),
                        PrimaryName = c.String(nullable: false, maxLength: 100, unicode: false),
                        SecondaryName = c.String(nullable: false, maxLength: 100, unicode: false),
                        IPAddress = c.String(nullable: false, maxLength: 50, unicode: false),
                        Latitude = c.String(nullable: false, maxLength: 30, unicode: false),
                        Longitude = c.String(nullable: false, maxLength: 30, unicode: false),
                        RegionID = c.Int(nullable: false),
                        ControllerTypeID = c.Int(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.VersionID)
                .ForeignKey("dbo.ControllerTypes", t => t.ControllerTypeID)
                .ForeignKey("dbo.Regions", t => t.RegionID, cascadeDelete: true)
                .ForeignKey("dbo.VersionActions", t => t.VersionActionId, cascadeDelete: true)
                .Index(t => t.VersionActionId)
                .Index(t => t.RegionID)
                .Index(t => t.ControllerTypeID);
            
            CreateTable(
                "dbo.Approaches",
                c => new
                    {
                        ApproachID = c.Int(nullable: false, identity: true),
                        SignalID = c.String(),
                        VersionID = c.Int(nullable: false),
                        DirectionTypeID = c.Int(nullable: false),
                        Description = c.String(),
                        MPH = c.Int(),
                        ProtectedPhaseNumber = c.Int(nullable: false),
                        IsProtectedPhaseOverlap = c.Boolean(nullable: false),
                        PermissivePhaseNumber = c.Int(),
                        IsPermissivePhaseOverlap = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ApproachID)
                .ForeignKey("dbo.DirectionTypes", t => t.DirectionTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Signals", t => t.VersionID, cascadeDelete: true)
                .Index(t => t.VersionID)
                .Index(t => t.DirectionTypeID);
            
            CreateTable(
                "dbo.Detectors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DetectorID = c.String(nullable: false, maxLength: 50),
                        DetChannel = c.Int(nullable: false),
                        DistanceFromStopBar = c.Int(),
                        MinSpeedFilter = c.Int(),
                        DateAdded = c.DateTime(nullable: false),
                        DateDisabled = c.DateTime(),
                        LaneNumber = c.Int(),
                        MovementTypeID = c.Int(),
                        LaneTypeID = c.Int(),
                        DecisionPoint = c.Int(),
                        MovementDelay = c.Int(),
                        LatencyCorrection = c.Double(nullable: false),
                        ApproachID = c.Int(nullable: false),
                        DetectionHardwareID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DetectionHardwares", t => t.DetectionHardwareID, cascadeDelete: true)
                .ForeignKey("dbo.LaneTypes", t => t.LaneTypeID)
                .ForeignKey("dbo.MovementTypes", t => t.MovementTypeID)
                .ForeignKey("dbo.Approaches", t => t.ApproachID, cascadeDelete: true)
                .Index(t => t.DetectorID, unique: true, name: "IX_DetectorIDUnique")
                .Index(t => t.MovementTypeID)
                .Index(t => t.LaneTypeID)
                .Index(t => t.ApproachID)
                .Index(t => t.DetectionHardwareID);
            
            CreateTable(
                "dbo.DetectionHardwares",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DetectionTypes",
                c => new
                    {
                        DetectionTypeID = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DetectionTypeID);
            
            CreateTable(
                "dbo.DetectorComments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        ID = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        CommentText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Detectors", t => t.ID, cascadeDelete: true)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.LaneTypes",
                c => new
                    {
                        LaneTypeID = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 30),
                        Abbreviation = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.LaneTypeID);
            
            CreateTable(
                "dbo.MovementTypes",
                c => new
                    {
                        MovementTypeID = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 30),
                        Abbreviation = c.String(nullable: false, maxLength: 5),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MovementTypeID);
            
            CreateTable(
                "dbo.DirectionTypes",
                c => new
                    {
                        DirectionTypeID = c.Int(nullable: false),
                        Description = c.String(maxLength: 30),
                        Abbreviation = c.String(maxLength: 5),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DirectionTypeID);
            
            CreateTable(
                "dbo.ControllerTypes",
                c => new
                    {
                        ControllerTypeID = c.Int(nullable: false),
                        Description = c.String(maxLength: 50, unicode: false),
                        SNMPPort = c.Long(nullable: false),
                        FTPDirectory = c.String(unicode: false),
                        ActiveFTP = c.Boolean(nullable: false),
                        UserName = c.String(maxLength: 50, unicode: false),
                        Password = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ControllerTypeID);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.VersionActions",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ApplicationEvents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        ApplicationName = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        SeverityLevel = c.Int(nullable: false),
                        Class = c.String(),
                        Function = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ApplicationSettings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApplicationID = c.Int(nullable: false),
                        EnableDatbaseArchive = c.Boolean(),
                        SelectedTableScheme = c.Int(),
                        MonthsToKeepIndex = c.Int(),
                        MonthsToKeepData = c.Int(),
                        ArchivePath = c.String(),
                        SelectedDeleteOrMove = c.Int(),
                        NumberOfRows = c.Int(),
                        StartTime = c.Int(),
                        TimeDuration = c.Int(),
                        ImageUrl = c.String(),
                        ImagePath = c.String(),
                        ReCaptchaPublicKey = c.String(),
                        ReCaptchaSecretKey = c.String(),
                        RawDataCountLimit = c.Int(),
                        ConsecutiveCount = c.Int(),
                        MinPhaseTerminations = c.Int(),
                        PercentThreshold = c.Double(),
                        MaxDegreeOfParallelism = c.Int(),
                        ScanDayStartHour = c.Int(),
                        ScanDayEndHour = c.Int(),
                        PreviousDayPMPeakStart = c.Int(),
                        PreviousDayPMPeakEnd = c.Int(),
                        MinimumRecords = c.Int(),
                        WeekdayOnly = c.Boolean(),
                        DefaultEmailAddress = c.String(),
                        FromEmailAddress = c.String(),
                        LowHitThreshold = c.Int(),
                        EmailServer = c.String(),
                        MaximumPedestrianEvents = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Applications", t => t.ApplicationID, cascadeDelete: true)
                .Index(t => t.ApplicationID);
            
            CreateTable(
                "dbo.ApproachCycleAggregations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        RedTime = c.Double(nullable: false),
                        YellowTime = c.Double(nullable: false),
                        GreenTime = c.Double(nullable: false),
                        TotalCycles = c.Int(nullable: false),
                        PedActuations = c.Int(nullable: false),
                        IsProtectedPhase = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, cascadeDelete: true)
                .Index(t => t.ApproachId);
            
            CreateTable(
                "dbo.ApproachEventCountAggregations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        EventCount = c.Int(nullable: false),
                        IsProtectedPhase = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApproachPcdAggregations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        ArrivalsOnGreen = c.Int(nullable: false),
                        ArrivalsOnRed = c.Int(nullable: false),
                        ArrivalsOnYellow = c.Int(nullable: false),
                        IsProtectedPhase = c.Boolean(nullable: false),
                        Volume = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, cascadeDelete: true)
                .Index(t => t.ApproachId);
            
            CreateTable(
                "dbo.ApproachSpeedAggregations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        SummedSpeed = c.Double(nullable: false),
                        SpeedVolume = c.Double(nullable: false),
                        Speed85Th = c.Double(nullable: false),
                        Speed15Th = c.Double(nullable: false),
                        IsProtectedPhase = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, cascadeDelete: true)
                .Index(t => t.ApproachId);
            
            CreateTable(
                "dbo.ApproachSplitFailAggregations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        SplitFailures = c.Int(nullable: false),
                        IsProtectedPhase = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, cascadeDelete: true)
                .Index(t => t.ApproachId);
            
            CreateTable(
                "dbo.ApproachYellowRedActivationAggregations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        SevereRedLightViolations = c.Int(nullable: false),
                        TotalRedLightViolations = c.Int(nullable: false),
                        IsProtectedPhase = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Approaches", t => t.ApproachId, cascadeDelete: true)
                .Index(t => t.ApproachId);
            
            CreateTable(
                "dbo.ATSPM_ExternalLink",
                c => new
                    {
                        ExternalLinkID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Url = c.String(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExternalLinkID);
            
            //CreateTable(
                //"dbo.Controller_Event_Log",
                //c => new
                //    {
                //        Timestamp = c.DateTime(nullable: false),
                //        SignalID = c.String(nullable: false, maxLength: 10),
                //        EventCode = c.Int(nullable: false),
                //        EventParam = c.Int(nullable: false),
                //    })
                //.PrimaryKey(t => new { t.Timestamp, t.SignalID, t.EventCode, t.EventParam });
            
            CreateTable(
                "dbo.DatabaseArchiveExcludedSignals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SignalId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DetectorAggregations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        DetectorPrimaryId = c.Int(nullable: false),
                        Volume = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Detectors", t => t.DetectorPrimaryId, cascadeDelete: true)
                .Index(t => t.DetectorPrimaryId);
            
            CreateTable(
                "dbo.DetectorEventCountAggregations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        DetectorId = c.String(nullable: false),
                        EventCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FAQs",
                c => new
                    {
                        FAQID = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                        Body = c.String(),
                        OrderNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FAQID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Menu",
                c => new
                    {
                        MenuId = c.Int(nullable: false),
                        MenuName = c.String(nullable: false, maxLength: 50),
                        Controller = c.String(nullable: false, maxLength: 50),
                        Action = c.String(nullable: false, maxLength: 50),
                        ParentId = c.Int(nullable: false),
                        Application = c.String(nullable: false, maxLength: 50),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MenuId);
            
            CreateTable(
                "dbo.MetricsFilterTypes",
                c => new
                    {
                        FilterID = c.Int(nullable: false, identity: true),
                        FilterName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FilterID);
            
            CreateTable(
                "dbo.PhasePedAggregations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        SignalId = c.String(nullable: false),
                        PhaseNumber = c.Int(nullable: false),
                        PedCount = c.Int(nullable: false),
                        PedDelay = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PhaseTerminationAggregations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        SignalId = c.String(nullable: false),
                        PhaseNumber = c.Int(nullable: false),
                        GapOuts = c.Int(nullable: false),
                        ForceOffs = c.Int(nullable: false),
                        MaxOuts = c.Int(nullable: false),
                        UnknownTerminationTypes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PreemptionAggregations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        SignalId = c.String(nullable: false, maxLength: 10),
                        VersionId = c.Int(nullable: false),
                        PreemptNumber = c.Int(nullable: false),
                        PreemptRequests = c.Int(nullable: false),
                        PreemptServices = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Signals", t => t.VersionId, cascadeDelete: true)
                .Index(t => t.VersionId);
            
            CreateTable(
                "dbo.PriorityAggregations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        VersionId = c.Int(nullable: false),
                        PriorityNumber = c.Int(nullable: false),
                        TotalCycles = c.Int(nullable: false),
                        PriorityRequests = c.Int(nullable: false),
                        PriorityServiceEarlyGreen = c.Int(nullable: false),
                        PriorityServiceExtendedGreen = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Signals", t => t.VersionId, cascadeDelete: true)
                .Index(t => t.VersionId);
            
            CreateTable(
                "dbo.RoutePhaseDirections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RouteSignalId = c.Int(nullable: false),
                        Phase = c.Int(nullable: false),
                        DirectionTypeId = c.Int(nullable: false),
                        IsOverlap = c.Boolean(nullable: false),
                        IsPrimaryApproach = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DirectionTypes", t => t.DirectionTypeId, cascadeDelete: true)
                .ForeignKey("dbo.RouteSignals", t => t.RouteSignalId, cascadeDelete: true)
                .Index(t => t.RouteSignalId)
                .Index(t => t.DirectionTypeId);
            
            CreateTable(
                "dbo.RouteSignals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RouteId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        SignalId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Routes", t => t.RouteId, cascadeDelete: true)
                .Index(t => t.RouteId);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RouteName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SignalEventCountAggregations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        SignalId = c.String(nullable: false),
                        EventCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Speed_Events",
                c => new
                    {
                        DetectorID = c.String(nullable: false, maxLength: 50),
                        MPH = c.Int(nullable: false),
                        KPH = c.Int(nullable: false),
                        timestamp = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => new { t.DetectorID, t.MPH, t.KPH, t.timestamp });
            
            CreateTable(
                "dbo.SPMWatchDogErrorEvents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        SignalID = c.String(nullable: false),
                        DetectorID = c.String(),
                        Direction = c.String(nullable: false),
                        Phase = c.Int(nullable: false),
                        ErrorCode = c.Int(nullable: false),
                        Message = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StatusOfProcessedTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartitionedTableName = c.String(nullable: false),
                        TimeEntered = c.DateTime(nullable: false),
                        PartitionName = c.String(),
                        PartitionYear = c.Int(nullable: false),
                        PartitionMonth = c.Int(nullable: false),
                        FunctionOrProcedure = c.String(),
                        SQLStatementOrMessage = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TablePartitionProcesseds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SwapTableName = c.String(nullable: false),
                        PartitionNumber = c.Int(nullable: false),
                        PartitionBeginYear = c.Int(nullable: false),
                        PartitionBeginMonth = c.Int(nullable: false),
                        FileGroupName = c.String(nullable: false),
                        PhysicalFileName = c.String(),
                        IndexRemoved = c.Boolean(nullable: false),
                        SwappedTableRemoved = c.Boolean(nullable: false),
                        TimeIndexdropped = c.DateTime(nullable: false),
                        TimeSwappedTableDropped = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ToBeProcessedTableIndexes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableId = c.Int(nullable: false),
                        IndexId = c.Int(nullable: false),
                        ClusteredText = c.String(),
                        TextForIndex = c.String(),
                        IndexName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ToBeProcessededTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartitionedTableName = c.String(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        PreserveDataSelect = c.String(),
                        TableId = c.Int(nullable: false),
                        PreserveDataWhere = c.String(nullable: false),
                        InsertValues = c.String(),
                        DataBaseName = c.String(),
                        Verbose = c.Boolean(nullable: false),
                        CreateColumns4Table = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ReceiveAlerts = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ActionLogActions",
                c => new
                    {
                        ActionLog_ActionLogID = c.Int(nullable: false),
                        Action_ActionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ActionLog_ActionLogID, t.Action_ActionID })
                .ForeignKey("dbo.ActionLogs", t => t.ActionLog_ActionLogID, cascadeDelete: true)
                .ForeignKey("dbo.Actions", t => t.Action_ActionID, cascadeDelete: true)
                .Index(t => t.ActionLog_ActionLogID)
                .Index(t => t.Action_ActionID);
            
            CreateTable(
                "dbo.MetricCommentMetricTypes",
                c => new
                    {
                        MetricComment_CommentID = c.Int(nullable: false),
                        MetricType_MetricID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MetricComment_CommentID, t.MetricType_MetricID })
                .ForeignKey("dbo.MetricComments", t => t.MetricComment_CommentID, cascadeDelete: true)
                .ForeignKey("dbo.MetricTypes", t => t.MetricType_MetricID, cascadeDelete: true)
                .Index(t => t.MetricComment_CommentID)
                .Index(t => t.MetricType_MetricID);
            
            CreateTable(
                "dbo.DetectionTypeMetricTypes",
                c => new
                    {
                        DetectionType_DetectionTypeID = c.Int(nullable: false),
                        MetricType_MetricID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DetectionType_DetectionTypeID, t.MetricType_MetricID })
                .ForeignKey("dbo.DetectionTypes", t => t.DetectionType_DetectionTypeID, cascadeDelete: true)
                .ForeignKey("dbo.MetricTypes", t => t.MetricType_MetricID, cascadeDelete: true)
                .Index(t => t.DetectionType_DetectionTypeID)
                .Index(t => t.MetricType_MetricID);
            
            CreateTable(
                "dbo.DetectionTypeDetector",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        DetectionTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID, t.DetectionTypeID })
                .ForeignKey("dbo.Detectors", t => t.ID, cascadeDelete: true)
                .ForeignKey("dbo.DetectionTypes", t => t.DetectionTypeID, cascadeDelete: true)
                .Index(t => t.ID)
                .Index(t => t.DetectionTypeID);
            
            CreateTable(
                "dbo.ActionLogMetricTypes",
                c => new
                    {
                        ActionLog_ActionLogID = c.Int(nullable: false),
                        MetricType_MetricID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ActionLog_ActionLogID, t.MetricType_MetricID })
                .ForeignKey("dbo.ActionLogs", t => t.ActionLog_ActionLogID, cascadeDelete: true)
                .ForeignKey("dbo.MetricTypes", t => t.MetricType_MetricID, cascadeDelete: true)
                .Index(t => t.ActionLog_ActionLogID)
                .Index(t => t.MetricType_MetricID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.RouteSignals", "RouteId", "dbo.Routes");
            DropForeignKey("dbo.RoutePhaseDirections", "RouteSignalId", "dbo.RouteSignals");
            DropForeignKey("dbo.RoutePhaseDirections", "DirectionTypeId", "dbo.DirectionTypes");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PriorityAggregations", "VersionId", "dbo.Signals");
            DropForeignKey("dbo.PreemptionAggregations", "VersionId", "dbo.Signals");
            DropForeignKey("dbo.DetectorAggregations", "DetectorPrimaryId", "dbo.Detectors");
            DropForeignKey("dbo.ApproachYellowRedActivationAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachSplitFailAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachSpeedAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachPcdAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApproachCycleAggregations", "ApproachId", "dbo.Approaches");
            DropForeignKey("dbo.ApplicationSettings", "ApplicationID", "dbo.Applications");
            DropForeignKey("dbo.ActionLogMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropForeignKey("dbo.ActionLogMetricTypes", "ActionLog_ActionLogID", "dbo.ActionLogs");
            DropForeignKey("dbo.Signals", "VersionActionId", "dbo.VersionActions");
            DropForeignKey("dbo.Signals", "RegionID", "dbo.Regions");
            DropForeignKey("dbo.Signals", "ControllerTypeID", "dbo.ControllerTypes");
            DropForeignKey("dbo.MetricComments", "VersionID", "dbo.Signals");
            DropForeignKey("dbo.Approaches", "VersionID", "dbo.Signals");
            DropForeignKey("dbo.Approaches", "DirectionTypeID", "dbo.DirectionTypes");
            DropForeignKey("dbo.Detectors", "ApproachID", "dbo.Approaches");
            DropForeignKey("dbo.Detectors", "MovementTypeID", "dbo.MovementTypes");
            DropForeignKey("dbo.Detectors", "LaneTypeID", "dbo.LaneTypes");
            DropForeignKey("dbo.DetectorComments", "ID", "dbo.Detectors");
            DropForeignKey("dbo.DetectionTypeDetector", "DetectionTypeID", "dbo.DetectionTypes");
            DropForeignKey("dbo.DetectionTypeDetector", "ID", "dbo.Detectors");
            DropForeignKey("dbo.DetectionTypeMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropForeignKey("dbo.DetectionTypeMetricTypes", "DetectionType_DetectionTypeID", "dbo.DetectionTypes");
            DropForeignKey("dbo.Detectors", "DetectionHardwareID", "dbo.DetectionHardwares");
            DropForeignKey("dbo.MetricCommentMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropForeignKey("dbo.MetricCommentMetricTypes", "MetricComment_CommentID", "dbo.MetricComments");
            DropForeignKey("dbo.ActionLogs", "AgencyID", "dbo.ATSPM_Agency");
            DropForeignKey("dbo.ActionLogActions", "Action_ActionID", "dbo.Actions");
            DropForeignKey("dbo.ActionLogActions", "ActionLog_ActionLogID", "dbo.ActionLogs");
            DropIndex("dbo.ActionLogMetricTypes", new[] { "MetricType_MetricID" });
            DropIndex("dbo.ActionLogMetricTypes", new[] { "ActionLog_ActionLogID" });
            DropIndex("dbo.DetectionTypeDetector", new[] { "DetectionTypeID" });
            DropIndex("dbo.DetectionTypeDetector", new[] { "ID" });
            DropIndex("dbo.DetectionTypeMetricTypes", new[] { "MetricType_MetricID" });
            DropIndex("dbo.DetectionTypeMetricTypes", new[] { "DetectionType_DetectionTypeID" });
            DropIndex("dbo.MetricCommentMetricTypes", new[] { "MetricType_MetricID" });
            DropIndex("dbo.MetricCommentMetricTypes", new[] { "MetricComment_CommentID" });
            DropIndex("dbo.ActionLogActions", new[] { "Action_ActionID" });
            DropIndex("dbo.ActionLogActions", new[] { "ActionLog_ActionLogID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.RouteSignals", new[] { "RouteId" });
            DropIndex("dbo.RoutePhaseDirections", new[] { "DirectionTypeId" });
            DropIndex("dbo.RoutePhaseDirections", new[] { "RouteSignalId" });
            DropIndex("dbo.PriorityAggregations", new[] { "VersionId" });
            DropIndex("dbo.PreemptionAggregations", new[] { "VersionId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.DetectorAggregations", new[] { "DetectorPrimaryId" });
            DropIndex("dbo.ApproachYellowRedActivationAggregations", new[] { "ApproachId" });
            DropIndex("dbo.ApproachSplitFailAggregations", new[] { "ApproachId" });
            DropIndex("dbo.ApproachSpeedAggregations", new[] { "ApproachId" });
            DropIndex("dbo.ApproachPcdAggregations", new[] { "ApproachId" });
            DropIndex("dbo.ApproachCycleAggregations", new[] { "ApproachId" });
            DropIndex("dbo.ApplicationSettings", new[] { "ApplicationID" });
            DropIndex("dbo.DetectorComments", new[] { "ID" });
            DropIndex("dbo.Detectors", new[] { "DetectionHardwareID" });
            DropIndex("dbo.Detectors", new[] { "ApproachID" });
            DropIndex("dbo.Detectors", new[] { "LaneTypeID" });
            DropIndex("dbo.Detectors", new[] { "MovementTypeID" });
            DropIndex("dbo.Detectors", "IX_DetectorIDUnique");
            DropIndex("dbo.Approaches", new[] { "DirectionTypeID" });
            DropIndex("dbo.Approaches", new[] { "VersionID" });
            DropIndex("dbo.Signals", new[] { "ControllerTypeID" });
            DropIndex("dbo.Signals", new[] { "RegionID" });
            DropIndex("dbo.Signals", new[] { "VersionActionId" });
            DropIndex("dbo.MetricComments", new[] { "VersionID" });
            DropIndex("dbo.ActionLogs", new[] { "AgencyID" });
            DropTable("dbo.ActionLogMetricTypes");
            DropTable("dbo.DetectionTypeDetector");
            DropTable("dbo.DetectionTypeMetricTypes");
            DropTable("dbo.MetricCommentMetricTypes");
            DropTable("dbo.ActionLogActions");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ToBeProcessededTables");
            DropTable("dbo.ToBeProcessedTableIndexes");
            DropTable("dbo.TablePartitionProcesseds");
            DropTable("dbo.StatusOfProcessedTables");
            DropTable("dbo.SPMWatchDogErrorEvents");
            DropTable("dbo.Speed_Events");
            DropTable("dbo.SignalEventCountAggregations");
            DropTable("dbo.Routes");
            DropTable("dbo.RouteSignals");
            DropTable("dbo.RoutePhaseDirections");
            DropTable("dbo.PriorityAggregations");
            DropTable("dbo.PreemptionAggregations");
            DropTable("dbo.PhaseTerminationAggregations");
            DropTable("dbo.PhasePedAggregations");
            DropTable("dbo.MetricsFilterTypes");
            DropTable("dbo.Menu");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.FAQs");
            DropTable("dbo.DetectorEventCountAggregations");
            DropTable("dbo.DetectorAggregations");
            DropTable("dbo.DatabaseArchiveExcludedSignals");
            DropTable("dbo.Controller_Event_Log");
            DropTable("dbo.ATSPM_ExternalLink");
            DropTable("dbo.ApproachYellowRedActivationAggregations");
            DropTable("dbo.ApproachSplitFailAggregations");
            DropTable("dbo.ApproachSpeedAggregations");
            DropTable("dbo.ApproachPcdAggregations");
            DropTable("dbo.ApproachEventCountAggregations");
            DropTable("dbo.ApproachCycleAggregations");
            DropTable("dbo.ApplicationSettings");
            DropTable("dbo.Applications");
            DropTable("dbo.ApplicationEvents");
            DropTable("dbo.VersionActions");
            DropTable("dbo.Regions");
            DropTable("dbo.ControllerTypes");
            DropTable("dbo.DirectionTypes");
            DropTable("dbo.MovementTypes");
            DropTable("dbo.LaneTypes");
            DropTable("dbo.DetectorComments");
            DropTable("dbo.DetectionTypes");
            DropTable("dbo.DetectionHardwares");
            DropTable("dbo.Detectors");
            DropTable("dbo.Approaches");
            DropTable("dbo.Signals");
            DropTable("dbo.MetricComments");
            DropTable("dbo.MetricTypes");
            DropTable("dbo.ATSPM_Agency");
            DropTable("dbo.Actions");
            DropTable("dbo.ActionLogs");
        }
    }
}
