namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DetectorEventAggregationUpdate : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DetectorAggregations", "PK_dbo.DetectorAggregations");
            AddColumn("dbo.DetectorAggregations", "SignalId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.DetectorAggregations", "ApproachId", c => c.Int(nullable: false));
            AddColumn("dbo.DetectorAggregations", "MovementTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.DetectorAggregations", "DirectionTypeId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.DetectorAggregations", new[] { "SignalId", "ApproachId", "BinStartTime", "DetectorPrimaryId" });
            DropColumn("dbo.DetectorAggregations", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DetectorAggregations", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.DetectorAggregations");
            DropColumn("dbo.DetectorAggregations", "DirectionTypeId");
            DropColumn("dbo.DetectorAggregations", "MovementTypeId");
            DropColumn("dbo.DetectorAggregations", "ApproachId");
            DropColumn("dbo.DetectorAggregations", "SignalId");
            AddPrimaryKey("dbo.DetectorAggregations", new[] { "BinStartTime", "DetectorPrimaryId", "Id" });
        }
    }
}
