namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YellowRedActivationsAggregation : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ApproachYellowRedActivationAggregations");
            AddPrimaryKey("dbo.ApproachYellowRedActivationAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase", "SignalId", "PhaseNumber" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ApproachYellowRedActivationAggregations");
            AddPrimaryKey("dbo.ApproachYellowRedActivationAggregations", new[] { "BinStartTime", "IsProtectedPhase", "SignalId", "PhaseNumber" });
        }
    }
}
