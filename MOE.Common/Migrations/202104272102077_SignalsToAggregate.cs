namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SignalsToAggregate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SignalToAggregates",
                c => new
                    {
                        SignalID = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.SignalID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SignalToAggregates");
        }
    }
}
