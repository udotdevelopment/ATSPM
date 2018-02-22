namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataPointsForAggregations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApproachCycleAggregations", "DataPoints", c => c.Int(nullable: false, defaultValue:5));
            AddColumn("dbo.ApproachPcdAggregations", "DataPoints", c => c.Int(nullable: false, defaultValue: 5));
            AddColumn("dbo.ApproachSpeedAggregations", "DataPoints", c => c.Int(nullable: false, defaultValue: 5));
            AddColumn("dbo.ApproachSplitFailAggregations", "DataPoints", c => c.Int(nullable: false, defaultValue: 5));
            AddColumn("dbo.ApproachYellowRedActivationAggregations", "DataPoints", c => c.Int(nullable: false, defaultValue: 5));
            AddColumn("dbo.DetectorAggregations", "DataPoints", c => c.Int(nullable: false, defaultValue: 5));
            AddColumn("dbo.PreemptionAggregations", "DataPoints", c => c.Int(nullable: false, defaultValue: 5));
            AddColumn("dbo.PriorityAggregations", "DataPoints", c => c.Int(nullable: false, defaultValue: 5));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PriorityAggregations", "DataPoints");
            DropColumn("dbo.PreemptionAggregations", "DataPoints");
            DropColumn("dbo.DetectorAggregations", "DataPoints");
            DropColumn("dbo.ApproachYellowRedActivationAggregations", "DataPoints");
            DropColumn("dbo.ApproachSplitFailAggregations", "DataPoints");
            DropColumn("dbo.ApproachSpeedAggregations", "DataPoints");
            DropColumn("dbo.ApproachPcdAggregations", "DataPoints");
            DropColumn("dbo.ApproachCycleAggregations", "DataPoints");
        }
    }
}
