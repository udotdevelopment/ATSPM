using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common;
using System.Web.Http.Cors;
using MOE.Common.Business.LeftTurnGapReport;

namespace LeftTurnAnalysisReportAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataCheckController : Controller
    {
        [HttpGet]
        [EnableCors(origins: "https://localhost:44361/, https://staging.udottraffic.utah.gov, https://udottraffic.utah.gov", headers: "*", methods: "*")]
        public Models.DataCheckResult Get(string signalId, int directionTypeId, DateTime startDate, DateTime endDate, TimeSpan amStartTime,
            TimeSpan amEndTime, TimeSpan pmStartTime, TimeSpan pmEndTime,int volumePerHourThreshold, double gapOutThreshold,
            double pedestrianThreshold)
        {
            var flowRate =LeftTurnReportPreCheck.GetAMPMPeakFlowRate(signalId, directionTypeId, startDate, endDate, amStartTime,
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
        
