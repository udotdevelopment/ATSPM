using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MOE.Common.Business
{
    public class LinkPivot
    {
        private List<LinkPivotPair> pairedApproaches = new List<LinkPivotPair>();
        public List<LinkPivotPair> PairedApproaches
        {
            get { return pairedApproaches; }
        }

        private Data.LinkPivot.LinkPivotAdjustmentDataTable adjustment = 
            new Data.LinkPivot.LinkPivotAdjustmentDataTable();
        public Data.LinkPivot.LinkPivotAdjustmentDataTable Adjustment
        {
            get { return adjustment; }
        }

        private List<DateTime> dates = new List<DateTime>();
        public List<DateTime> Dates
        {
            get { return dates; }
        }

        /// <summary>
        /// Creates a list of LinkPivotPair objects and exposes a LinkPivotAdjustment Table
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="cycleTime"></param>
        /// <param name="chartLocation"></param>
        /// <param name="direction"></param>
        /// <param name="bias"></param>
        /// <param name="biasDirection"></param>
        /// <param name="days"></param>
        public LinkPivot(int routeId, DateTime startDate, DateTime endDate, int cycleTime, string chartLocation,
            string direction, double bias, string biasDirection, List<DayOfWeek> days)
        {
            MOE.Common.Models.SPM db = new Models.SPM();
            var _ApproachRouteDetail = (from ard in db.ApproachRouteDetails
                                        .Include("Approach")
                                        where ard.ApproachRouteId == routeId
                                        orderby ard.ApproachOrder
                                        select ard).ToList();



            // Get a list of dates that matches the parameters passed by the user
            dates = GetDates(startDate, endDate, days);

            //Make a list of numbers to use as indices to perform parallelism 
            List<int> indices = new List<int>();
            if (direction == "Upstream")
            {
                for (int i = _ApproachRouteDetail.Count - 1; i > 0; i--)
                {
                    indices.Add(i);
                }
                //Parallel.ForEach(indices, i =>
                foreach(int i in indices)
                {
                    pairedApproaches.Add(new LinkPivotPair(_ApproachRouteDetail[i].Approach.SignalID, GetOppositeDirection(_ApproachRouteDetail[i].Approach.DirectionType),
                            _ApproachRouteDetail[i].Approach.Signal.PrimaryName + " " + _ApproachRouteDetail[i].Approach.Signal.SecondaryName, _ApproachRouteDetail[i - 1].Approach.SignalID,
                            GetOppositeDirection(_ApproachRouteDetail[i - 1].Approach.DirectionType), _ApproachRouteDetail[i - 1].Approach.Signal.PrimaryName + " " + _ApproachRouteDetail[i - 1].Approach.Signal.SecondaryName,
                            startDate, endDate, cycleTime, chartLocation, bias, biasDirection, dates, i + 1));
                }
                //);
            }
            else
            {
                for (int i = 0; i < _ApproachRouteDetail.Count - 1; i++)
                {
                    indices.Add(i);
                }
                //Parallel.ForEach(indices, i =>
                foreach(int i in indices)
                {
                    pairedApproaches.Add(new LinkPivotPair(_ApproachRouteDetail[i].Approach.SignalID, _ApproachRouteDetail[i].Approach.DirectionType.Description,
                            _ApproachRouteDetail[i].Approach.Signal.PrimaryName + " " + _ApproachRouteDetail[i].Approach.Signal.SecondaryName, _ApproachRouteDetail[i + 1].Approach.SignalID, _ApproachRouteDetail[i + 1].Approach.DirectionType.Description,
                            _ApproachRouteDetail[i+1].Approach.Signal.PrimaryName + " " + _ApproachRouteDetail[i+1].Approach.Signal.SecondaryName, startDate, endDate, cycleTime, chartLocation, bias, biasDirection, dates, i+1));
                }
                //);
            }

            

            ////If the user has selected the end of the list create a reverse sort the LinkPivotPair list
            ////otherwise build the list as ordered in the database
            //if (direction == "Upstream")
            //{
            //    //build a reverse sort LinkPivotPair list
            //   for (int i = detailTable.Rows.Count-1; i > 0; i--)
            //   {
            //       pairedApproaches.Add(new LinkPivotPair(detailTable[i].SignalID, detailTable[i].Direction,
            //           detailTable[i].Location, detailTable[i - 1].SignalID, detailTable[i - 1].Direction,
            //           detailTable[i - 1].Location, startDate, endDate, cycleTime, chartLocation, bias, biasDirection, dates));
            //   }
            //}
            //else
            //{
            //    //build a LinkPivotPair list as ordered in the database
            //    for (int i = 0; i < detailTable.Rows.Count - 1; i++)
            //    {
            //        pairedApproaches.Add(new LinkPivotPair(detailTable[i].SignalID, detailTable[i].Direction,
            //            detailTable[i].Location, detailTable[i + 1].SignalID, detailTable[i + 1].Direction,
            //            detailTable[i + 1].Location, startDate, endDate, cycleTime, chartLocation, bias, biasDirection, dates));
            //    }
            //}

            //Cycle through the LinkPivotPair list and add the statistics to the LinkPivotadjustmentTable
            foreach (int i in indices)
            {
                //Make sure the list is in the correct order after parrallel processing
                var lpps = from pair in pairedApproaches
                           where pair.SignalId == _ApproachRouteDetail[i].Approach.SignalID
                          select pair;

                foreach (var lpp in lpps)
                {
                    adjustment.AddLinkPivotAdjustmentRow(lpp.SignalId, Convert.ToInt32(lpp.SecondsAdded), 0,
                        lpp.PAOGUpstreamBefore, lpp.PAOGDownstreamBefore, lpp.AOGUpstreamBefore, lpp.AOGDownstreamBefore,
                        lpp.PAOGUpstreamPredicted, lpp.PAOGDownstreamPredicted, lpp.AOGUpstreamPredicted, lpp.AOGDownstreamPredicted,
                        lpp.Location, lpp.DownSignalId, lpp.DownstreamApproachDirection, lpp.UpstreamApproachDirection,
                        lpp.ResultChartLocation, lpp.DownstreamLocation, lpp.AOGTotalBefore, lpp.PAOGTotalBefore,
                        lpp.AOGTotalPredicted, lpp.PAOGTotalPredicted, lpp.LinkNumber,lpp.TotalVolumeDownstream,
                        lpp.TotalVolumeUpstream);
                }
            }

            //Set the end row to have zero for the ajustments. No adjustment can be made because 
            //downstream is unknown. The end row is determined by the starting point seleceted by the user
            if (direction == "Upstream")
            {
                //End row for upstream is index 0
                adjustment.AddLinkPivotAdjustmentRow(_ApproachRouteDetail[0].Approach.SignalID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, _ApproachRouteDetail[0].Approach.Signal.PrimaryName + " " + _ApproachRouteDetail[0].Approach.Signal.SecondaryName,
                    "","","","","",0,0,0,0,1,0,0);
            }
            else
            {
                //End row for downstream is last row in the detail table
                adjustment.AddLinkPivotAdjustmentRow(_ApproachRouteDetail[_ApproachRouteDetail.Count - 1].Approach.SignalID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                    _ApproachRouteDetail[_ApproachRouteDetail.Count - 1].Approach.Signal.PrimaryName + " " + _ApproachRouteDetail[_ApproachRouteDetail.Count - 1].Approach.Signal.SecondaryName, "", "", "", "", "", 0, 0, 0, 0, _ApproachRouteDetail.Count, 0, 0);
            }

            int cumulativeChange = 0;

            //Determine the adjustment by adding the previous rows adjustment to the current rows delta
            for (int i = adjustment.Count - 1; i >= 0; i--)
            {
                //if the new adjustment is greater than the cycle time than the adjustment should subtract
                // the cycle time from the current adjustment and the result should be the new adjustment
                if (cumulativeChange + adjustment[i].Delta > cycleTime)
                {
                    adjustment[i].Adjustment = cumulativeChange + adjustment[i].Delta - cycleTime;
                    cumulativeChange = cumulativeChange + adjustment[i].Delta - cycleTime;
                }
                else
                {
                    adjustment[i].Adjustment = cumulativeChange + adjustment[i].Delta;
                    cumulativeChange = cumulativeChange + adjustment[i].Delta;
                }
            }

        }

        private string GetOppositeDirection(MOE.Common.Models.DirectionType direction)
        {
            string oppositeDirection = string.Empty;
            if (direction.Description.ToUpper() == "Northbound".ToUpper())
            {
                oppositeDirection = "Southbound";
            }
            else if (direction.Description.ToUpper() == "Southbound".ToUpper())
            {
                oppositeDirection = "Northbound";
            }
            else if (direction.Description.ToUpper() == "Eastbound".ToUpper())
            {
                oppositeDirection = "Westbound";
            }
            else if (direction.Description.ToUpper() == "Westbound".ToUpper())
            {
                oppositeDirection = "Eastbound";
            }

            return oppositeDirection;
        }

        /// <summary>
        /// Based on a start date and an end date find each date within that 
        /// matches the day type ie Monday, Tuesday etc.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static List<DateTime> GetDates(DateTime startDate, DateTime endDate, List<DayOfWeek> days)
        {

            //Find each day in the given period that matches one of the specified day types and add it to the return list
            List<DateTime> dates = new List<DateTime>();
            for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                if (days.Contains(dt.DayOfWeek))
                {
                    dates.Add(dt);
                }
            }
            return dates;
        }
    }
}
