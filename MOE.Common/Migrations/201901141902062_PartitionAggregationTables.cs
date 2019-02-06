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

            //string sqlResVerboseStatus = typeof(PartitionAggregationTables).Namespace + ".Createpf.sql";
            //this.SqlResource(sqlResVerboseStatus);

            //using (StreamWriter writetext = new StreamWriter(@"c:\temp\TestAndre.txt"))
            //{
            //    writetext.WriteLine(shouldPartitionTablesBePartition);
            //    writetext.WriteLine(locationOfAggregationPartitionFiles);
            //}

            DropPrimaryKey("dbo.ApproachCycleAggregations");
            DropPrimaryKey("dbo.ApproachEventCountAggregations");
            DropPrimaryKey("dbo.ApproachPcdAggregations");
            DropPrimaryKey("dbo.ApproachSpeedAggregations");
            DropPrimaryKey("dbo.ApproachSplitFailAggregations");
            DropPrimaryKey("dbo.ApproachYellowRedActivationAggregations");
            DropPrimaryKey("dbo.DetectorAggregations");
            DropPrimaryKey("dbo.DetectorEventCountAggregations");
            DropPrimaryKey("dbo.PhasePedAggregations");
            DropPrimaryKey("dbo.PhaseTerminationAggregations");
            DropPrimaryKey("dbo.PreemptionAggregations");
            DropPrimaryKey("dbo.PriorityAggregations");
            DropPrimaryKey("dbo.SignalEventCountAggregations");

            AddPrimaryKey("dbo.ApproachCycleAggregations","Id", null, false);
            AddPrimaryKey("dbo.ApproachEventCountAggregations", "Id", null, false);
            AddPrimaryKey("dbo.ApproachPcdAggregations", "Id", null, false);
            AddPrimaryKey("dbo.ApproachSpeedAggregations", "Id", null, false);
            AddPrimaryKey("dbo.ApproachSplitFailAggregations", "Id", null, false);
            AddPrimaryKey("dbo.ApproachYellowRedActivationAggregations", "Id", null, false);
            AddPrimaryKey("dbo.DetectorAggregations", "Id", null, false);
            AddPrimaryKey("dbo.DetectorEventCountAggregations", "Id", null, false);
            AddPrimaryKey("dbo.PhasePedAggregations", "Id", null, false);
            AddPrimaryKey("dbo.PhaseTerminationAggregations", "Id", null, false);
            AddPrimaryKey("dbo.PreemptionAggregations", "Id", null, false);
            AddPrimaryKey("dbo.PriorityAggregations", "Id", null, false);
            AddPrimaryKey("dbo.SignalEventCountAggregations", "Id", null, false);

 

            CreateIndex("dbo.ApproachCycleAggregations", "BinStartTime", false,
                "IX_ClusteredApproachCycleAggregationsBinStartTime", true);
            CreateIndex("dbo.ApproachCycleAggregations", new string[2] { "ApproachId", "TotalCycles" }, false,
                "IX_ApproachEventCountAggregationsBinTimeStartbySignalId");

            CreateIndex("dbo.ApproachEventCountAggregations", "BinStartTime", false,
                "IX_ClusteredApproachEventCountAggregationsBinStartTime", true);
            CreateIndex("dbo.ApproachEventCountAggregations", new string[2] { "ApproachId", "BinStartTime"  }, false,
                "IX_BinApproachEventCountAggregationsTimeStartbySignalId");

            CreateIndex("dbo.ApproachPcdAggregations", "BinStartTime", false,
                "IX_ClusteredApproachPcdAggregationsBinStartTime", true);
            CreateIndex("dbo.ApproachPcdAggregations", new string[2] { "ApproachId", "BinStartTime"}, false,
                "IX_ApproachPcdAggregationsBinTimeStartbySignalId");

            CreateIndex("dbo.ApproachSpeedAggregations", "BinStartTime", false,
                "IX_ClusteredApproachSpeedAggregationsBinStartTime", true);
            CreateIndex("dbo.ApproachSpeedAggregations", new string[2] { "ApproachId", "BinStartTime" }, false,
                "IX_ApproachSpeedAggregationsBinTimeStartbySignalId");

            CreateIndex("dbo.ApproachSplitFailAggregations", "BinStartTime", false,
                "IX_ClusteredApproachSplitFailAggregationsBinStartTime", true);
            CreateIndex("dbo.ApproachSplitFailAggregations", new string[2] { "BinStartTime", "ApproachId" }, false,
                "IX_ApproachSplitFailAggregationsBinTimeStartbySignalId");

            CreateIndex("dbo.ApproachYellowRedActivationAggregations", "BinStartTime", false,
                "IX_ClusteredApproachYellowRedBinStartTime", true);
            CreateIndex("dbo.ApproachYellowRedActivationAggregations", new string[2] { "BinStartTime", "ApproachId" }, false,
                "IX_ApproachYellowRedBinTimeStartbySignalId");

            CreateIndex("dbo.DetectorAggregations", "BinStartTime", false,
                "IX_ClusteredDetectorAggregationsBinStartTime", true);
            CreateIndex("dbo.DetectorAggregations", new string[2] { "BinStartTime", "DetectorPrimaryId" }, false,
                "IX_DetectorAggregationsBinTimeStartbySignalId");

            CreateIndex("dbo.DetectorEventCountAggregations", "BinStartTime", false,
                "IX_ClusteredDetectorEventCountAggregationsBinStartTime", true);
            //CreateIndex("dbo.DetectorEventCountAggregations", new string[2] { "BinStartTime", "DetectorId" }, false,
            //    "IX_DetectorEventCountAggregationsBinTimeStartbySignalId");

            CreateIndex("dbo.PhasePedAggregations", "BinStartTime", false,
                "IX_ClusteredPhasePedAggregationsBinStartTime", true);
            //CreateIndex("dbo.PhasePedAggregations", new string[2] { "BinStartTime", "SignalId" }, false,
            //    "IX_PhasePedAggregationsBinTimeStartbySignalId");

            CreateIndex("dbo.PhaseTerminationAggregations", "BinStartTime", false,
                "IX_ClusteredPhaseTerminationBinStartTime", true);
            //CreateIndex("dbo.PhaseTerminationAggregations", new string[2] { "BinStartTime", "SignalId" }, false,
            //    "IX_PhaseTerminationBinTimeStartbySignalId");

            CreateIndex("dbo.PreemptionAggregations", "BinStartTime", false,
                "IX_ClusteredPreemptionAggregationsBinStartTime", true);
            //CreateIndex("dbo.PreemptionAggregations", new string[2] { "BinStartTime", "SignalId" }, false,
            //    "IX_PreemptionAggregationsBinTimeStartbySignalId");

            CreateIndex("dbo.PriorityAggregations", "BinStartTime", false,
                "IX_ClusteredPriorityAggregationsBinTimeStart", true);
            //CreateIndex("dbo.PriorityAggregations", new string[2] {"BinStartTime", "SignalId"}, false,
            //    "IX_PriorityAggregationsBinTimeStartbySignalId");

            CreateIndex("dbo.SignalEventCountAggregations", "BinStartTime", false,
                "IX_ClusteredSignalEventCountBinStartTime", true);
            //CreateIndex("dbo.SignalEventCountAggregations", new string[2] { "BinStartTime", "SignalId" }, false,
            //    "IX_SignalEventCountBinTimeStartbySignalId");
        }


        public override void Down()
        {
            // Need to put the items that are required for the down or restopre before this event is run.
        }

    }
}
