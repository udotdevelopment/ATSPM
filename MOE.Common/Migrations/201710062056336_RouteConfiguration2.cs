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
                        ApproachRouteDetailId = c.Int(nullable: false),
                        Phase = c.Int(nullable: false),
                        IsPhaseDirection1Overlap = c.Boolean(nullable: false),
                        ApproachRouteDetail_RouteDetailId = c.Int(),
                        Direction_DirectionTypeID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RouteSignals", t => t.ApproachRouteDetail_RouteDetailId)
                .ForeignKey("dbo.DirectionTypes", t => t.Direction_DirectionTypeID)
                .Index(t => t.ApproachRouteDetail_RouteDetailId)
                .Index(t => t.Direction_DirectionTypeID);
            
            CreateTable(
                "dbo.RouteSignals",
                c => new
                    {
                        RouteDetailId = c.Int(nullable: false, identity: true),
                        ApproachRouteId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        SignalId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.RouteDetailId)
                .ForeignKey("dbo.Routes", t => t.ApproachRouteId, cascadeDelete: true)
                .Index(t => t.ApproachRouteId);
            
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
            DropForeignKey("dbo.RoutePhaseDirections", "Direction_DirectionTypeID", "dbo.DirectionTypes");
            DropForeignKey("dbo.RoutePhaseDirections", "ApproachRouteDetail_RouteDetailId", "dbo.RouteSignals");
            DropForeignKey("dbo.RouteSignals", "ApproachRouteId", "dbo.Routes");
            DropIndex("dbo.RouteSignals", new[] { "ApproachRouteId" });
            DropIndex("dbo.RoutePhaseDirections", new[] { "Direction_DirectionTypeID" });
            DropIndex("dbo.RoutePhaseDirections", new[] { "ApproachRouteDetail_RouteDetailId" });
            DropTable("dbo.Routes");
            DropTable("dbo.RouteSignals");
            DropTable("dbo.RoutePhaseDirections");
        }
    }
}
