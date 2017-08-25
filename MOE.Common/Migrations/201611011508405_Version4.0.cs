namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version40 : DbMigration
    {
        public override void Up()
        {
            //Only used for Upgrades

            DropForeignKey("dbo.ApproachRouteDetail", "FK_ApproachRouteDetail_Signals");
            DropForeignKey("dbo.Action_Log_Actions", "FK_Action_Log_Actions_Action_Log");
            DropForeignKey("dbo.Action_Log_Metrics", "FK_Action_Log_Metrics_Action_Log");
            DropForeignKey("dbo.Detector_Comment", "FK_Detector_Comment_Graph_Detectors");
            DropForeignKey("dbo.Detector_Error", "FK_Detector_Error_Graph_Detectors");
            DropForeignKey("dbo.Route_Detectors", "FK_Route_Detectors_Graph_Detectors");
            DropForeignKey("dbo.ApproachRouteDetail", "FK_dbo.ApproachRouteDetail_dbo.ApproachRoute_ApproachRouteId");
            DropForeignKey("dbo.Action_Log", "FK_Action_Log_Signals");
            DropForeignKey("dbo.LastUpdate", "FK_Lastupdate_Signals");
            DropForeignKey("dbo.Graph_Detectors", "FK_Graph_Detectors_Signals");
            DropForeignKey("dbo.Action_Log", "FK_Action_Log_Agency_List");
            DropForeignKey("dbo.Action_Log", "FK_Action_Log_Signals");
            DropForeignKey("dbo.Action_Log_Actions", "FK_Action_Log_Actions");
            DropForeignKey("dbo.Action_Log_Actions", "FK_Action_Log_Actions_Action_Log");
            Sql("TRUNCATE TABLE ApproachRouteDetail");
            Sql("TRUNCATE TABLE Signals");
            Sql("TRUNCATE TABLE Route_Detectors");
            Sql("Truncate TABLE Graph_Detectors");
            Sql("Truncate TABLE SPM_Comment");
            Sql("Truncate TABLE Graph_Detectors");
            Sql("Truncate TABLE ApproachRoute");
            Sql("Truncate TABLE Action_Log");
            Sql("Truncate TABLE Action_Log_Action_List");
            Sql("Truncate TABLE Action_Log_Agency_List");
            Sql("Truncate TABLE Action_Log_Actions");
            Sql("Truncate TABLE MOE_Users");

            DropForeignKey("dbo.Action_Log_Actions", "Action_Id", "dbo.Action_Log_Action_List");
            DropForeignKey("dbo.Action_Log_Actions", "Action_Log_Id", "dbo.Action_Log");
            DropForeignKey("dbo.Action_Log", "Agency", "dbo.Action_Log_Agency_List");
            DropForeignKey("dbo.Action_Log_Metrics", "Metric_Id", "dbo.Action_Log_Metric_List");
            DropForeignKey("dbo.Action_Log_Metrics", "Action_Log_Id", "dbo.Action_Log");
            DropForeignKey("dbo.Action_Log", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.ApproachRouteDetail", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.Detector_Comment", "DetectorID", "dbo.Graph_Detectors");
            DropForeignKey("dbo.Detector_Error", "DetectorID", "dbo.Graph_Detectors");
            DropForeignKey("dbo.Route_Detectors", "DetectorID", "dbo.Graph_Detectors");
            DropForeignKey("dbo.Graph_Detectors", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.Lastupdate", "SignalID", "dbo.Signals");
            DropIndex("dbo.Action_Log", new[] { "Agency" });
            DropIndex("dbo.Action_Log", new[] { "SignalID" });
            DropIndex("dbo.Action_Log_Actions", new[] { "Action_Log_Id" });
            DropIndex("dbo.Action_Log_Actions", new[] { "Action_Id" });
            DropIndex("dbo.Action_Log_Metrics", new[] { "Action_Log_Id" });
            DropIndex("dbo.Action_Log_Metrics", new[] { "Metric_Id" });
            DropIndex("dbo.ApproachRouteDetail", new[] { "SignalID" });
            DropIndex("dbo.Graph_Detectors", new[] { "SignalID" });
            DropIndex("dbo.Detector_Comment", new[] { "DetectorID" });
            DropIndex("dbo.Detector_Error", new[] { "DetectorID" });
            DropIndex("dbo.Lastupdate", new[] { "SignalID" });
           // DropPrimaryKey("dbo.ApproachRouteDetail");
            CreateTable(
                "dbo.ActionLogs",
                c => new
                    {
                        ActionLogID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        AgencyID = c.Int(nullable: false),
                        Comment = c.String(maxLength: 255),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ActionLogID)
                .ForeignKey("dbo.Agencies", t => t.AgencyID, cascadeDelete: true)
                .ForeignKey("dbo.Signals", t => t.SignalID)
                .Index(t => t.AgencyID)
                .Index(t => t.SignalID);
            
            CreateTable(
                "dbo.Actions",
                c => new
                    {
                        ActionID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ActionID);
            
            CreateTable(
                "dbo.Agencies",
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
                        MetricID = c.Int(nullable: false, identity: true),
                        ChartName = c.String(nullable: false),
                        Abbreviation = c.String(nullable: false),
                        ShowOnWebsite = c.Boolean(nullable: false),
                        DetectionType_DetectionTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MetricID)
                .ForeignKey("dbo.DetectionTypes", t => t.DetectionType_DetectionTypeID, cascadeDelete: true)
                .Index(t => t.DetectionType_DetectionTypeID);
            
            CreateTable(
                "dbo.MetricComments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        TimeStamp = c.DateTime(nullable: false),
                        CommentText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Signals", t => t.SignalID, cascadeDelete: true)
                .Index(t => t.SignalID);
            
            CreateTable(
                "dbo.Approaches",
                c => new
                    {
                        ApproachID = c.Int(nullable: false, identity: true),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        DirectionTypeID = c.Int(nullable: false),
                        Description = c.String(),
                        MPH = c.Int(),
                        ProtectedPhaseNumber = c.Int(nullable: false),
                        IsProtectedPhaseOverlap = c.Boolean(nullable: false),
                        PermissivePhaseNumber = c.Int(),
                    })
                .PrimaryKey(t => t.ApproachID)
                .ForeignKey("dbo.DirectionTypes", t => t.DirectionTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Signals", t => t.SignalID, cascadeDelete: true)
                .Index(t => t.SignalID)
                .Index(t => t.DirectionTypeID);
            
            CreateTable(
                "dbo.Detectors",
                c => new
                    {
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
                        ApproachID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DetectorID)
                .ForeignKey("dbo.LaneTypes", t => t.LaneTypeID)
                .ForeignKey("dbo.MovementTypes", t => t.MovementTypeID)
                .ForeignKey("dbo.Approaches", t => t.ApproachID, cascadeDelete: true)
                .Index(t => t.MovementTypeID)
                .Index(t => t.LaneTypeID)
                .Index(t => t.ApproachID);
            
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
                        DetectorID = c.String(nullable: false, maxLength: 50),
                        TimeStamp = c.DateTime(nullable: false),
                        CommentText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Detectors", t => t.DetectorID, cascadeDelete: true)
                .Index(t => t.DetectorID);
            
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
                    })
                .PrimaryKey(t => t.MovementTypeID);
            
            CreateTable(
                "dbo.DirectionTypes",
                c => new
                    {
                        DirectionTypeID = c.Int(nullable: false),
                        Description = c.String(maxLength: 30),
                        Abbreviation = c.String(maxLength: 5),
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
                "dbo.ApplicationEvents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        ApplicationName = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        SeverityLevel = c.Int(nullable: false),
                        Class = c.String(maxLength: 50),
                        Function = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ExternalLinks",
                c => new
                    {
                        ExternalLinkID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Url = c.String(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExternalLinkID);
            
            CreateTable(
                "dbo.FAQs",
                c => new
                    {
                        FAQID = c.Int(nullable: false, identity: true),
                        Header = c.String(nullable: false),
                        Body = c.String(nullable: false),
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
                "dbo.LastUpdates",
                c => new
                    {
                        UpdateID = c.Int(nullable: false, identity: true),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        LastUpdateTime = c.DateTime(),
                        LastErrorTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.UpdateID);
            
            CreateTable(
                "dbo.MetricsFilterTypes",
                c => new
                    {
                        FilterID = c.Int(nullable: false, identity: true),
                        FilterName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FilterID);
            
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
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RecieveAlerts = c.Boolean(nullable: false),
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
                "dbo.DetectionTypeDetector",
                c => new
                    {
                        DetectorID = c.String(nullable: false, maxLength: 50),
                        DetectionTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DetectorID, t.DetectionTypeID })
                .ForeignKey("dbo.Detectors", t => t.DetectorID, cascadeDelete: true)
                .ForeignKey("dbo.DetectionTypes", t => t.DetectionTypeID, cascadeDelete: true)
                .Index(t => t.DetectorID)
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
            
            AddColumn("dbo.Signals", "PrimaryName", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.Signals", "SecondaryName", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.Signals", "IPAddress", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AddColumn("dbo.Signals", "RegionID", c => c.Int(nullable: false));
            AddColumn("dbo.Signals", "ControllerTypeID", c => c.Int(nullable: false));
            AddColumn("dbo.Signals", "Enabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApproachRouteDetail", "RouteDetailID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.ApproachRouteDetail", "ApproachID", c => c.Int(nullable: false));
            AddColumn("dbo.Menu", "Controller", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Menu", "Action", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Signals", "Latitude", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AlterColumn("dbo.Signals", "Longitude", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AlterColumn("dbo.ApproachRouteDetail", "ApproachOrder", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ApproachRouteDetail", "RouteDetailID");
            CreateIndex("dbo.Signals", "RegionID");
            CreateIndex("dbo.Signals", "ControllerTypeID");
            CreateIndex("dbo.ApproachRouteDetail", "ApproachID");
            AddForeignKey("dbo.ApproachRouteDetail", "ApproachID", "dbo.Approaches", "ApproachID", cascadeDelete: true);
            AddForeignKey("dbo.Signals", "ControllerTypeID", "dbo.ControllerTypes", "ControllerTypeID");
            AddForeignKey("dbo.Signals", "RegionID", "dbo.Region", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Route_Detectors", "DetectorID", "dbo.Detectors", "DetectorID", cascadeDelete: true);
            DropColumn("dbo.Signals", "Primary_Name");
            DropColumn("dbo.Signals", "Secondary_Name");
            DropColumn("dbo.Signals", "IP_Address");
            DropColumn("dbo.Signals", "Region");
            DropColumn("dbo.Signals", "Controller_Type");
            DropColumn("dbo.Signals", "Collection_Frequency");
            DropColumn("dbo.ApproachRouteDetail", "SignalID");
            DropColumn("dbo.ApproachRouteDetail", "Direction");
            DropColumn("dbo.Menu", "MenuLocation");
            DropTable("dbo.Action_Log");
            DropTable("dbo.Action_Log_Actions");
            DropTable("dbo.Action_Log_Action_List");
            DropTable("dbo.Action_Log_Agency_List");
            DropTable("dbo.Action_Log_Metrics");
            DropTable("dbo.Action_Log_Metric_List");
            DropTable("dbo.Graph_Detectors");
            DropTable("dbo.Detector_Comment");
            DropTable("dbo.Detector_Error");
            DropTable("dbo.Lastupdate");
            DropTable("dbo.Alert_Recipients");
            DropTable("dbo.Alert_Types");
            DropTable("dbo.ApproachDirection");
            DropTable("dbo.Archived_Metrics_Temp");
            DropTable("dbo.Controller_Type");
            DropTable("dbo.Error_Types");
            DropTable("dbo.MOE_Users");
            DropTable("dbo.SPM_Comment");
            DropTable("dbo.SPM_Error");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SPM_Error",
                c => new
                    {
                        ErrorID = c.Long(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        ErrorType = c.Int(nullable: false),
                        Param1 = c.String(maxLength: 50, unicode: false),
                        Param2 = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ErrorID);
            
            CreateTable(
                "dbo.SPM_Comment",
                c => new
                    {
                        CommentID = c.Long(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        Entity = c.String(nullable: false, maxLength: 50, unicode: false),
                        ChartType = c.Int(nullable: false),
                        Comment = c.String(nullable: false, unicode: false),
                        EntityType = c.Int(),
                    })
                .PrimaryKey(t => t.CommentID);
            
            CreateTable(
                "dbo.MOE_Users",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        ReceiveAlerts = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.ID, t.Name, t.Email, t.Password });
            
            CreateTable(
                "dbo.Error_Types",
                c => new
                    {
                        ErrorType = c.Int(nullable: false),
                        ErrorDescription = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ErrorType);
            
            CreateTable(
                "dbo.Controller_Type",
                c => new
                    {
                        TypeID = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 50, unicode: false),
                        SNMPPort = c.Long(nullable: false),
                        FTPDirectory = c.String(nullable: false, maxLength: 128, unicode: false),
                        ActiveFTP = c.Boolean(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Password = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.TypeID, t.Description, t.SNMPPort, t.FTPDirectory, t.ActiveFTP, t.UserName, t.Password });
            
            CreateTable(
                "dbo.Archived_Metrics_Temp",
                c => new
                    {
                        Timestamp = c.DateTime(nullable: false),
                        DetectorID = c.String(nullable: false, maxLength: 50),
                        BinSize = c.Int(nullable: false),
                        Volume = c.Int(),
                        speed = c.Int(),
                        delay = c.Int(),
                        AoR = c.Int(),
                        SpeedHits = c.Int(),
                        BinGreenTime = c.Int(),
                        BinYellowTime = c.Int(),
                        BinRedTime = c.Int(),
                    })
                .PrimaryKey(t => new { t.Timestamp, t.DetectorID, t.BinSize });
            
            CreateTable(
                "dbo.ApproachDirection",
                c => new
                    {
                        DirectionID = c.Int(nullable: false),
                        DirectionName = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.DirectionID, t.DirectionName });
            
            CreateTable(
                "dbo.Alert_Types",
                c => new
                    {
                        AlertID = c.Int(nullable: false),
                        Alert_Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.AlertID, t.Alert_Description });
            
            CreateTable(
                "dbo.Alert_Recipients",
                c => new
                    {
                        RowID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(),
                        AlertID = c.Int(),
                    })
                .PrimaryKey(t => t.RowID);
            
            CreateTable(
                "dbo.Lastupdate",
                c => new
                    {
                        SignalID = c.String(nullable: false, maxLength: 10),
                        LastUpdateTime = c.DateTime(),
                        LastErrorTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.SignalID);
            
            CreateTable(
                "dbo.Detector_Error",
                c => new
                    {
                        ErrorID = c.Int(nullable: false),
                        DetectorID = c.String(nullable: false, maxLength: 50),
                        Timestamp = c.DateTime(),
                        ErrorType = c.Int(),
                        Phase = c.Int(),
                    })
                .PrimaryKey(t => new { t.ErrorID, t.DetectorID });
            
            CreateTable(
                "dbo.Detector_Comment",
                c => new
                    {
                        CommentId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Comment = c.String(nullable: false, maxLength: 500),
                        DetectorID = c.String(nullable: false, maxLength: 50),
                        SignalID = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => new { t.CommentId, t.Date, t.Comment, t.DetectorID, t.SignalID });
            
            CreateTable(
                "dbo.Graph_Detectors",
                c => new
                    {
                        DetectorID = c.String(nullable: false, maxLength: 50),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        Lane = c.String(maxLength: 50, unicode: false),
                        Phase = c.String(nullable: false, maxLength: 50, unicode: false),
                        Loops = c.String(maxLength: 50, unicode: false),
                        Comments = c.String(maxLength: 500, unicode: false),
                        Direction = c.String(nullable: false, maxLength: 50, unicode: false),
                        Det_Channel = c.Int(nullable: false),
                        IPaddr = c.String(maxLength: 50, unicode: false),
                        DistanceFromStopBar = c.Int(),
                        Port = c.Long(),
                        MPH = c.Int(),
                        Decision_Point = c.Int(),
                        Region = c.Int(),
                        Movement_Delay = c.Int(),
                        Min_Speed_Filter = c.Int(),
                        Has_Speed_Detector = c.Boolean(),
                        Has_PCD = c.Boolean(),
                        Monitor_Date = c.DateTime(),
                        Is_Overlap = c.Boolean(),
                        Has_Phase_Data = c.Boolean(),
                        Has_TMC = c.Boolean(),
                        TMC_Lane_Type = c.String(maxLength: 50, unicode: false),
                        Date_Added = c.DateTime(),
                        Has_RLM = c.Boolean(),
                        Perm_Phase = c.String(maxLength: 50, unicode: false),
                        Has_SplitFail = c.Boolean(),
                    })
                .PrimaryKey(t => t.DetectorID);
            
            CreateTable(
                "dbo.Action_Log_Metric_List",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Action_Log_Metrics",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Action_Log_Id = c.Int(nullable: false),
                        Metric_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.Action_Log_Id, t.Metric_Id });
            
            CreateTable(
                "dbo.Action_Log_Agency_List",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Action_Log_Action_List",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Action_Log_Actions",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Action_Log_Id = c.Int(nullable: false),
                        Action_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID, t.Action_Log_Id, t.Action_Id });
            
            CreateTable(
                "dbo.Action_Log",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        Agency = c.Int(),
                        Comment = c.String(maxLength: 255),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        Name = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Menu", "MenuLocation", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.ApproachRouteDetail", "Direction", c => c.String(maxLength: 15));
            AddColumn("dbo.ApproachRouteDetail", "SignalID", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Signals", "Collection_Frequency", c => c.Int());
            AddColumn("dbo.Signals", "Controller_Type", c => c.Int());
            AddColumn("dbo.Signals", "Region", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.Signals", "IP_Address", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.Signals", "Secondary_Name", c => c.String(maxLength: 30, unicode: false));
            AddColumn("dbo.Signals", "Primary_Name", c => c.String(maxLength: 30, unicode: false));
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Route_Detectors", "DetectorID", "dbo.Detectors");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ActionLogMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropForeignKey("dbo.ActionLogMetricTypes", "ActionLog_ActionLogID", "dbo.ActionLogs");
            DropForeignKey("dbo.Signals", "RegionID", "dbo.Region");
            DropForeignKey("dbo.Signals", "ControllerTypeID", "dbo.ControllerTypes");
            DropForeignKey("dbo.MetricComments", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.Approaches", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.Approaches", "DirectionTypeID", "dbo.DirectionTypes");
            DropForeignKey("dbo.Detectors", "ApproachID", "dbo.Approaches");
            DropForeignKey("dbo.Detectors", "MovementTypeID", "dbo.MovementTypes");
            DropForeignKey("dbo.Detectors", "LaneTypeID", "dbo.LaneTypes");
            DropForeignKey("dbo.DetectorComments", "DetectorID", "dbo.Detectors");
            DropForeignKey("dbo.DetectionTypeDetector", "DetectionTypeID", "dbo.DetectionTypes");
            DropForeignKey("dbo.DetectionTypeDetector", "DetectorID", "dbo.Detectors");
            DropForeignKey("dbo.MetricTypes", "DetectionType_DetectionTypeID", "dbo.DetectionTypes");
            DropForeignKey("dbo.ApproachRouteDetail", "ApproachID", "dbo.Approaches");
            DropForeignKey("dbo.ActionLogs", "SignalID", "dbo.Signals");
            DropForeignKey("dbo.MetricCommentMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropForeignKey("dbo.MetricCommentMetricTypes", "MetricComment_CommentID", "dbo.MetricComments");
            DropForeignKey("dbo.ActionLogs", "AgencyID", "dbo.Agencies");
            DropForeignKey("dbo.ActionLogActions", "Action_ActionID", "dbo.Actions");
            DropForeignKey("dbo.ActionLogActions", "ActionLog_ActionLogID", "dbo.ActionLogs");
            DropIndex("dbo.ActionLogMetricTypes", new[] { "MetricType_MetricID" });
            DropIndex("dbo.ActionLogMetricTypes", new[] { "ActionLog_ActionLogID" });
            DropIndex("dbo.DetectionTypeDetector", new[] { "DetectionTypeID" });
            DropIndex("dbo.DetectionTypeDetector", new[] { "DetectorID" });
            DropIndex("dbo.MetricCommentMetricTypes", new[] { "MetricType_MetricID" });
            DropIndex("dbo.MetricCommentMetricTypes", new[] { "MetricComment_CommentID" });
            DropIndex("dbo.ActionLogActions", new[] { "Action_ActionID" });
            DropIndex("dbo.ActionLogActions", new[] { "ActionLog_ActionLogID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.DetectorComments", new[] { "DetectorID" });
            DropIndex("dbo.Detectors", new[] { "ApproachID" });
            DropIndex("dbo.Detectors", new[] { "LaneTypeID" });
            DropIndex("dbo.Detectors", new[] { "MovementTypeID" });
            DropIndex("dbo.ApproachRouteDetail", new[] { "ApproachID" });
            DropIndex("dbo.Approaches", new[] { "DirectionTypeID" });
            DropIndex("dbo.Approaches", new[] { "SignalID" });
            DropIndex("dbo.Signals", new[] { "ControllerTypeID" });
            DropIndex("dbo.Signals", new[] { "RegionID" });
            DropIndex("dbo.MetricComments", new[] { "SignalID" });
            DropIndex("dbo.MetricTypes", new[] { "DetectionType_DetectionTypeID" });
            DropIndex("dbo.ActionLogs", new[] { "SignalID" });
            DropIndex("dbo.ActionLogs", new[] { "AgencyID" });
            DropPrimaryKey("dbo.ApproachRouteDetail");
            AlterColumn("dbo.ApproachRouteDetail", "ApproachOrder", c => c.Int());
            AlterColumn("dbo.Signals", "Longitude", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.Signals", "Latitude", c => c.String(maxLength: 30, unicode: false));
            DropColumn("dbo.Menu", "Action");
            DropColumn("dbo.Menu", "Controller");
            DropColumn("dbo.ApproachRouteDetail", "ApproachID");
            DropColumn("dbo.ApproachRouteDetail", "RouteDetailID");
            DropColumn("dbo.Signals", "Enabled");
            DropColumn("dbo.Signals", "ControllerTypeID");
            DropColumn("dbo.Signals", "RegionID");
            DropColumn("dbo.Signals", "IPAddress");
            DropColumn("dbo.Signals", "SecondaryName");
            DropColumn("dbo.Signals", "PrimaryName");
            DropTable("dbo.ActionLogMetricTypes");
            DropTable("dbo.DetectionTypeDetector");
            DropTable("dbo.MetricCommentMetricTypes");
            DropTable("dbo.ActionLogActions");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.SignalWithDetections");
            DropTable("dbo.MetricsFilterTypes");
            DropTable("dbo.LastUpdates");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.FAQs");
            DropTable("dbo.ExternalLinks");
            DropTable("dbo.ApplicationEvents");
            DropTable("dbo.ControllerTypes");
            DropTable("dbo.DirectionTypes");
            DropTable("dbo.MovementTypes");
            DropTable("dbo.LaneTypes");
            DropTable("dbo.DetectorComments");
            DropTable("dbo.DetectionTypes");
            DropTable("dbo.Detectors");
            DropTable("dbo.Approaches");
            DropTable("dbo.MetricComments");
            DropTable("dbo.MetricTypes");
            DropTable("dbo.Agencies");
            DropTable("dbo.Actions");
            DropTable("dbo.ActionLogs");
            AddPrimaryKey("dbo.ApproachRouteDetail", new[] { "ApproachRouteId", "SignalID" });
            CreateIndex("dbo.Lastupdate", "SignalID");
            CreateIndex("dbo.Detector_Error", "DetectorID");
            CreateIndex("dbo.Detector_Comment", "DetectorID");
            CreateIndex("dbo.Graph_Detectors", "SignalID");
            CreateIndex("dbo.ApproachRouteDetail", "SignalID");
            CreateIndex("dbo.Action_Log_Metrics", "Metric_Id");
            CreateIndex("dbo.Action_Log_Metrics", "Action_Log_Id");
            CreateIndex("dbo.Action_Log_Actions", "Action_Id");
            CreateIndex("dbo.Action_Log_Actions", "Action_Log_Id");
            CreateIndex("dbo.Action_Log", "SignalID");
            CreateIndex("dbo.Action_Log", "Agency");
            AddForeignKey("dbo.Lastupdate", "SignalID", "dbo.Signals", "SignalID");
            AddForeignKey("dbo.Graph_Detectors", "SignalID", "dbo.Signals", "SignalID");
            AddForeignKey("dbo.Route_Detectors", "DetectorID", "dbo.Graph_Detectors", "DetectorID");
            AddForeignKey("dbo.Detector_Error", "DetectorID", "dbo.Graph_Detectors", "DetectorID");
            AddForeignKey("dbo.Detector_Comment", "DetectorID", "dbo.Graph_Detectors", "DetectorID");
            AddForeignKey("dbo.ApproachRouteDetail", "SignalID", "dbo.Signals", "SignalID");
            AddForeignKey("dbo.Action_Log", "SignalID", "dbo.Signals", "SignalID");
            AddForeignKey("dbo.Action_Log_Metrics", "Action_Log_Id", "dbo.Action_Log", "ID");
            AddForeignKey("dbo.Action_Log_Metrics", "Metric_Id", "dbo.Action_Log_Metric_List", "ID");
            AddForeignKey("dbo.Action_Log", "Agency", "dbo.Action_Log_Agency_List", "ID");
            AddForeignKey("dbo.Action_Log_Actions", "Action_Log_Id", "dbo.Action_Log", "ID");
            AddForeignKey("dbo.Action_Log_Actions", "Action_Id", "dbo.Action_Log_Action_List", "ID");
        }
    }
}
