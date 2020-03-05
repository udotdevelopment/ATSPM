namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetSignlIdtoTenCharacters : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActionLogs", "SignalID", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.MetricComments", "SignalID", c => c.String(maxLength: 10));
            AlterColumn("dbo.DatabaseArchiveExcludedSignals", "SignalId", c => c.String(maxLength: 10));
            AlterColumn("dbo.RouteSignals", "SignalId", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.SPMWatchDogErrorEvents", "SignalID", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SPMWatchDogErrorEvents", "SignalID", c => c.String(nullable: false));
            AlterColumn("dbo.RouteSignals", "SignalId", c => c.String(nullable: false));
            AlterColumn("dbo.DatabaseArchiveExcludedSignals", "SignalId", c => c.String());
            AlterColumn("dbo.MetricComments", "SignalID", c => c.String());
            AlterColumn("dbo.ActionLogs", "SignalID", c => c.String(nullable: false));
        }
    }
}
