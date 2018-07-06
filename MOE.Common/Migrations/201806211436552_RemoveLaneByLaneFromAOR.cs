namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLaneByLaneFromAOR : DbMigration
    {
        public override void Up()
        {

            Sql("delete DetectionTypeMetricTypes where MetricType_MetricID = 9 and DetectionType_DetectionTypeID = 4");
        }
        
        public override void Down()
        {
        }
    }
}
