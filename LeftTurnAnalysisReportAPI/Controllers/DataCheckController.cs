using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common;
using System.Web.Http.Cors;

namespace LeftTurnAnalysisReportAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataCheckController : Controller
    {
        [HttpGet]
        [EnableCors(origins: "https://localhost:44361/, https://staging.udottraffic.utah.gov, https://udottraffic.utah.gov", headers: "*", methods: "*")]
        public Models.IDataCheckResult Get(string signalId)
        {
            var dataCheck = new Models.DataCheckResult(signalId);
            return dataCheck;
        }
    }
}
        
