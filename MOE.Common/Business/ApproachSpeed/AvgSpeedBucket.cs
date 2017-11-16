using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public class AvgSpeedBucket
    {
        public DateTime XAxis { get; set; }

        public DateTime TotalMph { get; set; }

        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public int AvgSpeed { get; }

        public int EightyFifth { get; }

        public int FifteenthPercentile { get; }
        

        public int MovementDelay { get; }

        public int SummedSpeed { get; }
        public int SpeedVolume { get; }

        private int _binSizeMultiplier;

        public AvgSpeedBucket(DateTime startTime, DateTime endTime, int binSize, int movementdelay, 
            List<CycleSpeed> cycles)
        {
            StartTime = startTime;
            EndTime = endTime;
            XAxis = endTime;
            _binSizeMultiplier = 60 / binSize;
            MovementDelay = movementdelay;
            List<int> speedsForBucket = new List<int>();
            foreach (CycleSpeed cycle in cycles)
            {
                speedsForBucket.AddRange(cycle.SpeedEvents.Select(s => s.MPH));
            }
            if (speedsForBucket.Count > 0)
            {
                speedsForBucket.Sort();
                SpeedVolume = speedsForBucket.Count();
                SummedSpeed = speedsForBucket.Sum();
                AvgSpeed = Convert.ToInt32(Math.Round(speedsForBucket.Average()));
                EightyFifth = GetPercentile(speedsForBucket, .85);
                FifteenthPercentile = GetPercentile(speedsForBucket, .15);
            }
            else
            {
                SpeedVolume = 0;
                SummedSpeed = 0;
                AvgSpeed = 0;
                EightyFifth = 0;
                FifteenthPercentile = 0;
            }
        }

        private int GetPercentile(List<int> speeds, double percentile)
        {
            int percentileValue = 0;
            try
            {
                double tempPercentileIndex = (SpeedVolume * percentile)-1;

                if (SpeedVolume > 3)
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
                var errorLog = ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                    this.GetType().ToString(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message);
                throw new Exception("Error creating Percentile");
            }
            return percentileValue;
        }

        
        

            }


        }
