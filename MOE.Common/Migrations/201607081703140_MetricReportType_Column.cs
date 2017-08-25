namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MetricReportType_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MetricTypes", "MetricReportType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MetricTypes", "MetricReportType");
        }
    }
}
