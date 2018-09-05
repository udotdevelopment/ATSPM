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
                    "dbo.ShrinkFileGroups",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileGroupName = c.String(),
                        CreatedTimeStamp = c.DateTime(),
                        StartedTimesStamp = c.DateTime(),
                        CompletedTimeStamp = c.DateTime(),
                        FileGroupNeedsShrink = c.Boolean(),
                        Notes = c.String(),
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

            string sqlResCreateConstraints = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".CreateConstraints.sql";
            this.SqlResource(sqlResCreateConstraints);

            string sqlResCreateIndexes = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".CreateIndexes.sql";
            this.SqlResource(sqlResCreateIndexes);

            string sqlResCreateTable = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".CreateTable.sql";
            this.SqlResource(sqlResCreateTable);

            string sqlResDoTheSwapTable = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".DoTheSwapTable.sql";
            this.SqlResource(sqlResDoTheSwapTable);

            string sqlResDropIndexes = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".DropIndexes.sql";
            this.SqlResource(sqlResDropIndexes);

            //string sqlResDropOrCompressTable = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".DropOrCompressTable.sql";
            //this.SqlResource(sqlResDropOrCompressTable);

            string sqlResFileGroup = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".FileGroup.sql";
            this.SqlResource(sqlResFileGroup);

            string sqlResIndexName = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".IndexName.sql";
            this.SqlResource(sqlResIndexName);

            string sqlResIndexNameClustered = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".IndexNameClustered.sql";
            this.SqlResource(sqlResIndexNameClustered);

            string sqlResIndexNameColumns = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".IndexNameColumns.sql";
            this.SqlResource(sqlResIndexNameColumns);

            string sqlResLowerBoundary = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".LowerBoundary.sql";
            this.SqlResource(sqlResLowerBoundary);

            string sqlResPreserveData = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".PreserveData.sql";
            this.SqlResource(sqlResPreserveData);

            string sqlResProcesstables = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".Processtables.sql";
            this.SqlResource(sqlResProcesstables);
            string sqlResStopCounter = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".StopCounter.sql";
            this.SqlResource(sqlResStopCounter);

            //string sqlResStopDropping = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".StopDropping.sql";
            //this.SqlResource(sqlResStopDropping);

            string sqlResTableName = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".TableName.sql";
            this.SqlResource(sqlResTableName);

            string sqlResUpperBoundary = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".UpperBoundary.sql";
            this.SqlResource(sqlResUpperBoundary);

            string sqlResVerbose = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".Verbose.sql";
            this.SqlResource(sqlResVerbose);

            //string sqlResReclaimFileSpaceForMoe = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".ReclaimFileSpaceForMoe.sql";
            //this.SqlResource(sqlResReclaimFileSpaceForMoe);

           
           // string sqlResName = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".CreateIntegratedSwapAndMoveDataJob.sql";
            //this.SqlResource(sqlResName);
        }


        public override void Down()
        {
            DropTable("dbo.ShrinkFileGroups");
            DropTable("dbo.StatusOfProcessedTables");
            DropTable("dbo.TablePartitionProcesseds");
            DropTable("dbo.ToBeProcessededTables");
            DropTable("dbo.ToBeProcessededIndexes");
        }
    }
}
