using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class SignalAggregationDataRepository : ISignalAggregationDataRepository
    {
        Models.SPM db = new SPM();
        public int GetTotalCycles(string signalID, DateTime startTime, DateTime endTime)
        {
            int count = (from cel in db.Controller_Event_Log
                         where cel.Timestamp >= startTime
                            && cel.Timestamp < endTime
                            && cel.SignalID == signalID
                            && cel.EventParam == 1
                            && cel.EventCode == 150
                            select cel).Count();
            return count;
        }

        public int GetAddCycles(string signalID, DateTime startTime, DateTime endTime)
        {
            int count = (from cel in db.Controller_Event_Log
                where cel.Timestamp >= startTime
                      && cel.Timestamp < endTime
                      && cel.SignalID == signalID
                      && cel.EventParam == 2
                      && cel.EventCode == 150
                select cel).Count();
            return count;
        }

        public int GetSubtractCycles(string signalID, DateTime startTime, DateTime endTime)
        {
            int count = (from cel in db.Controller_Event_Log
                where cel.Timestamp >= startTime
                      && cel.Timestamp < endTime
                      && cel.SignalID == signalID
                      && cel.EventParam == 3
                      && cel.EventCode == 150
                select cel).Count();
            return count;
        }

        public int GetDwellCycles(string signalID, DateTime startTime, DateTime endTime)
        {
            int count = (from cel in db.Controller_Event_Log
                where cel.Timestamp >= startTime
                      && cel.Timestamp < endTime
                      && cel.SignalID == signalID
                      && cel.EventParam == 4
                      && cel.EventCode == 150
                select cel).Count();
            return count;
        }

        public void SaveSignalData(SignalAggregation signalAggregation)
        {
            try
            {
                db.SignalAggregations.Add(signalAggregation);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                IApplicationEventRepository applicationEventRepository = ApplicationEventRepositoryFactory.Create();
                applicationEventRepository.QuickAdd("DataAggregation", "SignalAggregationDataRepository", "SaveSignalData", ApplicationEvent.SeverityLevels.High, e.Message);
            }
        }
    }
}
