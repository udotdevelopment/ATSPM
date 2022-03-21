using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class RouteSignalsRepository : IRouteSignalsRepository
    {
        private readonly SPM db = new SPM();

        public List<RouteSignal> GetAllRoutesDetails()
        {
            var routes = (from r in db.RouteSignals
                select r).ToList();
            return routes;
        }

        public void MoveRouteSignalUp(int routeId, int routeSignalId)
        {
            var route = db.Routes.Find(routeId);
            var signal = route.RouteSignals.FirstOrDefault(r => r.Id == routeSignalId);
            var order = signal.Order;
            var swapSignal = route.RouteSignals.FirstOrDefault(r => r.Order == order - 1);
            if (swapSignal != null)
            {
                signal.Order--;
                swapSignal.Order++;
                db.SaveChanges();
            }
        }

        public void MoveRouteSignalDown(int routeId, int routeSignalId)
        {
            var route = db.Routes.Find(routeId);
            var signal = route.RouteSignals.FirstOrDefault(r => r.Id == routeSignalId);
            var order = signal.Order;
            var swapSignal = route.RouteSignals.FirstOrDefault(r => r.Order == order + 1);
            if (swapSignal != null)
            {
                signal.Order++;
                swapSignal.Order--;
                db.SaveChanges();
            }
        }

        public List<RouteSignal> GetByRouteID(int routeID)
        {
            var routes = (from r in db.RouteSignals
                where r.RouteId == routeID
                select r).ToList();

            if (routes.Count > 0)
                return routes;
            {
                var repository =
                    ApplicationEventRepositoryFactory.Create();
                var error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.ApproachRouteDetailsRepository";
                error.Function = "GetByRouteID";
                error.Description = "No Route for ID.  Attempted ID# = " + routeID;
                error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw new Exception("There is no ApproachRouteDetail for this ID");
            }
        }

        public RouteSignal GetByRouteSignalId(int id)
        {
            var routeSignal = db.RouteSignals.Include("PhaseDirections").FirstOrDefault(r => r.Id == id);
            var signalRepository = SignalsRepositoryFactory.Create();
            if (routeSignal == null)
                return routeSignal;
            routeSignal.Signal = signalRepository.GetLatestVersionOfSignalBySignalID(routeSignal.SignalId);
            return routeSignal;
        }

        public void DeleteById(int id)
        {
            var routeSignal = db.RouteSignals.Find(id);
            if (routeSignal != null)
            {
                db.RouteSignals.Remove(routeSignal);
                db.SaveChanges();
            }
        }

        public void DeleteByRouteID(int routeID)
        {
            var routes = (from r in db.RouteSignals
                where r.RouteId == routeID
                select r).ToList();

            try
            {
                db.RouteSignals.RemoveRange(routes);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                var repository =
                    ApplicationEventRepositoryFactory.Create();
                var error = new ApplicationEvent();
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
            var RouteDetail = (from r in db.RouteSignals
                where r.RouteId == routeID
                      && r.SignalId == signalId
                select r).FirstOrDefault();
            if (RouteDetail != null)
            {
                var newRouteDetail = new RouteSignal();
                newRouteDetail.Order = newOrderNumber;

                try
                {
                    db.Entry(RouteDetail).CurrentValues.SetValues(newRouteDetail);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var error = new ApplicationEvent();
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

        public void Add(RouteSignal newRouteDetail)
        {
            try
            {
                if (!db.RouteSignals.Any(s =>
                    s.SignalId == newRouteDetail.SignalId && s.RouteId == newRouteDetail.RouteId))
                {
                    db.RouteSignals.Add(newRouteDetail);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var repository =
                    ApplicationEventRepositoryFactory.Create();
                var error = new ApplicationEvent();
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