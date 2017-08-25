namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {


            CreateTable(
                "dbo.Controller_Event_Log",
                c => new
                    {
                        SignalID = c.String(maxLength: 10, nullable: false),
                        Timestamp = c.DateTime(precision: 7, nullable: false),
                        EventCode = c.Int(nullable: false),
                        EventParam = c.Int(nullable: false)
                    }).Index(t => new { t.SignalID, t.Timestamp, t.EventCode, t.EventParam })
                    .Index(t => new { t.Timestamp }, "IX_Clustered_Controller_Event_Log_Timestamp", false, true, null);

            CreateTable(
                "dbo.Accordian",
                c => new
                    {
                        AccID = c.Int(nullable: false, identity: true),
                        AccHeader = c.String(maxLength: 150),
                        AccContent = c.String(),
                        AccOrder = c.Int(),
                        Application = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.AccID);

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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Action_Log_Agency_List", t => t.Agency)
                .ForeignKey("dbo.Signals", t => t.SignalID)
                .Index(t => t.Agency)
                .Index(t => t.SignalID);

            CreateTable(
                "dbo.Action_Log_Actions",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Action_Log_Id = c.Int(nullable: false),
                        Action_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID, t.Action_Log_Id, t.Action_Id })
                .ForeignKey("dbo.Action_Log_Action_List", t => t.Action_Id)
                .ForeignKey("dbo.Action_Log", t => t.Action_Log_Id)
                .Index(t => t.Action_Log_Id)
                .Index(t => t.Action_Id);

            CreateTable(
                "dbo.Action_Log_Action_List",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Action_Log_Agency_List",
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
                .PrimaryKey(t => new { t.Id, t.Action_Log_Id, t.Metric_Id })
                .ForeignKey("dbo.Action_Log_Metric_List", t => t.Metric_Id)
                .ForeignKey("dbo.Action_Log", t => t.Action_Log_Id)
                .Index(t => t.Action_Log_Id)
                .Index(t => t.Metric_Id);

            CreateTable(
                "dbo.Action_Log_Metric_List",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Signals",
                c => new
                    {
                        SignalID = c.String(nullable: false, maxLength: 10),
                        Primary_Name = c.String(maxLength: 30, unicode: false),
                        Secondary_Name = c.String(maxLength: 30, unicode: false),
                        IP_Address = c.String(maxLength: 50, unicode: false),
                        Latitude = c.String(maxLength: 30, unicode: false),
                        Longitude = c.String(maxLength: 30, unicode: false),
                        Region = c.String(maxLength: 50, unicode: false),
                        Controller_Type = c.Int(),
                        Collection_Frequency = c.Int(),
                    })
                .PrimaryKey(t => t.SignalID);

            CreateTable(
                "dbo.ApproachRouteDetail",
                c => new
                    {
                        ApproachRouteId = c.Int(nullable: false),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        ApproachOrder = c.Int(),
                        Direction = c.String(maxLength: 15),
                    })
                    .PrimaryKey(t => t.ApproachRouteId)
                    .ForeignKey("dbo.ApproachRoute", t => t.ApproachRouteId, cascadeDelete: true)
                    .ForeignKey("dbo.Signals", t => t.SignalID, cascadeDelete: false)
                .Index(t => t.ApproachRouteId)
                .Index(t => t.SignalID);



            CreateTable(
                "dbo.ApproachRoute",
                c => new
                    {
                        ApproachRouteId = c.Int(nullable: false, identity: true),
                        RouteName = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.ApproachRouteId);

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
                .PrimaryKey(t => t.DetectorID)
                .ForeignKey("dbo.Signals", t => t.SignalID)
                .Index(t => t.SignalID);

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
                .PrimaryKey(t => new { t.CommentId, t.Date, t.Comment, t.DetectorID, t.SignalID })
                .ForeignKey("dbo.Graph_Detectors", t => t.DetectorID)
                .Index(t => t.DetectorID);

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
                .PrimaryKey(t => new { t.ErrorID, t.DetectorID })
                .ForeignKey("dbo.Graph_Detectors", t => t.DetectorID)
                .Index(t => t.DetectorID);

            CreateTable(
                "dbo.Route_Detectors",
                c => new
                    {
                        DetectorID = c.String(nullable: false, maxLength: 50),
                        RouteID = c.Int(nullable: false),
                        RouteOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DetectorID, t.RouteID, t.RouteOrder })
                .ForeignKey("dbo.Graph_Detectors", t => t.DetectorID)
                .Index(t => t.DetectorID);

            CreateTable(
                "dbo.Lastupdate",
                c => new
                    {
                        SignalID = c.String(nullable: false, maxLength: 10),
                        LastUpdateTime = c.DateTime(),
                        LastErrorTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.SignalID)
                .ForeignKey("dbo.Signals", t => t.SignalID)
                .Index(t => t.SignalID);

            CreateTable(
                "dbo.Alert_Day_Types",
                c => new
                    {
                        DayTypeNumber = c.Int(nullable: false),
                        DayTypeDesctiption = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DayTypeNumber);

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
                "dbo.Alert_Types",
                c => new
                    {
                        AlertID = c.Int(nullable: false),
                        Alert_Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.AlertID, t.Alert_Description });

            CreateTable(
                "dbo.ApproachDirection",
                c => new
                    {
                        DirectionID = c.Int(nullable: false),
                        DirectionName = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.DirectionID, t.DirectionName });

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
            //CreateTable(
            //    "dbo.Controller_Event_Log",
            //    c => new
            //        {
            //            Timestamp = c.DateTime(nullable: false),
            //            SignalID = c.String(nullable: false, maxLength: 10),
            //            EventCode = c.Int(nullable: false),
            //            EventParam = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.Timestamp, t.SignalID, t.EventCode, t.EventParam });

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
                //.PrimaryKey(t => new { t.TypeID, t.Description, t.SNMPPort, t.FTPDirectory, t.ActiveFTP, t.UserName, t.Password });
                .PrimaryKey(t => new { t.TypeID });

            CreateTable(
                "dbo.DownloadAgreements",
                c => new
                    {
                        DownloadAgreementID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        Acknowledged = c.Boolean(nullable: false),
                        AgreementDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DownloadAgreementID);

            CreateTable(
                "dbo.Error_Types",
                c => new
                    {
                        ErrorType = c.Int(nullable: false),
                        ErrorDescription = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ErrorType);

            CreateTable(
                "dbo.Menu",
                c => new
                    {
                        MenuId = c.Int(nullable: false),
                        MenuName = c.String(nullable: false, maxLength: 50),
                        MenuLocation = c.String(nullable: false, maxLength: 100),
                        ParentId = c.Int(nullable: false),
                        Application = c.String(nullable: false, maxLength: 50),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MenuId);

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
                "dbo.Program_Message",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Priority = c.String(maxLength: 10),
                        Program = c.String(maxLength: 50),
                        Message = c.String(maxLength: 500),
                        Timestamp = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Program_Settings",
                c => new
                    {
                        SettingName = c.String(nullable: false, maxLength: 50),
                        SettingValue = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.SettingName, t.SettingValue });

            CreateTable(
                "dbo.Region",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Route",
                c => new
                    {
                        RouteID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, unicode: false),
                        Region = c.Int(nullable: false),
                        Name = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.RouteID);

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
                "dbo.Comment",
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

        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Lastupdate", "SignalID", "dbo.Signals");
            //DropForeignKey("dbo.Graph_Detectors", "SignalID", "dbo.Signals");
            //DropForeignKey("dbo.Route_Detectors", "DetectorID", "dbo.Graph_Detectors");
            //DropForeignKey("dbo.Detector_Error", "DetectorID", "dbo.Graph_Detectors");
            //DropForeignKey("dbo.Detector_Comment", "DetectorID", "dbo.Graph_Detectors");
            //DropForeignKey("dbo.ApproachRouteDetail", "SignalID", "dbo.Signals");
            //DropForeignKey("dbo.ApproachRouteDetail", "ApproachRouteId", "dbo.ApproachRoute");
            //DropForeignKey("dbo.Action_Log", "SignalID", "dbo.Signals");
            //DropForeignKey("dbo.Action_Log_Metrics", "Action_Log_Id", "dbo.Action_Log");
            //DropForeignKey("dbo.Action_Log_Metrics", "Metric_Id", "dbo.Action_Log_Metric_List");
            //DropForeignKey("dbo.Action_Log", "Agency", "dbo.Action_Log_Agency_List");
            //DropForeignKey("dbo.Action_Log_Actions", "Action_Log_Id", "dbo.Action_Log");
            //DropForeignKey("dbo.Action_Log_Actions", "Action_Id", "dbo.Action_Log_Action_List");
            //DropIndex("dbo.Lastupdate", new[] { "SignalID" });
            //DropIndex("dbo.Route_Detectors", new[] { "DetectorID" });
            //DropIndex("dbo.Detector_Error", new[] { "DetectorID" });
            //DropIndex("dbo.Detector_Comment", new[] { "DetectorID" });
            //DropIndex("dbo.Graph_Detectors", new[] { "SignalID" });
            //DropIndex("dbo.ApproachRouteDetail", new[] { "SignalID" });
            //DropIndex("dbo.ApproachRouteDetail", new[] { "ApproachRouteId" });
            //DropIndex("dbo.Action_Log_Metrics", new[] { "Metric_Id" });
            //DropIndex("dbo.Action_Log_Metrics", new[] { "Action_Log_Id" });
            //DropIndex("dbo.Action_Log_Actions", new[] { "Action_Id" });
            //DropIndex("dbo.Action_Log_Actions", new[] { "Action_Log_Id" });
            //DropIndex("dbo.Action_Log", new[] { "SignalID" });
            //DropIndex("dbo.Action_Log", new[] { "Agency" });
            //DropTable("dbo.SPM_Error");
            //DropTable("dbo.Comment");
            //DropTable("dbo.Speed_Events");
            //DropTable("dbo.Route");
            //DropTable("dbo.Region");
            //DropTable("dbo.Program_Settings");
            //DropTable("dbo.Program_Message");
            //DropTable("dbo.MOE_Users");
            //DropTable("dbo.Menu");
            //DropTable("dbo.Error_Types");
            //DropTable("dbo.DownloadAgreements");
            //DropTable("dbo.Controller_Type");
            //DropTable("dbo.Controller_Event_Log");
            //DropTable("dbo.Archived_Metrics_Temp");
            //DropTable("dbo.Archived_Metrics");
            //DropTable("dbo.ApproachDirection");
            //DropTable("dbo.Alert_Types");
            //DropTable("dbo.Alert_Recipients");
            //DropTable("dbo.Alert_Day_Types");
            //DropTable("dbo.Lastupdate");
            //DropTable("dbo.Route_Detectors");
            //DropTable("dbo.Detector_Error");
            //DropTable("dbo.Detector_Comment");
            //DropTable("dbo.Graph_Detectors");
            //DropTable("dbo.ApproachRoute");
            //DropTable("dbo.ApproachRouteDetail");
            //DropTable("dbo.Signals");
            //DropTable("dbo.Action_Log_Metric_List");
            //DropTable("dbo.Action_Log_Metrics");
            //DropTable("dbo.Action_Log_Agency_List");
            //DropTable("dbo.Action_Log_Action_List");
            //DropTable("dbo.Action_Log_Actions");
            //DropTable("dbo.Action_Log");
            //DropTable("dbo.Accordian");
        }
    }
}
