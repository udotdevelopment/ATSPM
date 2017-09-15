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
        //Xaxis is time
        public DateTime XAxis { get; set; }

        public DateTime TotalMph { get; set; }

        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public int AvgSpeed { get; }

        public int EightyFifth { get; }

        public int FifteenthPercentile { get; }

        public int MinSpeedFilter { get; }

        public int MovementDelay { get; }

        public int SummedSpeed { get; }
        public int SpeedVolume { get; }

        private int _binSizeMultiplier;

        public AvgSpeedBucket(DateTime startTime, DateTime endTime, List<Cycle> cycleCollection, int binSize, int minspeedfilter, int movementdelay)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.XAxis = endTime;
            this._binSizeMultiplier = 60 / binSize;
            this.MinSpeedFilter = minspeedfilter;
            this.MovementDelay = movementdelay;
            Cycle cycle = cycleCollection.FirstOrDefault(c => c.StartTime > this.StartTime && c.EndTime < this.EndTime);
            if (cycle != null && cycle.SpeedsForCycle.Count > 0)
            {
                SpeedVolume = cycle.SpeedsForCycle.Count();
                cycle.SpeedsForCycle = cycle.SpeedsForCycle.OrderBy(s => s.MPH).ToList();
                SummedSpeed = cycle.SpeedsForCycle.Sum(s => s.MPH);
                AvgSpeed = Convert.ToInt32(Math.Round(cycle.SpeedsForCycle.Average(s => s.MPH)));
                EightyFifth = GetPercentile(cycle, .85);
                FifteenthPercentile = GetPercentile(cycle, .15);
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

        private int GetPercentile(Cycle cycle, double percentile)
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
                        percentileValue = cycle.SpeedsForCycle[percentileIndex].MPH;
                    }
                    else
                    {
                        percentileIndex = Convert.ToInt32(tempPercentileIndex);
                        int speed1 = cycle.SpeedsForCycle[percentileIndex].MPH;
                        int speed2 = cycle.SpeedsForCycle[percentileIndex + 1].MPH;
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

        //public Tuple<int, int> AddspeedHitToAverage(List<Cycle> cycleCollection)
        //{
        //    int averageSpeed;
        //    int totalSpeed = 0;
        //    int totalHits = 0;
        //    int eightyfifth = 0;
        //    int eighyFiveIndexInt = 0;
        //    List<int> speeds = new List<int>();
        //    var cycle = cycleCollection.FirstOrDefault(c => c.StartTime > this.StartTime &&
        //                                               c.EndTime < this.EndTime);


        //    foreach (var cy in query)
        //    {
        //        foreach (Models.Speed_Events sh in cy.SpeedsForCycle)
        //        {

        //                totalSpeed = totalSpeed + sh.MPH;
        //                speeds.Add(sh.MPH);
        //                totalHits++;
        //        }
        //    }

        //    if (totalHits > 0)
        //    {
        //        double rawAverageSpeed = (totalSpeed / totalHits);
        //        averageSpeed = Convert.ToInt32( Math.Round(rawAverageSpeed));
                
        //        double eighyFiveIndex =  ((totalHits * .85) + .5);
        //        speeds.Sort();
        //        if (speeds.Count > 3)
        //        {
        //            if ((eighyFiveIndex % 1) == 0)
        //            {
        //                eighyFiveIndexInt = Convert.ToInt16(eighyFiveIndex);
        //                eightyfifth = speeds.ElementAt(eighyFiveIndexInt - 2);
        //            }
        //            else
        //            {
        //                double indexMod = (eighyFiveIndex % 1);
        //                eighyFiveIndexInt = Convert.ToInt16(eighyFiveIndex);
        //                int speed1 = speeds.ElementAt(eighyFiveIndexInt - 2);
        //                int speed2 = speeds.ElementAt(eighyFiveIndexInt - 1);
        //                double rawEightyfifth = (1 - indexMod) * speed1 + indexMod * speed2;
        //                eightyfifth = Convert.ToInt32(Math.Round(rawEightyfifth));
        //            }
        //        }
        //    }
        //    else
        //    {
        //        averageSpeed = 0;
        //        eightyfifth = 0;
        //    }

        //    return new Tuple<int, int>(averageSpeed, eightyfifth);

        //        }

        

            }


        }
