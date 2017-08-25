using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace MOE.Common.Business
{
    public class RLMSignalPhase
    {
        public bool IsPermissive { get; set; }

        private VolumeCollection volume;
        public VolumeCollection Volume
        {
            get { return volume; }
        }

        public int PhaseNumber { get; set; }

        public double Violations
        {
            get
            {
                return Plans.PlanList.Sum(d => d.Violations);
            }
        }

        private MOE.Common.Business.RLMPlanCollection plans;
        public MOE.Common.Business.RLMPlanCollection Plans
        {
            get { return plans; }
        }

        private double srlvSeconds = 0;
        public double SRLVSeconds
        {
            get
            {
                return srlvSeconds;
            }
        }

        public double Srlv
        {
            get
            {
                return Plans.PlanList.Sum(d => d.Srlv);
            }
        }

        private double totalVolume = 0;
        public double TotalVolume
        {
            get
            {
                return totalVolume;
            }
        }

        public double PercentViolations
        {
            get
            {
                if (TotalVolume > 0)
                {
                    return Math.Round((Violations / TotalVolume) * 100, 0);
                }
                else
                {
                    return 0;
                }
            }
        }

        public double PercentSevereViolations
        {
            get
            {
                if (TotalVolume > 0)
                {
                    return Math.Round((Srlv / TotalVolume) * 100, 2);
                }
                else
                {
                    return 0;
                }
            }
        }

        public double YellowOccurrences
        {
            get
            {
                return this.plans.PlanList.Sum(d => d.YellowOccurrences);
            }
        }

        public double TotalYellowTime
        {
            get
            {
                return this.plans.PlanList.Sum(d => d.TotalYellowTime);
            }
        }

        public double AverageTYLO
        {
            get
            {
                return Math.Round(TotalYellowTime / YellowOccurrences, 1);
            }
        }

        public double PercentYellowOccurrences
        {
            get
            {
                if (TotalVolume > 0)
                {
                    return Math.Round((YellowOccurrences / TotalVolume) * 100, 0);
                }
                else
                {
                    return 0;
                }
            }
        }
        public MOE.Common.Models.Approach Approach { get; set; }


        private DateTime startDate;
        private DateTime endDate;
        private int detChannel;
        private bool showVolume;
        private int binSize;




        /// <summary>
        /// Constructor for Signal phase
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="signalId"></param>
        /// <param name="eventData1"></param>
        /// <param name="region"></param>
        /// <param name="detChannel"></param>
        public RLMSignalPhase(DateTime startDate, DateTime endDate, int binSize, double srlvSeconds,
            int metricTypeID, MOE.Common.Models.Approach approach, bool usePermissivePhase)
        {            
            this.srlvSeconds = srlvSeconds;
            this.startDate = startDate;
            this.endDate = endDate;
            this.Approach = approach;
            
            this.binSize = binSize;

            IsPermissive = usePermissivePhase;

            Models.Repositories.IControllerEventLogRepository controllerRepository =
                Models.Repositories.ControllerEventLogRepositoryFactory.Create();
           

            if (!Approach.IsProtectedPhaseOverlap)
            {
                
                GetSignalPhaseData(startDate, endDate, showVolume, binSize, usePermissivePhase);
            }
            else
            {
                totalVolume = controllerRepository.GetTMCVolume(startDate, endDate, Approach.SignalID, Approach.ProtectedPhaseNumber);
                GetSignalOverlapData(startDate, endDate, showVolume, binSize);
            }

        }


        private void GetSignalPhaseData(DateTime startDate, DateTime endDate, bool showVolume, int binSize, bool usePermissivePhase)
        {
            DateTime redLightTimeStamp = DateTime.MinValue;
            MOE.Common.Models.Repositories.IControllerEventLogRepository controllerRepository =
                MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            List<Models.Controller_Event_Log> cycleEvents;

            if (!usePermissivePhase)
            {
                PhaseNumber = Approach.ProtectedPhaseNumber;

            }
            else 
            {
                PhaseNumber = Approach.PermissivePhaseNumber??0;
            }

            totalVolume = controllerRepository.GetTMCVolume(startDate, endDate, Approach.SignalID, PhaseNumber);
            cycleEvents = controllerRepository.GetEventsByEventCodesParam(Approach.SignalID,
                startDate, endDate, new List<int>() { 1, 8, 9, 10, 11 }, PhaseNumber);

            plans = new RLMPlanCollection(cycleEvents, startDate, endDate, this.SRLVSeconds, Approach);
            if (plans.PlanList.Count == 0)
            {
                plans.AddItem(new RLMPlan(startDate, endDate, 0, cycleEvents, this.SRLVSeconds, Approach));
            }
        }       


        private void GetSignalOverlapData(DateTime startDate, DateTime endDate, bool showVolume, int binSize)
        {
            
            DateTime redLightTimeStamp = DateTime.MinValue;
            List<int> li = new List<int>{62,63,64};
            MOE.Common.Models.Repositories.IControllerEventLogRepository controllerRepository =
                MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            var cycleEvents = controllerRepository.GetEventsByEventCodesParam(Approach.SignalID,
                startDate, endDate, li, Approach.ProtectedPhaseNumber);
            plans = new RLMPlanCollection(cycleEvents, startDate, endDate, this.SRLVSeconds, Approach);
            if (plans.PlanList.Count == 0)
            {
                Plans.AddItem(new RLMPlan(startDate, endDate, 0, cycleEvents, this.SRLVSeconds, Approach));
            }            
        }

       



 
              
    }
}
