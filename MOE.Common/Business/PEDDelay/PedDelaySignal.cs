using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.PEDDelay
{
    public class PedDelaySignal
    {
        protected DateTime _EndDate;
        protected List<PedPhase> _PedPhases = new List<PedPhase>();
        protected PlansBase _Plans;
        protected string _SignalID;
        protected DateTime _StartDate;

        public PedDelaySignal(string signalID, DateTime startDate,
            DateTime endDate)
        {
            _SignalID = signalID;
            _StartDate = startDate;
            _EndDate = endDate;
            try
            {
                _Plans = new PlansBase(signalID, startDate, endDate);
                var pedPhaseNumbers = GetPedPhases();
                ConcurrentBag<PedPhase> pedPhases = new ConcurrentBag<PedPhase>();
                Parallel.ForEach(pedPhaseNumbers, currentPhase =>
                //foreach(int currentPhase in pedPhaseNumbers)
                {
                    var pedPhase = new PedPhase(currentPhase, signalID, startDate, endDate, _Plans);
                    pedPhases.Add(pedPhase);
                });
                _PedPhases = pedPhases.OrderBy(x => x.PhaseNumber).ToList();
            }
            catch (Exception e)
            {
                var errorLog = ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                    this.GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                
            }
        }

        private List<int> GetPedPhases()
        {
            var pedEventCodes = new List<int> { 21, 45, 90, 23 };
            var CELRepo = MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            var events = CELRepo.GetSignalEventsByEventCodes(_SignalID, _StartDate, _EndDate, pedEventCodes);

            var distinctEvents = (from i in events
                                  select i.EventParam).Distinct();


            List<int> converted = new List<int>();
            foreach (var i in distinctEvents)
            {
                var c = Convert.ToInt32(i);
                converted.Add(c);
            }

            return converted;
        }

        public string SignalID => _SignalID;
        public DateTime StartDate => _StartDate;
        public DateTime EndDate => _EndDate;
        public List<PedPhase> PedPhases => _PedPhases;
    }
}