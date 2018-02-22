using System.Data.Entity.Migrations;

namespace MOE.Common.Migrations
{
    public partial class Version401 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MetricTypes", "DetectionType_DetectionTypeID", "dbo.DetectionTypes");
            DropIndex("dbo.MetricTypes", new[] {"DetectionType_DetectionTypeID"});
            CreateTable(
                    "dbo.DetectionHardwares",
                    c => new
                    {
                        ID = c.Int(false),
                        Name = c.String(false)
                    })
                .PrimaryKey(t => t.ID);

            Sql("Insert into DetectionHardwares values (0,'Unknown')");
            CreateTable(
                    "dbo.Applications",
                    c => new
                    {
                        ID = c.Int(false, true),
                        Name = c.String()
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.ApplicationSettings",
                    c => new
                    {
                        ID = c.Int(false, true),
                        ApplicationID = c.Int(false),
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
                        Discriminator = c.String(false, 128)
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Applications", t => t.ApplicationID, true)
                .Index(t => t.ApplicationID);

            CreateTable(
                    "dbo.DetectionTypeMetricTypes",
                    c => new
                    {
                        DetectionType_DetectionTypeID = c.Int(false),
                        MetricType_MetricID = c.Int(false)
                    })
                .PrimaryKey(t => new {t.DetectionType_DetectionTypeID, t.MetricType_MetricID})
                .ForeignKey("dbo.DetectionTypes", t => t.DetectionType_DetectionTypeID, true)
                .ForeignKey("dbo.MetricTypes", t => t.MetricType_MetricID, true)
                .Index(t => t.DetectionType_DetectionTypeID)
                .Index(t => t.MetricType_MetricID);

            //This line was created manually
            RenameColumn("dbo.AspNetUsers", "RecieveAlerts", "ReceiveAlerts");
            AddColumn("dbo.Detectors", "DetectionHardwareID", c => c.Int(false));
            AddColumn("dbo.MovementTypes", "DisplayOrder", c => c.Int(false));
            AddColumn("dbo.DirectionTypes", "DisplayOrder", c => c.Int(false));
            AddColumn("dbo.FAQs", "OrderNumber", c => c.Int(false));
            CreateIndex("dbo.Detectors", "DetectionHardwareID");
            AddForeignKey("dbo.Detectors", "DetectionHardwareID", "dbo.DetectionHardwares", "ID", true);
            DropColumn("dbo.MetricTypes", "DetectionType_DetectionTypeID");
        }

        public override void Down()
        {
            AddColumn("dbo.MetricTypes", "DetectionType_DetectionTypeID", c => c.Int(false));
            DropForeignKey("dbo.ApplicationSettings", "ApplicationID", "dbo.Applications");
            DropForeignKey("dbo.DetectionTypeMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropForeignKey("dbo.DetectionTypeMetricTypes", "DetectionType_DetectionTypeID", "dbo.DetectionTypes");
            DropForeignKey("dbo.Detectors", "DetectionHardwareID", "dbo.DetectionHardwares");
            DropIndex("dbo.DetectionTypeMetricTypes", new[] {"MetricType_MetricID"});
            DropIndex("dbo.DetectionTypeMetricTypes", new[] {"DetectionType_DetectionTypeID"});
            DropIndex("dbo.ApplicationSettings", new[] {"ApplicationID"});
            DropIndex("dbo.Detectors", new[] {"DetectionHardwareID"});
            DropColumn("dbo.FAQs", "OrderNumber");
            DropColumn("dbo.DirectionTypes", "DisplayOrder");
            DropColumn("dbo.MovementTypes", "DisplayOrder");
            DropColumn("dbo.Detectors", "DetectionHardwareID");
            DropTable("dbo.DetectionTypeMetricTypes");
            DropTable("dbo.ApplicationSettings");
            DropTable("dbo.Applications");
            DropTable("dbo.DetectionHardwares");
            CreateIndex("dbo.MetricTypes", "DetectionType_DetectionTypeID");
            AddForeignKey("dbo.MetricTypes", "DetectionType_DetectionTypeID", "dbo.DetectionTypes", "DetectionTypeID",
                true);
        }
    }
}