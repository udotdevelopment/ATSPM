namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PCDAggregateUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApproachPcdAggregations", "SignalId", c => c.String(nullable: false));
            AddColumn("dbo.ApproachPcdAggregations", "PhaseNumber", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachPcdAggregations", "TotalDelay", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApproachPcdAggregations", "TotalDelay");
            DropColumn("dbo.ApproachPcdAggregations", "PhaseNumber");
            DropColumn("dbo.ApproachPcdAggregations", "SignalId");
        }
    }
}
