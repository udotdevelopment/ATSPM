namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApproachCycleAggregationUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApproachCycleAggregations", "ApproachId", "dbo.Approaches");
            DropIndex("dbo.ApproachCycleAggregations", new[] { "ApproachId" });
            DropPrimaryKey("dbo.ApproachCycleAggregations");
            AddColumn("dbo.ApproachCycleAggregations", "SignalId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.ApproachCycleAggregations", "PhaseNumber", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachCycleAggregations", "TotalRedToRedCycles", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachCycleAggregations", "TotalGreenToGreenCycles", c => c.Int(nullable: false));
            AlterColumn("dbo.ApproachCycleAggregations", "RedTime", c => c.Int(nullable: false));
            AlterColumn("dbo.ApproachCycleAggregations", "YellowTime", c => c.Int(nullable: false));
            AlterColumn("dbo.ApproachCycleAggregations", "GreenTime", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ApproachCycleAggregations", new[] { "BinStartTime", "SignalId", "ApproachId", "PhaseNumber" });
            DropColumn("dbo.ApproachCycleAggregations", "TotalCycles");
            DropColumn("dbo.ApproachCycleAggregations", "PedActuations");
            DropColumn("dbo.ApproachCycleAggregations", "IsProtectedPhase");
            DropColumn("dbo.ApproachCycleAggregations", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApproachCycleAggregations", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.ApproachCycleAggregations", "IsProtectedPhase", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApproachCycleAggregations", "PedActuations", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachCycleAggregations", "TotalCycles", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.ApproachCycleAggregations");
            AlterColumn("dbo.ApproachCycleAggregations", "GreenTime", c => c.Double(nullable: false));
            AlterColumn("dbo.ApproachCycleAggregations", "YellowTime", c => c.Double(nullable: false));
            AlterColumn("dbo.ApproachCycleAggregations", "RedTime", c => c.Double(nullable: false));
            DropColumn("dbo.ApproachCycleAggregations", "TotalGreenToGreenCycles");
            DropColumn("dbo.ApproachCycleAggregations", "TotalRedToRedCycles");
            DropColumn("dbo.ApproachCycleAggregations", "PhaseNumber");
            DropColumn("dbo.ApproachCycleAggregations", "SignalId");
            AddPrimaryKey("dbo.ApproachCycleAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase" });
            CreateIndex("dbo.ApproachCycleAggregations", "ApproachId");
            AddForeignKey("dbo.ApproachCycleAggregations", "ApproachId", "dbo.Approaches", "ApproachID", cascadeDelete: true);
        }
    }
}
