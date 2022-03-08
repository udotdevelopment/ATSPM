using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport
{
    public class LeftTurnReportPreCheck
    {
        private readonly IPhasePedAggregationRepository _phasePedAggregationRepository;
        private readonly IApproachRepository _approachRepository;
        private readonly IApproachCycleAggregationRepository _approachCycleAggregationRepository;
        private readonly IPhaseTerminationAggregationRepository _phaseTerminationAggregationRepository;
        private readonly ISignalsRepository _signalRepository;
        private readonly IDetectorRepository _detectorRepository;
        private readonly IDetectorEventCountAggregationRepository _detectorEventCountAggregationRepository;

        public LeftTurnReportPreCheck(IPhasePedAggregationRepository phasePedAggregationRepository, IApproachRepository approachRepository, IApproachCycleAggregationRepository approachCycleAggregationRepository,
            ISignalsRepository signalRepository, IDetectorRepository detectorRepository, IDetectorEventCountAggregationRepository detectorEventCountAggregationRepository, IPhaseTerminationAggregationRepository phaseTerminationAggregationRepository)
        {
            _phasePedAggregationRepository = phasePedAggregationRepository;
            _approachRepository = approachRepository;
            _approachCycleAggregationRepository = approachCycleAggregationRepository;
            _signalRepository = signalRepository;
            _detectorRepository = detectorRepository;
            _detectorEventCountAggregationRepository = detectorEventCountAggregationRepository;
            _phaseTerminationAggregationRepository = phaseTerminationAggregationRepository;
        }

        public Dictionary<TimeSpan, double> GetAMPMPeakPedCyclesPercentages(string signalId, int approachId, DateTime startDate, DateTime endDate, TimeSpan amStartTime,
           TimeSpan amEndTime, TimeSpan pmStartTime, TimeSpan pmEndTime, int[] daysOfWeek)
        {
            Dictionary<TimeSpan, int> peaks = GetAMPMPeakFlowRate(signalId, approachId, startDate, endDate, 
                amStartTime, amEndTime, pmStartTime, pmEndTime, daysOfWeek, _signalRepository, _approachRepository, _detectorEventCountAggregationRepository);
            Approach approach = _approachRepository.GetApproachByApproachID(approachId);
            int opposingPhase = GetOpposingPhase(approach);
            Dictionary<TimeSpan, double> averageCyles = GetAverageCycles(signalId, opposingPhase, startDate, endDate, peaks);
            Dictionary<TimeSpan, double> averagePedCycles = GetAveragePedCycles(signalId, opposingPhase, startDate, endDate, peaks);
            return GetPercentageOfPedCycles(averageCyles, averagePedCycles);
        }

        private Dictionary<TimeSpan, double> GetPercentageOfPedCycles(Dictionary<TimeSpan, double> averageCyles, Dictionary<TimeSpan, double> averagePedCycles)
        {
            if (averageCyles.Values.Contains(0))
            {
                throw new ArithmeticException("Average Gap Out cannot be zero");
            }
            if (!averageCyles.Keys.Contains(averagePedCycles.Keys.Min()) ||
                !averageCyles.Keys.Contains(averagePedCycles.Keys.Max()))
            {
                throw new IndexOutOfRangeException("Peak hours do not align for AM/PM peak");
            }
            var amPeak = averagePedCycles.Keys.Min();
            var pmPeak = averagePedCycles.Keys.Max();
            Dictionary<TimeSpan, double> percentages = new Dictionary<TimeSpan, double>();
            percentages.Add(amPeak, averagePedCycles[amPeak] / averageCyles[amPeak]);
            percentages.Add(pmPeak, averagePedCycles[pmPeak] / averageCyles[pmPeak]);
            return percentages;
        }

        private Dictionary<TimeSpan, double> GetAveragePedCycles(string signalId, int phase, DateTime startDate, DateTime endDate, Dictionary<TimeSpan, int> peaks)
        {
            //TODO: This needs to be a sum of the hour and then do average of the hour over multiple days
            Dictionary<TimeSpan, double> averagePedCycles = new Dictionary<TimeSpan, double>();
            List<PhasePedAggregation> amAggregations = new List<PhasePedAggregation>();
            var amPeak = peaks.Min(p => p.Key);
            for (var tempDate = startDate.Date; tempDate <= endDate; tempDate = tempDate.AddDays(1))
            {
                amAggregations.AddRange(_phasePedAggregationRepository.GetPhasePedsAggregationBySignalIdPhaseNumberAndDateRange(signalId, phase, tempDate.Date.Add(amPeak), tempDate.Date.Add(amPeak).AddHours(1)));
            }
            if(amAggregations.Count > 0)
                averagePedCycles.Add(amPeak, amAggregations.Average(a => a.PedCycles));
            else
                averagePedCycles.Add(amPeak, 0);
            var pmPeak = peaks.Max(p => p.Key);
            List<PhasePedAggregation> pmAggregations = new List<PhasePedAggregation>();
            for (var tempDate = startDate.Date; tempDate <= endDate; tempDate = tempDate.AddDays(1))
            {
                pmAggregations.AddRange(_phasePedAggregationRepository.GetPhasePedsAggregationBySignalIdPhaseNumberAndDateRange(signalId, phase, tempDate.Date.Add(pmPeak), tempDate.Date.Add(pmPeak).AddHours(1)));
            }
            if(pmAggregations.Count > 0)
                averagePedCycles.Add(pmPeak, pmAggregations.Average(a => a.PedCycles));
            else
                averagePedCycles.Add(pmPeak, 0);
            return averagePedCycles;
        }

        private Dictionary<TimeSpan, double> GetAverageCycles(string signalId, int phase, DateTime startDate, DateTime endDate, Dictionary<TimeSpan, int> peaks)
        {
            Dictionary<TimeSpan, double> averageCycles = new Dictionary<TimeSpan, double>();
            List<PhaseCycleAggregation> amAggregations = new List<PhaseCycleAggregation>();
            var amPeak = peaks.Min(p => p.Key);
            for (var tempDate = startDate.Date; tempDate <= endDate; tempDate = tempDate.AddDays(1))
            {
                amAggregations.AddRange(_approachCycleAggregationRepository.GetApproachCyclesAggregationBySignalIdPhaseAndDateRange(signalId, phase, tempDate.Date.Add(amPeak), tempDate.Date.Add(amPeak).AddHours(1)));
            }
            averageCycles.Add(amPeak, amAggregations.Average(a => a.TotalRedToRedCycles));
            var pmPeak = peaks.Max(p => p.Key);
            List<PhaseCycleAggregation> pmAggregations = new List<PhaseCycleAggregation>();
            for (var tempDate = startDate.Date; tempDate <= endDate; tempDate = tempDate.AddDays(1))
            {
                pmAggregations.AddRange(_approachCycleAggregationRepository.GetApproachCyclesAggregationBySignalIdPhaseAndDateRange(signalId, phase, tempDate.Date.Add(pmPeak), tempDate.Date.Add(pmPeak).AddHours(1)));
            }
            averageCycles.Add(pmPeak, pmAggregations.Average(a => a.TotalRedToRedCycles));
            return averageCycles;
        }

        public static int GetOpposingPhase(Approach approach)
        {
            //If permissive only 2 = 6, 4 = 8, 6 = 2 and 8 = 4
            if (approach.ProtectedPhaseNumber == 0 && approach.PermissivePhaseNumber.HasValue)
            {
                switch (approach.PermissivePhaseNumber)
                {
                    case 2: return 6;
                    case 4: return 8;
                    case 6: return 2;
                    case 8: return 4;
                    default: throw new ArgumentException("Invalid Phase");
                }
            }
            //1=2, 3=4, 5=6, and 7=8 if there is a protected.
            else
            {
                switch (approach.ProtectedPhaseNumber)
                {
                    case 1: return 2;                   
                    case 3: return 4;                  
                    case 5: return 6;                   
                    case 7: return 8;
                    case 2: return 6;
                    case 4: return 8;
                    case 6: return 2;
                    case 8: return 4;
                    default: throw new ArgumentException("Invalid Phase");
                }
            }

        }

        public Dictionary<TimeSpan, double> GetAMPMPeakGapOut(string signalId, int approachId, DateTime startDate, DateTime endDate, TimeSpan amStartTime,
            TimeSpan amEndTime, TimeSpan pmStartTime, TimeSpan pmEndTime, int[] daysOfWeek)
        {
            Dictionary<TimeSpan, int> peaks = GetAMPMPeakFlowRate(signalId, approachId, startDate, endDate,
                amStartTime, amEndTime, pmStartTime, pmEndTime, daysOfWeek, _signalRepository, _approachRepository,
                _detectorEventCountAggregationRepository);
            var maxCycles = GetMaxCycles(signalId, startDate, endDate, peaks);
            int phaseNumber = GetLTPhaseNumberByDirection(approachId);
            //Dictionary<TimeSpan, double> averageTerminations = GetAverageTerminationsForPhase(signalId, phaseNumber, startDate, endDate, peaks);
            Dictionary<TimeSpan, double> averageGapOuts = GetAverageGapOutsForPhase(signalId, phaseNumber, startDate, endDate, amStartTime, peaks);
            return GetPercentageOfGapOuts(maxCycles, averageGapOuts);

        }

        private Dictionary<TimeSpan, double> GetMaxCycles(string signalId, DateTime startDate, DateTime endDate, Dictionary<TimeSpan, int> peaks)
        {
            var maxCycles = new Dictionary<TimeSpan, double>();
            var amCycleCount = new List<int>();
            for (var tempDate = startDate; tempDate <= endDate; tempDate = tempDate.AddDays(1))
            {
                amCycleCount.Add(_approachCycleAggregationRepository.GetCycleCountBySignalIdAndDateRange(signalId, tempDate.Add(peaks.First().Key), tempDate.Add(peaks.First().Key).AddHours(1)));
            }
            maxCycles.Add(peaks.First().Key, amCycleCount.Average());
            var pmCycleCount = new List<int>();
            for (var tempDate = startDate; tempDate <= endDate; tempDate = tempDate.AddDays(1))
            {
                pmCycleCount.Add(_approachCycleAggregationRepository.GetCycleCountBySignalIdAndDateRange(signalId, tempDate.Add(peaks.Last().Key), tempDate.Add(peaks.Last().Key).AddHours(1)));
            }
            maxCycles.Add(peaks.Last().Key, pmCycleCount.Average());
            return maxCycles;
        }

        private Dictionary<TimeSpan, double> GetPercentageOfGapOuts(Dictionary<TimeSpan, double> maxCycles, Dictionary<TimeSpan, double> averageGapOuts)
        {
            //if (averageGapOuts.Values.Contains(0))
            //{
            //    throw new ArithmeticException("Average Gap Out cannot be zero");
            //}
            if (!averageGapOuts.Keys.Contains(averageGapOuts.Keys.Min()) ||
                !averageGapOuts.Keys.Contains(averageGapOuts.Keys.Max()))
            {
                throw new IndexOutOfRangeException("Peak hours do not align for AM/PM peak");
            }
            var amPeak = averageGapOuts.Keys.Min();
            var pmPeak = averageGapOuts.Keys.Max();
            Dictionary<TimeSpan, double> percentages = new Dictionary<TimeSpan, double>();
            percentages.Add(amPeak, averageGapOuts[amPeak] / maxCycles[amPeak]);
            percentages.Add(pmPeak, averageGapOuts[pmPeak] / maxCycles[pmPeak]);

            //TODO: Change from average terminations to max cycles sum cycles for all phases separately for an hour take the max volume
            return percentages;
        }

        private Dictionary<TimeSpan, double> GetAverageGapOutsForPhase(string signalId, int phaseNumber, DateTime startDate, DateTime endDate, TimeSpan amStartTime, Dictionary<TimeSpan, int> peaks)
        {
            Dictionary<TimeSpan, double> averages = new Dictionary<TimeSpan, double>();
            var amPeak = peaks.Min(p => p.Key);
            List<PhaseTerminationAggregation> amAggregations = new List<PhaseTerminationAggregation>();
            for (var tempDate = startDate.Date; tempDate <= endDate; tempDate = tempDate.AddDays(1))
            {
                amAggregations.AddRange(_phaseTerminationAggregationRepository.GetPhaseTerminationsAggregationBySignalIdPhaseNumberAndDateRange(signalId, phaseNumber, tempDate.Date.Add(amPeak), tempDate.Date.Add(amPeak).AddHours(1)));
            }
            LoadGapOutAverages(averages, amPeak, amAggregations);

            var pmPeak = peaks.Max(p => p.Key);
            List<PhaseTerminationAggregation> pmAggregations = new List<PhaseTerminationAggregation>();
            for (var tempDate = startDate.Date; tempDate <= endDate; tempDate = tempDate.AddDays(1))
            {
                pmAggregations.AddRange(_phaseTerminationAggregationRepository.GetPhaseTerminationsAggregationBySignalIdPhaseNumberAndDateRange(signalId, phaseNumber, tempDate.Date.Add(pmPeak), tempDate.Date.Add(pmPeak).AddHours(1)));
            }
            LoadGapOutAverages(averages, pmPeak, pmAggregations);

            return averages;
        }

        private static void LoadGapOutAverages(Dictionary<TimeSpan, double> averages, TimeSpan peak, List<PhaseTerminationAggregation> aggregations)
        {
            if (aggregations.Count() > 0)
                averages.Add(peak, aggregations.Average(a => a.GapOuts));
            else
                averages.Add(peak, 0);
        }

        private Dictionary<TimeSpan, double> GetAverageTerminationsForPhase(string signalId, int phaseNumber, DateTime startDate, DateTime endDate, Dictionary<TimeSpan, int> peaks)
        {
            Dictionary<TimeSpan, double> averages = new Dictionary<TimeSpan, double>();
            var amPeak = peaks.Min(p => p.Key);
            var pmPeak = peaks.Max(p => p.Key);
            List<PhaseTerminationAggregation> amAggregations = new List<PhaseTerminationAggregation>();
            for (var tempDate = startDate.Date; tempDate <= endDate; tempDate = tempDate.AddDays(1))
            {
                amAggregations.AddRange(_phaseTerminationAggregationRepository.GetPhaseTerminationsAggregationBySignalIdPhaseNumberAndDateRange(signalId, phaseNumber, tempDate.Date.Add(amPeak), tempDate.Date.Add(amPeak).AddHours(1)));
            }
            loadAverages(averages, amPeak, amAggregations);

            List<PhaseTerminationAggregation> pmAggregations = new List<PhaseTerminationAggregation>();
            for (var tempDate = startDate.Date; tempDate <= endDate; tempDate = tempDate.AddDays(1))
            {
                pmAggregations.AddRange(_phaseTerminationAggregationRepository.GetPhaseTerminationsAggregationBySignalIdPhaseNumberAndDateRange(signalId, phaseNumber, tempDate.Date.Add(pmPeak), tempDate.Date.Add(pmPeak).AddHours(1)));
            }
            loadAverages(averages, pmPeak, pmAggregations);
            return averages;
        }

        private static void loadAverages(Dictionary<TimeSpan, double> averages, TimeSpan peakHour, List<PhaseTerminationAggregation> aggregations)
        {
            if (aggregations.Count() > 0)
                averages.Add(peakHour, aggregations.Average(a => a.ForceOffs + a.GapOuts + a.MaxOuts + a.Unknown));
            else
                averages.Add(peakHour, 0);
        }

        private int GetLTPhaseNumberByDirection(int approachId)
        {
            var approach = _approachRepository.GetApproachByApproachID(approachId);
            return approach.PermissivePhaseNumber.HasValue ? approach.PermissivePhaseNumber.Value : approach.ProtectedPhaseNumber;
        }

        //public static Approach GetLTPhaseNumberPhaseTypeByDirection(string signalId, int approachId, IApproachRepository approachRepository)
        //{
        //    var detectors = GetLeftTurnDetectors(signalId, approachId, approachRepository);
        //    if (!detectors.Any())
        //    {
        //        throw new NotSupportedException("Detectors not found");
        //    }
        //    return approachRepository.GetApproachByApproachID(detectors.First().ApproachId);

        //}

        public static Dictionary<TimeSpan, int> GetAMPMPeakFlowRate(string signalId, int approachId, DateTime startDate, DateTime endDate, TimeSpan amStartTime,
            TimeSpan amEndTime, TimeSpan pmStartTime, TimeSpan pmEndTime, int[] daysOfWeek, ISignalsRepository signalsRepository, 
            IApproachRepository approachRepository, IDetectorEventCountAggregationRepository detectorEventCountAggregationRepository)
        {
            //TODO: This should be getting all detectors for detector type 4 and peak hour should be the same for all phases
            if (!signalsRepository.Exists(signalId))
            {
                throw new ArgumentException("Signal Not Found");
            }
            List<Models.Detector> detectors = GetAllDetectorsForSignal(signalId, startDate, signalsRepository);
            if (!detectors.Any())
            {
                throw new NotSupportedException("No Detectors found");
            }
            List<Models.DetectorEventCountAggregation> volumeAggregations = 
                GetDetectorVolumebyDetector(detectors, startDate, endDate, amStartTime,
                amEndTime, pmStartTime, pmEndTime, daysOfWeek, detectorEventCountAggregationRepository);
            if (!volumeAggregations.Any())
            {
                throw new NotSupportedException("No Detector Activation Aggregations found");
            }
            List<TimeSpan> distinctTimeSpans = volumeAggregations.Select(v => v.BinStartTime.TimeOfDay).Distinct().OrderBy(v => v).ToList();

            Dictionary<TimeSpan, int> averageByBin = GetAveragesForBins(volumeAggregations, distinctTimeSpans);

            Dictionary<TimeSpan, int> hourlyFlowRates = GetHourlyFlowRates(distinctTimeSpans, averageByBin);

            return GetAmPmPeaks(amStartTime, amEndTime, pmStartTime, pmEndTime, hourlyFlowRates);
        }

        public static Dictionary<TimeSpan, int> GetAmPmPeaks(TimeSpan amStartTime, TimeSpan amEndTime, TimeSpan pmStartTime, TimeSpan pmEndTime,
            Dictionary<TimeSpan, int> hourlyFlowRates)
        {
            var amPeak = hourlyFlowRates.Where(h => h.Key >= amStartTime && h.Key < amEndTime)
                            .OrderByDescending(h => h.Value)
                            .First();

            var pmPeak = hourlyFlowRates.Where(h => h.Key >= pmStartTime && h.Key < pmEndTime)
                .OrderByDescending(h => h.Value)
                .First();

            var returnPeaks = new Dictionary<TimeSpan, int>();
            returnPeaks.Add(amPeak.Key, amPeak.Value);
            returnPeaks.Add(pmPeak.Key, pmPeak.Value);
            return returnPeaks;
        }

        public static Dictionary<TimeSpan, int> GetHourlyFlowRates(List<TimeSpan> distinctTimeSpans, Dictionary<TimeSpan, int> averageByBin)
        {
            var hourlyFlowRates = new Dictionary<TimeSpan, int>();
            foreach (var timeSpan in distinctTimeSpans)
            {
                TimeSpan hourEnd = timeSpan.Add(TimeSpan.FromHours(1));
                hourlyFlowRates.Add(timeSpan, averageByBin.Where(d => d.Key >= timeSpan && d.Key < hourEnd).Sum(d => d.Value));
            }

            return hourlyFlowRates;
        }

        public static Dictionary<TimeSpan, int> GetAveragesForBins(List<DetectorEventCountAggregation> volumeAggregations, List<TimeSpan> distinctTimeSpans)
        {
            Dictionary<TimeSpan, int> averageByBin = new Dictionary<TimeSpan, int>();

            foreach (TimeSpan time in distinctTimeSpans)
            {
                int average = Convert.ToInt32(
                    Math.Round(volumeAggregations
                    .Where(v => v.BinStartTime.TimeOfDay == time)
                    .Average(v => v.EventCount)
                    ));
                averageByBin.Add(time, average);
            }

            return averageByBin;
        }

        public static List<DetectorEventCountAggregation> GetDetectorVolumebyDetector(List<Models.Detector> detectors, DateTime startDate,
            DateTime endDate, TimeSpan amStartTime, TimeSpan amEndTime, TimeSpan pmStartTime, TimeSpan pmEndTime,
            int[] daysOfWeek, IDetectorEventCountAggregationRepository detectorEventCountAggregationRepository)
        {
            var detectorAggregations = new List<DetectorEventCountAggregation>();
            for (var tempDate = startDate.Date; tempDate <= endDate; tempDate = tempDate.AddDays(1))
            {
                if (daysOfWeek.Contains((int)startDate.DayOfWeek))
                    foreach (var detector in detectors)
                    {
                        detectorAggregations.AddRange(detectorEventCountAggregationRepository
                            .GetDetectorEventCountAggregationByDetectorIdAndDateRange(detector.Id, tempDate.Add(amStartTime), tempDate.Add(amEndTime)));
                        detectorAggregations.AddRange(detectorEventCountAggregationRepository
                            .GetDetectorEventCountAggregationByDetectorIdAndDateRange(detector.Id, tempDate.Add(pmStartTime), tempDate.Add(pmEndTime)));
                    }
            }
            return detectorAggregations;
        }

        public static List<Models.Detector> GetLeftTurnDetectors(int approachId, IApproachRepository approachRepository)
        {
            var movementTypes = new List<int>() { 3, 5 };
            //only return detector types of type 4
            return approachRepository.GetApproachByApproachID(approachId).Detectors.Where(d => 
            d.DetectionTypeDetectors.Select(t => t.DetectionTypeId).Contains(4)
            &&  movementTypes.Contains(d.MovementTypeId.Value)).ToList();
        }

        public static List<Models.Detector> GetAllDetectorsForSignal(string signalId, DateTime date, ISignalsRepository signalsRepository)
        {
            var detectors = signalsRepository.GetVersionOfSignalByDate(signalId, date)
                .GetDetectorsForSignal();
            List<Detector> detectorsList = new List<Detector>();
            foreach (var detector in detectors)
            {
                var detectionTypeIdList = detector.DetectionTypeDetectors.Select(d => d.DetectionTypeId).ToList();
                if (detectionTypeIdList.Contains(4))
                    detectorsList.Add(detector);
            }
            if (detectorsList.Count > 0)
                return detectorsList;

            foreach (var detector in detectors)
            {
                if (detectors.Any(d => d.DetectionHardwareId == 6))
                    detectorsList.Add(detector);
            }
            return detectorsList;
        }
    }
}
