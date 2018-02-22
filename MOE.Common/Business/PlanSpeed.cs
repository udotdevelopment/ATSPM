using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public class PlanSpeed : Plan
    {
        public PlanSpeed(DateTime start, DateTime end, int planNumber, List<CycleSpeed> cycles) : base(start, end,
            planNumber)
        {
            SetSpeedStatistics(cycles);
        }

        public int EightyFifth { get; set; }
        public int Fifteenth { get; set; }

        public int AvgSpeed { get; set; }

        public int StdDevAvgSpeed { get; set; }


        public void SetSpeedStatistics(List<CycleSpeed> cycles)
        {
            var rawSpeeds = new List<int>();
            var cyclesForPlan = cycles.Where(c => c.StartTime >= StartTime && c.StartTime < EndTime);
            foreach (var cycle in cyclesForPlan)
                rawSpeeds.AddRange(cycle.SpeedEvents.Select(s => s.MPH));

            //find stddev of average
            if (rawSpeeds.Count > 0)
            {
                var rawaverage = rawSpeeds.Average();
                AvgSpeed = Convert.ToInt32(Math.Round(rawaverage));
                StdDevAvgSpeed =
                    Convert.ToInt32(Math.Round(Math.Sqrt(rawSpeeds.Average(v => Math.Pow(v - rawaverage, 2)))));
            }
            EightyFifth = GetPercentile(rawSpeeds, .85);
            Fifteenth = GetPercentile(rawSpeeds, .15);
        }

        private int GetPercentile(List<int> speeds, double percentile)
        {
            speeds.Sort();
            var percentileValue = 0;
            try
            {
                var tempPercentileIndex = speeds.Count * percentile - 1;

                if (speeds.Count > 3)
                {
                    var percentileIndex = 0;
                    if (tempPercentileIndex % 1 > 0)
                    {
                        percentileIndex = Convert.ToInt32(Math.Round(tempPercentileIndex + .5));
                        percentileValue = speeds[percentileIndex];
                    }
                    else
                    {
                        percentileIndex = Convert.ToInt32(tempPercentileIndex);
                        var speed1 = speeds[percentileIndex];
                        var speed2 = speeds[percentileIndex + 1];
                        double rawEightyfifth = (speed1 + speed2) / 2;
                        percentileValue = Convert.ToInt32(Math.Round(rawEightyfifth));
                    }
                }
            }
            catch (Exception e)
            {
                var errorLog = ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(Assembly.GetExecutingAssembly().GetName().ToString(),
                    GetType().ToString(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                throw new Exception("Error creating Percentile");
            }
            return percentileValue;
        }
    }
}