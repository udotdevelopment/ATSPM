namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PCDVolumeAggregation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApproachPcdAggregations", "Volume", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApproachPcdAggregations", "Volume");
        }
    }
}
