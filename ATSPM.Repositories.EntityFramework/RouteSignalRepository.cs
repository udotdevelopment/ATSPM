using ATSPM.IRepositories;
using ATSPM.Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class RouteSignalsRepository : IRouteSignalsRepository
    {
        private readonly MOEContext _db;

        public RouteSignalsRepository(MOEContext db)
        {
            this._db = db;
        }

        public List<RouteSignal> GetAllRoutesDetails()
        {
            var routes = (from r in _db.RouteSignals
                          select r).ToList();
            return routes;
        }

        public void MoveRouteSignalUp(int routeId, int routeSignalId)
        {
            var route = _db.Routes.Find(routeId);
            var signal = route.RouteSignals.FirstOrDefault(r => r.Id == routeSignalId);
            var order = signal.Order;
            var swapSignal = route.RouteSignals.FirstOrDefault(r => r.Order == order - 1);
            if (swapSignal != null)
            {
                signal.Order--;
                swapSignal.Order++;
                _db.SaveChanges();
            }
        }

        public void MoveRouteSignalDown(int routeId, int routeSignalId)
        {
            var route = _db.Routes.Find(routeId);
            var signal = route.RouteSignals.FirstOrDefault(r => r.Id == routeSignalId);
            var order = signal.Order;
            var swapSignal = route.RouteSignals.FirstOrDefault(r => r.Order == order + 1);
            if (swapSignal != null)
            {
                signal.Order++;
                swapSignal.Order--;
                _db.SaveChanges();
            }
        }

        public List<RouteSignal> GetByRouteID(int routeID)
        {
            var routes = (from r in _db.RouteSignals
                          where r.RouteId == routeID
                          select r).ToList();

            if (routes.Count > 0)
                return routes;
            {

                var repository = new ApplicationEventRepository(_db);
                var error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.ApproachRouteDetailsRepository";
                error.Function = "GetByRouteID";
                error.Description = "No Route for ID.  Attempted ID# = " + routeID;
                error.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw new Exception("There is no ApproachRouteDetail for this ID");
            }
        }

        public RouteSignal GetByRouteSignalId(int id, 
            IDetectionHardwareRepository detectionHardwareRepository,
            IDetectionTypeRepository detectionTypeRepository)
        {
            var routeSignal = _db.RouteSignals.Include("PhaseDirections").FirstOrDefault(r => r.Id == id);
            var signalsRepository = new SignalsRepository(_db);
            routeSignal.Signal = signalsRepository.GetLatestVersionOfSignalBySignalID(routeSignal.SignalId);
            return routeSignal;
        }

        public void DeleteById(int id)
        {
            var routeSignal = _db.RouteSignals.Find(id);
            if (routeSignal != null)
            {
                _db.RouteSignals.Remove(routeSignal);
                _db.SaveChanges();
            }
        }

        public void DeleteByRouteID(int routeID)
        {
            var routes = (from r in _db.RouteSignals
                          where r.RouteId == routeID
                          select r).ToList();

            try
            {
                _db.RouteSignals.RemoveRange(routes);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                var repository = new ApplicationEventRepository(_db);
                var error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.ApproachRouteDetailsRepository";
                error.Function = "DeleteByRouteID";
                error.Description = ex.Message;
                error.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw;
            }
        }

        public void UpdateByRouteAndApproachID(int routeID, string signalId, int newOrderNumber)
        {
            var RouteDetail = (from r in _db.RouteSignals
                               where r.RouteId == routeID
                                     && r.SignalId == signalId
                               select r).FirstOrDefault();
            if (RouteDetail != null)
            {
                var newRouteDetail = new RouteSignal();
                newRouteDetail.Order = newOrderNumber;

                try
                {
                    _db.Entry(RouteDetail).CurrentValues.SetValues(newRouteDetail);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository = new ApplicationEventRepository(_db);
                    var error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.ApproachRouteDetailsRepository";
                    error.Function = "UpdateByRouteAndApproachID";
                    error.Description = ex.Message;
                    error.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
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
                if (!_db.RouteSignals.Any(s =>
                    s.SignalId == newRouteDetail.SignalId && s.RouteId == newRouteDetail.RouteId))
                {
                    _db.RouteSignals.Add(newRouteDetail);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var repository = new ApplicationEventRepository(_db);
                var error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.ApproachRouteDetailsRepository";
                error.Function = "UpdateByRouteAndApproachID";
                error.Description = ex.Message;
                error.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw;
            }
        }

        public RouteSignal GetByRouteSignalId(int id)
        {
            throw new NotImplementedException();
        }
    }
}