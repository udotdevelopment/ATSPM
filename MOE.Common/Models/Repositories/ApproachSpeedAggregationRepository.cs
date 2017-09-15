using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ApproachSpeedAggregationRepository : IApproachSpeedAggregationRepository
    {
        Models.SPM db = new SPM();
        
        public void Update(MOE.Common.Models.ApproachSpeedAggregationData approachSpeedAggregationData)
        {
            MOE.Common.Models.ApplicationEvent g = (from r in db.ApplicationEvents
                                             where r.ID == approachSpeedAggregationData.ID
                                             select r).FirstOrDefault();
            try
            {
                if (g != null)
                {
                    db.Entry(g).CurrentValues.SetValues(approachSpeedAggregationData);
                    db.SaveChanges();
                }
                else
                {
                    db.ApproachSpeedAggregationDatas.Add(approachSpeedAggregationData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                var errorLog = ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                    this.GetType().ToString(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                throw e;
            }
            
        }

        public void Remove(MOE.Common.Models.ApproachSpeedAggregationData approachSpeedAggregationData)
        {
            MOE.Common.Models.ApplicationEvent g = (from r in db.ApplicationEvents
                                             where r.ID == approachSpeedAggregationData.ID
                                             select r).FirstOrDefault();
            if (g != null)
            {
                try
                {
                    db.ApplicationEvents.Remove(g);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().ToString(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw e;
                }
            }
        }

        public void Remove(int id)
        {
            MOE.Common.Models.ApplicationEvent g = db.ApplicationEvents.Find(id);
            if (g != null)
            {
                try
                {
                    db.ApplicationEvents.Remove(g);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    var errorLog = ApplicationEventRepositoryFactory.Create();
                    errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                        this.GetType().ToString(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                    throw e;
                }
            }
        }
        public void Add(MOE.Common.Models.ApproachSpeedAggregationData approachSpeedAggregationData)
        {
            try
            {
                db.ApproachSpeedAggregationDatas.Add(approachSpeedAggregationData);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                var errorLog = ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                    this.GetType().ToString(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                throw e;
            }
            

        }
    }
}
