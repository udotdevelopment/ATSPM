namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SplitMonitorAggregation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhaseSplitMonitorAggregations",
                c => new
                    {
                        BinStartTime = c.DateTime(nullable: false),
                        SignalId = c.String(nullable: false, maxLength: 128),
                        PhaseNumber = c.Int(nullable: false),
                        EightyFifthPercentileSplit = c.Int(nullable: false),
                        SkippedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BinStartTime, t.SignalId, t.PhaseNumber });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PhaseSplitMonitorAggregations");
        }
    }
}
