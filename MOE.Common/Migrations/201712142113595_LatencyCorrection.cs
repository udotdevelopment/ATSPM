using System.Data.Entity.Migrations;

namespace MOE.Common.Migrations
{
    public partial class LatencyCorrection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MetricTypes", "ShowOnAggregationSite", c => c.Boolean(false));
            AddColumn("dbo.Detectors", "LatencyCorrection", c => c.Double(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Detectors", "LatencyCorrection");
            DropColumn("dbo.MetricTypes", "ShowOnAggregationSite");
        }
    }
}