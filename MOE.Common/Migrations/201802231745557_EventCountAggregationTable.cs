namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventCountAggregationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventCountAggregations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        SignalId = c.String(nullable: false),
                        EventCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EventCountAggregations");
        }
    }
}
