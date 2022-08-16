using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;

namespace MOE.Common.Models.Repositories
{
    public class RoutePhaseDirectionRepository : IRoutePhaseDirectionRepository
    {
        private readonly SPM db = new SPM();

        public List<RoutePhaseDirection> GetAll()
        {
            return db.RoutePhaseDirections.ToList();
        }

        public RoutePhaseDirection GetByID(int id)
        {
            return db.RoutePhaseDirections.Where(r => r.Id == id).FirstOrDefault();
        }

        public void DeleteByID(int id)
        {
            var routePhaseDirection = db.RoutePhaseDirections.Where(r => r.Id == id).FirstOrDefault();
            db.RoutePhaseDirections.Remove(routePhaseDirection);
            db.SaveChanges();
        }

        public void Update(RoutePhaseDirection newRoutePhaseDirection)
        {
            CheckForExistingApproach(newRoutePhaseDirection);
            var routePhaseDirection =
                db.RoutePhaseDirections.Where(r => r.Id == newRoutePhaseDirection.Id).FirstOrDefault();

            if (routePhaseDirection != null)
                try
                {
                    db.Entry(routePhaseDirection).CurrentValues.SetValues(newRoutePhaseDirection);
                    db.SaveChanges();
                }

                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
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
                db.RoutePhaseDirections.Add(newRoutePhaseDirection);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                var errorLog = ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(Assembly.GetExecutingAssembly().GetName().ToString(),
                    GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                throw new Exception("");
            }
        }

        private void CheckForExistingApproach(RoutePhaseDirection newRoutePhaseDirection)
        {
            var routePhaseDirection = db.RoutePhaseDirections.Where(r =>
                r.RouteSignalId == newRoutePhaseDirection.RouteSignalId &&
                r.IsPrimaryApproach == newRoutePhaseDirection.IsPrimaryApproach).FirstOrDefault();
            if (routePhaseDirection != null)
            {
                db.RoutePhaseDirections.Remove(routePhaseDirection);
                db.SaveChanges();
            }
        }
    }
}