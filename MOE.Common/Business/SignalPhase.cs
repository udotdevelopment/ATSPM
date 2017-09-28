using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;


namespace MOE.Common.Business
{
    public class SignalPhase
    {
        public string Direction { get; }

        public VolumeCollection Volume { get; private set; }

        public MOE.Common.Business.PlanCollection Plans { get; private set; }

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

        private double totalArrivalOnYellow = -1;
        public double TotalArrivalOnYellow
        {
            get
            {
                if (totalArrivalOnYellow == -1)
                    totalArrivalOnYellow = Plans.PlanList.Sum(d => d.TotalArrivalOnYellow);
                return totalArrivalOnYellow;
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
                return totalRedTime;
            }
        }

        public double TotalTime
        {
            get
            {
                return Plans.PlanList.Sum(d => d.TotalTime);
            }
        }

        public bool IsPermissive { get; }
        public Models.Approach Approach { get; }
        private List<Models.Controller_Event_Log> cycleEvents { get; set; }
        private List<Models.Controller_Event_Log> preemptEvents { get; set; }

        
        public SignalPhase(DateTime startDate, DateTime endDate, Models.Approach approach,
            bool showVolume, int binSize, int metricTypeId, bool isPermissive)
        {
            this.Approach = approach;
            if (approach.DirectionType != null) this.Direction = approach.DirectionType.Description;
            IsPermissive = isPermissive;
            if (!approach.IsProtectedPhaseOverlap)
            {
                GetSignalPhaseData(startDate, endDate, showVolume, binSize, metricTypeId);
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
            this.Volume = null;
            foreach (MOE.Common.Models.Controller_Event_Log row in detectorEvents)
            {
                row.Timestamp = row.Timestamp.AddSeconds(seconds);
            }
            Plans.LinkPivotAddDetectorData(this.detectorEvents);
        }

        /// <summary>
        /// Gets the data for the green yellow and red lines as well as the 
        /// detector events
        /// </summary>
        private void GetSignalPhaseData(DateTime startDate, DateTime endDate,
            bool showVolume, int binSize, int metricTypeId)
        {  

            MOE.Common.Models.Repositories.IControllerEventLogRepository celRepository =
                MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            if (IsPermissive)
            {
                if (Approach.PermissivePhaseNumber != null && Approach.PermissivePhaseNumber > 0)
                    cycleEvents = celRepository.GetEventsByEventCodesParam(Approach.SignalID, startDate,
                        endDate, new List<int>() {1, 8, 10}, Approach.PermissivePhaseNumber.Value);
            }
            else
            {
                cycleEvents = celRepository.GetEventsByEventCodesParam(Approach.SignalID, startDate,
                endDate, new List<int>() { 1, 8, 10 }, Approach.ProtectedPhaseNumber);
            }
            
            this.preemptEvents = celRepository.GetSignalEventsByEventCodes(Approach.SignalID, startDate,
                endDate, new List<int>() { 102 });

           
            
            this.detectorEvents = new List<Models.Controller_Event_Log>();
            var detectorsForMetric = Approach.GetDetectorsForMetricType(metricTypeId);

            foreach (Models.Detector d in detectorsForMetric)
            {
                this.detectorEvents.AddRange(celRepository.GetEventsByEventCodesParamWithOffset(Approach.SignalID, startDate,
                    endDate, new List<int> { 81 }, d.DetChannel, d.GetOffset()));

            }



            Plans = new PlanCollection( cycleEvents, detectorEvents, startDate, endDate, Approach, preemptEvents );

            if (Plans.PlanList.Count == 0)
            {
                Plans.AddItem(new Plan(startDate, endDate, 0, cycleEvents, detectorEvents, preemptEvents, Approach));
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
            this.cycleEvents = celRepository.GetEventsByEventCodesParam(Approach.SignalID, startDate,
                endDate, new List<int>() { 61, 63, 64 }, Approach.ProtectedPhaseNumber);
            this.preemptEvents = celRepository.GetSignalEventsByEventCodes(Approach.SignalID, startDate,
                endDate, new List<int>() { 102 });

      
            this.detectorEvents = new List<Models.Controller_Event_Log>();
            foreach (Models.Detector d in Approach.Detectors)
            {
                this.detectorEvents.AddRange(celRepository.GetEventsByEventCodesParam(Approach.SignalID, startDate,
                    endDate, new List<int> { 81 }, d.DetChannel));
            }
            Plans = new PlanCollection(cycleEvents, detectorEvents, startDate, endDate, Approach, preemptEvents);

            if (Plans.PlanList.Count == 0)
            {
                Plans.AddItem(new Plan(startDate, endDate, 0, cycleEvents, detectorEvents, preemptEvents, Approach));
            }

            if (showVolume)
            {
                SetVolume(detectorEvents, startDate, endDate, binSize);
            }
        }

        private void SetVolume(List<Models.Controller_Event_Log> detectorEvents, DateTime startDate, DateTime endDate,
            int binSize)
        {
            Volume = new VolumeCollection(startDate, endDate, detectorEvents, binSize);            
        }



        public List<Models.Controller_Event_Log> detectorEvents { get; set; }
    }
}
