using System.Data.Entity.Migrations;

namespace MOE.Common.Migrations
{
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
                        EventCode = c.Int(false),
                        EventParam = c.Int(false)
                    }).Index(t => new {t.SignalID, t.Timestamp, t.EventCode, t.EventParam})
                .Index(t => new {t.Timestamp}, "IX_Clustered_Controller_Event_Log_Timestamp", false, true, null);

            CreateTable(
                    "dbo.Accordian",
                    c => new
                    {
                        AccID = c.Int(false, true),
                        AccHeader = c.String(maxLength: 150),
                        AccContent = c.String(),
                        AccOrder = c.Int(),
                        Application = c.String(maxLength: 50)
                    })
                .PrimaryKey(t => t.AccID);

            CreateTable(
                    "dbo.Action_Log",
                    c => new
                    {
                        ID = c.Int(false, true),
                        Date = c.DateTime(),
                        Agency = c.Int(),
                        Comment = c.String(maxLength: 255),
                        SignalID = c.String(false, 10),
                        Name = c.String(maxLength: 100)
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
                        ID = c.Int(false),
                        Action_Log_Id = c.Int(false),
                        Action_Id = c.Int(false)
                    })
                .PrimaryKey(t => new {t.ID, t.Action_Log_Id, t.Action_Id})
                .ForeignKey("dbo.Action_Log_Action_List", t => t.Action_Id)
                .ForeignKey("dbo.Action_Log", t => t.Action_Log_Id)
                .Index(t => t.Action_Log_Id)
                .Index(t => t.Action_Id);

            CreateTable(
                    "dbo.Action_Log_Action_List",
                    c => new
                    {
                        ID = c.Int(false, true),
                        Description = c.String(false, 50)
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.Action_Log_Agency_List",
                    c => new
                    {
                        ID = c.Int(false, true),
                        Description = c.String(maxLength: 50)
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.Action_Log_Metrics",
                    c => new
                    {
                        Id = c.Int(false),
                        Action_Log_Id = c.Int(false),
                        Metric_Id = c.Int(false)
                    })
                .PrimaryKey(t => new {t.Id, t.Action_Log_Id, t.Metric_Id})
                .ForeignKey("dbo.Action_Log_Metric_List", t => t.Metric_Id)
                .ForeignKey("dbo.Action_Log", t => t.Action_Log_Id)
                .Index(t => t.Action_Log_Id)
                .Index(t => t.Metric_Id);

            CreateTable(
                    "dbo.Action_Log_Metric_List",
                    c => new
                    {
                        ID = c.Int(false, true),
                        Description = c.String(maxLength: 50)
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.Signals",
                    c => new
                    {
                        SignalID = c.String(false, 10),
                        Primary_Name = c.String(maxLength: 30, unicode: false),
                        Secondary_Name = c.String(maxLength: 30, unicode: false),
                        IP_Address = c.String(maxLength: 50, unicode: false),
                        Latitude = c.String(maxLength: 30, unicode: false),
                        Longitude = c.String(maxLength: 30, unicode: false),
                        Region = c.String(maxLength: 50, unicode: false),
                        Controller_Type = c.Int(),
                        Collection_Frequency = c.Int()
                    })
                .PrimaryKey(t => t.SignalID);

            CreateTable(
                    "dbo.ApproachRouteDetail",
                    c => new
                    {
                        ApproachRouteId = c.Int(false),
                        SignalID = c.String(false, 10),
                        ApproachOrder = c.Int(),
                        Direction = c.String(maxLength: 15)
                    })
                .PrimaryKey(t => t.ApproachRouteId)
                .ForeignKey("dbo.Route", t => t.ApproachRouteId, true)
                .ForeignKey("dbo.Signals", t => t.SignalID, false)
                .Index(t => t.ApproachRouteId)
                .Index(t => t.SignalID);

            CreateTable(
                    "dbo.Graph_Detectors",
                    c => new
                    {
                        DetectorID = c.String(false, 50),
                        SignalID = c.String(false, 10),
                        Lane = c.String(maxLength: 50, unicode: false),
                        Phase = c.String(false, 50, unicode: false),
                        Loops = c.String(maxLength: 50, unicode: false),
                        Comments = c.String(maxLength: 500, unicode: false),
                        Direction = c.String(false, 50, unicode: false),
                        Det_Channel = c.Int(false),
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
                        Has_SplitFail = c.Boolean()
                    })
                .PrimaryKey(t => t.DetectorID)
                .ForeignKey("dbo.Signals", t => t.SignalID)
                .Index(t => t.SignalID);

            CreateTable(
                    "dbo.Detector_Comment",
                    c => new
                    {
                        CommentId = c.Int(false),
                        Date = c.DateTime(false),
                        Comment = c.String(false, 500),
                        DetectorID = c.String(false, 50),
                        SignalID = c.String(false, 10)
                    })
                .PrimaryKey(t => new {t.CommentId, t.Date, t.Comment, t.DetectorID, t.SignalID})
                .ForeignKey("dbo.Graph_Detectors", t => t.DetectorID)
                .Index(t => t.DetectorID);

            CreateTable(
                    "dbo.Detector_Error",
                    c => new
                    {
                        ErrorID = c.Int(false),
                        DetectorID = c.String(false, 50),
                        Timestamp = c.DateTime(),
                        ErrorType = c.Int(),
                        Phase = c.Int()
                    })
                .PrimaryKey(t => new {t.ErrorID, t.DetectorID})
                .ForeignKey("dbo.Graph_Detectors", t => t.DetectorID)
                .Index(t => t.DetectorID);

            CreateTable(
                    "dbo.Route_Detectors",
                    c => new
                    {
                        DetectorID = c.String(false, 50),
                        RouteID = c.Int(false),
                        RouteOrder = c.Int(false)
                    })
                .PrimaryKey(t => new {t.DetectorID, t.RouteID, t.RouteOrder})
                .ForeignKey("dbo.Graph_Detectors", t => t.DetectorID)
                .Index(t => t.DetectorID);

            CreateTable(
                    "dbo.Lastupdate",
                    c => new
                    {
                        SignalID = c.String(false, 10),
                        LastUpdateTime = c.DateTime(),
                        LastErrorTime = c.DateTime()
                    })
                .PrimaryKey(t => t.SignalID)
                .ForeignKey("dbo.Signals", t => t.SignalID)
                .Index(t => t.SignalID);

            CreateTable(
                    "dbo.Alert_Day_Types",
                    c => new
                    {
                        DayTypeNumber = c.Int(false),
                        DayTypeDesctiption = c.String(maxLength: 50)
                    })
                .PrimaryKey(t => t.DayTypeNumber);

            CreateTable(
                    "dbo.Alert_Recipients",
                    c => new
                    {
                        RowID = c.Int(false, true),
                        UserID = c.Int(),
                        AlertID = c.Int()
                    })
                .PrimaryKey(t => t.RowID);

            CreateTable(
                    "dbo.Alert_Types",
                    c => new
                    {
                        AlertID = c.Int(false),
                        Alert_Description = c.String(false, 50)
                    })
                .PrimaryKey(t => new {t.AlertID, t.Alert_Description});

            CreateTable(
                    "dbo.ApproachDirection",
                    c => new
                    {
                        DirectionID = c.Int(false),
                        DirectionName = c.String(false, 50, unicode: false)
                    })
                .PrimaryKey(t => new {t.DirectionID, t.DirectionName});

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

            CreateTable(
                    "dbo.Archived_Metrics_Temp",
                    c => new
                    {
                        Timestamp = c.DateTime(false),
                        DetectorID = c.String(false, 50),
                        BinSize = c.Int(false),
                        Volume = c.Int(),
                        speed = c.Int(),
                        delay = c.Int(),
                        AoR = c.Int(),
                        SpeedHits = c.Int(),
                        BinGreenTime = c.Int(),
                        BinYellowTime = c.Int(),
                        BinRedTime = c.Int()
                    })
                .PrimaryKey(t => new {t.Timestamp, t.DetectorID, t.BinSize});

            CreateTable(
                    "dbo.SPM_Comment",
                    c => new
                    {
                        CommentID = c.Long(false, true),
                        TimeStamp = c.DateTime(false),
                        Entity = c.String(false, 50, unicode: false),
                        ChartType = c.Int(false),
                        Comment = c.String(false, unicode: false),
                        EntityType = c.Int()
                    })
                .PrimaryKey(t => t.CommentID);
            
            CreateTable(
                    "dbo.Controller_Type",
                    c => new
                    {
                        TypeID = c.Int(false),
                        Description = c.String(false, 50, unicode: false),
                        SNMPPort = c.Long(false),
                        FTPDirectory = c.String(false, 128, unicode: false),
                        ActiveFTP = c.Boolean(false),
                        UserName = c.String(false, 50, unicode: false),
                        Password = c.String(false, 50, unicode: false)
                    })
                //.PrimaryKey(t => new { t.TypeID, t.Description, t.SNMPPort, t.FTPDirectory, t.ActiveFTP, t.UserName, t.Password });
                .PrimaryKey(t => new {t.TypeID});

            CreateTable(
                    "dbo.DownloadAgreements",
                    c => new
                    {
                        DownloadAgreementID = c.Int(false, true),
                        CompanyName = c.String(false),
                        Address = c.String(false),
                        PhoneNumber = c.String(false),
                        EmailAddress = c.String(false),
                        Acknowledged = c.Boolean(false),
                        AgreementDate = c.DateTime(false)
                    })
                .PrimaryKey(t => t.DownloadAgreementID);

            CreateTable(
                    "dbo.Error_Types",
                    c => new
                    {
                        ErrorType = c.Int(false),
                        ErrorDescription = c.String(maxLength: 50)
                    })
                .PrimaryKey(t => t.ErrorType);

            CreateTable(
                    "dbo.Menu",
                    c => new
                    {
                        MenuId = c.Int(false),
                        MenuName = c.String(false, 50),
                        MenuLocation = c.String(false, 100),
                        ParentId = c.Int(false),
                        Application = c.String(false, 50),
                        DisplayOrder = c.Int(false)
                    })
                .PrimaryKey(t => t.MenuId);

            CreateTable(
                    "dbo.MOE_Users",
                    c => new
                    {
                        ID = c.Int(false),
                        Name = c.String(false, 50),
                        Email = c.String(false, 50),
                        Password = c.String(false, 50),
                        ReceiveAlerts = c.Boolean()
                    })
                .PrimaryKey(t => new {t.ID, t.Name, t.Email, t.Password});

            CreateTable(
                    "dbo.Program_Message",
                    c => new
                    {
                        ID = c.Int(false, true),
                        Priority = c.String(maxLength: 10),
                        Program = c.String(maxLength: 50),
                        Message = c.String(maxLength: 500),
                        Timestamp = c.DateTime()
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.Program_Settings",
                    c => new
                    {
                        SettingName = c.String(false, 50),
                        SettingValue = c.String(false, 50)
                    })
                .PrimaryKey(t => new {t.SettingName, t.SettingValue});

            CreateTable(
                    "dbo.Region",
                    c => new
                    {
                        ID = c.Int(false),
                        Description = c.String(maxLength: 50)
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.Route",
                    c => new
                    {
                        RouteID = c.Int(false, true),
                        Description = c.String(false, unicode: false),
                        Region = c.Int(false),
                        Name = c.String(false, unicode: false)
                    })
                .PrimaryKey(t => t.RouteID);

            CreateTable(
                    "dbo.Speed_Events",
                    c => new
                    {
                        DetectorID = c.String(false, 50),
                        MPH = c.Int(false),
                        KPH = c.Int(false),
                        timestamp = c.DateTime(false, 7, storeType: "datetime2")
                    })
                .PrimaryKey(t => new {t.DetectorID, t.MPH, t.KPH, t.timestamp});

            CreateTable(
                    "dbo.Comment",
                    c => new
                    {
                        CommentID = c.Long(false, true),
                        TimeStamp = c.DateTime(false),
                        Entity = c.String(false, 50, unicode: false),
                        ChartType = c.Int(false),
                        Comment = c.String(false, unicode: false),
                        EntityType = c.Int()
                    })
                .PrimaryKey(t => t.CommentID);

            CreateTable(
                    "dbo.SPM_Error",
                    c => new
                    {
                        ErrorID = c.Long(false, true),
                        TimeStamp = c.DateTime(false),
                        ErrorType = c.Int(false),
                        Param1 = c.String(maxLength: 50, unicode: false),
                        Param2 = c.String(maxLength: 50, unicode: false)
                    })
                .PrimaryKey(t => t.ErrorID);
        }

        public override void Down()
        {
            //DropForeignKey("dbo.Lastupdate", "SignalId", "dbo.Signals");
            //DropForeignKey("dbo.Graph_Detectors", "SignalId", "dbo.Signals");
            //DropForeignKey("dbo.Route_Detectors", "DetectorID", "dbo.Graph_Detectors");
            //DropForeignKey("dbo.Detector_Error", "DetectorID", "dbo.Graph_Detectors");
            //DropForeignKey("dbo.Detector_Comment", "DetectorID", "dbo.Graph_Detectors");
            //DropForeignKey("dbo.ApproachRouteDetail", "SignalId", "dbo.Signals");
            //DropForeignKey("dbo.ApproachRouteDetail", "RouteId", "dbo.Route");
            //DropForeignKey("dbo.Action_Log", "SignalId", "dbo.Signals");
            //DropForeignKey("dbo.Action_Log_Metrics", "Action_Log_Id", "dbo.Action_Log");
            //DropForeignKey("dbo.Action_Log_Metrics", "Metric_Id", "dbo.Action_Log_Metric_List");
            //DropForeignKey("dbo.Action_Log", "Agency", "dbo.Action_Log_Agency_List");
            //DropForeignKey("dbo.Action_Log_Actions", "Action_Log_Id", "dbo.Action_Log");
            //DropForeignKey("dbo.Action_Log_Actions", "Action_Id", "dbo.Action_Log_Action_List");
            //DropIndex("dbo.Lastupdate", new[] { "SignalId" });
            //DropIndex("dbo.Route_Detectors", new[] { "DetectorID" });
            //DropIndex("dbo.Detector_Error", new[] { "DetectorID" });
            //DropIndex("dbo.Detector_Comment", new[] { "DetectorID" });
            //DropIndex("dbo.Graph_Detectors", new[] { "SignalId" });
            //DropIndex("dbo.ApproachRouteDetail", new[] { "SignalId" });
            //DropIndex("dbo.ApproachRouteDetail", new[] { "RouteId" });
            //DropIndex("dbo.Action_Log_Metrics", new[] { "Metric_Id" });
            //DropIndex("dbo.Action_Log_Metrics", new[] { "Action_Log_Id" });
            //DropIndex("dbo.Action_Log_Actions", new[] { "Action_Id" });
            //DropIndex("dbo.Action_Log_Actions", new[] { "Action_Log_Id" });
            //DropIndex("dbo.Action_Log", new[] { "SignalId" });
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
            //DropTable("dbo.Route");
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