using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common;

namespace LeftTurnAnalysisReportAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataCheckController : Controller
    {
        [HttpGet]
        public Models.IDataCheckResult Get(bool checkLeftTurnVolumes, bool checkGapOut, bool checkPedCycle)
        {
            var dataCheck = new Models.DataCheckResult(checkLeftTurnVolumes, checkGapOut, checkPedCycle);
            return dataCheck;
        }
    }
}
        
