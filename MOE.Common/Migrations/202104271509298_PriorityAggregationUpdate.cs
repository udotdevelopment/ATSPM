namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriorityAggregationUpdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PriorityAggregations", "TotalCycles");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PriorityAggregations", "TotalCycles", c => c.Int(nullable: false));
        }
    }
}
