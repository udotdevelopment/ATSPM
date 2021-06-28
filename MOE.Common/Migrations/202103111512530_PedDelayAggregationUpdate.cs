namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PedDelayAggregationUpdate : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.PhasePedAggregations");
            AddColumn("dbo.PhasePedAggregations", "PedCycles", c => c.Int(nullable: false));
            AddColumn("dbo.PhasePedAggregations", "PedDelaySum", c => c.Int(nullable: false));
            AddColumn("dbo.PhasePedAggregations", "MinPedDelay", c => c.Int(nullable: false));
            AddColumn("dbo.PhasePedAggregations", "MaxPedDelay", c => c.Int(nullable: false));
            AddColumn("dbo.PhasePedAggregations", "PedActuations", c => c.Int(nullable: false));
            AlterColumn("dbo.PhasePedAggregations", "SignalId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.PhasePedAggregations", new[] { "BinStartTime", "SignalId", "PhaseNumber" });
            DropColumn("dbo.PhasePedAggregations", "PedCount");
            DropColumn("dbo.PhasePedAggregations", "PedDelay");
            DropColumn("dbo.PhasePedAggregations", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PhasePedAggregations", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.PhasePedAggregations", "PedDelay", c => c.Double(nullable: false));
            AddColumn("dbo.PhasePedAggregations", "PedCount", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.PhasePedAggregations");
            AlterColumn("dbo.PhasePedAggregations", "SignalId", c => c.String(nullable: false, maxLength: 10));
            DropColumn("dbo.PhasePedAggregations", "PedActuations");
            DropColumn("dbo.PhasePedAggregations", "MaxPedDelay");
            DropColumn("dbo.PhasePedAggregations", "MinPedDelay");
            DropColumn("dbo.PhasePedAggregations", "PedDelaySum");
            DropColumn("dbo.PhasePedAggregations", "PedCycles");
            AddPrimaryKey("dbo.PhasePedAggregations", new[] { "BinStartTime", "Id" });
        }
    }
}
