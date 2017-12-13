namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MetricTypeUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MetricTypes", "ShowOnAggregationSite", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MetricTypes", "ShowOnAggregationSite");
        }
    }
}
