namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDataTypesAggregationTables_AddedEOSController : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MetricTypes", "DisplayOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MetricTypes", "DisplayOrder");
        }
    }
}
