namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RouteConfiguration1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApproachRouteDetail", "DirectionType1_DirectionTypeID", "dbo.DirectionTypes");
            DropForeignKey("dbo.ApproachRouteDetail", "DirectionType2_DirectionTypeID", "dbo.DirectionTypes");
            DropForeignKey("dbo.ApproachRouteDetail", "ApproachRouteId", "dbo.ApproachRoute");
            DropForeignKey("dbo.ApproachRouteMetricTypes", "ApproachRoute_ApproachRouteId", "dbo.ApproachRoute");
            DropForeignKey("dbo.ApproachRouteMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropForeignKey("dbo.Route_Detectors", "Detectors_ID", "dbo.Detectors");
            DropIndex("dbo.ApproachRouteDetail", new[] { "ApproachRouteId" });
            DropIndex("dbo.ApproachRouteDetail", new[] { "DirectionType1_DirectionTypeID" });
            DropIndex("dbo.ApproachRouteDetail", new[] { "DirectionType2_DirectionTypeID" });
            DropIndex("dbo.Route_Detectors", new[] { "Detectors_ID" });
            DropIndex("dbo.ApproachRouteMetricTypes", new[] { "ApproachRoute_ApproachRouteId" });
            DropIndex("dbo.ApproachRouteMetricTypes", new[] { "MetricType_MetricID" });
            DropTable("dbo.Accordian");
            DropTable("dbo.ApproachRoute");
            DropTable("dbo.ApproachRouteDetail");
            DropTable("dbo.Alert_Day_Types");
            DropTable("dbo.DownloadAgreements");
            DropTable("dbo.LastUpdates");
            DropTable("dbo.Program_Message");
            DropTable("dbo.Program_Settings");
            DropTable("dbo.Route_Detectors");
            DropTable("dbo.Route");
            DropTable("dbo.SignalWithDetections");
            DropTable("dbo.ApproachRouteMetricTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApproachRouteMetricTypes",
                c => new
                    {
                        ApproachRoute_ApproachRouteId = c.Int(nullable: false),
                        MetricType_MetricID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApproachRoute_ApproachRouteId, t.MetricType_MetricID });
            
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
                "dbo.Route_Detectors",
                c => new
                    {
                        DetectorID = c.String(nullable: false, maxLength: 50),
                        RouteID = c.Int(nullable: false),
                        RouteOrder = c.Int(nullable: false),
                        Detectors_ID = c.Int(),
                    })
                .PrimaryKey(t => new { t.DetectorID, t.RouteID, t.RouteOrder });
            
            CreateTable(
                "dbo.Program_Settings",
                c => new
                    {
                        SettingName = c.String(nullable: false, maxLength: 50),
                        SettingValue = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.SettingName, t.SettingValue });
            
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
                "dbo.Alert_Day_Types",
                c => new
                    {
                        DayTypeNumber = c.Int(nullable: false),
                        DayTypeDesctiption = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DayTypeNumber);
            
            CreateTable(
                "dbo.ApproachRouteDetail",
                c => new
                    {
                        RouteDetailId = c.Int(nullable: false, identity: true),
                        ApproachRouteId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        SignalId = c.String(nullable: false),
                        PhaseDirection1 = c.Int(nullable: false),
                        IsPhaseDirection1Overlap = c.Boolean(nullable: false),
                        PhaseDirection2 = c.Int(nullable: false),
                        IsPhaseDirection2Overlap = c.Boolean(nullable: false),
                        DirectionType1_DirectionTypeID = c.Int(),
                        DirectionType2_DirectionTypeID = c.Int(),
                    })
                .PrimaryKey(t => t.RouteDetailId);
            
            CreateTable(
                "dbo.ApproachRoute",
                c => new
                    {
                        ApproachRouteId = c.Int(nullable: false, identity: true),
                        RouteName = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.ApproachRouteId);
            
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
            
            CreateIndex("dbo.ApproachRouteMetricTypes", "MetricType_MetricID");
            CreateIndex("dbo.ApproachRouteMetricTypes", "ApproachRoute_ApproachRouteId");
            CreateIndex("dbo.Route_Detectors", "Detectors_ID");
            CreateIndex("dbo.ApproachRouteDetail", "DirectionType2_DirectionTypeID");
            CreateIndex("dbo.ApproachRouteDetail", "DirectionType1_DirectionTypeID");
            CreateIndex("dbo.ApproachRouteDetail", "ApproachRouteId");
            AddForeignKey("dbo.Route_Detectors", "Detectors_ID", "dbo.Detectors", "ID");
            AddForeignKey("dbo.ApproachRouteMetricTypes", "MetricType_MetricID", "dbo.MetricTypes", "MetricID", cascadeDelete: true);
            AddForeignKey("dbo.ApproachRouteMetricTypes", "ApproachRoute_ApproachRouteId", "dbo.ApproachRoute", "ApproachRouteId", cascadeDelete: true);
            AddForeignKey("dbo.ApproachRouteDetail", "ApproachRouteId", "dbo.ApproachRoute", "ApproachRouteId", cascadeDelete: true);
            AddForeignKey("dbo.ApproachRouteDetail", "DirectionType2_DirectionTypeID", "dbo.DirectionTypes", "DirectionTypeID");
            AddForeignKey("dbo.ApproachRouteDetail", "DirectionType1_DirectionTypeID", "dbo.DirectionTypes", "DirectionTypeID");
        }
    }
}
