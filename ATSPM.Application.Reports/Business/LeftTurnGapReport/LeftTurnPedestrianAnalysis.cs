﻿using ATSPM.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport
{
    public class LeftTurnPedestrianAnalysis
    {

        private readonly IApproachRepository _approachRepository;
        private readonly IDetectorRepository _detectorRepository;
        private readonly IPhasePedAggregationRepository _phasePedAggregationRepository; 
        private readonly IApproachCycleAggregationRepository _approachCycleAggregationRepository;

        public LeftTurnPedestrianAnalysis(IApproachRepository approachRepository, IDetectorRepository detectorRepository, IPhasePedAggregationRepository phasePedAggregationRepository, IApproachCycleAggregationRepository approachCycleAggregationRepository)
        {
            _approachRepository = approachRepository;
            _detectorRepository = detectorRepository;
            _phasePedAggregationRepository = phasePedAggregationRepository;
            _approachCycleAggregationRepository = approachCycleAggregationRepository;
        }
        public PedActuationResult GetPedestrianPercentage(string signalId, int approachId, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime, int[] daysOfWeek)
        {
            var detectors = LeftTurnReportPreCheck.GetLeftTurnDetectors(approachId, _approachRepository);
            var approach = _approachRepository.GetApproachByApproachID(detectors.First().ApproachId);
            int opposingPhase = LeftTurnReportPreCheck.GetOpposingPhase(approach);
            var cycleAverage = GetCycleAverage(signalId, start, end, startTime, endTime, opposingPhase, daysOfWeek);
            var pedCycleAverage = GetPedCycleAverage(signalId, start, end, startTime, endTime, opposingPhase, daysOfWeek);

            Dictionary<DateTime, double> cycleList = new Dictionary<DateTime, double>();
            foreach (var avg in cycleAverage.CycleAverageList)
            {
                if(avg.Value !=0)
                {
                    var pedAvg = pedCycleAverage.PedCycleAverageList.FirstOrDefault(p => p.Key == avg.Key);
                    cycleList.Add(avg.Key, pedAvg.Value / avg.Value);
                }
            }
            var result = new PedActuationResult();
            if (cycleAverage.CycleAverage == 0)
                result.PedActuationPercent = 0;
            else
                result.PedActuationPercent = pedCycleAverage.PedCycleAverage / cycleAverage.CycleAverage;
            result.CyclesWithPedCalls = Convert.ToInt32(Math.Round(pedCycleAverage.PedCycleAverage));
            result.PercentCyclesWithPedsList = cycleList;

            return result;
        }

        private PedCycleAverageResult GetPedCycleAverage(string signalId, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime, int phase, int[] daysOfWeek)
        {
            List<Models.PhasePedAggregation> cycleAggregations = new List<Models.PhasePedAggregation>();
            for (var tempDate = start.Date; tempDate <= end; tempDate = tempDate.AddDays(1))
            {
                if (daysOfWeek.Contains((int)start.DayOfWeek))
                    cycleAggregations.AddRange(_phasePedAggregationRepository.GetPhasePedsAggregationBySignalIdPhaseNumberAndDateRange(signalId, phase, tempDate.Date.Add(startTime), tempDate.Date.Add(endTime)));
            }
            double averagePedCycles = 0;
            if(cycleAggregations.Any())
            {
                averagePedCycles = cycleAggregations.Average(a => a.PedCycles);
            }
            Dictionary<DateTime, double> cycleList = new Dictionary<DateTime, double>();
            for (var tempDate = start.Date; tempDate <= end; tempDate = tempDate.AddDays(1))
            {
                if (daysOfWeek.Contains((int)start.DayOfWeek))
                    for (var tempstart = tempDate.Date.Add(startTime); tempstart <= tempDate.Add(endTime); tempstart = tempstart.AddMinutes(30))
                    {
                        if (cycleAggregations.Where(c => c.BinStartTime >= tempstart && c.BinStartTime < tempstart.AddMinutes(30)).Any())
                        {
                            cycleList.Add(tempstart, cycleAggregations.Where(c => c.BinStartTime >= tempstart && c.BinStartTime < tempstart.AddMinutes(30)).Average(c => c.PedCycles));
                        }
                        else
                        {
                            cycleList.Add(tempstart, 0);
                        }
                    }
            }
            var pedCycleAverageResult = new PedCycleAverageResult();
            pedCycleAverageResult.PedCycleAverageList = cycleList;
            pedCycleAverageResult.PedCycleAverage = averagePedCycles;
            return pedCycleAverageResult;
        }

        private CycleAverageResult GetCycleAverage(string signalId, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime, int phase, int[] daysOfWeek)
        {
            List<Models.PhaseCycleAggregation> cycleAggregations = new List<Models.PhaseCycleAggregation>();
            for (var tempDate = start.Date; tempDate <= end; tempDate = tempDate.AddDays(1))
            {
                if (daysOfWeek.Contains((int)start.DayOfWeek))
                    cycleAggregations.AddRange(_approachCycleAggregationRepository.GetApproachCyclesAggregationBySignalIdPhaseAndDateRange(signalId, phase, tempDate.Date.Add(startTime), tempDate.Date.Add(endTime)));
            }
            Dictionary<DateTime, double> cycleList = new Dictionary<DateTime, double>();
            for (var tempDate = start.Date; tempDate <= end; tempDate = tempDate.AddDays(1))
            {
                if (daysOfWeek.Contains((int)start.DayOfWeek)) 
                    for (var tempstart = tempDate.Date.Add(startTime); tempstart <= tempDate.Add(endTime); tempstart = tempstart.AddMinutes(30))
                    {
                        cycleList.Add(tempstart, cycleAggregations.Where(c => c.BinStartTime >= tempstart && c.BinStartTime < tempstart.AddMinutes(30)).Average(c => c.TotalRedToRedCycles));
                    }
            }
            var cycleAverage = new CycleAverageResult();
            cycleAverage.CycleAverage = cycleAggregations.Average(a => a.TotalRedToRedCycles);
            cycleAverage.CycleAverageList = cycleList;
            return cycleAverage;
        }
    }

}

public class CycleAverageResult
{
    public double CycleAverage { get; set; }
    public Dictionary<DateTime, double> CycleAverageList { get; set; }
}
public class PedCycleAverageResult
{
    public double PedCycleAverage { get; set; }
    public Dictionary<DateTime, double> PedCycleAverageList { get; set; }
}