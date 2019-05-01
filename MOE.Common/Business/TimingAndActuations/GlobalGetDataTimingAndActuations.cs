using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.TimingAndActuations
{
    public class GlobalGetDataTimingAndActuations
    {
        public Dictionary<string, List<Controller_Event_Log>> GlobalCustomEvents { get; set; }

        public GlobalGetDataTimingAndActuations(string signalId, TimingAndActuationsOptions options)
        {
            if (options.GlobalEventCodesList != null && options.GlobalEventParamsList != null &&
                options.GlobalEventCodesList.Any() && options.GlobalEventCodesList.Count > 0 &&
                options.GlobalEventParamsList.Any() && options.GlobalEventParamsList.Count > 0)
            {
                GlobalCustomEvents = new Dictionary<string, List<Controller_Event_Log>>();
                foreach (var globalEventCode in options.GlobalEventCodesList)
                {
                    foreach (var globalEventParam in options.GlobalEventParamsList)
                    {
                        options.GlobalEventCounter = 1;
                        var controllerEventLogRepository =
                            Models.Repositories.ControllerEventLogRepositoryFactory.Create();
                        var globalCustomEvents = controllerEventLogRepository.GetEventsByEventCodesParam
                        (signalId, options.StartDate, options.EndDate,
                            new List<int> { globalEventCode }, globalEventParam);
                        if (globalCustomEvents.Count > 0)
                        {
                            GlobalCustomEvents.Add("Global Events: Code: " + globalEventCode + " Param: " +
                                    globalEventParam,
//                                globalEventParam + " Counter: " + options.GlobalEventCounter++,
                                    globalCustomEvents);
                        }
                    }
                }

                
            }
        }
    }
}


