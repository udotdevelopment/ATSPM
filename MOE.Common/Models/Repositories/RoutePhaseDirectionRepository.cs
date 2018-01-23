using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Microsoft.EntityFrameworkCore.Internal;
using NuGet;

namespace MOE.Common.Models.Repositories 
{
    public class RoutePhaseDirectionRepository : IRoutePhaseDirectionRepository
    {
        MOE.Common.Models.SPM db = new SPM();

        public List<Models.RoutePhaseDirection> GetAll()
        {
            return db.RoutePhaseDirections.ToList();
        }

        public Models.RoutePhaseDirection GetByID(int id)
        {
            return db.RoutePhaseDirections.Where(r => r.Id == id).FirstOrDefault();
        }

        public void DeleteByID(int id)
        {
            Models.RoutePhaseDirection routePhaseDirection = db.RoutePhaseDirections.Where(r => r.Id == id).FirstOrDefault();
            db.RoutePhaseDirections.Remove(routePhaseDirection);
            db.SaveChanges();
        }

        public void Update(RoutePhaseDirection newRoutePhaseDirection)
        {
            CheckForExistingApproach(newRoutePhaseDirection);
            Models.RoutePhaseDirection routePhaseDirection = db.RoutePhaseDirections.Where(r => r.Id == newRoutePhaseDirection.Id).FirstOrDefault();

            if(routePhaseDirection != null)
            {
                try
                {
                    db.Entry(routePhaseDirection).CurrentValues.SetValues(newRoutePhaseDirection);
                    db.SaveChanges();
                }

                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw new Exception("");
                }
            
            }
            else
            {
                Add(newRoutePhaseDirection);
            }
        }

        private void CheckForExistingApproach(RoutePhaseDirection newRoutePhaseDirection)
        {
            var routePhaseDirection = db.RoutePhaseDirections.Where(r => r.RouteSignalId == newRoutePhaseDirection.RouteSignalId && r.IsPrimaryApproach == newRoutePhaseDirection.IsPrimaryApproach).FirstOrDefault();
            if (routePhaseDirection != null)
            {
                db.RoutePhaseDirections.Remove(routePhaseDirection);
                db.SaveChanges();
            }
        }

        public void Add(Models.RoutePhaseDirection newRoutePhaseDirection)
        {
            try
            {
                db.RoutePhaseDirections.Add(newRoutePhaseDirection);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                var errorLog = ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                    this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                throw new Exception("");
            }
        }
    }
}
