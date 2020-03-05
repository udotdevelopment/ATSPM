using System;
using System.Collections.Generic;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.CustomReport
{
    public class Signal
    {
        public Signal(string signalID, DateTime startDate, DateTime endDate,
            List<int> eventCodes, int startOfCycle)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;
            EventCodes = eventCodes;
            var repository =
                SignalsRepositoryFactory.Create();
            SignalModel = repository.GetLatestVersionOfSignalBySignalID(signalID);
            Detectors = SignalModel.GetDetectorsForSignal();
            Plans = new PlansBase(signalID, startDate, endDate);

            GetPhases(startOfCycle);
        }

        public List<Models.Detector> Detectors { get; set; }

        public List<Phase> Phases { get; set; } = new List<Phase>();

        public PlansBase Plans { get; set; }

        public string SignalID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<int> EventCodes { get; set; }

        public Models.Signal SignalModel { get; set; }

        private void GetPhases(int startOfCycle)
        {
            foreach (var a in SignalModel.Approaches)
            {
                Phases.Add(new Phase(a, StartDate, EndDate, EventCodes, startOfCycle, false));

                if (a.PermissivePhaseNumber != null && a.PermissivePhaseNumber > 0)
                    Phases.Add(new Phase(a, StartDate, EndDate, EventCodes, startOfCycle, true));
            }
        }
    }
}