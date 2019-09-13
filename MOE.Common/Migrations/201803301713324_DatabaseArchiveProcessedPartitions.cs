using System.IO;

namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseArchiveProcessedPartitions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToBeProcessedTableIndexes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TableId = c.Int(),
                    IndexId = c.Int(),
                    ClusteredText = c.String(),
                    TextForIndex = c.String(),
                    IndexName = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.StatusOfProcessedTables",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartitionedTableName = c.String(),
                        TimeEntered = c.DateTime(),
                        PartitionName = c.String(),
                        PartitionYear = c.Int(),
                        PartitionMonth = c.Int(),
                        FunctionOrProcedure = c.String(),
                        SQLStatementOrMessage = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TablePartitionProcesseds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SwapTableName = c.String(nullable: false),
                        PartitionNumber = c.Int(nullable: false),
                        PartitionBeginYear = c.Int(nullable: false),
                        PartitionBeginMonth = c.Int(nullable: false),
                        FileGroupName = c.String(nullable: false),
                        PhysicalFileName = c.String(),
                        IndexRemoved = c.Boolean(nullable: false),
                        SwappedTableRemoved = c.Boolean(nullable: false),
                        TimeIndexdropped = c.DateTime(nullable: false),
                        TimeSwappedTableDropped = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.ToBeProcessededTables",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartitionedTableName = c.String(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        PreserveDataSelect = c.String(nullable: false),
                        TableId = c.Int(nullable: false),
                        PreserveDataWhere = c.String(nullable: false),
                        InsertValues = c.String(nullable: false),
                        DataBaseName = c.String(nullable: false),
                        Verbose = c.Boolean(nullable: false),
                        CreateColumns4Table = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.ToBeProcessededIndexes",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableId = c.Int(nullable: false),
                        IndexId = c.Int(nullable: false),
                        ClusterText = c.String(nullable: false),
                        TextForIndex = c.String(nullable: false),
                        IndexName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
          
        }


        public override void Down()
        {
            DropTable("dbo.ToBeProcessededTables");
            DropTable("dbo.StatusOfProcessedTables");
            DropTable("dbo.TablePartitionProcesseds");
            DropTable("dbo.ToBeProcessededIndexes");
            //DropTable("dbo.ShrinkFileGroups");
                   }
    }
}
