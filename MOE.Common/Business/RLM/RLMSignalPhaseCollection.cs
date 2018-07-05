using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public class RLMSignalPhaseCollection
    {
        public RLMSignalPhaseCollection(DateTime startDate, DateTime endDate, string signalID,
            int binSize, double srlvSeconds)
        {
            var metricTypeID = 11;
            var repository =
                SignalsRepositoryFactory.Create();
            var signal = repository.GetVersionOfSignalByDate(signalID, startDate);
            SevereRedLightViolationsSeconds = srlvSeconds;
            var approachesForMetric = signal.GetApproachesForSignalThatSupportMetric(metricTypeID);
            //If there are phases in the database add the charts
            if (approachesForMetric.Any())
                foreach (var approach in approachesForMetric)
                {
                    SignalPhaseList.Add(new RLMSignalPhase(
                        startDate, endDate, binSize, SevereRedLightViolationsSeconds, approach, false));

                    if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber > 0)
                        SignalPhaseList.Add(new RLMSignalPhase(
                            startDate, endDate, binSize, SevereRedLightViolationsSeconds, approach, true));
                }
        }

        public double Violations
        {
            get { return SignalPhaseList.Sum(d => d.Violations); }
        }

        public List<RLMSignalPhase> SignalPhaseList { get; } = new List<RLMSignalPhase>();

        public double SevereRedLightViolationsSeconds { get; }

        public double Srlv
        {
            get { return SignalPhaseList.Sum(d => d.SevereRedLightViolations); }
        }
    }
}