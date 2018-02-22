using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface ISPMWatchDogErrorEventRepository
    {
        List<SPMWatchDogErrorEvent> GetAllSPMWatchDogErrorEvents();
        List<SPMWatchDogErrorEvent> GetSPMWatchDogErrorEventsBetweenDates(DateTime StartDate, DateTime EndDate);
        SPMWatchDogErrorEvent GetSPMWatchDogErrorEventByID(int SPMWatchDogErrorEventID);
        void Update(SPMWatchDogErrorEvent SPMWatchDogErrorEvent);
        void AddList(List<SPMWatchDogErrorEvent> SPMWatchDogErrorEvents);
        void Add(SPMWatchDogErrorEvent SPMWatchDogErrorEvent);
        void Remove(SPMWatchDogErrorEvent SPMWatchDogErrorEvent);
        void Remove(int id);
    }
}