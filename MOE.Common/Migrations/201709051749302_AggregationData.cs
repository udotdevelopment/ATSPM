namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggregationData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SignalAggregationDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SignalID = c.String(nullable: false, maxLength: 10),
                        TotalCycles = c.Int(nullable: false),
                        AddCyclesInTransition = c.Int(nullable: false),
                        SubtractCyclesInTransition = c.Int(nullable: false),
                        DwellCyclesInTransition = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Signals", t => t.SignalID, cascadeDelete: true)
                .Index(t => t.SignalID);
            
            DropTable("dbo.Archived_Metrics");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Archived_Metrics",
                c => new
                    {
                        Timestamp = c.DateTime(nullable: false),
                        DetectorID = c.String(nullable: false, maxLength: 50, unicode: false),
                        BinSize = c.Int(nullable: false),
                        Volume = c.Int(),
                        speed = c.Int(),
                        delay = c.Int(),
                        AoR = c.Int(),
                        SpeedHits = c.Int(),
                        BinGreenTime = c.Int(),
                    })
                .PrimaryKey(t => new { t.Timestamp, t.DetectorID, t.BinSize });
            
            DropForeignKey("dbo.SignalAggregationDatas", "SignalID", "dbo.Signals");
            DropIndex("dbo.SignalAggregationDatas", new[] { "SignalID" });
            DropTable("dbo.SignalAggregationDatas");
        }
    }
}
