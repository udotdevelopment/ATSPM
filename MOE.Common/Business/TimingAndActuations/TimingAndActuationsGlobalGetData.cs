using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.TimingAndActuations
{
    public class TimingAndActuationsGlobalGetData
    {
        public Dictionary<string, List<Controller_Event_Log>> GlobalCustomEvents { get; set; }
        public TimingAndActuationsOptions Options { get; }
        public Approach Approach { get; set; }
        public string SignalId { get; }

        public TimingAndActuationsGlobalGetData(string signalId, TimingAndActuationsOptions options)
        {
            Options = options;
            SignalId = signalId;
            CheckAndGetGlobalCustomEvents();
        }

        private void CheckAndGetGlobalCustomEvents()
        {
            List<int> globalCodes = new List<int>();
            var globalCustomCode1 = 0;
            if (Options.GlobalCustomCode1.HasValue)
            {
                globalCustomCode1 = Options.GlobalCustomCode1.Value;
                globalCodes.Add(Options.GlobalCustomCode1.Value);
            }
            if (Options.GlobalCustomCode2.HasValue)
            {
                var globalCustomCode2 = Options.GlobalCustomCode2.Value;
                if (globalCustomCode1 != globalCustomCode2)
                {
                    globalCodes.Add(Options.GlobalCustomCode2.Value);
                }
            }
            GetGlobalCustomAllEvents(globalCodes);
        }

        private void GetGlobalCustomAllEvents(List<int> globalCustomCodes)
        {
            GlobalCustomEvents = new Dictionary<string, List<Controller_Event_Log>>();
            foreach (var globalCustomCode in globalCustomCodes)
            {
                var controllerEventLogRepository =
                    Models.Repositories.ControllerEventLogRepositoryFactory.Create();
                var globalCustomEvents = controllerEventLogRepository.GetSignalEventsByEventCode(SignalId,
                    Options.StartDate, Options.EndDate, globalCustomCode);
                GlobalCustomEvents.Add("Global Custom Events for Event Code: " + globalCustomCode, globalCustomEvents);
            }
        }
    }
}
