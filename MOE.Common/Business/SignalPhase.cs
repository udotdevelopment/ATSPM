using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public class SignalPhase
    {
        private readonly bool _showVolume;
        private readonly int _binSize;
        private readonly int _metricTypeId;
        private readonly int _pcdCycleTime = 0;

        public SignalPhase(DateTime startDate, DateTime endDate, Approach approach,
            bool showVolume, int binSize, int metricTypeId, bool getPermissivePhase)
        {
            _showVolume = showVolume;
            _binSize = binSize;
            _metricTypeId = metricTypeId;
            StartDate = startDate;
            EndDate = endDate;
            Approach = approach;
            GetPermissivePhase = getPermissivePhase;
            GetSignalPhaseData();
        }

        public SignalPhase(DateTime startDate, DateTime endDate, Approach approach,
            bool showVolume, int binSize, int metricTypeId, bool getPermissivePhase, int pcdCycleTime)
        {
            _showVolume = showVolume;
            _binSize = binSize;
            _metricTypeId = metricTypeId;
            _pcdCycleTime = pcdCycleTime;
            StartDate = startDate;
            EndDate = endDate;
            Approach = approach;
            GetPermissivePhase = getPermissivePhase;
            GetSignalPhaseData();
        }

        public VolumeCollection Volume { get; private set; }
        public List<PlanPcd> Plans { get; private set; }
        public List<CyclePcd> Cycles { get; private set; }
        private List<Controller_Event_Log> DetectorEvents { get; set; }
        public bool GetPermissivePhase { get; }
        public Approach Approach { get; }
        public double AvgDelay => TotalDelay / TotalVolume;

        public double PercentArrivalOnGreen
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round(TotalArrivalOnGreen / TotalVolume * 100);
                return 0;
            }
        }

        public double PercentGreen
        {
            get
            {
                if (TotalTime > 0)
                    return Math.Round(TotalGreenTime / TotalTime * 100);
                return 0;
            }
        }

        public double PlatoonRatio
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round(PercentArrivalOnGreen / PercentGreen, 2);
                return 0;
            }
        }

        public double TotalArrivalOnGreen => Cycles.Sum(d => d.TotalArrivalOnGreen);
        public double TotalArrivalOnRed => Cycles.Sum(d => d.TotalArrivalOnRed);
        public double TotalArrivalOnYellow => Cycles.Sum(d => d.TotalArrivalOnYellow);
        public double TotalDelay => Cycles.Sum(d => d.TotalDelay);
        public double TotalVolume => Cycles.Sum(d => d.TotalVolume);
        public double TotalGreenTime => Cycles.Sum(d => d.TotalGreenTime);
        public double TotalYellowTime => Cycles.Sum(d => d.TotalYellowTime);
        public double TotalRedTime => Cycles.Sum(d => d.TotalRedTime);
        public double TotalTime => Cycles.Sum(d => d.TotalTime);
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public void LinkPivotAddSeconds(int seconds)
        {
            Volume = null;
            foreach (var cyclePcd in Cycles)
            {
                cyclePcd.AddSecondsToDetectorEvents(seconds);
            }
        }

        private void GetSignalPhaseData()
        {
            using (var db = new SPM())
            {
                //db.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                GetDetectorEvents(_metricTypeId,db);
                GetPlansCyclesAndEvents(db);
            }
        }

        private void GetPlansCyclesAndEvents(SPM db)
        {
            Cycles = CycleFactory.GetPcdCycles(StartDate, EndDate, Approach, DetectorEvents, GetPermissivePhase, _pcdCycleTime, db);
            Plans = PlanFactory.GetPcdPlans(Cycles, StartDate, EndDate, Approach, db);
            //GetPreemptEvents();
            if (_showVolume)
                SetVolume(DetectorEvents, _binSize);
        }

        private void GetDetectorEvents(int metricTypeId, SPM db)
        {
                var celRepository = ControllerEventLogRepositoryFactory.Create(db);
                DetectorEvents = new List<Controller_Event_Log>();
                var detectorsForMetric = Approach.GetDetectorsForMetricType(metricTypeId);
                foreach (var d in detectorsForMetric)
                    DetectorEvents.AddRange(celRepository.GetEventsByEventCodesParamWithOffsetAndLatencyCorrection(
                        Approach.SignalID, StartDate,
                        EndDate, new List<int> {82}, d.DetChannel, d.GetOffset(), d.LatencyCorrection));
            
        }


        private void SetVolume(List<Controller_Event_Log> detectorEvents, int binSize)
        {
            Volume = new VolumeCollection(StartDate, EndDate, detectorEvents, binSize);
        }
    }
}