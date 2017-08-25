using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class SPMWatchDogErrorEventRepository : ISPMWatchDogErrorEventRepository
    {
     Models.SPM db = new SPM();

        public List<Models.SPMWatchDogErrorEvent> GetSPMWatchDogErrorEventsBetweenDates(DateTime StartDate, DateTime EndDate)
        {
            var SPMWatchDogErrorEvents = (from r in db.SPMWatchDogErrorEvents
                                    where r.TimeStamp >= StartDate && r.TimeStamp < EndDate
                                    select r).ToList();

            return SPMWatchDogErrorEvents;
        }       

        public List<Models.SPMWatchDogErrorEvent> GetAllSPMWatchDogErrorEvents()
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<Models.SPMWatchDogErrorEvent> SPMWatchDogErrorEvents = (from r in db.SPMWatchDogErrorEvents
                                                 select r).ToList();

            return SPMWatchDogErrorEvents;
        }

        public Models.SPMWatchDogErrorEvent GetSPMWatchDogErrorEventByID(int SPMWatchDogErrorEventID)
        {
            var SPMWatchDogErrorEvent = (from r in db.SPMWatchDogErrorEvents
                             where r.ID == SPMWatchDogErrorEventID
                             select r);

            return SPMWatchDogErrorEvent.FirstOrDefault();
        }

        public void Update(MOE.Common.Models.SPMWatchDogErrorEvent SPMWatchDogErrorEvent)
        {


            MOE.Common.Models.SPMWatchDogErrorEvent g = (from r in db.SPMWatchDogErrorEvents
                                             where r.ID == SPMWatchDogErrorEvent.ID
                                             select r).FirstOrDefault();
            if (g != null)
            {
                db.Entry(g).CurrentValues.SetValues(SPMWatchDogErrorEvent);
                db.SaveChanges();
            }
            else
            {
                db.SPMWatchDogErrorEvents.Add(SPMWatchDogErrorEvent);
                db.SaveChanges();

            }


        }

        public void Remove(MOE.Common.Models.SPMWatchDogErrorEvent SPMWatchDogErrorEvent)
        {


            MOE.Common.Models.SPMWatchDogErrorEvent g = (from r in db.SPMWatchDogErrorEvents
                                             where r.ID == SPMWatchDogErrorEvent.ID
                                             select r).FirstOrDefault();
            if (g != null)
            {
                db.SPMWatchDogErrorEvents.Remove(g);
                db.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            MOE.Common.Models.SPMWatchDogErrorEvent g = db.SPMWatchDogErrorEvents.Find(id);
            if (g != null)
            {
                db.SPMWatchDogErrorEvents.Remove(g);
                db.SaveChanges();
            }
        }

        public void Add(MOE.Common.Models.SPMWatchDogErrorEvent SPMWatchDogErrorEvent)
        {


            MOE.Common.Models.SPMWatchDogErrorEvent g = (from r in db.SPMWatchDogErrorEvents
                                             where r.ID == SPMWatchDogErrorEvent.ID
                                             select r).FirstOrDefault();
            if (g == null)
            {
                try
                {
                    db.SPMWatchDogErrorEvents.Add(SPMWatchDogErrorEvent);
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw;
                }
            }

        }

        public void AddList(List<Models.SPMWatchDogErrorEvent> SPMWatchDogErrorEvents)
        {
            try
            {
                db.SPMWatchDogErrorEvents.AddRange(SPMWatchDogErrorEvents);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository er =
                                MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                er.QuickAdd("MOE.Common", "SPMWatchDogErrrorEventRepository", "AddList",
                    MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
            }

        }


      

    }
}
