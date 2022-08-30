
using ATSPM.Application.Models;
using MOE.Common.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class RouteRepository : IRouteRepository
    {
        private readonly MOEContext _db;

        public RouteRepository(MOEContext db)
        {
            this._db = db;
        }

        public List<Route> GetAllRoutes()
        {
            var routes = (from r in _db.Routes
                          orderby r.RouteName
                          select r).ToList();
            return routes;
        }

        public Route GetRouteByID(int routeID)
        {

            throw new NotImplementedException();
            //var route = db.Routes
            //    .Include(r => r.RouteSignals.Select(s => s.PhaseDirections)).FirstOrDefault(r => r.Id == routeID);
            //route.RouteSignals = route.RouteSignals.OrderBy(s => s.Order).ToList();
            //var signalRepository = new SignalsRepository();
            //foreach (var routeSignal in route.RouteSignals)
            //{
            //    routeSignal.Signal = signalRepository.GetLatestVersionOfSignalBySignalID(routeSignal.SignalId);
            //}
            //if (route != null)
            //    return route;
            //var repository = new ApplicationEventRepository();
            //var error = new ApplicationEvent();
            //error.ApplicationName = "MOE.Common";
            //error.Class = "Models.Repository.ApproachRouteRepository";
            //error.Function = "GetByRouteID";
            //error.Description = "No Route for ID.  Attempted ID# = " + routeID;
            //error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
            //error.Timestamp = DateTime.Now;
            //repository.Add(error);
            //throw new Exception("There is no Route for this ID");
        }

        public Route GetRouteByIDAndDate(int routeId, DateTime startDate)
        {
            throw new NotImplementedException();
            //var route = db.Routes
            //    //TODO.Include(r => r.RouteSignals.Select(s => s.PhaseDirections)).FirstOrDefault(r => r.Id == routeId);
            //route.RouteSignals = route.RouteSignals.OrderBy(s => s.Order).ToList();
            //var signalRepository = new SignalsRepository();
            //foreach (var routeSignal in route.RouteSignals)
            //{
            //    routeSignal.Signal = signalRepository.GetVersionOfSignalByDate(routeSignal.SignalId, startDate);
            //}
            //if (route != null)
            //    return route;
            //return route;
        }

        public Route GetRouteByName(string routeName)
        {
            var route = (from r in _db.Routes
                         where r.RouteName == routeName
                         select r).FirstOrDefault();
            return route;
        }

        public void DeleteByID(int routeID)
        {
            var route = (from r in _db.Routes
                         where r.Id == routeID
                         select r).FirstOrDefault();

            _db.Routes.Remove(route);
            _db.SaveChanges();
        }

        public void Update(Route newRoute)
        {
            var route = (from r in _db.Routes
                         where r.Id == newRoute.Id
                         select r).FirstOrDefault();

            if (route != null)
            {
                var newroute = new Route();

                newroute.Id = route.Id;
                newroute.RouteName = newroute.RouteName;
                try
                {
                    _db.Entry(route).CurrentValues.SetValues(newroute);
                    _db.SaveChanges();
                }

                catch (Exception ex)
                {

                    var repository = new ApplicationEventRepository(_db);
                    var error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.ApproachRouteRepository";
                    error.Function = "UpdateByID";
                    error.Description = ex.Message;
                    error.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
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
                _db.Routes.Add(newRoute);
                _db.SaveChanges();
            }

            catch (Exception ex)
            {

                var repository = new ApplicationEventRepository(_db);
                var error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.ApproachRouteRepository";
                error.Function = "Add";
                error.Description = ex.Message;
                error.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw;
            }
        }
    }
}