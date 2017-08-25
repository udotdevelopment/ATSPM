namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShowMetricOnWebsite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MetricTypes", "ShowOnWebsite", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MetricTypes", "ShowOnWebsite");
        }
    }
}
