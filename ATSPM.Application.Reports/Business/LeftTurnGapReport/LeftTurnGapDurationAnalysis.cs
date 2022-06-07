using ATSPM.Application.Models;
using ATSPM.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport
{
    public class LeftTurnGapDurationAnalysis
    {
        private readonly IApproachRepository _approachRepository;
        private readonly IDetectorRepository _detectorRepository;
        private readonly IDetectorEventCountAggregationRepository _detectorEventCountAggregationRepository;
        private readonly IPhaseLeftTurnGapAggregationRepository _phaseLeftTurnGapAggregationRepository;
        private readonly ISignalsRepository _signalsRepository;
        public LeftTurnGapDurationAnalysis(
            IApproachRepository approachRepository,
            IDetectorRepository detectorRepository,
            IDetectorEventCountAggregationRepository detectorEventCountAggregationRepository,
            IPhaseLeftTurnGapAggregationRepository phaseLeftTurnGapAggregationRepository, 
            ISignalsRepository signalsRepository)
        {
            _approachRepository = approachRepository;
            _detectorRepository = detectorRepository;
            _detectorEventCountAggregationRepository = detectorEventCountAggregationRepository;
            _phaseLeftTurnGapAggregationRepository = phaseLeftTurnGapAggregationRepository;
            _signalsRepository = signalsRepository;
        }
        public GapDurationResult GetPercentOfGapDuration(string signalId, int approachId, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime, int[] daysOfWeek)
        {
            var signal = _signalsRepository.GetVersionOfSignalByDate(signalId, start);
            var approach = signal.Approaches.Where(a => a.ApproachId == approachId).FirstOrDefault();
            int opposingPhase = LeftTurnReportPreCheck.GetOpposingPhase(approach);
            int numberOfOposingLanes = GetNumberOfOpposingLanes(signal, opposingPhase);
            double criticalGap = GetCriticalGap(numberOfOposingLanes);
            var gapDurationResult = new GapDurationResult
            {
                Capacity = GetGapSummedTotal(signalId, opposingPhase, start, end, startTime, endTime, criticalGap, daysOfWeek),
                AcceptableGaps = GetGapsList(signalId, opposingPhase, start, end, startTime, endTime, criticalGap, daysOfWeek),
                Demand = GetGapDemand(approachId, start, end, startTime, endTime, criticalGap),
                Direction = approach.DirectionType.Abbreviation + approach.Detectors.FirstOrDefault()?.MovementType.Abbreviation,
                OpposingDirection = GetOpposingPhaseDirection(signal, opposingPhase)
            };
            if (gapDurationResult.Capacity == 0)
                throw new ArithmeticException("Gap Count cannot be zero");
            gapDurationResult.GapDurationPercent = gapDurationResult.Demand / (gapDurationResult.Capacity);
            return gapDurationResult;
        }

        private Dictionary<DateTime, double> GetGapsList(string signalId, int phaseNumber, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime, double criticalGap, int[] daysOfWeek)
        {
            List<Models.PhaseLeftTurnGapAggregation> amAggregations = new List<Models.PhaseLeftTurnGapAggregation>();
            int gapColumn = GetGapColumn(criticalGap);
            Dictionary<DateTime, double> acceptableGaps = new Dictionary<DateTime, double>();
            for (var tempDate = start.Date; tempDate <= end; tempDate = tempDate.AddDays(1))
            {
                for (var tempStart = tempDate.Date.Add(startTime); tempStart < tempDate.Date.Add(endTime); tempStart = tempStart.AddMinutes(15))
                {
                    if (daysOfWeek.Contains((int)start.DayOfWeek))
                    {
                        var leftTurnGaps = _phaseLeftTurnGapAggregationRepository.GetPhaseLeftTurnGapAggregationBySignalIdPhaseNumberAndDateRange(
                                 signalId, phaseNumber, tempStart, tempStart.Add(startTime).AddMinutes(15));
                        int count = 0;
                        double sum = 0;
                        if (gapColumn == 12)
                        {
                            count = leftTurnGaps.Sum(l => l.GapCount6 + l.GapCount7 + l.GapCount8 + l.GapCount9);
                            sum = leftTurnGaps.Sum(l => (l.SumGapDuration1/4.1));
                        }
                        else
                        {
                            count = leftTurnGaps.Sum(l => l.GapCount7 + l.GapCount8 + l.GapCount9);
                            sum = leftTurnGaps.Sum(l => (l.SumGapDuration2/5.3));
                        }

                        acceptableGaps.Add(tempStart, sum);
                    }
                }
            }
            return acceptableGaps;
        }

        private double GetGapDemand( int approachId, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime, double criticalGap)
        {
            var detectors = LeftTurnReportPreCheck.GetLeftTurnDetectors(approachId, _approachRepository);
            int totalActivations = 0;
            for (var tempDate = start.Date; tempDate <= end; tempDate = tempDate.AddDays(1))
            {
                foreach (var detector in detectors)
                {
                    totalActivations += _detectorEventCountAggregationRepository.GetDetectorEventCountSumAggregationByDetectorIdAndDateRange(detector.Id, tempDate.Date.Add(startTime), tempDate.Date.Add(endTime));
                }
            }
            return CalculateGapDemand(criticalGap, totalActivations);
        }

        private double GetGapSummedTotal(string signalId, int phaseNumber, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime, double criticalGap, int[] daysOfWeek)
        {
            List<Models.PhaseLeftTurnGapAggregation> amAggregations = new List<Models.PhaseLeftTurnGapAggregation>();
            int gapColumn = GetGapColumn(criticalGap);
            double gapTotal = 0;
            for (var tempDate = start.Date; tempDate <= end; tempDate = tempDate.AddDays(1))
            {
                if (daysOfWeek.Contains((int)start.DayOfWeek))
                    gapTotal += _phaseLeftTurnGapAggregationRepository.GetSummedGapsBySignalIdPhaseNumberAndDateRange(
                         signalId, phaseNumber, tempDate.Date.Add(startTime), tempDate.Date.Add(endTime), gapColumn);
            }
            return gapTotal;
        }

        

        public int GetNumberOfOpposingLanes(Signal signal,  int opposingPhase)
        {
            return signal
                .Approaches
                .SelectMany(a => a.Detectors)
                .Where(d => d.DetectionTypeDetectors.First().DetectionTypeId == 4)
                .Count(d => d.Approach.ProtectedPhaseNumber == opposingPhase);            
        }

        public string GetOpposingPhaseDirection(Signal signal, int opposingPhase)
        {
            return signal
                .Approaches
                .Where(d => d.ProtectedPhaseNumber == opposingPhase)
                .FirstOrDefault()?.DirectionType.Abbreviation;
        }

        //static functions

        public static int GetGapColumn(double criticalGap)
        {
            return criticalGap switch
            {
                4.1 => 12,
                5.3 => 13,
                _ => 12
            };
        }

        public static double GetCriticalGap(int numberOfOposingLanes)
        {
            if (numberOfOposingLanes <= 2)
            {
                return 4.1;
            }
            else
            {
                return 5.3;
            }
        }

        public static int SumGapColumns(int gapColumn, List<Models.PhaseLeftTurnGapAggregation> leftTurnGaps)
        {
            return gapColumn switch
            {
                12 => leftTurnGaps.Sum(l => l.GapCount6 + l.GapCount7 + l.GapCount8 + l.GapCount9),
                _ => leftTurnGaps.Sum(l => l.GapCount7 + l.GapCount8 + l.GapCount9)
            };
        }

        public static double CalculateGapDemand(double criticalGap, int totalActivations)
        {
            return totalActivations * criticalGap;
        }
    }
}
