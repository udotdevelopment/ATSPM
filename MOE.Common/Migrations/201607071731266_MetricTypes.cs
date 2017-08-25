namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MetricTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MetricsFilterTypes",
                c => new
                    {
                        FilterID = c.Int(nullable: false, identity: true),
                        FilterName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FilterID);
            
            CreateTable(
                "dbo.MetricTypes",
                c => new
                    {
                        MetricID = c.Int(nullable: false, identity: true),
                        ChartName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MetricID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MetricTypes");
            DropTable("dbo.MetricsFilterTypes");
        }
    }
}
