using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories 
{
    public class ApproachRouteRepository : IApproachRouteRepository
    {
        MOE.Common.Models.SPM db = new SPM();

        public List<Models.ApproachRoute> GetAllRoutes()
        {
            List<Models.ApproachRoute> routes = (from r in db.ApproachRoutes
                                                 orderby r.RouteName
                                                 select r).ToList();
            return routes;
        }

        public Models.ApproachRoute GetRouteByID(int routeID)
        {
            Models.ApproachRoute route = (from r in db.ApproachRoutes
                                          where r.ApproachRouteId == routeID
                                                 select r).FirstOrDefault();
            if(route != null)
            {
            return route;
            }
            else
            {

                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.ApproachRouteRepository";
                    error.Function = "GetByRouteID";
                    error.Description = "No ApproachRoute for ID.  Attempted ID# = " + routeID.ToString();
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw (new Exception("There is no ApproachRoute for this ID"));
                }
            }
        }

        public Models.ApproachRoute GetRouteByName(string routeName)
    {
                    Models.ApproachRoute route = (from r in db.ApproachRoutes
                                                  where r.RouteName == routeName
                                                 select r).FirstOrDefault();
            return route;
    }
        public void DeleteByID(int routeID)
        {
            Models.ApproachRoute route = (from r in db.ApproachRoutes
                                          where r.ApproachRouteId == routeID
                                          select r).FirstOrDefault();

            db.ApproachRoutes.Remove(route);
            db.SaveChanges();
        }

        public void UpdateByID(int routeID, string newDescription)
        {
            Models.ApproachRoute route = (from r in db.ApproachRoutes
                                          where r.ApproachRouteId == routeID
                                          select r).FirstOrDefault();

            if(route != null)
            {
            Models.ApproachRoute newroute = new Models.ApproachRoute();

            newroute.ApproachRouteId = route.ApproachRouteId;
            newroute.RouteName = newDescription;
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
        public void Add(Models.ApproachRoute newRoute)
    {
        try
        {
            db.ApproachRoutes.Add(newRoute);
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
