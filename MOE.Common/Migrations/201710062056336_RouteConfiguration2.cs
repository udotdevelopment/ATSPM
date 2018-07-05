using System.Data.Entity.Migrations;

namespace MOE.Common.Migrations
{
    public partial class RouteConfiguration2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.RoutePhaseDirections",
                    c => new
                    {
                        Id = c.Int(false, true),
                        RouteSignalId = c.Int(false),
                        Phase = c.Int(false),
                        DirectionTypeId = c.Int(false),
                        IsOverlap = c.Boolean(false),
                        IsPrimaryApproach = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DirectionTypes", t => t.DirectionTypeId, true)
                .ForeignKey("dbo.RouteSignals", t => t.RouteSignalId, true)
                .Index(t => t.RouteSignalId)
                .Index(t => t.DirectionTypeId);

            CreateTable(
                    "dbo.RouteSignals",
                    c => new
                    {
                        Id = c.Int(false, true),
                        RouteId = c.Int(false),
                        Order = c.Int(false),
                        SignalId = c.String(false),
                        Signal_VersionID = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Routes", t => t.RouteId, true)
                .ForeignKey("dbo.Signals", t => t.Signal_VersionID)
                .Index(t => t.RouteId)
                .Index(t => t.Signal_VersionID);

            CreateTable(
                    "dbo.Routes",
                    c => new
                    {
                        Id = c.Int(false, true),
                        RouteName = c.String(false)
                    })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.RouteSignals", "Signal_VersionID", "dbo.Signals");
            DropForeignKey("dbo.RouteSignals", "RouteId", "dbo.Routes");
            DropForeignKey("dbo.RoutePhaseDirections", "RouteSignalId", "dbo.RouteSignals");
            DropForeignKey("dbo.RoutePhaseDirections", "DirectionTypeId", "dbo.DirectionTypes");
            DropIndex("dbo.RouteSignals", new[] {"Signal_VersionID"});
            DropIndex("dbo.RouteSignals", new[] {"RouteId"});
            DropIndex("dbo.RoutePhaseDirections", new[] {"DirectionTypeId"});
            DropIndex("dbo.RoutePhaseDirections", new[] {"RouteSignalId"});
            DropTable("dbo.Routes");
            DropTable("dbo.RouteSignals");
            DropTable("dbo.RoutePhaseDirections");
        }
    }
}