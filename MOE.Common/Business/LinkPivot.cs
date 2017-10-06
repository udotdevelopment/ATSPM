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
        public  LinkPivot(int routeId, DateTime startDate, DateTime endDate, int cycleTime, string chartLocation,
            string direction, double bias, string biasDirection, List<DayOfWeek> days)
        {
            MOE.Common.Models.SPM db = new Models.SPM();
            //TODO:Fix for Routes
            //var _ApproachRouteDetail = (from ard in db.RouteSignals
            //                            .Include("Approach")
            //                            where ard.ApproachRouteId == routeId
            //                            orderby ard.Order
            //                            select ard).ToList();



            // Get a list of dates that matches the parameters passed by the user
            dates = GetDates(startDate, endDate, days);

            //Make a list of numbers to use as indices to perform parallelism 
            List<int> indices = new List<int>();
            if (direction == "Upstream")
            {
                //TODO:Fix for Routes
                //for (int i = _ApproachRouteDetail.Count - 1; i > 0; i--)
                //{
                //    indices.Add(i);
                //}
                //Parallel.ForEach(indices, i =>
                foreach (int i in indices)
                {
                    //TODO: Fix for new routes
                    //pairedApproaches.Add(new LinkPivotPair(_ApproachRouteDetail[i].SignalId, GetOppositeDirection(_ApproachRouteDetail[i].DirectionType1),
                    //        _ApproachRouteDetail[i].Signal.PrimaryName + " " + _ApproachRouteDetail[i].Signal.SecondaryName, _ApproachRouteDetail[i - 1].SignalId,
                    //        GetOppositeDirection(_ApproachRouteDetail[i - 1].DirectionType1), _ApproachRouteDetail[i - 1].Signal.PrimaryName + " " + _ApproachRouteDetail[i - 1].Signal.SecondaryName,
                    //        startDate, endDate, cycleTime, chartLocation, bias, biasDirection, dates, i + 1));
                }
                //);
            }
            else
            {
                //TODO:Fix for Routes
                //for (int i = 0; i < _ApproachRouteDetail.Count - 1; i++)
                //{
                //    indices.Add(i);
                //}
                //Parallel.ForEach(indices, i =>
                foreach (int i in indices)
                {
                    //TODO: Fix for new routes
                    //pairedApproaches.Add(new LinkPivotPair(_ApproachRouteDetail[i].SignalId, _ApproachRouteDetail[i].DirectionType1.Description,
                    //        _ApproachRouteDetail[i].Signal.PrimaryName + " " + _ApproachRouteDetail[i].Signal.SecondaryName, _ApproachRouteDetail[i + 1].SignalId, _ApproachRouteDetail[i + 1].DirectionType1.Description,
                    //        _ApproachRouteDetail[i+1].Signal.PrimaryName + " " + _ApproachRouteDetail[i+1].Signal.SecondaryName, startDate, endDate, cycleTime, chartLocation, bias, biasDirection, dates, i+1));
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
            //       pairedApproaches.Add(new LinkPivotPair(detailTable[i].SignalId, detailTable[i].Direction,
            //           detailTable[i].Location, detailTable[i - 1].SignalId, detailTable[i - 1].Direction,
            //           detailTable[i - 1].Location, startDate, endDate, cycleTime, chartLocation, bias, biasDirection, dates));
            //   }
            //}
            //else
            //{
            //    //build a LinkPivotPair list as ordered in the database
            //    for (int i = 0; i < detailTable.Rows.Count - 1; i++)
            //    {
            //        pairedApproaches.Add(new LinkPivotPair(detailTable[i].SignalId, detailTable[i].Direction,
            //            detailTable[i].Location, detailTable[i + 1].SignalId, detailTable[i + 1].Direction,
            //            detailTable[i + 1].Location, startDate, endDate, cycleTime, chartLocation, bias, biasDirection, dates));
            //    }
            //}

            //Cycle through the LinkPivotPair list and add the statistics to the LinkPivotadjustmentTable
            foreach (int i in indices)
            {
                //Make sure the list is in the correct order after parrallel processing
                //TODO:Fix for Routes
                //var lpps = from pair in pairedApproaches
                //           where pair.SignalId == _ApproachRouteDetail[i].SignalId
                //          select pair;

                //foreach (var lpp in lpps)
                //{
                //    adjustment.AddLinkPivotAdjustmentRow(lpp.SignalId, Convert.ToInt32(lpp.SecondsAdded), 0,
                //        lpp.PAOGUpstreamBefore, lpp.PAOGDownstreamBefore, lpp.AOGUpstreamBefore, lpp.AOGDownstreamBefore,
                //        lpp.PAOGUpstreamPredicted, lpp.PAOGDownstreamPredicted, lpp.AOGUpstreamPredicted, lpp.AOGDownstreamPredicted,
                //        lpp.Location, lpp.DownSignalId, lpp.DownstreamApproachDirection, lpp.UpstreamApproachDirection,
                //        lpp.ResultChartLocation, lpp.DownstreamLocation, lpp.AOGTotalBefore, lpp.PAOGTotalBefore,
                //        lpp.AOGTotalPredicted, lpp.PAOGTotalPredicted, lpp.LinkNumber,lpp.TotalVolumeDownstream,
                //        lpp.TotalVolumeUpstream);
                //}
            }

            //Set the end row to have zero for the ajustments. No adjustment can be made because 
            //downstream is unknown. The end row is determined by the starting point seleceted by the user
            if (direction == "Upstream")
            {
                //TODO: Fix for new routes
                //End row for upstream is index 0
                //adjustment.AddLinkPivotAdjustmentRow(_ApproachRouteDetail[0].SignalId, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, _ApproachRouteDetail[0].Signal.PrimaryName + " " + _ApproachRouteDetail[0].Signal.SecondaryName,
                //    "","","","","",0,0,0,0,1,0,0);
            }
            else
            {
                //TODO: Fix for new routes
                //End row for downstream is last row in the detail table
                //adjustment.AddLinkPivotAdjustmentRow(_ApproachRouteDetail[_ApproachRouteDetail.Count - 1].SignalId, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                //    _ApproachRouteDetail[_ApproachRouteDetail.Count - 1].Signal.PrimaryName + " " + _ApproachRouteDetail[_ApproachRouteDetail.Count - 1].Signal.SecondaryName, "", "", "", "", "", 0, 0, 0, 0, _ApproachRouteDetail.Count, 0, 0);
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
