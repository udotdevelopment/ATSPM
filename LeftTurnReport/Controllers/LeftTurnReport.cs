using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common.Business.LeftTurnGapReport;

namespace LeftTurnReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeftTurnReport : ControllerBase
    {
        private readonly ILogger<LeftTurnReport> _logger;

        public LeftTurnReport(ILogger<LeftTurnReport> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Models.DataCheckResult Get(string signalId, int directionTypeId, DateTime startDate, DateTime endDate, TimeSpan amStartTime,
            TimeSpan amEndTime, TimeSpan pmStartTime, TimeSpan pmEndTime, int volumePerHourThreshold, double gapOutThreshold,
            double pedestrianThreshold)
        {
            var flowRate = LeftTurnReportPreCheck.GetAMPMPeakFlowRate(signalId, directionTypeId, startDate, endDate, amStartTime,
            amEndTime, pmStartTime, pmEndTime);
            var dataCheck = new Models.DataCheckResult();
            dataCheck.LeftTurnVolumeOk = flowRate.First().Value >= volumePerHourThreshold
                && flowRate.Last().Value >= volumePerHourThreshold;

            var gapOut = LeftTurnReportPreCheck.GetAMPMPeakGapOut(signalId, directionTypeId, startDate, endDate, amStartTime,
            amEndTime, pmStartTime, pmEndTime);
            dataCheck.GapOutOk = gapOut.First().Value <= gapOutThreshold && gapOut.Last().Value <= gapOutThreshold;

            var pedestrianPercentage = LeftTurnReportPreCheck.GetAMPMPeakPedCyclesPercentages(signalId, directionTypeId, startDate, endDate, amStartTime,
            amEndTime, pmStartTime, pmEndTime);
            dataCheck.PedCycleOk = pedestrianPercentage.First().Value <= pedestrianThreshold && pedestrianPercentage.Last().Value <= pedestrianThreshold;

            return dataCheck;
        }
    }
}
