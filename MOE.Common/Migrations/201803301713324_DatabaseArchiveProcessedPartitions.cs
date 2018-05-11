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
           // string sqlResName = typeof(DatabaseArchiveProcessedPartitions).Namespace + ".CreateIntegratedSwapAndMoveDataJob.sql";
            //this.SqlResource(sqlResName);
        }


        public override void Down()
        {
            DropTable("dbo.TablePartitionProcesseds");
        }
    }
}
