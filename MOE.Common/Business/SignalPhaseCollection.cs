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
        

        List<SignalPhase> signalPhaseList = new List<SignalPhase>();
        public List<SignalPhase> SignalPhaseList
        {
            get { return signalPhaseList; }
        }

        public SignalPhaseCollection(DateTime startDate, DateTime endDate, string signalID,
            bool showVolume, int binSize, int metricTypeID)
        {
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = repository.GetVersionOfSignalByDate(signalID, startDate);
     
            List<Models.Approach> approaches = signal.GetApproachesForSignalThatSupportMetric(metricTypeID);
            if (signal.Approaches != null && approaches.Count > 0)
            {
                Parallel.ForEach(approaches, approach =>
                //foreach (Models.Approach approach in approaches)
                {
                    String direction = approach.DirectionType.Description;
                    bool isOverlap = approach.IsProtectedPhaseOverlap;
                    int phaseNumber = approach.ProtectedPhaseNumber;
                    //double offset = approach.GetOffset();

                    //Get the phase
                    MOE.Common.Business.SignalPhase signalPhase = new MOE.Common.Business.SignalPhase(
                        startDate, endDate, approach, showVolume, binSize, metricTypeID, false);

                    //try not to add the same direction twice
                    var ExsitingPhases = from MOE.Common.Business.SignalPhase phase in this.SignalPhaseList
                                         where phase.Approach.DirectionType.Description == signalPhase.Approach.DirectionType.Description
                                         select phase;

                    if (ExsitingPhases.Count() < 1)
                    {
                        this.SignalPhaseList.Add(signalPhase);
                    }

                });
                this.signalPhaseList = signalPhaseList.OrderBy(s => s.Approach.ProtectedPhaseNumber).ToList();
            }
        }

 
    }
}
