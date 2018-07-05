using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class SPMWatchDogErrorEventRepository : ISPMWatchDogErrorEventRepository
    {
        private readonly SPM db = new SPM();

        public List<SPMWatchDogErrorEvent> GetSPMWatchDogErrorEventsBetweenDates(DateTime StartDate, DateTime EndDate)
        {
            var SPMWatchDogErrorEvents = (from r in db.SPMWatchDogErrorEvents
                where r.TimeStamp >= StartDate && r.TimeStamp < EndDate
                select r).ToList();

            return SPMWatchDogErrorEvents;
        }

        public List<SPMWatchDogErrorEvent> GetAllSPMWatchDogErrorEvents()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var SPMWatchDogErrorEvents = (from r in db.SPMWatchDogErrorEvents
                select r).ToList();

            return SPMWatchDogErrorEvents;
        }

        public SPMWatchDogErrorEvent GetSPMWatchDogErrorEventByID(int SPMWatchDogErrorEventID)
        {
            var SPMWatchDogErrorEvent = from r in db.SPMWatchDogErrorEvents
                where r.ID == SPMWatchDogErrorEventID
                select r;

            return SPMWatchDogErrorEvent.FirstOrDefault();
        }

        public void Update(SPMWatchDogErrorEvent SPMWatchDogErrorEvent)
        {
            var g = (from r in db.SPMWatchDogErrorEvents
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

        public void Remove(SPMWatchDogErrorEvent SPMWatchDogErrorEvent)
        {
            var g = (from r in db.SPMWatchDogErrorEvents
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
            var g = db.SPMWatchDogErrorEvents.Find(id);
            if (g != null)
            {
                db.SPMWatchDogErrorEvents.Remove(g);
                db.SaveChanges();
            }
        }

        public void Add(SPMWatchDogErrorEvent SPMWatchDogErrorEvent)
        {
            var g = (from r in db.SPMWatchDogErrorEvents
                where r.ID == SPMWatchDogErrorEvent.ID
                select r).FirstOrDefault();
            if (g == null)
                try
                {
                    db.SPMWatchDogErrorEvents.Add(SPMWatchDogErrorEvent);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
        }

        public void AddList(List<SPMWatchDogErrorEvent> SPMWatchDogErrorEvents)
        {
            try
            {
                db.SPMWatchDogErrorEvents.AddRange(SPMWatchDogErrorEvents);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                var er =
                    ApplicationEventRepositoryFactory.Create();
                er.QuickAdd("MOE.Common", "SPMWatchDogErrrorEventRepository", "AddList",
                    ApplicationEvent.SeverityLevels.Medium, ex.Message);
            }
        }
    }
}