using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace MOE.Common.Business
{
    public class SignalPhase
    {


        private string direction;
        public string Direction
        {
            get { return direction; }
        }

        //private int phase;
        //public int Phase
        //{
        //    get { return phase; }
        //}

        //private bool isOverLap;
        //public bool IsOverlap
        //{
        //    get { return isOverLap; }
        //}

        private VolumeCollection volume;
        public VolumeCollection Volume
        {
            get { return volume; }
        }



        private MOE.Common.Business.PlanCollection plans;
        public MOE.Common.Business.PlanCollection Plans
        {
            get { return plans; }
        }



        public double AvgDelay
        {
            get
            {
                return TotalDelay / TotalVolume;
            }
        }


        /// <summary>
        /// A calculation to get the percent of activations on green
        /// </summary>
        public double PercentArrivalOnGreen
        {
            get
            {
                if (TotalVolume > 0)
                {
                    return Math.Round(((TotalArrivalOnGreen / TotalVolume) * 100));
                }
                else
                {
                    return 0;
                }
            }
        }

        public double PercentGreen
        {
            get
            {
                if (TotalTime > 0)
                {
                    return Math.Round(((TotalGreenTime / TotalTime) * 100));
                }
                else
                {
                    return 0;
                }
            }
        }

        public double PlatoonRatio
        {
            get
            {
                if (TotalVolume > 0)
                {
                    return Math.Round((PercentArrivalOnGreen / PercentGreen), 2);
                }
                else
                {
                    return 0;
                }
            }
        }

        private double totalArrivalOnGreen = -1;
        public double TotalArrivalOnGreen
        {
            get
            {
                if (totalArrivalOnGreen == -1)
                    totalArrivalOnGreen = Plans.PlanList.Sum(d => d.TotalArrivalOnGreen);
                return totalArrivalOnGreen;
            }
        }
        

        private double totalArrivalOnRed = -1;
        public double TotalArrivalOnRed
        {
            get { 
                if(totalArrivalOnRed == -1)
                    totalArrivalOnRed = Plans.PlanList.Sum(d => d.TotalArrivalOnRed);
                return totalArrivalOnRed;
            }
        }

        public double TotalDelay
        {
            get
            {
                return Plans.PlanList.Sum(d => d.TotalDelay);
            }
        }

        private double totalVolume = -1;
        public double TotalVolume
        {
            get
            {
                if(totalVolume == -1)
                    totalVolume = Plans.PlanList.Sum(d=> d.TotalVolume);
                return totalVolume;
            }

        }

        private double totalGreenTime = -1;
        public double TotalGreenTime
        {
            get
            {
                if(totalGreenTime == -1)
                {
                    totalGreenTime = Plans.PlanList.Sum(d => d.TotalGreenTime);
                }
                return totalGreenTime;
            }
        }

        private double totalYellowTime = -1;
        public double TotalYellowTime
        {
            get
            {
                if (totalYellowTime == -1)
                {
                    totalYellowTime = Plans.PlanList.Sum(d => d.TotalYellowTime);
                }
                return totalYellowTime;
            }
        }

        private double totalRedTime = -1;
        public double TotalRedTime
        {
            get
            {
                if (totalRedTime == -1)
                {
                    totalRedTime = Plans.PlanList.Sum(d => d.TotalRedTime);
                }
                return totalYellowTime;
            }
        }

        public double TotalTime
        {
            get
            {
                return Plans.PlanList.Sum(d => d.TotalTime);
            }
        }

        private DateTime startDate;
        private DateTime endDate;
        private bool showVolume;
        private int binSize;
        private Models.Approach approach;
        public Models.Approach Approach
        {
            get
            {
                return approach;
            }
        }
        private List<Models.Controller_Event_Log> cycleEvents { get; set; }
        private List<Models.Controller_Event_Log> preemptEvents { get; set; }


        /// <summary>
        /// Constructor for Signal phase
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="signalId"></param>
        /// <param name="phaseNumber"></param>
        /// <param name="region"></param>
        /// <param name="detChannel"></param>
        public SignalPhase(DateTime startDate, DateTime endDate, Models.Approach approach,
            bool showVolume, int binSize, int metricTypeID)
        {
            this.approach = approach;
            this.startDate = startDate;
            this.endDate = endDate;
            this.direction = approach.DirectionType.Description;
            this.showVolume = showVolume;
            this.binSize = binSize;
           

            if (!approach.IsProtectedPhaseOverlap)
            {
                GetSignalPhaseData(startDate, endDate, showVolume, binSize, metricTypeID);
            }
            else
            {
                GetSignalOverlapData(startDate, endDate, showVolume, binSize);
            }

        }

        public void LinkPivotAddSeconds(int seconds)
        {
            totalArrivalOnRed = -1;
            totalVolume = -1;
            totalGreenTime = -1;
            totalArrivalOnGreen = -1;
            this.volume = null;
            DateTime redLightTimeStamp = DateTime.MinValue;

            foreach (MOE.Common.Models.Controller_Event_Log row in detectorEvents)
            {
                row.Timestamp = row.Timestamp.AddSeconds(seconds);
            }

            plans.LinkPivotAddDetectorData(this.detectorEvents);
        }

        /// <summary>
        /// Gets the data for the green yellow and red lines as well as the 
        /// detector events
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="signalId"></param>
        /// <param name="eventData1"></param>
        /// <param name="region"></param>
        /// <param name="detChannel"></param>
        private void GetSignalPhaseData(DateTime startDate, DateTime endDate,
            bool showVolume, int binSize, int metricTypeID)
        {  

            MOE.Common.Models.Repositories.IControllerEventLogRepository celRepository =
                MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            this.cycleEvents = celRepository.GetEventsByEventCodesParam(approach.SignalID, startDate,
                endDate, new List<int>() { 1, 8, 10 }, approach.ProtectedPhaseNumber);
            this.preemptEvents = celRepository.GetSignalEventsByEventCodes(approach.SignalID, startDate,
                endDate, new List<int>() { 102 });

           
            
            this.detectorEvents = new List<Models.Controller_Event_Log>();
            var detectorsForMetric = approach.GetDetectorsForMetricType(metricTypeID);

            foreach (Models.Detector d in detectorsForMetric)
            {
                this.detectorEvents.AddRange(celRepository.GetEventsByEventCodesParamWithOffset(approach.SignalID, startDate,
                    endDate, new List<int> { 81 }, d.DetChannel, d.GetOffset()));

            }



            plans = new PlanCollection( cycleEvents, detectorEvents, startDate, endDate, approach, preemptEvents );

            if (plans.PlanList.Count == 0)
            {
                plans.AddItem(new Plan(startDate, endDate, 0, cycleEvents, detectorEvents, preemptEvents, approach));
            }

            if (showVolume)
            {
                SetVolume(detectorEvents, startDate, endDate, binSize);
            }

            
        }

        


        /// <summary>
        /// Gets the data for the green yellow and red lines as well as the 
        /// detector events for overlap phases
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="signalId"></param>
        /// <param name="eventData1"></param>
        /// <param name="region"></param>
        /// <param name="detChannel"></param>
        private void GetSignalOverlapData(DateTime startDate, DateTime endDate,
            bool showVolume, int binSize)
        {            
           
            MOE.Common.Models.Repositories.IControllerEventLogRepository celRepository =
                MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            this.cycleEvents = celRepository.GetEventsByEventCodesParam(approach.SignalID, startDate,
                endDate, new List<int>() { 61, 63, 64 }, approach.ProtectedPhaseNumber);
            this.preemptEvents = celRepository.GetSignalEventsByEventCodes(approach.SignalID, startDate,
                endDate, new List<int>() { 102 });

      
            this.detectorEvents = new List<Models.Controller_Event_Log>();
            foreach (Models.Detector d in approach.Detectors)
            {
                this.detectorEvents.AddRange(celRepository.GetEventsByEventCodesParam(approach.SignalID, startDate,
                    endDate, new List<int> { 81 }, d.DetChannel));
            }
            plans = new PlanCollection(cycleEvents, detectorEvents, startDate, endDate, approach, preemptEvents);

            if (plans.PlanList.Count == 0)
            {
                Plans.AddItem(new Plan(startDate, endDate, 0, cycleEvents, detectorEvents, preemptEvents, approach));
            }

            if (showVolume)
            {
                SetVolume(detectorEvents, startDate, endDate, binSize);
            }
        }

        private void SetVolume(List<Models.Controller_Event_Log> detectorEvents, DateTime startDate, DateTime endDate,
            int binSize)
        {
            volume = new VolumeCollection(startDate, endDate, detectorEvents, binSize);            
        }



        public List<Models.Controller_Event_Log> detectorEvents { get; set; }
    }
}
