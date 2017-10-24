namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RouteConfiguration2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoutePhaseDirections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RouteSignalId = c.Int(nullable: false),
                        Phase = c.Int(nullable: false),
                        DirectionTypeId = c.Int(nullable: false),
                        IsOverlap = c.Boolean(nullable: false),
                        IsPrimaryApproach = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DirectionTypes", t => t.DirectionTypeId, cascadeDelete: true)
                .ForeignKey("dbo.RouteSignals", t => t.RouteSignalId, cascadeDelete: true)
                .Index(t => t.RouteSignalId)
                .Index(t => t.DirectionTypeId);
            
            CreateTable(
                "dbo.RouteSignals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RouteId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        SignalId = c.String(nullable: false),
                        Signal_VersionID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Routes", t => t.RouteId, cascadeDelete: true)
                .ForeignKey("dbo.Signals", t => t.Signal_VersionID)
                .Index(t => t.RouteId)
                .Index(t => t.Signal_VersionID);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RouteName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RouteSignals", "Signal_VersionID", "dbo.Signals");
            DropForeignKey("dbo.RouteSignals", "RouteId", "dbo.Routes");
            DropForeignKey("dbo.RoutePhaseDirections", "RouteSignalId", "dbo.RouteSignals");
            DropForeignKey("dbo.RoutePhaseDirections", "DirectionTypeId", "dbo.DirectionTypes");
            DropIndex("dbo.RouteSignals", new[] { "Signal_VersionID" });
            DropIndex("dbo.RouteSignals", new[] { "RouteId" });
            DropIndex("dbo.RoutePhaseDirections", new[] { "DirectionTypeId" });
            DropIndex("dbo.RoutePhaseDirections", new[] { "RouteSignalId" });
            DropTable("dbo.Routes");
            DropTable("dbo.RouteSignals");
            DropTable("dbo.RoutePhaseDirections");
        }
    }
}
