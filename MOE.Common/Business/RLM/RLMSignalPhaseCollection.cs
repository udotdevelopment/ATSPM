using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class RLMSignalPhaseCollection
    {
        public double Violations
        {
            get
            {
                return SignalPhaseList.Sum(d => d.Violations);
            }
        }

        List<RLMSignalPhase> signalPhaseList = new List<RLMSignalPhase>();
        public List<RLMSignalPhase> SignalPhaseList
        {
            get { return signalPhaseList; }
        }

        private double severeRedLightViolationsSeconds = 0;
        public double SevereRedLightViolationsSeconds
        {
            get
            {
                return severeRedLightViolationsSeconds;
            }
        }

        public double Srlv
        {
            get
            {
                return SignalPhaseList.Sum(d => d.Srlv);
            }
        }

        public RLMSignalPhaseCollection(DateTime startDate, DateTime endDate, string signalID,
             int binSize, double srlvSeconds)
        {
            int metricTypeID = 11;
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = repository.GetSignalBySignalID(signalID);
            this.severeRedLightViolationsSeconds = srlvSeconds;
            var approachesForMetric = signal.GetApproachesForSignalThatSupportMetric(metricTypeID);
            //If there are phases in the database add the charts
            if (approachesForMetric.Count() > 0)
            {
                foreach (MOE.Common.Models.Approach approach in approachesForMetric)
                {                    

                    this.SignalPhaseList.Add(new MOE.Common.Business.RLMSignalPhase(
                    startDate, endDate, binSize, this.SevereRedLightViolationsSeconds, 
                    metricTypeID, approach, false));

                    if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber > 0)
                    {
                        this.SignalPhaseList.Add(new MOE.Common.Business.RLMSignalPhase(
                        startDate, endDate, binSize, this.SevereRedLightViolationsSeconds,
                        metricTypeID, approach, true));
                    }
                }
            }
        }
    }
}
