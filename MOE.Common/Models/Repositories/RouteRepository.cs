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
        SPM db = new SPM();

        public List<Route> GetAllRoutes()
        {
            List<Route> routes = (from r in db.Routes
                                                 orderby r.RouteName
                                                 select r).ToList();
            return routes;
        }

        public Route GetRouteByID(int routeID)
        {
            Route route = db.Routes
                .Include(r => r.RouteSignals.Select(s => s.PhaseDirections)).FirstOrDefault(r => r.Id == routeID);
            route.RouteSignals = route.RouteSignals.OrderBy(s => s.Order).ToList();
            if(route != null)
            {
                return route;
            }
            else
            {
                IApplicationEventRepository repository =
                    ApplicationEventRepositoryFactory.Create();
                ApplicationEvent error = new ApplicationEvent();
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
                .Include(r => r.RouteSignals.Select(s => s.PhaseDirections)).FirstOrDefault(r => r.Id == routeId);
            return route;
        }

        public Route GetRouteByName(string routeName)
    {
                    Route route = (from r in db.Routes
                                                  where r.RouteName == routeName
                                                 select r).FirstOrDefault();
            return route;
    }
        public void DeleteByID(int routeID)
        {
            Route route = (from r in db.Routes
                                          where r.Id == routeID
                                          select r).FirstOrDefault();

            db.Routes.Remove(route);
            db.SaveChanges();
        }

        public void Update(Route newRoute)
        {
            Route route = (from r in db.Routes
                                          where r.Id == newRoute.Id
                                          select r).FirstOrDefault();

            if(route != null)
            {
            Route newroute = new Route();

            newroute.Id = route.Id;
            newroute.RouteName = newroute.RouteName;
            try
            {
                db.Entry(route).CurrentValues.SetValues(newroute);
                db.SaveChanges();
            }

                    catch (Exception ex)
            {
                IApplicationEventRepository repository =
                        ApplicationEventRepositoryFactory.Create();
                ApplicationEvent error = new ApplicationEvent();
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
        public void Add(Route newRoute)
    {
        try
        {
            db.Routes.Add(newRoute);
            db.SaveChanges();
        }

        catch (Exception ex)
        {
            IApplicationEventRepository repository =
                    ApplicationEventRepositoryFactory.Create();
            ApplicationEvent error = new ApplicationEvent();
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
