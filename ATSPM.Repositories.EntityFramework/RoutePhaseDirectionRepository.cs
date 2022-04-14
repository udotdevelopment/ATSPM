using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ATSPM.IRepositories;
using ATSPM.Application.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class RoutePhaseDirectionRepository : IRoutePhaseDirectionRepository
    {
        private readonly MOEContext _db;

        public RoutePhaseDirectionRepository(MOEContext db)
        {
            this._db = db;
        }

        public List<RoutePhaseDirection> GetAll()
        {
            return _db.RoutePhaseDirections.ToList();
        }

        public RoutePhaseDirection GetByID(int id)
        {
            return _db.RoutePhaseDirections.Where(r => r.Id == id).FirstOrDefault();
        }

        public void DeleteByID(int id)
        {
            var routePhaseDirection = _db.RoutePhaseDirections.Where(r => r.Id == id).FirstOrDefault();
            _db.RoutePhaseDirections.Remove(routePhaseDirection);
            _db.SaveChanges();
        }

        public void Update(RoutePhaseDirection newRoutePhaseDirection)
        {
            CheckForExistingApproach(newRoutePhaseDirection);
            var routePhaseDirection =
                _db.RoutePhaseDirections.Where(r => r.Id == newRoutePhaseDirection.Id).FirstOrDefault();

            if (routePhaseDirection != null)
                try
                {
                    _db.Entry(routePhaseDirection).CurrentValues.SetValues(newRoutePhaseDirection);
                    _db.SaveChanges();
                }

                catch (Exception e)
                {
                    var errorLog =  new ApplicationEventRepository(_db);
                    errorLog.QuickAdd(Assembly.GetExecutingAssembly().GetName().ToString(),
                        GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High,
                        e.Message);
                    throw new Exception("");
                }
            else
                Add(newRoutePhaseDirection);
        }

        public void Add(RoutePhaseDirection newRoutePhaseDirection)
        {
            try
            {
                _db.RoutePhaseDirections.Add(newRoutePhaseDirection);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                var errorLog =new ApplicationEventRepository(_db);
                errorLog.QuickAdd(Assembly.GetExecutingAssembly().GetName().ToString(),
                    GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                throw new Exception("");
            }
        }

        private void CheckForExistingApproach(RoutePhaseDirection newRoutePhaseDirection)
        {
            var routePhaseDirection = _db.RoutePhaseDirections.Where(r =>
                r.RouteSignalId == newRoutePhaseDirection.RouteSignalId &&
                r.IsPrimaryApproach == newRoutePhaseDirection.IsPrimaryApproach).FirstOrDefault();
            if (routePhaseDirection != null)
            {
                _db.RoutePhaseDirections.Remove(routePhaseDirection);
                _db.SaveChanges();
            }
        }
    }
}