namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApproachSpeedAggregationUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApproachSpeedAggregations", "ApproachId", "dbo.Approaches");
            DropIndex("dbo.ApproachSpeedAggregations", new[] { "ApproachId" });
            DropPrimaryKey("dbo.ApproachSpeedAggregations");
            AddColumn("dbo.ApproachSpeedAggregations", "SignalId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.ApproachSpeedAggregations", "PhaseNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.ApproachSpeedAggregations", "SummedSpeed", c => c.Int(nullable: false));
            AlterColumn("dbo.ApproachSpeedAggregations", "SpeedVolume", c => c.Int(nullable: false));
            AlterColumn("dbo.ApproachSpeedAggregations", "Speed85Th", c => c.Int(nullable: false));
            AlterColumn("dbo.ApproachSpeedAggregations", "Speed15Th", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ApproachSpeedAggregations", new[] { "BinStartTime", "SignalId", "ApproachId", "PhaseNumber" });
            DropColumn("dbo.ApproachSpeedAggregations", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApproachSpeedAggregations", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.ApproachSpeedAggregations");
            AlterColumn("dbo.ApproachSpeedAggregations", "Speed15Th", c => c.Double(nullable: false));
            AlterColumn("dbo.ApproachSpeedAggregations", "Speed85Th", c => c.Double(nullable: false));
            AlterColumn("dbo.ApproachSpeedAggregations", "SpeedVolume", c => c.Double(nullable: false));
            AlterColumn("dbo.ApproachSpeedAggregations", "SummedSpeed", c => c.Double(nullable: false));
            DropColumn("dbo.ApproachSpeedAggregations", "PhaseNumber");
            DropColumn("dbo.ApproachSpeedAggregations", "SignalId");
            AddPrimaryKey("dbo.ApproachSpeedAggregations", new[] { "BinStartTime", "ApproachId", "IsProtectedPhase", "Id" });
            CreateIndex("dbo.ApproachSpeedAggregations", "ApproachId");
            AddForeignKey("dbo.ApproachSpeedAggregations", "ApproachId", "dbo.Approaches", "ApproachID", cascadeDelete: true);
        }
    }
}
