namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LatencyCorrection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MetricTypes", "ShowOnAggregationSite", c => c.Boolean(nullable: false));
            AddColumn("dbo.Detectors", "LatencyCorrection", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Detectors", "LatencyCorrection");
            DropColumn("dbo.MetricTypes", "ShowOnAggregationSite");
        }
    }
}
