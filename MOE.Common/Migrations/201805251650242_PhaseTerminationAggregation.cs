namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhaseTerminationAggregation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhaseTerminationAggregations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BinStartTime = c.DateTime(nullable: false),
                        SignalId = c.String(nullable: false),
                        PhaseNumber = c.Int(nullable: false),
                        GapOuts = c.Int(nullable: false),
                        ForceOffs = c.Int(nullable: false),
                        MaxOuts = c.Int(nullable: false),
                        UnknownTerminationTypes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.ApproachSplitFailAggregations", "GapOuts");
            DropColumn("dbo.ApproachSplitFailAggregations", "ForceOffs");
            DropColumn("dbo.ApproachSplitFailAggregations", "MaxOuts");
            DropColumn("dbo.ApproachSplitFailAggregations", "UnknownTerminationTypes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApproachSplitFailAggregations", "UnknownTerminationTypes", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "MaxOuts", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "ForceOffs", c => c.Int(nullable: false));
            AddColumn("dbo.ApproachSplitFailAggregations", "GapOuts", c => c.Int(nullable: false));
            DropTable("dbo.PhaseTerminationAggregations");
        }
    }
}
