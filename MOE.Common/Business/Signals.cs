using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Reflection;

namespace MOE.Common.Business
{
    public class Signals
    {
        /// <summary>
        /// Gets all the signals based on the region
        /// </summary>
        /// <param name="region"></param>
        /// <returns>Data.Signals.MasterDataTable</returns>


        public static List<Models.Signal> GetSignals()
        {
            MOE.Common.Models.SPM db = new Models.SPM();

            var table = (from r in db.Signals select r).ToList();         

            return table;
        }
        /// <summary>
        /// Gets the Appraoches for a given signal
        /// </summary>
        /// <param name="signalId"></param>
        /// <returns>Data.Signals.DetectorsDataTable</returns>
        //public static List<Models.Detectors> GetDistinctPhases(string signalId)//, string region)
        //{
        //    MOE.Common.Models.Repositories.GraphDetectorRepository gdr = new Models.Repositories.GraphDetectorRepository();
            
        //        var table = gdr.GetPCDPhases(signalId);

        //        return table;
            
        //    //Data.MOE.DistinctSignalPhaseDataTable table =
        //    //    new MOE.Common.Data.MOE.DistinctSignalPhaseDataTable();
        //    //Data.MOETableAdapters.DistinctSignalPhaseTableAdapter adapter =
        //    //    new MOE.Common.Data.MOETableAdapters.DistinctSignalPhaseTableAdapter();
        //    //adapter.Fill(table, signalId);
        //    //return table;
        //}

        

        /// <summary>
        /// Gets the Appraoches for a given signal filtered by report type
        /// </summary>
        /// <param name="signalId"></param>
        /// <returns>Data.Signals.DetectorsDataTable</returns>
        //public static List<Models.Detectors> GetDistinctPhases(string signalId, string reporttype)
        //{

        //    using (MOE.Common.Models.SPM db = new Models.SPM())
        //    {
        //        var table = (from r in db.Detectors
        //                     where r.SignalID == signalId && r.CheckReportAvialbility(reporttype) == true                            
        //                     select r).ToList();

        //        return table;
        //    }
        //    //Data.MOE.DistinctSignalPhasebyReportDataTable table =
        //    //    new MOE.Common.Data.MOE.DistinctSignalPhasebyReportDataTable();
        //    //Data.MOETableAdapters.DistinctSignalPhasebyReportTableAdapter adapter =
        //    //    new MOE.Common.Data.MOETableAdapters.DistinctSignalPhasebyReportTableAdapter();
        //    //adapter.Fill(table, signalId.ToString(), reporttype);
        //    //return table;
        //}

        

        
       
    }
}
