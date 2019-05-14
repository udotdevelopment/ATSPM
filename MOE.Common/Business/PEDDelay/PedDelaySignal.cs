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

        public PedDelaySignal(Signal signal, DateTime startDate,
            DateTime endDate)
        {
            _SignalID = signal.SignalID;
            _StartDate = startDate;
            _EndDate = endDate;
            try
            {
                _Plans = new PlansBase(_SignalID, startDate, endDate);
                var pedPhaseNumbers = ControllerEventLogs.GetPedPhases(_SignalID, startDate, endDate);
                ConcurrentBag<PedPhase> pedPhases = new ConcurrentBag<PedPhase>();
                Parallel.ForEach(pedPhaseNumbers, currentPhase =>
                //foreach(int currentPhase in pedPhaseNumbers)
                {
                    var pedPhase = new PedPhase(currentPhase, signal, startDate, endDate, _Plans);
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

        public string SignalID => _SignalID;
        public DateTime StartDate => _StartDate;
        public DateTime EndDate => _EndDate;
        public List<PedPhase> PedPhases => _PedPhases;
    }
}