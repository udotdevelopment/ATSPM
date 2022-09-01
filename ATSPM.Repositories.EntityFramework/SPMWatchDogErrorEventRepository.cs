using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class SPMWatchDogErrorEventRepository : ISPMWatchDogErrorEventRepository
    {
        private readonly MOEContext _db;

        public SPMWatchDogErrorEventRepository(MOEContext db)
        {
            this._db = db;
        }

        public List<SpmwatchDogErrorEvent> GetSPMWatchDogErrorEventsBetweenDates(DateTime StartDate, DateTime EndDate)
        {
            var SpmwatchDogErrorEvents = (from r in _db.SpmwatchDogErrorEvents
                                          where r.TimeStamp >= StartDate && r.TimeStamp < EndDate
                                          select r).ToList();

            return SpmwatchDogErrorEvents;
        }

        public List<SpmwatchDogErrorEvent> GetAllSPMWatchDogErrorEvents()
        {
            //db.Configuration.LazyLoadingEnabled = false;
            var SpmwatchDogErrorEvents = (from r in _db.SpmwatchDogErrorEvents
                                          select r).ToList();

            return SpmwatchDogErrorEvents;
        }

        public SpmwatchDogErrorEvent GetSPMWatchDogErrorEventByID(int SPMWatchDogErrorEventID)
        {
            var SpmwatchDogErrorEvent = from r in _db.SpmwatchDogErrorEvents
                                        where r.Id == SPMWatchDogErrorEventID
                                        select r;

            return SpmwatchDogErrorEvent.FirstOrDefault();
        }

        public void Update(SpmwatchDogErrorEvent SpmwatchDogErrorEvent)
        {
            var g = (from r in _db.SpmwatchDogErrorEvents
                     where r.Id == SpmwatchDogErrorEvent.Id
                     select r).FirstOrDefault();
            if (g != null)
            {
                _db.Entry(g).CurrentValues.SetValues(SpmwatchDogErrorEvent);
                _db.SaveChanges();
            }
            else
            {
                _db.SpmwatchDogErrorEvents.Add(SpmwatchDogErrorEvent);
                _db.SaveChanges();
            }
        }

        public void Remove(SpmwatchDogErrorEvent SpmwatchDogErrorEvent)
        {
            var g = (from r in _db.SpmwatchDogErrorEvents
                     where r.Id == SpmwatchDogErrorEvent.Id
                     select r).FirstOrDefault();
            if (g != null)
            {
                _db.SpmwatchDogErrorEvents.Remove(g);
                _db.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            var g = _db.SpmwatchDogErrorEvents.Find(id);
            if (g != null)
            {
                _db.SpmwatchDogErrorEvents.Remove(g);
                _db.SaveChanges();
            }
        }

        public void Add(SpmwatchDogErrorEvent SpmwatchDogErrorEvent)
        {
            var g = (from r in _db.SpmwatchDogErrorEvents
                     where r.Id == SpmwatchDogErrorEvent.Id
                     select r).FirstOrDefault();
            if (g == null)
                try
                {
                    _db.SpmwatchDogErrorEvents.Add(SpmwatchDogErrorEvent);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
        }

        public void AddListAndSaveToDatabase(List<SpmwatchDogErrorEvent> SpmwatchDogErrorEvents)
        {
            try
            {
                _db.SpmwatchDogErrorEvents.AddRange(SpmwatchDogErrorEvents);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                var er = new ApplicationEventRepository(_db);
                er.QuickAdd("MOE.Common", "SPMWatchDogErrrorEventRepository", "AddListAndSaveToDatabase",
                    ApplicationEvent.SeverityLevels.Medium, ex.Message);
            }
        }
    }
}