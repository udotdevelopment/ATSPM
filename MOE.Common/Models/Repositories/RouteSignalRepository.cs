using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class RouteSignalsRepository : IRouteSignalsRepository
    {
        MOE.Common.Models.SPM db = new SPM();

        public List<Models.RouteSignal> GetAllRoutesDetails()
        {
            List<Models.RouteSignal> routes = (from r in db.RouteSignals
                                                        select r).ToList();
            return routes;
        }

        public List<Models.RouteSignal> GetByRouteID(int routeID)
        {
            List<Models.RouteSignal> routes = (from r in db.RouteSignals
                                                 where r.RouteId == routeID
                                                 select r).ToList();

            if (routes.Count > 0)
            {
                return routes;
            }
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.ApproachRouteDetailsRepository";
                error.Function = "GetByRouteID";
                error.Description = "No Route for ID.  Attempted ID# = " + routeID.ToString();
                error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw (new Exception("There is no ApproachRouteDetail for this ID"));
            }
        }

        public Models.RouteSignal GetByRouteSignalId(int id)
        {
            var routeSignal = db.RouteSignals.Include("PhaseDirections").Where(r => r.Id ==id).FirstOrDefault();
            var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            routeSignal.Signal = signalRepository.GetLatestVersionOfSignalBySignalID(routeSignal.SignalId);
            return routeSignal;
        }

        public void DeleteByRouteID(int routeID)
        {
            List<Models.RouteSignal> routes = (from r in db.RouteSignals
                                                       where r.RouteId == routeID
                                                       select r).ToList();

            try
            {
                db.RouteSignals.RemoveRange(routes);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.ApproachRouteDetailsRepository";
                error.Function = "DeleteByRouteID";
                error.Description = ex.Message;
                error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw;
            }
        }

        public void UpdateByRouteAndApproachID(int routeID, string signalId, int newOrderNumber)
        {
            Models.RouteSignal RouteDetail = (from r in db.RouteSignals
                                                 where r.RouteId == routeID 
                                                 && r.SignalId == signalId
                                                      select r).FirstOrDefault();
            if (RouteDetail != null)
            {
                Models.RouteSignal newRouteDetail = new Models.RouteSignal();
                newRouteDetail.Order = newOrderNumber;

                try
                {
                    db.Entry(RouteDetail).CurrentValues.SetValues(newRouteDetail);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                            MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.ApproachRouteDetailsRepository";
                    error.Function = "UpdateByRouteAndApproachID";
                    error.Description = ex.Message;
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }

            }
        }
        public void Add(Models.RouteSignal newRouteDetail)
        {
            try
            {
                db.RouteSignals.Add(newRouteDetail);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.ApproachRouteDetailsRepository";
                error.Function = "UpdateByRouteAndApproachID";
                error.Description = ex.Message;
                error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw;
            }
        }
    }
}

