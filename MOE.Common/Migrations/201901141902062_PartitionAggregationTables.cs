using System.Configuration;
using System.Data.Entity.Migrations.Model;
using System.IO;
using System.ServiceModel.Channels;
using System.Web.UI.WebControls;

namespace MOE.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartitionAggregationTables : DbMigration
    {
        public override void Up()
        {
            //string shouldPartitionTablesBePartition = ConfigurationManager.AppSettings["PartitionAggregrationTables"];
            //string locationOfAggregationPartitionFiles = ConfigurationManager.AppSettings["LocationOfAggregationPartitionFiles"];
            //string IntialFileSize = ConfigurationManager.AppSettings["IntialFileSizeSize"];
            //string StartYear = ConfigurationManager.AppSettings["StartYear"];
            //string EndYear = ConfigurationManager.AppSettings["EndYear"];
            //string DBName = ConfigurationManager.AppSettings["DBName"];

            //CreateTable(
            //        "dbo.PartitionFiles",
            //        c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            PartitionFileLocation = c.String(),
            //            ShouldBePartition = c.Boolean(nullable: false),
            //            })
            //    .PrimaryKey(t => t.Id);

            //string sqlResVerboseStatus = typeof(PartitionAggregationTables).Namespace + ".Createpf.sql";
            //this.SqlResource(sqlResVerboseStatus);

            //using (StreamWriter writetext = new StreamWriter(@"c:\temp\TestAndre.txt"))
            //{
            //    writetext.WriteLine(shouldPartitionTablesBePartition);
            //    writetext.WriteLine(locationOfAggregationPartitionFiles);
            //}

           // DropPrimaryKey("dbo.ApproachCycleAggregations");
           // DropPrimaryKey("dbo.ApproachEventCountAggregations");
           // DropPrimaryKey("dbo.ApproachPcdAggregations");
           // DropPrimaryKey("dbo.ApproachSpeedAggregations");
           // DropPrimaryKey("dbo.ApproachSplitFailAggregations");
           // DropPrimaryKey("dbo.ApproachYellowRedActivationAggregations");
           // DropPrimaryKey("dbo.DetectorAggregations");
           // DropPrimaryKey("dbo.DetectorEventCountAggregations");
           // DropPrimaryKey("dbo.PhasePedAggregations");
           // DropPrimaryKey("dbo.PhaseTerminationAggregations");
           // DropPrimaryKey("dbo.PreemptionAggregations");
           // DropPrimaryKey("dbo.PriorityAggregations");
           // DropPrimaryKey("dbo.SignalEventCountAggregations");
           // CreateIndex("dbo.PriorityAggregations", "BinTimeStamp", "SignalId" );

        }

        public override void Down()
        {
        }

    }
}
