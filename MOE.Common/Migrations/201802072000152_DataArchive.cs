namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataArchive : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DatabaseArchiveExcludedSignals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SignalId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ApplicationSettings", "SelectedUseArchive", c => c.Boolean());
            AddColumn("dbo.ApplicationSettings", "SelectedTablePartition", c => c.Int());
            AddColumn("dbo.ApplicationSettings", "MonthsToRemoveIndex", c => c.Int());
            AddColumn("dbo.ApplicationSettings", "MonthsToRemoveData", c => c.Int());
            AddColumn("dbo.ApplicationSettings", "ArchivePath", c => c.String());
            AddColumn("dbo.ApplicationSettings", "SelectedDeleteOrMove", c => c.Int());
            AddColumn("dbo.ApplicationSettings", "NumberOfRows", c => c.Int());
            AddColumn("dbo.ApplicationSettings", "StartTime", c => c.Int());
            AddColumn("dbo.ApplicationSettings", "TimeDuration", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationSettings", "TimeDuration");
            DropColumn("dbo.ApplicationSettings", "StartTime");
            DropColumn("dbo.ApplicationSettings", "NumberOfRows");
            DropColumn("dbo.ApplicationSettings", "SelectedDeleteOrMove");
            DropColumn("dbo.ApplicationSettings", "ArchivePath");
            DropColumn("dbo.ApplicationSettings", "MonthsToRemoveData");
            DropColumn("dbo.ApplicationSettings", "MonthsToRemoveIndex");
            DropColumn("dbo.ApplicationSettings", "SelectedTablePartition");
            DropColumn("dbo.ApplicationSettings", "SelectedUseArchive");
            DropTable("dbo.DatabaseArchiveExcludedSignals");
        }
    }
}
