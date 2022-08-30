using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public class RLMSignalPhase
    {
        private int _detChannel;
        private bool _showVolume;


        public RLMSignalPhase(DateTime startDate, DateTime endDate, int binSize, double severeRedLightViolatinsSeconds,
            Approach approach, bool usePermissivePhase)
        {
            SevereRedLightViolationSeconds = severeRedLightViolatinsSeconds;
            Approach = approach;
            GetPermissivePhase = usePermissivePhase;
            using (var db = new SPM())
            {
                var controllerRepository =
                    ControllerEventLogRepositoryFactory.Create(db);
                if (!Approach.IsProtectedPhaseOverlap)
                {
                    GetSignalPhaseData(startDate, endDate, usePermissivePhase, db);
                }
                else
                {
                    TotalVolume = controllerRepository.GetTmcVolume(startDate, endDate, Approach.SignalID,
                        Approach.ProtectedPhaseNumber);
                    GetSignalOverlapData(startDate, endDate, _showVolume, binSize, db);
                }
            }
        }

        public RLMSignalPhase()
        {
        }

        public bool GetPermissivePhase { get; set; }

        public VolumeCollection Volume { get; }

        public int PhaseNumber { get; set; }

        public double Violations
        {
            get { return Plans.PlanList.Sum(d => d.Violations); }
        }

        public RLMPlanCollection Plans { get; private set; }

        public double SevereRedLightViolationSeconds { get; }

        public double SevereRedLightViolations
        {
            get { return Plans.PlanList.Sum(d => d.SevereRedLightViolations); }
        }

        public double TotalVolume { get; private set; }

        public double PercentViolations
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round(Violations / TotalVolume * 100, 0);
                return 0;
            }
        }

        public double PercentSevereViolations
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round(SevereRedLightViolations / TotalVolume * 100, 2);
                return 0;
            }
        }

        public double YellowOccurrences
        {
            get { return Plans.PlanList.Sum(d => d.YellowOccurrences); }
        }

        public double TotalYellowTime
        {
            get { return Plans.PlanList.Sum(d => d.TotalYellowTime); }
        }

        public double AverageTYLO => Math.Round(TotalYellowTime / YellowOccurrences, 1);

        public double PercentYellowOccurrences
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round(YellowOccurrences / TotalVolume * 100, 0);
                return 0;
            }
        }

        public Approach Approach { get; set; }

        private void GetSignalPhaseData(DateTime startDate, DateTime endDate, bool usePermissivePhase, SPM db)
        {
            if (!usePermissivePhase)
                PhaseNumber = Approach.ProtectedPhaseNumber;
            else
                PhaseNumber = Approach.PermissivePhaseNumber ?? 0;
                var controllerRepository =
                    ControllerEventLogRepositoryFactory.Create(db);
                TotalVolume = controllerRepository.GetTmcVolume(startDate, endDate, Approach.SignalID, PhaseNumber);
                var cycleEvents = controllerRepository.GetEventsByEventCodesParam(Approach.SignalID,
                    startDate, endDate, new List<int> { 1, 8, 9, 10, 11 }, PhaseNumber);
                Plans = new RLMPlanCollection(cycleEvents, startDate, endDate, SevereRedLightViolationSeconds, Approach, db);
                if (Plans.PlanList.Count == 0)
                    Plans.AddItem(new RLMPlan(startDate, endDate, 0, cycleEvents, SevereRedLightViolationSeconds,
                        Approach));
            
        }


        private void GetSignalOverlapData(DateTime startDate, DateTime endDate, bool showVolume, int binSize, SPM db)
        {
            var redLightTimeStamp = DateTime.MinValue;
            var li = new List<int> {62, 63, 64};
            var controllerRepository =
                ControllerEventLogRepositoryFactory.Create();
            var cycleEvents = controllerRepository.GetEventsByEventCodesParam(Approach.SignalID,
                startDate, endDate, li, Approach.ProtectedPhaseNumber);
            Plans = new RLMPlanCollection(cycleEvents, startDate, endDate, SevereRedLightViolationSeconds, Approach, db);
            if (Plans.PlanList.Count == 0)
                Plans.AddItem(new RLMPlan(startDate, endDate, 0, cycleEvents, SevereRedLightViolationSeconds,
                    Approach));
        }
    }
}