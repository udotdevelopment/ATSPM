namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SignalPlanAggregation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SignalPlanAggregations",
                c => new
                    {
                        SignalId = c.String(nullable: false, maxLength: 128),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        PlanNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SignalId, t.Start, t.End });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SignalPlanAggregations");
        }
    }
}
