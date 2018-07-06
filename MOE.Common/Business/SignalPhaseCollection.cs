using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public class SignalPhaseCollection
    {
        public SignalPhaseCollection(DateTime startDate, DateTime endDate, string signalID,
            bool showVolume, int binSize, int metricTypeId)
        {
            var repository = SignalsRepositoryFactory.Create();
            var signal = repository.GetVersionOfSignalByDate(signalID, startDate);
            var approaches = signal.GetApproachesForSignalThatSupportMetric(metricTypeId);
            if (signal.Approaches != null && approaches.Count > 0)
            {
                //Parallel.ForEach(approaches, approach =>
                foreach (Models.Approach approach in approaches)
                {
                    var protectedSignalPhase = new SignalPhase(startDate, endDate, approach, showVolume, binSize,
                        metricTypeId, false);
                    SignalPhaseList.Add(protectedSignalPhase);
                    if (approach.PermissivePhaseNumber.HasValue)
                    {
                        var permissiveSignalPhase = new SignalPhase(startDate, endDate, approach, showVolume, binSize,
                            metricTypeId, true);
                        SignalPhaseList.Add(permissiveSignalPhase);
                    }
                }//);
                //TODO: Should we remove phases with no cycles?
                SignalPhaseList = SignalPhaseList.OrderBy(s => s.Approach.ProtectedPhaseNumber).ToList();
            }
        }

        public SignalPhaseCollection(MetricOptions options, bool showVolume, int binSize)
        {
            var repository = SignalsRepositoryFactory.Create();
            var signal = repository.GetVersionOfSignalByDate(options.SignalID, options.StartDate);
            var approaches = signal.GetApproachesForSignalThatSupportMetric(options.MetricTypeID);
            if (signal.Approaches != null && approaches.Count > 0)
            {
                //Parallel.ForEach(approaches, approach =>
                foreach (Models.Approach approach in approaches)
                {
                    if (approach.ProtectedPhaseNumber != 0)
                    {
                        var protectedSignalPhase = new SignalPhase(options.StartDate, options.EndDate, approach, showVolume, binSize, options.MetricTypeID, false);
                        SignalPhaseList.Add(protectedSignalPhase);
                    }
                }//);
                //TODO: Should we remove phases with no cycles?
                SignalPhaseList = SignalPhaseList.OrderBy(s => s.Approach.ProtectedPhaseNumber).ToList();
            }
        }

        public List<SignalPhase> SignalPhaseList { get; } = new List<SignalPhase>();
    }
}