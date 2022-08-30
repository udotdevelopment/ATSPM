namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class speedAggregationUpdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ApproachSpeedAggregations", "IsProtectedPhase");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApproachSpeedAggregations", "IsProtectedPhase", c => c.Boolean(nullable: false));
        }
    }
}
