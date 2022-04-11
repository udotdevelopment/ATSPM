using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class Signals
    {
        public enum SignalType
        {
            [Description("Protected Only")]
            ProtectedOnly,
            [Description("Permissive Only")]
            PermissiveOnly,
            [Description("5-Head")]
            FiveHead,
            [Description("Flashing Yellow Arrow")]
            FYA
        }

        /// <summary>
        ///     Gets all the signals based on the region
        /// </summary>
        /// <param name="region"></param>
        /// <returns>Data.Signals.MasterDataTable</returns>
        public static List<Models.Signal> GetSignals()
        {
            var db = new SPM();

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
        //                     where r.SignalId == signalId && r.CheckReportAvialbility(reporttype) == true                            
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

        /// <summary>
        /// Gets the signal type for a signal
        /// </summary>
        /// <param name="protectedPhaseNumber"></param>
        /// <param name="permissivePhaseNumber"></param>
        /// <returns>Data.Signal.SignalType</returns>
        public static SignalType GetSignalType(int protectedPhaseNumber, int? permissivePhaseNumber)
        {
            if (protectedPhaseNumber > 0 && permissivePhaseNumber == null)
            {
                return SignalType.ProtectedOnly;

            }

            if (protectedPhaseNumber == 0 && permissivePhaseNumber > 0)
            {
                return SignalType.PermissiveOnly;
            }

            if (protectedPhaseNumber == 1 && permissivePhaseNumber == 6 ||
                protectedPhaseNumber == 3 && permissivePhaseNumber == 8 ||
                protectedPhaseNumber == 5 && permissivePhaseNumber == 6 ||
                protectedPhaseNumber == 7 && permissivePhaseNumber == 8)
            {
                return SignalType.FiveHead;
            }

            return SignalType.FYA;
        }
    }
}