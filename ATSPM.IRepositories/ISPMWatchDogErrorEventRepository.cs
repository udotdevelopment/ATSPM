using ATSPM.Application.Models;
using System;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface ISPMWatchDogErrorEventRepository
    {
        List<Application.Models.SpmwatchDogErrorEvent> GetAllSPMWatchDogErrorEvents();
        List<SpmwatchDogErrorEvent> GetSPMWatchDogErrorEventsBetweenDates(DateTime StartDate, DateTime EndDate);
        SpmwatchDogErrorEvent GetSPMWatchDogErrorEventByID(int SPMWatchDogErrorEventID);
        void Update(SpmwatchDogErrorEvent SPMWatchDogErrorEvent);
        void AddListAndSaveToDatabase(List<SpmwatchDogErrorEvent> SPMWatchDogErrorEvents);
        void Add(SpmwatchDogErrorEvent SPMWatchDogErrorEvent);
        void Remove(SpmwatchDogErrorEvent SPMWatchDogErrorEvent);
        void Remove(int id);
    }
}