using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                Parallel.ForEach(approaches, approach =>
                    //foreach (Models.Approach approach in approaches)
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
                });
                SignalPhaseList = SignalPhaseList.OrderBy(s => s.Approach.ProtectedPhaseNumber).ToList();
            }
        }

        public List<SignalPhase> SignalPhaseList { get; } = new List<SignalPhase>();
    }
}