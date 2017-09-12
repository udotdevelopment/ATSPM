using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.Speed
{
    public class DetectorSpeed
    {
        public double TotalDetectorHits { get; set; }
        public double TotalOnGreenArrivals { get; set; }
        public double PercentArrivalOnGreen { get; set; }

        private readonly ISpeedEventRepository _speedEventRepository = SpeedEventRepositoryFactory.Create();
        private readonly IControllerEventLogRepository _controllerEventLogRepository = ControllerEventLogRepositoryFactory.Create();

        public DetectorSpeed(Models.Detector detector, DateTime startDate, DateTime endDate)
        {
            List<Models.Controller_Event_Log> phaseevents = _controllerEventLogRepository.GetEventsByEventCodesParam(detector.Approach.SignalID, startDate, endDate, new List<int>() { 0, 1, 7, 8, 9, 10, 11 }, detector.Approach.ProtectedPhaseNumber);
            List<Models.Controller_Event_Log> detEvents = new List<Controller_Event_Log>();
            List<Models.Controller_Event_Log> preemptEvents = new List<Controller_Event_Log>();
            var speedEvents = _speedEventRepository.GetSpeedEventsByDetector(startDate, endDate, detector);
            PlanCollection plans = new PlanCollection(phaseevents, detEvents, startDate, endDate, detector.Approach, preemptEvents);
            foreach (MOE.Common.Business.Plan plan in plans.PlanList)
            {
                foreach (Cycle c in plan.CycleCollection)
                {
                    c.FindSpeedEventsForCycle(speedEvents);
                }
                plan.AvgSpeedBucketCollection = new AvgSpeedBucketCollection(plan.StartTime, plan.EndTime, plan.CycleCollection, binSize, detector.MinSpeedFilter ?? 5, detector.MovementDelay ?? 0);
                if (plan.AvgSpeedBucketCollection.Items.Count > 0)
                {
                    foreach (MOE.Common.Business.AvgSpeedBucket bucket in plan.AvgSpeedBucketCollection.Items)
                    {
                        chart.Series["Average MPH"].Points.AddXY(bucket.StartTime, bucket.AvgSpeed);
                        chart.Series["85th Percentile Speed"].Points.AddXY(bucket.StartTime, bucket.EightyFifth);
                        if (ShowPlanStatistics && ShowPostedSpeed)
                        {
                            chart.Series["Posted Speed"].Points.AddXY(bucket.StartTime, detector.Approach.MPH);
                        }
                    }
                }
            }


            //if arrivals on green is selected add the data to the chart
            if (ShowPlanStatistics)
            {
                if (totalDetectorHits > 0)
                {
                    percentArrivalOnGreen = (totalOnGreenArrivals / totalDetectorHits) * 100;
                }
                else
                {
                    percentArrivalOnGreen = 0;
                }
                SetSpeedPlanStrips(plans, chart, startDate, detector.MinSpeedFilter ?? 0);
            }
        }
    }
}
