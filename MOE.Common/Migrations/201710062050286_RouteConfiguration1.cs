using System.Data.Entity.Migrations;

namespace MOE.Common.Migrations
{
    public partial class RouteConfiguration1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApproachRouteDetail", "RouteId", "dbo.Route");
            DropForeignKey("dbo.ApproachRouteMetricTypes", "ApproachRoute_ApproachRouteId", "dbo.Route");
            DropForeignKey("dbo.ApproachRouteMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropForeignKey("dbo.Route_Detectors", "Detectors_ID", "dbo.Detectors");
            DropIndex("dbo.ApproachRouteDetail", new[] {"RouteId"});
            DropIndex("dbo.Route_Detectors", new[] {"Detectors_ID"});
            DropIndex("dbo.ApproachRouteMetricTypes", new[] {"ApproachRoute_ApproachRouteId"});
            DropIndex("dbo.ApproachRouteMetricTypes", new[] {"MetricType_MetricID"});
            DropTable("dbo.Accordian");

            DropTable("dbo.Route");
            DropTable("dbo.Alert_Day_Types");
            DropTable("dbo.DownloadAgreements");
            DropTable("dbo.LastUpdates");
            DropTable("dbo.Program_Message");
            DropTable("dbo.Program_Settings");
            DropTable("dbo.Route_Detectors");
        }

        public override void Down()
        {
            CreateTable(
                    "dbo.ApproachRouteMetricTypes",
                    c => new
                    {
                        ApproachRoute_ApproachRouteId = c.Int(false),
                        MetricType_MetricID = c.Int(false)
                    })
                .PrimaryKey(t => new {t.ApproachRoute_ApproachRouteId, t.MetricType_MetricID});

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
                    "dbo.Route_Detectors",
                    c => new
                    {
                        DetectorID = c.String(false, 50),
                        RouteID = c.Int(false),
                        RouteOrder = c.Int(false),
                        Detectors_ID = c.Int()
                    })
                .PrimaryKey(t => new {t.DetectorID, t.RouteID, t.RouteOrder});

            CreateTable(
                    "dbo.Program_Settings",
                    c => new
                    {
                        SettingName = c.String(false, 50),
                        SettingValue = c.String(false, 50)
                    })
                .PrimaryKey(t => new {t.SettingName, t.SettingValue});

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
                    "dbo.LastUpdates",
                    c => new
                    {
                        UpdateID = c.Int(false, true),
                        SignalID = c.String(false, 10),
                        LastUpdateTime = c.DateTime(),
                        LastErrorTime = c.DateTime()
                    })
                .PrimaryKey(t => t.UpdateID);

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
                    "dbo.Alert_Day_Types",
                    c => new
                    {
                        DayTypeNumber = c.Int(false),
                        DayTypeDesctiption = c.String(maxLength: 50)
                    })
                .PrimaryKey(t => t.DayTypeNumber);

            CreateTable(
                    "dbo.ApproachRouteDetail",
                    c => new
                    {
                        RouteDetailId = c.Int(false, true),
                        ApproachRouteId = c.Int(false),
                        Order = c.Int(false),
                        SignalId = c.String(false),
                        PhaseDirection1 = c.Int(false),
                        IsPhaseDirection1Overlap = c.Boolean(false),
                        PhaseDirection2 = c.Int(false),
                        IsPhaseDirection2Overlap = c.Boolean(false),
                        DirectionType1_DirectionTypeID = c.Int(),
                        DirectionType2_DirectionTypeID = c.Int()
                    })
                .PrimaryKey(t => t.RouteDetailId);

            CreateTable(
                    "dbo.Route",
                    c => new
                    {
                        ApproachRouteId = c.Int(false, true),
                        RouteName = c.String(false, unicode: false)
                    })
                .PrimaryKey(t => t.ApproachRouteId);

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

            CreateIndex("dbo.ApproachRouteMetricTypes", "MetricType_MetricID");
            CreateIndex("dbo.ApproachRouteMetricTypes", "ApproachRoute_ApproachRouteId");
            CreateIndex("dbo.Route_Detectors", "Detectors_ID");
            CreateIndex("dbo.ApproachRouteDetail", "DirectionType2_DirectionTypeID");
            CreateIndex("dbo.ApproachRouteDetail", "DirectionType1_DirectionTypeID");
            CreateIndex("dbo.ApproachRouteDetail", "RouteId");
            AddForeignKey("dbo.Route_Detectors", "Detectors_ID", "dbo.Detectors", "ID");
            AddForeignKey("dbo.ApproachRouteMetricTypes", "MetricType_MetricID", "dbo.MetricTypes", "MetricID", true);
            AddForeignKey("dbo.ApproachRouteMetricTypes", "ApproachRoute_ApproachRouteId", "dbo.Route", "RouteId",
                true);
            AddForeignKey("dbo.ApproachRouteDetail", "RouteId", "dbo.Route", "RouteId", true);
            AddForeignKey("dbo.ApproachRouteDetail", "DirectionType2_DirectionTypeID", "dbo.DirectionTypes",
                "DirectionTypeID");
            AddForeignKey("dbo.ApproachRouteDetail", "DirectionType1_DirectionTypeID", "dbo.DirectionTypes",
                "DirectionTypeID");
        }
    }
}