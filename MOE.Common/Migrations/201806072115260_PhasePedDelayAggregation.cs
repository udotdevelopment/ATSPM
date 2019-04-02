namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhasePedDelayAggregation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhasePedAggregations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        SignalId = c.String(nullable: false),
                        PhaseNumber = c.Int(nullable: false),
                        PedCount = c.Int(nullable: false),
                        PedDelay = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PhasePedAggregations");
        }
    }
}
