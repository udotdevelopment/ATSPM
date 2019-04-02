namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventCounts : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EventCountAggregations", newName: "SignalEventCountAggregations");
            CreateTable(
                "dbo.DetectorEventCountAggregations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        DetectorId = c.String(nullable: false),
                        EventCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PhaseEventCountAggregations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        EventCount = c.Int(nullable: false),
                        IsProtectedPhase = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PhaseEventCountAggregations");
            DropTable("dbo.DetectorEventCountAggregations");
            RenameTable(name: "dbo.SignalEventCountAggregations", newName: "EventCountAggregations");
        }
    }
}
