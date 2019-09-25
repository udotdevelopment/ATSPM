using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using MOE.Common;
using MOE.Common.Models;

namespace AtspmApi.Controllers
{
    public class DataController : ApiController
    {
        public List<Controller_Event_Log> Events { get; set; }
        [AllowAnonymous]

        [HttpGet]
        [Route("api/data/forall")]
        public IHttpActionResult Get()
        {
            return Ok("Now server time is: " + DateTime.Now.ToString());
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/authenticate")]
        public IHttpActionResult GetForAuthenticate()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Hello " + identity.Name);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/data/authorize")]
        public IHttpActionResult GetForAdmin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
            return Ok("Hello " + identity.Name + " Role: " + string.Join(",", roles.ToList()));
        }


        [Authorize]
        [HttpGet]
        [Route("api/data/controllerEventLogs/{id}")]  //here id is SignalID first
        public MOE.Common.Business.ControllerEventLogs GetAllControllerEventLogsFromSignal(string id)
        {
            //var identity = (ClaimsIdentity)User.Identity;
            DateTime StartDate = new DateTime(2019, 9, 1, 7, 0, 0);
            DateTime EndDate = new DateTime(2019, 9, 1, 7, 5, 0);
            var eventsTable = new MOE.Common.Business.ControllerEventLogs();
            eventsTable.FillforPreempt(id, StartDate, EndDate);
            return  eventsTable;
        }

        public void OrderEventsBytimestamp()
        {
            var tempEvents = Events.OrderBy(x => x.Timestamp).ToList();

            Events.Clear();
            Events.AddRange(tempEvents);
        }

        //[Authorize]
        //[HttpGet]
        //[Route("api/data/controllerEventLogs/{id}")]
        //public IHttpActionResult GetControllerEventLog(int id)
        //{
        //    var announcement = controllerEventLogs.FirstOrDefault((p) => p.MsgId == id);
        //    if (announcement == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(announcement);
        //}
    }
}
