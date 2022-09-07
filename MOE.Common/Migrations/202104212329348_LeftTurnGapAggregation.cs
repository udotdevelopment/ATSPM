namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeftTurnGapAggregation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhaseLeftTurnGapAggregations",
                c => new
                    {
                        BinStartTime = c.DateTime(nullable: false),
                        SignalId = c.String(nullable: false, maxLength: 10),
                        PhaseNumber = c.Int(nullable: false),
                        ApproachId = c.Int(nullable: false),
                        GapCount1 = c.Int(nullable: false),
                        GapCount2 = c.Int(nullable: false),
                        GapCount3 = c.Int(nullable: false),
                        GapCount4 = c.Int(nullable: false),
                        GapCount5 = c.Int(nullable: false),
                        GapCount6 = c.Int(nullable: false),
                        GapCount7 = c.Int(nullable: false),
                        GapCount8 = c.Int(nullable: false),
                        GapCount9 = c.Int(nullable: false),
                        GapCount10 = c.Int(nullable: false),
                        GapCount11 = c.Int(nullable: false),
                        SumGapDuration1 = c.Double(nullable: false),
                        SumGapDuration2 = c.Double(nullable: false),
                        SumGapDuration3 = c.Double(nullable: false),
                        SumGreenTime = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.BinStartTime, t.SignalId, t.PhaseNumber });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PhaseLeftTurnGapAggregations");
        }
    }
}
