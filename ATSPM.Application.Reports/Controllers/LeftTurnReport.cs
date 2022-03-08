using ATSPM.Application.Reports.Business.LeftTurnGapReport;
using ATSPM.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATSPM.Application.Reports.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeftTurnReport : ControllerBase
    {

        private readonly ILogger<LeftTurnReport> _logger;
        private readonly IPhasePedAggregationRepository _phasePedAggregationRepository;
        private readonly IApproachRepository _approachRepository;
        private readonly IApproachCycleAggregationRepository _approachCycleAggregationRepository;
        private readonly IPhaseTerminationAggregationRepository _phaseTerminationAggregationRepository;
        private readonly ISignalsRepository _signalRepository;
        private readonly IDetectorRepository _detectorRepository;
        private readonly IDetectorEventCountAggregationRepository _detectorEventCountAggregationRepository;
        private readonly IPhaseLeftTurnGapAggregationRepository _phaseLeftTurnGapAggregationRepository;
        private readonly IApproachSplitFailAggregationRepository _approachSplitFailAggregationRepository;

        public LeftTurnReport(ILogger<LeftTurnReport> logger,
            IPhasePedAggregationRepository phasePedAggregationRepository,
            IApproachRepository approachRepository,
            IApproachCycleAggregationRepository approachCycleAggregationRepository,
            IPhaseTerminationAggregationRepository phaseTerminationAggregationRepository,
            ISignalsRepository signalRepository,
            IDetectorRepository detectorRepository,
            IDetectorEventCountAggregationRepository detectorEventCountAggregationRepository,
            IPhaseLeftTurnGapAggregationRepository phaseLeftTurnGapAggregationRepository
,           IApproachSplitFailAggregationRepository approachSplitFailAggregationRepository
            )
        {
            _logger = logger;
            _phasePedAggregationRepository = phasePedAggregationRepository;
            _approachRepository = approachRepository;
            _approachCycleAggregationRepository = approachCycleAggregationRepository;
            _phaseTerminationAggregationRepository = phaseTerminationAggregationRepository;
            _signalRepository = signalRepository;
            _detectorRepository = detectorRepository;
            _detectorEventCountAggregationRepository = detectorEventCountAggregationRepository;
            _phaseLeftTurnGapAggregationRepository = phaseLeftTurnGapAggregationRepository;
            _approachSplitFailAggregationRepository = approachSplitFailAggregationRepository;
        }

        [HttpPost("/DataCheck")]
        public DataCheckResult Get([FromBody]DataCheckParameters parameters)
            //(string signalId, int approachId, DateTime startDate, DateTime endDate, int volumePerHourThreshold, 
            //double gapOutThreshold, double pedestrianThreshold, int[] daysOfWeek)
        {
            var amStartTime = new TimeSpan(6, 0, 0);
            var amEndTime = new TimeSpan(9, 0, 0);
            var pmStartTime = new TimeSpan(15, 0, 0);
            var pmEndTime = new TimeSpan(18, 0, 0);

            var approach = _approachRepository.GetApproachByApproachID(parameters.ApproachId);
            var dataCheck = new DataCheckResult();
            dataCheck.ApproachDescriptions = approach.Description;
            dataCheck.GapOutOk = false;
            dataCheck.LeftTurnVolumeOk = false;
            dataCheck.PedCycleOk = false;
            dataCheck.InsufficientDetectorEventCount = false;
            dataCheck.InsufficientCycleAggregation = false;
            dataCheck.InsufficientPhaseTermination = false;
            var detectors = new List<Models.Detector>();
            //if(approach.Detectors.Any(d => d.DetectionIDs.Contains(4)))
            var movementTypes = new List<int>() { 3 };
            foreach (var detector in approach.Detectors.Where(d => 
            d.MovementTypeId.HasValue 
            && movementTypes.Contains(d.MovementTypeId.Value)).ToList())
            {
                if (!_detectorEventCountAggregationRepository.DetectorEventCountAggregationExists(detector.Id, parameters.StartDate.Add(amStartTime), parameters.StartDate.Add(amEndTime)) &&
                    !_detectorEventCountAggregationRepository.DetectorEventCountAggregationExists(detector.Id, parameters.StartDate.Add(pmStartTime), parameters.StartDate.Add(pmEndTime)))
                {
                    dataCheck.InsufficientDetectorEventCount = true;
                    break;
                }
            }
            int opposingPhase = LeftTurnReportPreCheck.GetOpposingPhase(approach);
            if (approach.ProtectedPhaseNumber != 0)
            {
                CheckTablesForData(approach.SignalId, approach.ProtectedPhaseNumber, parameters, amStartTime, amEndTime, pmStartTime, pmEndTime, dataCheck, opposingPhase);
            }
            else if(approach.PermissivePhaseNumber.HasValue)
            {
                CheckTablesForData(approach.SignalId, approach.PermissivePhaseNumber.Value, parameters, amStartTime, amEndTime, pmStartTime, pmEndTime, dataCheck, opposingPhase);
            }

            if (dataCheck.InsufficientDetectorEventCount || dataCheck.InsufficientCycleAggregation || dataCheck.InsufficientPhaseTermination)
                return dataCheck;
            LeftTurnReportPreCheck leftTurnReportPreCheck = new LeftTurnReportPreCheck(_phasePedAggregationRepository, _approachRepository,
                _approachCycleAggregationRepository, _signalRepository, _detectorRepository,
                _detectorEventCountAggregationRepository, _phaseTerminationAggregationRepository);

            var flowRate = LeftTurnReportPreCheck.GetAMPMPeakFlowRate(parameters.SignalId, parameters.ApproachId, parameters.StartDate, parameters.EndDate, amStartTime,
            amEndTime, pmStartTime, pmEndTime, parameters.DaysOfWeek, _signalRepository, _approachRepository, _detectorEventCountAggregationRepository);
            dataCheck.LeftTurnVolumeOk = flowRate.First().Value >= parameters.VolumePerHourThreshold
                && flowRate.Last().Value >= parameters.VolumePerHourThreshold;

            var gapOut = leftTurnReportPreCheck.GetAMPMPeakGapOut(parameters.SignalId, parameters.ApproachId, parameters.StartDate, parameters.EndDate, amStartTime,
            amEndTime, pmStartTime, pmEndTime, parameters.DaysOfWeek);
            dataCheck.GapOutOk = gapOut.First().Value <= parameters.GapOutThreshold && gapOut.Last().Value <= parameters.GapOutThreshold;

            var pedestrianPercentage = leftTurnReportPreCheck.GetAMPMPeakPedCyclesPercentages(parameters.SignalId, parameters.ApproachId, parameters.StartDate, parameters.EndDate, amStartTime,
            amEndTime, pmStartTime, pmEndTime, parameters.DaysOfWeek);
            dataCheck.PedCycleOk = pedestrianPercentage.First().Value <= parameters.PedestrianThreshold && pedestrianPercentage.Last().Value <= parameters.PedestrianThreshold;
            
            return dataCheck;
        }

        private void CheckTablesForData(string signalId, int phaseNumber, DataCheckParameters parameters, TimeSpan amStartTime, TimeSpan amEndTime, TimeSpan pmStartTime, 
            TimeSpan pmEndTime, DataCheckResult dataCheck, int opposingPhase)
        {
            if (!_approachCycleAggregationRepository.Exists(signalId, phaseNumber, parameters.StartDate.Add(amStartTime), parameters.StartDate.Add(amEndTime)) &&
                                !_approachCycleAggregationRepository.Exists(signalId, phaseNumber, parameters.StartDate.Add(pmStartTime), parameters.StartDate.Add(pmEndTime)))
            {
                dataCheck.InsufficientCycleAggregation = true;
            }
            if (!_phaseTerminationAggregationRepository.Exists(signalId, phaseNumber,
                parameters.StartDate.Add(amStartTime), parameters.StartDate.Add(amEndTime)) &&
                   !_phaseTerminationAggregationRepository.Exists(signalId, phaseNumber,
                parameters.StartDate.Add(pmStartTime), parameters.StartDate.Add(pmEndTime)))
            {
                dataCheck.InsufficientPhaseTermination = true;
            }
            if (!_phasePedAggregationRepository.Exists(signalId, opposingPhase,
                parameters.StartDate.Add(amStartTime), parameters.StartDate.Add(amEndTime)) &&
                   !_phasePedAggregationRepository.Exists(signalId, opposingPhase,
                parameters.StartDate.Add(pmStartTime), parameters.StartDate.Add(pmEndTime)))
            {
                dataCheck.InsufficientPedAggregations = true;
            }
            if (!_approachSplitFailAggregationRepository.Exists(signalId, phaseNumber,
                parameters.StartDate.Add(amStartTime), parameters.StartDate.Add(amEndTime)) &&
                   !_phasePedAggregationRepository.Exists(signalId, phaseNumber,
                parameters.StartDate.Add(pmStartTime), parameters.StartDate.Add(pmEndTime)))
            {
                dataCheck.InsufficientSplitFailAggregations = true;
            }
            if (!_phaseLeftTurnGapAggregationRepository.Exists(signalId, phaseNumber,
                parameters.StartDate.Add(amStartTime), parameters.StartDate.Add(amEndTime)) &&
                   !_phasePedAggregationRepository.Exists(signalId, phaseNumber,
                parameters.StartDate.Add(pmStartTime), parameters.StartDate.Add(pmEndTime)))
            {
                dataCheck.InsufficientLeftTurnGapAggregations = true;
            }
        }

        [HttpPost("/PeakHours")]
        public PeakHourResult GetPeakHours(PeakHourParameters parameters)
        {
            var amStartTime = new TimeSpan(6, 0, 0);
            var amEndTime = new TimeSpan(9, 0, 0);
            var pmStartTime = new TimeSpan(15, 0, 0);
            var pmEndTime = new TimeSpan(18, 0, 0);
            PeakHourResult result = new PeakHourResult();
            var peakResult = LeftTurnReportPreCheck.GetAMPMPeakFlowRate(parameters.SignalId, parameters.ApproachId, parameters.StartDate, parameters.EndDate, amStartTime,
            amEndTime, pmStartTime, pmEndTime, parameters.DaysOfWeek, _signalRepository, _approachRepository, _detectorEventCountAggregationRepository);
            var amPeak = peakResult.First();
            result.AmStartHour = amPeak.Key.Hours;
            result.AmStartMinute = amPeak.Key.Minutes;
            result.AmEndHour = amPeak.Key.Hours + 1;
            result.AmEndMinute = amPeak.Key.Minutes;

            var pmPeak = peakResult.Last();
            result.PmStartHour = pmPeak.Key.Hours;
            result.PmStartMinute = pmPeak.Key.Minutes;
            result.PmEndHour = pmPeak.Key.Hours + 1;
            result.PmEndMinute = pmPeak.Key.Minutes;

            return result;
        }

            [HttpPost("/GapOut")]
        public GapOutResult GetGapOutAnalysis(ReportParameters parameters)
        {
            var startTime = new TimeSpan(parameters.StartHour, parameters.StartMinute, 0);
            var endTime = new TimeSpan(parameters.EndHour, parameters.EndMinute, 0);
            var gapOutResult = new GapOutResult();
            var gapOutAnalysis = new LeftTurnGapOutAnalysis(_approachRepository, _detectorRepository, _detectorEventCountAggregationRepository,
                _phaseLeftTurnGapAggregationRepository, _signalRepository);
            gapOutResult = gapOutAnalysis.GetPercentOfGapDuration(parameters.SignalId, parameters.ApproachId, parameters.StartDate, parameters.EndDate,
                startTime, endTime, parameters.DaysOfWeek);

           return gapOutResult;
        }

        [HttpPost("/SplitFail")]
        public SplitFailResult GetSplitFailAnalysis(ReportParameters parameters)
        {
            var startTime = new TimeSpan(parameters.StartHour, parameters.StartMinute, 0);
            var endTime = new TimeSpan(parameters.EndHour, parameters.EndMinute, 0);
            var splitFailResult = new SplitFailResult();
            var splitFailAnalysis = new LeftTurnSplitFailAnalysis(_approachRepository,_approachSplitFailAggregationRepository);
            splitFailResult = splitFailAnalysis.GetSplitFailPercent(parameters.ApproachId, parameters.StartDate, parameters.EndDate, startTime, endTime, parameters.DaysOfWeek);

            return splitFailResult;
        }

        [HttpPost("/PedActuation")]
        public PedActuationResult GetPedActuationAnalysis(ReportParameters parameters )
        {
            var startTime = new TimeSpan(parameters.StartHour, parameters.StartMinute, 0);
            var endTime = new TimeSpan(parameters.EndHour, parameters.EndMinute, 0);

            var pedAnalysis = new LeftTurnPedestrianAnalysis(_approachRepository, _detectorRepository, _phasePedAggregationRepository,
                _approachCycleAggregationRepository);
            var pedActuationResult = new PedActuationResult();
            pedActuationResult = pedAnalysis.GetPedestrianPercentage(parameters.SignalId, parameters.ApproachId, parameters.StartDate, parameters.EndDate, startTime, endTime, parameters.DaysOfWeek);

            return pedActuationResult;
        }

        [HttpPost("/Volume")]
        public LeftTurnVolumeValue GetVolumeAnalysis(ReportParameters parameters)
        {
            var startTime = new TimeSpan(parameters.StartHour, parameters.StartMinute, 0);
            var endTime = new TimeSpan(parameters.EndHour, parameters.EndMinute, 0);

            var volumeResult = LeftTurnVolumeAnalysis.GetLeftTurnVolumeStats(parameters.SignalId, parameters.ApproachId, parameters.StartDate, parameters.EndDate,
                startTime, endTime, _signalRepository, _approachRepository, _detectorRepository, _detectorEventCountAggregationRepository, parameters.DaysOfWeek);

            return volumeResult;
        }
    }
}



