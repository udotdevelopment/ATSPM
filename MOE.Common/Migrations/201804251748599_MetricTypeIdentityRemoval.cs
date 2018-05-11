namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MetricTypeIdentityRemoval : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MetricCommentMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropForeignKey("dbo.DetectionTypeMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropForeignKey("dbo.ActionLogMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropPrimaryKey("dbo.MetricTypes");
            DropTable("dbo.MetricTypes");
            CreateTable(
                    "dbo.MetricTypes",
                    c => new
                    {
                        MetricID = c.Int(false),
                        ChartName = c.String(false),
                        Abbreviation = c.String(false),
                        ShowOnWebsite = c.Boolean(false),
                        ShowOnAggregationSite = c.Boolean(false),
                    })
                .PrimaryKey(t => t.MetricID);
            AddForeignKey("dbo.MetricCommentMetricTypes", "MetricType_MetricID", "dbo.MetricTypes", "MetricID", cascadeDelete: true);
            AddForeignKey("dbo.DetectionTypeMetricTypes", "MetricType_MetricID", "dbo.MetricTypes", "MetricID", cascadeDelete: true);
            AddForeignKey("dbo.ActionLogMetricTypes", "MetricType_MetricID", "dbo.MetricTypes", "MetricID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActionLogMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropForeignKey("dbo.DetectionTypeMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropForeignKey("dbo.MetricCommentMetricTypes", "MetricType_MetricID", "dbo.MetricTypes");
            DropPrimaryKey("dbo.MetricTypes");
            AlterColumn("dbo.MetricTypes", "MetricID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.MetricTypes", "MetricID");
            AddForeignKey("dbo.ActionLogMetricTypes", "MetricType_MetricID", "dbo.MetricTypes", "MetricID", cascadeDelete: true);
            AddForeignKey("dbo.DetectionTypeMetricTypes", "MetricType_MetricID", "dbo.MetricTypes", "MetricID", cascadeDelete: true);
            AddForeignKey("dbo.MetricCommentMetricTypes", "MetricType_MetricID", "dbo.MetricTypes", "MetricID", cascadeDelete: true);
        }
    }
}
