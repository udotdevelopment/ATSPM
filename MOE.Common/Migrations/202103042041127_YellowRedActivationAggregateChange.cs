namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YellowRedActivationAggregateChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApproachYellowRedActivationAggregations", "YellowActivations", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachYellowRedActivationAggregations", "ViolationTime", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachYellowRedActivationAggregations", "Cycles", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachYellowRedActivationAggregations", "SignalId", c => c.String(nullable: false));
            AddColumn("dbo.ApproachYellowRedActivationAggregations", "PhaseNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApproachYellowRedActivationAggregations", "PhaseNumber");
            DropColumn("dbo.ApproachYellowRedActivationAggregations", "SignalId");
            DropColumn("dbo.ApproachYellowRedActivationAggregations", "Cycles");
            DropColumn("dbo.ApproachYellowRedActivationAggregations", "ViolationTime");
            DropColumn("dbo.ApproachYellowRedActivationAggregations", "YellowActivations");
        }
    }
}
