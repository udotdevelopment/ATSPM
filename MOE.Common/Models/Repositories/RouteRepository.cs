using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using NuGet;

namespace MOE.Common.Models.Repositories 
{
    public class RouteRepository : IRouteRepository
    {
        MOE.Common.Models.SPM db = new SPM();

        public List<Models.Route> GetAllRoutes()
        {
            List<Models.Route> routes = (from r in db.Routes
                                                 orderby r.RouteName
                                                 select r).ToList();
            return routes;
        }

        public Models.Route GetRouteByID(int routeID)
        {
            Models.Route route = db.Routes
                .Include(r => r.RouteSignals.Select(s => s.PhaseDirections))
                .Where(r => r.Id == routeID).FirstOrDefault();
            if(route != null)
            {
                var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
                foreach (var routeSignal in route.RouteSignals)
                {
                    var signal = signalRepository.GetLatestVersionOfSignalBySignalID(routeSignal.SignalId);
                    if (signal != null)
                    {
                        routeSignal.Signal = signal;
                    }
                }
                return route;
            }
            else
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.ApproachRouteRepository";
                error.Function = "GetByRouteID";
                error.Description = "No Route for ID.  Attempted ID# = " + routeID.ToString();
                error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw (new Exception("There is no Route for this ID"));
            }
        }

        public Route GetRouteByIDAndDate(int routeId, DateTime startDate)
        {
            Route route = db.Routes
                .Include(r => r.RouteSignals.Select(s => s.PhaseDirections))
                .Where(r => r.Id == routeId).FirstOrDefault();
            if (route != null)
            {
                var signalRepository = SignalsRepositoryFactory.Create();
                foreach (var routeSignal in route.RouteSignals)
                {
                    var signal = signalRepository.GetVersionOfSignalByDate(routeSignal.SignalId, startDate);
                    if (signal != null)
                    {
                        routeSignal.Signal = signal;
                    }
                }
            }
            return route;
        }

        public Models.Route GetRouteByName(string routeName)
    {
                    Models.Route route = (from r in db.Routes
                                                  where r.RouteName == routeName
                                                 select r).FirstOrDefault();
            return route;
    }
        public void DeleteByID(int routeID)
        {
            Models.Route route = (from r in db.Routes
                                          where r.Id == routeID
                                          select r).FirstOrDefault();

            db.Routes.Remove(route);
            db.SaveChanges();
        }

        public void Update(Route newRoute)
        {
            Models.Route route = (from r in db.Routes
                                          where r.Id == newRoute.Id
                                          select r).FirstOrDefault();

            if(route != null)
            {
            Models.Route newroute = new Models.Route();

            newroute.Id = route.Id;
            newroute.RouteName = newroute.RouteName;
            try
            {
                db.Entry(route).CurrentValues.SetValues(newroute);
                db.SaveChanges();
            }

                    catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.ApproachRouteRepository";
                error.Function = "UpdateByID";
                error.Description = ex.Message;
                error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw;
            }
            
            }
        }
        public void Add(Models.Route newRoute)
    {
        try
        {
            db.Routes.Add(newRoute);
            db.SaveChanges();
        }

        catch (Exception ex)
        {
            MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
            MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
            error.ApplicationName = "MOE.Common";
            error.Class = "Models.Repository.ApproachRouteRepository";
            error.Function = "Add";
            error.Description = ex.Message;
            error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
            error.Timestamp = DateTime.Now;
            repository.Add(error);
            throw;
        }
    }

        
    }
}
