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
        protected int _TimeBuffer;
        protected DateTime _StartDate;

        public PedDelaySignal(Signal signal, int timeBuffer, DateTime startDate,
            DateTime endDate)
        {
            _SignalID = signal.SignalID;
            _TimeBuffer = timeBuffer;
            _StartDate = startDate;
            _EndDate = endDate;
            try
            {
                _Plans = new PlansBase(_SignalID, startDate, endDate);
                ConcurrentBag<PedPhase> pedPhases = new ConcurrentBag<PedPhase>();

                foreach (var approach in signal.Approaches)
                {
                    if (approach.PedestrianPhaseNumber != null || approach.ProtectedPhaseNumber != 0)
                    {
                        var pedPhase = new PedPhase(approach, signal, timeBuffer, startDate, endDate, _Plans);
                        pedPhases.Add(pedPhase);
                    }
                }

                _PedPhases = pedPhases.Where(p => p.Events.Count > 0).OrderBy(x => x.PhaseNumber).ToList();
            }
            catch (Exception e)
            {
                var errorLog = ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                    this.GetType().Name, e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                
            }
        }

        public string SignalID => _SignalID;
        public int TimeBuffer => _TimeBuffer;
        public DateTime StartDate => _StartDate;
        public DateTime EndDate => _EndDate;
        public List<PedPhase> PedPhases => _PedPhases;
    }
}