namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shrink : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShrinkFileGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileGroupName = c.String(nullable: false),
                        CreatedTimeStamp = c.DateTime(nullable: false),
                        StartedTimesStamp = c.DateTime(nullable: false),
                        CompletedTimeStamp = c.DateTime(nullable: false),
                        FileGroupNeedsShrink = c.Boolean(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StatusOfProcessedTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartitionedTableName = c.String(nullable: false),
                        TimeEntered = c.DateTime(nullable: false),
                        PartitionName = c.String(),
                        PartitionYear = c.Int(nullable: false),
                        PartitionMonth = c.Int(nullable: false),
                        FunctionOrProcedure = c.String(),
                        SQLStatementOrMessage = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ToBeProcessedTableIndexes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableId = c.Int(nullable: false),
                        IndexId = c.Int(nullable: false),
                        ClusteredText = c.String(),
                        TextForIndex = c.String(),
                        IndexName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ToBeProcessededTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartitionedTableName = c.String(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        PreserveDataSelect = c.String(),
                        TableId = c.Int(nullable: false),
                        PreserveDataWhere = c.String(nullable: false),
                        InsertValues = c.String(),
                        DataBaseName = c.String(),
                        Verbose = c.Boolean(nullable: false),
                        CreateColumns4Table = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ToBeProcessededTables");
            DropTable("dbo.ToBeProcessedTableIndexes");
            DropTable("dbo.StatusOfProcessedTables");
            DropTable("dbo.ShrinkFileGroups");
        }
    }
}
