using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using AtspmApi.Models;

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
        [Route("api/data/ControllerType")]  
        public List<ControllerType>  ControllerType()
        {
            var repo = Repositories.ControllerTypeRepositoryFactory.Create();
            var types = repo.GetControllerTypes();
            return types;
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/DirectionType")]  
        public List<DirectionType> DirectionType()
        {
            var repo = Repositories.DirectionTypeRepositoryFactory.Create();
            var types = repo.GetAllDirections();
            return types;
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/LaneType")]  
        public List<LaneType> LaneType()
        {
            var repo = Repositories.LaneTypeRepositoryFactory.Create();
            var types = repo.GetAllLaneTypes();
            return types;
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/MovementType")]  
        public List<MovementType> MovementType()
        {
            var repo = Repositories.MovementTypeRepositoryFactory.Create();
            var types = repo.GetAllMovementTypes();
            return types;
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/SignalConfig/{Id}")]  
        public Signal SignalConfig(string id)
        {
            var signalRepository = Repositories.SignalsRepositoryFactory.Create();
            var signal = signalRepository.GetSignalWithApproachesBySignalID(id);
            //var eventsTable = new MOE.Common.Business.ControllerEventLogs();
            //eventsTable.FillforPreempt(id, StartDate, EndDate);
            return signal;
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/ApproachConfig/{Id}")]  
        public Approach ApproachConfig(int id)
        {
            var approachRepository = Repositories.ApproachRepositoryFactory.Create();
            var approachThis = approachRepository.GetApproachByApproachID(id);
            return approachThis;
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/DetectorConfig/{Id}")]  
        public Detector DetectorConfig(string id)
        {
            var detectorRepository = Repositories.DetectorRepositoryFactory.Create();
            var detectorThis = detectorRepository.GetDetectorByDetectorID(id);
            return detectorThis;
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/controllerEventLogsFromSignal/{SignalId=Id}/{StartTime=StartTime}/{EndTime=EndTime}")]  //here id is SignalID first
        public List<Controller_Event_Log> controllerEventLogsFromSignal(string id, DateTime StartTime, DateTime EndTime)
        {
            //var identity = (ClaimsIdentity)User.Identity;
            DateTime StartDate = StartTime; 
            //StartDate = new DateTime(2019, 10, 1, 0, 0, 0);
            DateTime EndDate = EndTime;
            //EndDate = new DateTime(2019, 10, 1, 0, 5, 0);

            var controllerEventLogRepository = Repositories.ControllerEventLogRepositoryFactory.Create();
            var singleSignalCount = controllerEventLogRepository.GetRecordCount(id, StartDate, EndDate);
            int NumberRecordsThreshold = Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
            if (singleSignalCount > NumberRecordsThreshold)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Request returns too many records. Please shorten timespan.")
                });
            }
            else
            {
                var events = controllerEventLogRepository.GetSignalEventsBetweenDates(id, StartDate, EndDate);
                //var eventsTable = new MOE.Common.Business.ControllerEventLogs();
                //eventsTable.FillforPreempt(id, StartDate, EndDate);
                return events;
            }
        }

        //[Authorize]
        //[HttpGet]
        //[Route("api/data/RouteSignals/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        //public List<Signal> AllSignalsFromRoutes([FromUri] int[] RouteIds)
        //{
        //    var routeRepo = Repositories.RouteSignalsRepositoryFactory.Create();
        //    var signals = new List<Signal>();
        //    foreach (int routeIdInt in RouteIds)
        //    {
        //        var routeSignals = routeRepo.GetByRouteID(routeIdInt);
        //        foreach (var eachRouteSignal in routeSignals)
        //            signals.Add(eachRouteSignal.Signal);
        //    }

        //    if (signals.Count > 0)
        //    {
        //        return signals;
        //    }
        //    else
        //    {
        //        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("This RouteId contains no valid signals")
        //        });
        //    }
        //}

        //[HttpGet]
        //[Route("api/data/RouteSignal/{RouteId=Id}")]
        ////here id is RouteId; each route contains multiple SignalIds
        //public List<RouteSignal> RouteSignal(int RouteId)
        //{
        //    var routeRepo = Repositories.RouteSignalsRepositoryFactory.Create();

        //    var routeSignals = routeRepo.GetByRouteID(RouteId);
        //    return routeSignals;

        //}


        [Authorize]
        [HttpGet]
        [Route("api/data/controllerEventLogs/{StartTime=StartTime}/{EndTime=EndTime}/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        //POSTMAN:
        //http://localhost:50688/api/data/controllerEventLogs?StartTime=2019-10-01T00:00:00&EndTime=2019-10-01T00:00:10&RouteIds=1&RouteIds=2
        //http://staging.udottraffic.utah.gov/AtspmApi/api/data/controllerEventLogs?StartTime=2019-10-01T00:00:00&EndTime=2019-10-01T00:00:10&RouteIds=1&RouteIds=2 
        //Header: Key Authorization, Value Bearer _token_
        //Params: Key id, Value 1219; Key StartTime, Value 2019-10-01T00:00:00; Key EndTime, Value 2019-10-01T00:00:10;
        public List<Controller_Event_Log> AllControllerEventLogsFromRoute(DateTime StartTime, DateTime EndTime, [FromUri] int[] RouteIds)
        {
            //var identity = (ClaimsIdentity)User.Identity;
            DateTime StartDate = StartTime;
            DateTime EndDate = EndTime;

            var routeRepo = Repositories.RouteSignalsRepositoryFactory.Create();
            var signalIDs = new List<string>();
            foreach (int routeIdInt in RouteIds)
            {
                var routeSignals = routeRepo.GetByRouteID(routeIdInt);
                foreach (var eachRouteSignal in routeSignals)
                    signalIDs.Add(eachRouteSignal.SignalId);
            }

            if (signalIDs.Count > 0)
            {
                var controllerEventLogRepository = Repositories.ControllerEventLogRepositoryFactory.Create();
                int totalCount = 0;
                foreach (var singleSignal in signalIDs)
                {
                    totalCount += controllerEventLogRepository.GetRecordCount(singleSignal, StartDate, EndDate);
                }
                int NumberRecordsThreshold = Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
                if (totalCount > NumberRecordsThreshold)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Request returns too many records. Please shorten timespan.")
                    });
                }
                else
                {
                    var totalEvents = new List<Controller_Event_Log>();
                    foreach (var singleSignal in signalIDs)
                    {
                        var events = controllerEventLogRepository.GetSignalEventsBetweenDates(singleSignal, StartDate, EndDate);
                        totalEvents.AddRange(events);
                    }
                    return totalEvents;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("This RouteId contains no valid signals")
                });
            }
        }


        [Authorize]
        [HttpGet]
        [Route("api/data/speedEvents/{StartTime=StartTime}/{EndTime=EndTime}/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        public List<Speed_Events> AllSpeedEventsFromRoute(DateTime StartTime, DateTime EndTime, [FromUri] int[] RouteIds)
        {
            //DateTime StartDate = StartTime;
            //DateTime EndDate = EndTime;

            var routeRepo = Repositories.RouteSignalsRepositoryFactory.Create();
            var signalIDs = new List<string>();
            foreach (int routeIdInt in RouteIds)
            {
                var routeSignals = routeRepo.GetByRouteID(routeIdInt);
                foreach (var eachRouteSignal in routeSignals)
                    signalIDs.Add(eachRouteSignal.SignalId);
            }

            var speedEventsTotal = new List<Speed_Events>();
            if (signalIDs.Count > 0)
            {

                //var detectorRepo = Repositories.DetectorRepositoryFactory.Create();
                //var DetectorsForSignal = detectorRepo.GetDetectorsBySignalID(id);
                var signalRepo = Repositories.SignalsRepositoryFactory.Create();
                foreach (var singleSignal in signalIDs)
                {
                    var thisSignal = signalRepo.GetSignalWithApproachesBySignalID(singleSignal);
                    var speedEventRepo = Repositories.SpeedEventRepositoryFactory.Create();
                    foreach (var singleApproach in thisSignal.Approaches)
                    {
                        var speedEvents = speedEventRepo.GetSpeedEventsBySignal(StartTime, EndTime, singleApproach);
                        speedEventsTotal.AddRange(speedEvents);
                    }
                }

                int NumberRecordsThreshold =
                    Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
                if (speedEventsTotal.Count > NumberRecordsThreshold)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Request returns too many records. Please shorten timespan.")
                    });
                }
                else
                {
                    return speedEventsTotal;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("This RouteId contains no valid signals")
                });
            }
        }


        [Authorize]
        [HttpGet]
        [Route("api/data/ApproachCycleAggregate/{StartTime=StartTime}/{EndTime=EndTime}/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        public List<ApproachCycleAggregation> ApproachCycleAggFromRoute(DateTime StartTime, DateTime EndTime, [FromUri] int[] RouteIds)
        {
            var signalList = GetSignalsFromRoutes(RouteIds);
            if (signalList.Count > 0)
            {
                var approachCycleAggTotal = new List<ApproachCycleAggregation>();
                var approachCycleAggRepo = Repositories.ApproachCycleAggregationRepositoryFactory.Create();
                int totalCount = 0;
                foreach (var singleSignal in signalList)
                {
                    foreach (var singleApproach in singleSignal.Approaches)
                    {
                        var approachCycleCount = approachCycleAggRepo.GetApproachCycleCountAggByApproachCount(singleApproach.ApproachID, StartTime, EndTime);
                        totalCount += approachCycleCount;
                    }
                }

                int NumberRecordsThreshold =
                    Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
                if (totalCount > NumberRecordsThreshold)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Request returns too many records. Please shorten timespan.")
                    });
                }
                else
                {
                    foreach (var signal in signalList)
                    {
                        foreach (var singleApproach in signal.Approaches)
                        {
                            var approachCycleAgg = approachCycleAggRepo.ApproachCycleAggByApproach(singleApproach.ApproachID, StartTime, EndTime);
                            approachCycleAggTotal.AddRange(approachCycleAgg);
                        }
                    }

                    return approachCycleAggTotal;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("This RouteId contains no valid signals")
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/ApproachEventCountAggregate/{StartTime=StartTime}/{EndTime=EndTime}/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        public List<ApproachEventCountAggregation> ApproachEventCountAggFromRoute(DateTime StartTime, DateTime EndTime, [FromUri] int[] RouteIds)
        {
            var signalList = GetSignalsFromRoutes(RouteIds);
            if (signalList.Count > 0)
            {
                var ApproachEventCountAggTotal = new List<ApproachEventCountAggregation>();
                var ApproachEventCountAggRepo = Repositories.ApproachEventCountAggregationRepositoryFactory.Create();
                int totalCount = 0;
                foreach (var singleSignal in signalList)
                {
                    foreach (var singleApproach in singleSignal.Approaches)
                    {
                        var ApproachEventCountCount = ApproachEventCountAggRepo.GetPhaseEventCountAggByApproach(singleApproach.ApproachID, StartTime, EndTime);
                        totalCount += ApproachEventCountCount;
                    }
                }

                int NumberRecordsThreshold =
                    Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
                if (totalCount > NumberRecordsThreshold)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Request returns too many records. Please shorten timespan.")
                    });
                }
                else
                {
                    foreach (var signal in signalList)
                    {
                        foreach (var singleApproach in signal.Approaches)
                        {
                            var ApproachEventCountAgg = ApproachEventCountAggRepo.GetApproachEventCountAggregationByApproach(singleApproach.ApproachID, StartTime, EndTime);
                            ApproachEventCountAggTotal.AddRange(ApproachEventCountAgg);
                        }
                    }

                    return ApproachEventCountAggTotal;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("This RouteId contains no valid signals")
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/ApproachPcdAggregate/{StartTime=StartTime}/{EndTime=EndTime}/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        public List<ApproachPcdAggregation> ApproachPcdAggFromRoute(DateTime StartTime, DateTime EndTime, [FromUri] int[] RouteIds)
        {
            var signalList = GetSignalsFromRoutes(RouteIds);
            if (signalList.Count > 0)
            {
                var ApproachPcdAggTotal = new List<ApproachPcdAggregation>();
                var ApproachPcdAggRepo = Repositories.ApproachPcdAggregationRepositoryFactory.Create();
                int totalCount = 0;
                foreach (var singleSignal in signalList)
                {
                    foreach (var singleApproach in singleSignal.Approaches)
                    {
                        var ApproachPcdCount = ApproachPcdAggRepo.GetApproachPcdAggCountByApproach(singleApproach.ApproachID, StartTime, EndTime);
                        totalCount += ApproachPcdCount;
                    }
                }

                int NumberRecordsThreshold =
                    Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
                if (totalCount > NumberRecordsThreshold)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Request returns too many records. Please shorten timespan.")
                    });
                }
                else
                {
                    foreach (var signal in signalList)
                    {
                        foreach (var singleApproach in signal.Approaches)
                        {
                            var ApproachPcdAgg = ApproachPcdAggRepo.GetApproachPcdsAggByApproach(singleApproach.ApproachID, StartTime, EndTime);
                            ApproachPcdAggTotal.AddRange(ApproachPcdAgg);
                        }
                    }

                    return ApproachPcdAggTotal;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("This RouteId contains no valid signals")
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/ApproachSpeedAggregate/{StartTime=StartTime}/{EndTime=EndTime}/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        public List<ApproachSpeedAggregation> ApproachSpeedAggFromRoute(DateTime StartTime, DateTime EndTime, [FromUri] int[] RouteIds)
        {
            var signalList = GetSignalsFromRoutes(RouteIds);
            if (signalList.Count > 0)
            {
                var ApproachSpeedAggTotal = new List<ApproachSpeedAggregation>();
                var ApproachSpeedAggRepo = Repositories.ApproachSpeedAggregationRepositoryFactory.Create();
                int totalCount = 0;
                foreach (var singleSignal in signalList)
                {
                    foreach (var singleApproach in singleSignal.Approaches)
                    {
                        var ApproachSpeedCount = ApproachSpeedAggRepo.GetApproachSpeedCountAggregationByApproachIdAndDateRange(singleApproach.ApproachID, StartTime, EndTime);
                        totalCount += ApproachSpeedCount;
                    }
                }

                int NumberRecordsThreshold =
                    Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
                if (totalCount > NumberRecordsThreshold)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Request returns too many records. Please shorten timespan.")
                    });
                }
                else
                {
                    foreach (var signal in signalList)
                    {
                        foreach (var singleApproach in signal.Approaches)
                        {
                            var ApproachSpeedAgg = ApproachSpeedAggRepo.GetSpeedsByApproachIDandDateRange(singleApproach.ApproachID, StartTime, EndTime);
                            ApproachSpeedAggTotal.AddRange(ApproachSpeedAgg);
                        }
                    }

                    return ApproachSpeedAggTotal;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("This RouteId contains no valid signals")
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/ApproachSplitFailAggregate/{StartTime=StartTime}/{EndTime=EndTime}/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        public List<ApproachSplitFailAggregation> ApproachSplitFailAggFromRoute(DateTime StartTime, DateTime EndTime, [FromUri] int[] RouteIds)
        {
            var signalList = GetSignalsFromRoutes(RouteIds);
            if (signalList.Count > 0)
            {
                var ApproachSplitFailAggTotal = new List<ApproachSplitFailAggregation>();
                var ApproachSplitFailAggRepo = Repositories.ApproachSplitFailAggregationRepositoryFactory.Create();
                int totalCount = 0;
                foreach (var singleSignal in signalList)
                {
                    foreach (var singleApproach in singleSignal.Approaches)
                    {
                        var ApproachSplitFailCount = ApproachSplitFailAggRepo.GetApproachSplitFailCountAggByApproach(singleApproach.ApproachID, StartTime, EndTime);
                        totalCount += ApproachSplitFailCount;
                    }
                }

                int NumberRecordsThreshold =
                    Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
                if (totalCount > NumberRecordsThreshold)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Request returns too many records. Please shorten timespan.")
                    });
                }
                else
                {
                    foreach (var signal in signalList)
                    {
                        foreach (var singleApproach in signal.Approaches)
                        {
                            var ApproachSplitFailAgg = ApproachSplitFailAggRepo.GetApproachSplitFailsAggregationByApproachId(singleApproach.ApproachID, StartTime, EndTime);
                            ApproachSplitFailAggTotal.AddRange(ApproachSplitFailAgg);
                        }
                    }

                    return ApproachSplitFailAggTotal;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("This RouteId contains no valid signals")
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/ApproachYellowRedActivationAggregate/{StartTime=StartTime}/{EndTime=EndTime}/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        public List<ApproachYellowRedActivationAggregation> ApproachYellowRedActivationsAggFromRoute(DateTime StartTime, DateTime EndTime, [FromUri] int[] RouteIds)
        {
            var signalList = GetSignalsFromRoutes(RouteIds);
            if (signalList.Count > 0)
            {
                var ApproachYellowRedActivationsAggTotal = new List<ApproachYellowRedActivationAggregation>();
                var ApproachYellowRedActivationsAggRepo = Repositories.ApproachYellowRedActivationsAggregationRepositoryFactory.Create();
                int totalCount = 0;
                foreach (var singleSignal in signalList)
                {
                    foreach (var singleApproach in singleSignal.Approaches)
                    {
                        var ApproachYellowRedActivationsCount = ApproachYellowRedActivationsAggRepo.GetApproachYellowRedActivationsCountAggByApproach(singleApproach.ApproachID, StartTime, EndTime);
                        totalCount += ApproachYellowRedActivationsCount;
                    }
                }

                int NumberRecordsThreshold =
                    Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
                if (totalCount > NumberRecordsThreshold)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Request returns too many records. Please shorten timespan.")
                    });
                }
                else
                {
                    foreach (var signal in signalList)
                    {
                        foreach (var singleApproach in signal.Approaches)
                        {
                            var ApproachYellowRedActivationsAgg = ApproachYellowRedActivationsAggRepo.GetApproachYellowRedActivationssAggregationByApproach(singleApproach.ApproachID, StartTime, EndTime);
                            ApproachYellowRedActivationsAggTotal.AddRange(ApproachYellowRedActivationsAgg);
                        }
                    }

                    return ApproachYellowRedActivationsAggTotal;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("This RouteId contains no valid signals")
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/PhasePedAggregate/{StartTime=StartTime}/{EndTime=EndTime}/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        public List<PhasePedAggregation> PhasePedAggFromRoute(DateTime StartTime, DateTime EndTime, [FromUri] int[] RouteIds)
        {
            var signalList = GetSignalsFromRoutes(RouteIds);
            if (signalList.Count > 0)
            {
                var PhasePedAggTotal = new List<PhasePedAggregation>();
                var PhasePedAggRepo = Repositories.PhasePedAggregationRepositoryFactory.Create();
                int totalCount = 0;
                foreach (var singleSignal in signalList)
                {
                        var PhasePedCount = PhasePedAggRepo.GetPhasePedsAggCountBySignal(singleSignal.SignalID, StartTime, EndTime);
                        totalCount += PhasePedCount;
                }

                int NumberRecordsThreshold =
                    Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
                if (totalCount > NumberRecordsThreshold)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Request returns too many records. Please shorten timespan.")
                    });
                }
                else
                {
                    foreach (var signal in signalList)
                    {
                             var PhasePedAgg = PhasePedAggRepo.GetPhasePedsAggBySignal(signal.SignalID, StartTime, EndTime);
                            PhasePedAggTotal.AddRange(PhasePedAgg);
                    }

                    return PhasePedAggTotal;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("This RouteId contains no valid signals")
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/PhaseTerminationAggregate/{StartTime=StartTime}/{EndTime=EndTime}/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        public List<PhaseTerminationAggregation> PhaseTerminationAggFromRoute(DateTime StartTime, DateTime EndTime, [FromUri] int[] RouteIds)
        {
            var signalList = GetSignalsFromRoutes(RouteIds);
            if (signalList.Count > 0)
            {
                var PhaseTerminationAggTotal = new List<PhaseTerminationAggregation>();
                var PhaseTerminationAggRepo = Repositories.PhaseTerminationAggregationRepositoryFactory.Create();
                int totalCount = 0;
                foreach (var singleSignal in signalList)
                {
                    var PhaseTerminationCount = PhaseTerminationAggRepo.GetPhaseTermAggCountBySignal(singleSignal.SignalID, StartTime, EndTime);
                    totalCount += PhaseTerminationCount;
                }

                int NumberRecordsThreshold =
                    Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
                if (totalCount > NumberRecordsThreshold)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Request returns too many records. Please shorten timespan.")
                    });
                }
                else
                {
                    foreach (var signal in signalList)
                    {
                        var PhaseTerminationAgg = PhaseTerminationAggRepo.GetPhaseTerminationsAggregationBySignal(signal.SignalID, StartTime, EndTime);
                        PhaseTerminationAggTotal.AddRange(PhaseTerminationAgg);
                    }

                    return PhaseTerminationAggTotal;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("This RouteId contains no valid signals")
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/PreemptionAggregate/{StartTime=StartTime}/{EndTime=EndTime}/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        public List<PreemptionAggregation> PreemptionAggFromRoute(DateTime StartTime, DateTime EndTime, [FromUri] int[] RouteIds)
        {
            var signalList = GetSignalsFromRoutes(RouteIds);
            if (signalList.Count > 0)
            {
                var PreemptionAggTotal = new List<PreemptionAggregation>();
                var PreemptionAggRepo = Repositories.PreemptAggregationDatasRepositoryFactory.Create();
                int totalCount = 0;
                foreach (var singleSignal in signalList)
                {
                    var PreemptionCount = PreemptionAggRepo.GetPreemptAggCountBySignal(singleSignal.SignalID, StartTime, EndTime);
                    totalCount += PreemptionCount;
                }

                int NumberRecordsThreshold =
                    Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
                if (totalCount > NumberRecordsThreshold)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Request returns too many records. Please shorten timespan.")
                    });
                }
                else
                {
                    foreach (var signal in signalList)
                    {
                        var PreemptionAgg = PreemptionAggRepo.GetPreemptionsBySignalIdAndDateRange(signal.SignalID, StartTime, EndTime);
                        PreemptionAggTotal.AddRange(PreemptionAgg);
                    }

                    return PreemptionAggTotal;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("This RouteId contains no valid signals")
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/PriorityAggregate/{StartTime=StartTime}/{EndTime=EndTime}/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        public List<PriorityAggregation> PriorityAggFromRoute(DateTime StartTime, DateTime EndTime, [FromUri] int[] RouteIds)
        {
            var signalList = GetSignalsFromRoutes(RouteIds);
            if (signalList.Count > 0)
            {
                var PriorityAggTotal = new List<PriorityAggregation>();
                var PriorityAggRepo = Repositories.PriorityAggregationDatasRepositoryFactory.Create();
                int totalCount = 0;
                foreach (var singleSignal in signalList)
                {
                    var PriorityCount = PriorityAggRepo.GetPriorityAggCountBySignal(singleSignal.SignalID, StartTime, EndTime);
                    totalCount += PriorityCount;
                }

                int NumberRecordsThreshold =
                    Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
                if (totalCount > NumberRecordsThreshold)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Request returns too many records. Please shorten timespan.")
                    });
                }
                else
                {
                    foreach (var signal in signalList)
                    {
                        var PriorityAgg = PriorityAggRepo.GetPriorityBySignalIdAndDateRange(signal.SignalID, StartTime, EndTime);
                        PriorityAggTotal.AddRange(PriorityAgg);
                    }

                    return PriorityAggTotal;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("This RouteId contains no valid signals")
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/SignalEventCountAggregate/{StartTime=StartTime}/{EndTime=EndTime}/{RouteId=Id}")]  //here id is RouteId; each route contains multiple SignalIds
        public List<SignalEventCountAggregation> SignalEventCountAggFromRoute(DateTime StartTime, DateTime EndTime, [FromUri] int[] RouteIds)
        {
            var signalList = GetSignalsFromRoutes(RouteIds);
            if (signalList.Count > 0)
            {
                var SignalEventCountAggTotal = new List<SignalEventCountAggregation>();
                var SignalEventCountAggRepo = Repositories.SignalEventCountAggregationRepositoryFactory.Create();
                int totalCount = 0;
                foreach (var singleSignal in signalList)
                {
                    var SignalEventCountCount = SignalEventCountAggRepo.GetSignalEventCountAggCountBySignal(singleSignal.SignalID, StartTime, EndTime);
                    totalCount += SignalEventCountCount;
                }

                int NumberRecordsThreshold =
                    Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecordsThreshold"]);
                if (totalCount > NumberRecordsThreshold)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Request returns too many records. Please shorten timespan.")
                    });
                }
                else
                {
                    foreach (var signal in signalList)
                    {
                        var SignalEventCountAgg = SignalEventCountAggRepo.GetSignalEventCountAggregationBySignal(signal.SignalID, StartTime, EndTime);
                        SignalEventCountAggTotal.AddRange(SignalEventCountAgg);
                    }

                    return SignalEventCountAggTotal;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("This RouteId contains no valid signals")
                });
            }
        }

        public void OrderEventsBytimestamp()
        {
            var tempEvents = Events.OrderBy(x => x.Timestamp).ToList();

            Events.Clear();
            Events.AddRange(tempEvents);
        }

        List<Signal> GetSignalsFromRoutes(int[] RouteIds)
        {
            var routeRepo = Repositories.RouteSignalsRepositoryFactory.Create();
            var signalRepo = Repositories.SignalsRepositoryFactory.Create();
            var signalList = new List<Signal>();
            foreach (int routeIdInt in RouteIds)
            {
                var routeSignals = routeRepo.GetByRouteID(routeIdInt);
                foreach (var eachRouteSignal in routeSignals)
                {
                    var thisSignal = signalRepo.GetSignalWithApproachesBySignalID(eachRouteSignal.SignalId);
                    signalList.Add(thisSignal);
                }
            }

            return signalList;
        }

    }
}
