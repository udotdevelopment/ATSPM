using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface ISPMWatchDogErrorEventRepository
    {
        List<Models.SPMWatchDogErrorEvent> GetAllSPMWatchDogErrorEvents();
        List<Models.SPMWatchDogErrorEvent> GetSPMWatchDogErrorEventsBetweenDates(DateTime StartDate, DateTime EndDate);
        Models.SPMWatchDogErrorEvent GetSPMWatchDogErrorEventByID(int SPMWatchDogErrorEventID);
        void Update(MOE.Common.Models.SPMWatchDogErrorEvent SPMWatchDogErrorEvent);
        void AddList(List<Models.SPMWatchDogErrorEvent> SPMWatchDogErrorEvents);
        void Add(MOE.Common.Models.SPMWatchDogErrorEvent SPMWatchDogErrorEvent);
        void Remove(MOE.Common.Models.SPMWatchDogErrorEvent SPMWatchDogErrorEvent);
        void Remove(int id);
    }
}
