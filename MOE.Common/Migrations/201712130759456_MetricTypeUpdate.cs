namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MetricTypeUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DetectorAggregations", "Id", "dbo.Detectors");
            DropIndex("dbo.DetectorAggregations", new[] { "Id" });
            AddColumn("dbo.MetricTypes", "ShowOnAggregationSite", c => c.Boolean(nullable: false));
            AddColumn("dbo.DetectorAggregations", "DetectorPrimaryId", c => c.Int(nullable: false));
            CreateIndex("dbo.DetectorAggregations", "DetectorPrimaryId");
            AddForeignKey("dbo.DetectorAggregations", "DetectorPrimaryId", "dbo.Detectors", "ID", cascadeDelete: true);
            DropColumn("dbo.DetectorAggregations", "DetectorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DetectorAggregations", "DetectorId", c => c.String(nullable: false, maxLength: 10));
            DropForeignKey("dbo.DetectorAggregations", "DetectorPrimaryId", "dbo.Detectors");
            DropIndex("dbo.DetectorAggregations", new[] { "DetectorPrimaryId" });
            DropColumn("dbo.DetectorAggregations", "DetectorPrimaryId");
            DropColumn("dbo.MetricTypes", "ShowOnAggregationSite");
            CreateIndex("dbo.DetectorAggregations", "Id");
            AddForeignKey("dbo.DetectorAggregations", "Id", "dbo.Detectors", "ID");
        }
    }
}
