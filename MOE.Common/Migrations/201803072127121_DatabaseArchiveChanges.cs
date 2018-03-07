namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseArchiveChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSettings", "EnableDatbaseArchive", c => c.Boolean());
            AddColumn("dbo.ApplicationSettings", "SelectedTableScheme", c => c.Int());
            AddColumn("dbo.ApplicationSettings", "MonthsToKeepIndex", c => c.Int());
            AddColumn("dbo.ApplicationSettings", "MonthsToKeepData", c => c.Int());
            DropColumn("dbo.ApplicationSettings", "SelectedUseArchive");
            DropColumn("dbo.ApplicationSettings", "SelectedTablePartition");
            DropColumn("dbo.ApplicationSettings", "MonthsToRemoveIndex");
            DropColumn("dbo.ApplicationSettings", "MonthsToRemoveData");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationSettings", "MonthsToRemoveData", c => c.Int());
            AddColumn("dbo.ApplicationSettings", "MonthsToRemoveIndex", c => c.Int());
            AddColumn("dbo.ApplicationSettings", "SelectedTablePartition", c => c.Int());
            AddColumn("dbo.ApplicationSettings", "SelectedUseArchive", c => c.Boolean());
            DropColumn("dbo.ApplicationSettings", "MonthsToKeepData");
            DropColumn("dbo.ApplicationSettings", "MonthsToKeepIndex");
            DropColumn("dbo.ApplicationSettings", "SelectedTableScheme");
            DropColumn("dbo.ApplicationSettings", "EnableDatbaseArchive");
        }
    }
}
