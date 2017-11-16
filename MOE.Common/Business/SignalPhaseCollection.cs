using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MOE.Common.Business
{
    public class SignalPhaseCollection
    {
        public List<SignalPhase> SignalPhaseList { get; } = new List<SignalPhase>();

        public SignalPhaseCollection(DateTime startDate, DateTime endDate, string signalID,
            bool showVolume, int binSize, int metricTypeId)
        {
            Models.Repositories.ISignalsRepository repository = Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = repository.GetVersionOfSignalByDate(signalID, startDate);
            List<Models.Approach> approaches = signal.GetApproachesForSignalThatSupportMetric(metricTypeId);
            if (signal.Approaches != null && approaches.Count > 0)
            {
                Parallel.ForEach(approaches, approach =>
                //foreach (Models.Approach approach in approaches)
                {
                    SignalPhase protectedSignalPhase = new SignalPhase(startDate, endDate, approach, showVolume, binSize, metricTypeId, false);
                    SignalPhaseList.Add(protectedSignalPhase);
                    if (approach.PermissivePhaseNumber.HasValue)
                    {
                        SignalPhase permissiveSignalPhase = new SignalPhase(startDate, endDate, approach, showVolume, binSize, metricTypeId, true);
                        SignalPhaseList.Add(permissiveSignalPhase);
                    }
                });
                SignalPhaseList = SignalPhaseList.OrderBy(s => s.Approach.ProtectedPhaseNumber).ToList();
            }
        }

 
    }
}
