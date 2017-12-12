using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MOE.Common.Business
{
    public class PlanSpeed:Plan
    {
        public int EightyFifth { get; set; }

        public int AvgSpeed { get; set; }

        public int StdDevAvgSpeed { get; set; }
        
        public PlanSpeed(DateTime start, DateTime end, int planNumber, List<CycleSpeed> cycles) : base(start, end, planNumber)
        {
            SetSpeedStatistics(cycles);
        }
       

        public void SetSpeedStatistics(List<CycleSpeed> cycles)
        {
            List<int> rawSpeeds = new List<int>();
            var cyclesForPlan = cycles.Where(c => c.StartTime >= StartTime && c.StartTime < EndTime);
            foreach (CycleSpeed cycle in cyclesForPlan)
            {
                rawSpeeds.AddRange(cycle.SpeedEvents.Select(s => s.MPH));
            }

            //find stddev of average
            if (rawSpeeds.Count > 0)
            {
                double rawaverage = rawSpeeds.Average();
                AvgSpeed = Convert.ToInt32(Math.Round(rawaverage));
                StdDevAvgSpeed = Convert.ToInt32(Math.Round(Math.Sqrt(rawSpeeds.Average(v => Math.Pow(v - rawaverage, 2)))));
            }
            EightyFifth = GetPercentile(rawSpeeds, .85);
        }

        private int GetPercentile(List<int> speeds, double percentile)
        {
            speeds.Sort();
            int percentileValue = 0;
            try
            {
                double tempPercentileIndex = (speeds.Count * percentile) - 1;

                if (speeds.Count > 3)
                {
                    var percentileIndex = 0;
                    if ((tempPercentileIndex % 1) > 0)
                    {
                        percentileIndex = Convert.ToInt32(Math.Round(tempPercentileIndex + .5));
                        percentileValue = speeds[percentileIndex];
                    }
                    else
                    {
                        percentileIndex = Convert.ToInt32(tempPercentileIndex);
                        int speed1 = speeds[percentileIndex];
                        int speed2 = speeds[percentileIndex + 1];
                        double rawEightyfifth = (speed1 + speed2) / 2;
                        percentileValue = Convert.ToInt32(Math.Round(rawEightyfifth));
                    }
                }
            }
            catch (Exception e)
            {
                var errorLog = Models.Repositories.ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                    this.GetType().ToString(), e.TargetSite.ToString(), Models.ApplicationEvent.SeverityLevels.High, e.Message);
                throw new Exception("Error creating Percentile");
            }
            return percentileValue;
        }

    }
}
