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
        public List<LinkPivotPair> PairedApproaches { get; } = new List<LinkPivotPair>();
        public Data.LinkPivot.LinkPivotAdjustmentDataTable Adjustment { get; } = new Data.LinkPivot.LinkPivotAdjustmentDataTable();
        public List<DateTime> Dates { get; } = new List<DateTime>();

        public  LinkPivot(int routeId, DateTime startDate, DateTime endDate, int cycleTime, string chartLocation,
            string direction, double bias, string biasDirection, List<DayOfWeek> days)
        {
            var routeRepository = Models.Repositories.RouteRepositoryFactory.Create();
            var route = routeRepository.GetRouteByIDAndDate(routeId, startDate);
            // Get a list of dates that matches the parameters passed by the user
            Dates = GetDates(startDate, endDate, days);
            //Make a list of numbers to use as indices to perform parallelism 
            List<int> indices = new List<int>();
            if (direction == "Upstream")
            {
                for (int i = route.RouteSignals.Count - 1; i > 0; i--)
                {
                    indices.Add(i);
                }
                //Parallel.ForEach(indices, i =>
                foreach (int i in indices)
                {
                    var signalRepository = Models.Repositories.SignalsRepositoryFactory.Create();
                    var signal = signalRepository.GetVersionOfSignalByDate(route.RouteSignals[i].SignalId, startDate);
                    var primaryPhaseDirection = route.RouteSignals[i].PhaseDirections.FirstOrDefault(p => p.IsPrimaryApproach);
                    var downstreamPrimaryPhaseDirection = route.RouteSignals[i-1].PhaseDirections.FirstOrDefault(p => p.IsPrimaryApproach == false);
                    var dowstreamSignal = signalRepository.GetVersionOfSignalByDate(route.RouteSignals[i-1].SignalId, startDate);
                    var approach = signal.Approaches.FirstOrDefault(a =>
                        a.DirectionTypeID == primaryPhaseDirection.DirectionTypeId &&
                        a.IsProtectedPhaseOverlap == primaryPhaseDirection.IsOverlap &&
                        a.ProtectedPhaseNumber == primaryPhaseDirection.Phase);
                    var downstreamApproach = dowstreamSignal.Approaches.FirstOrDefault(a =>
                        a.DirectionTypeID == downstreamPrimaryPhaseDirection.DirectionTypeId &&
                        a.IsProtectedPhaseOverlap == downstreamPrimaryPhaseDirection.IsOverlap &&
                        a.ProtectedPhaseNumber == downstreamPrimaryPhaseDirection.Phase);
                    PairedApproaches.Add(new LinkPivotPair(approach, downstreamApproach, startDate, endDate, cycleTime, chartLocation, bias, 
                        biasDirection, Dates, i + 1));
                }
                //);
            }
            else
            {
                for (int i = 0; i < route.RouteSignals.Count - 1; i++)
                {
                    indices.Add(i);
                }
                //Parallel.ForEach(indices, i =>
                foreach (int i in indices)
                {
                    var signalRepository = Models.Repositories.SignalsRepositoryFactory.Create();
                    var signal = signalRepository.GetVersionOfSignalByDate(route.RouteSignals[i].SignalId, startDate);
                    var primaryPhaseDirection = route.RouteSignals[i].PhaseDirections.FirstOrDefault(p => p.IsPrimaryApproach);
                    var downstreamPrimaryPhaseDirection = route.RouteSignals[i + 1].PhaseDirections.FirstOrDefault(p => p.IsPrimaryApproach == false);
                    var dowstreamSignal = signalRepository.GetVersionOfSignalByDate(route.RouteSignals[i + 1].SignalId, startDate);
                    var approach = signal.Approaches.FirstOrDefault(a =>
                        a.DirectionTypeID == primaryPhaseDirection.DirectionTypeId &&
                        a.IsProtectedPhaseOverlap == primaryPhaseDirection.IsOverlap &&
                        a.ProtectedPhaseNumber == primaryPhaseDirection.Phase);
                    var downstreamApproach = dowstreamSignal.Approaches.FirstOrDefault(a =>
                        a.DirectionTypeID == downstreamPrimaryPhaseDirection.DirectionTypeId &&
                        a.IsProtectedPhaseOverlap == downstreamPrimaryPhaseDirection.IsOverlap &&
                        a.ProtectedPhaseNumber == downstreamPrimaryPhaseDirection.Phase);
                    PairedApproaches.Add(new LinkPivotPair(approach, downstreamApproach, startDate, endDate, cycleTime, chartLocation, bias,
                        biasDirection, Dates, i + 1));
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
            //    for (int i = detailTable.Rows.Count - 1; i > 0; i--)
            //    {
            //        pairedApproaches.Add(new LinkPivotPair(detailTable[i].SignalId, detailTable[i].Direction,
            //            detailTable[i].Location, detailTable[i - 1].SignalId, detailTable[i - 1].Direction,
            //            detailTable[i - 1].Location, startDate, endDate, cycleTime, chartLocation, bias, biasDirection, dates));
            //    }
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
                var lpp = PairedApproaches.FirstOrDefault(p => p.SignalApproach.SignalID == route.RouteSignals[i].SignalId);
                Adjustment.AddLinkPivotAdjustmentRow(lpp.SignalApproach.SignalID, Convert.ToInt32(lpp.SecondsAdded), 0,
                    lpp.PaogUpstreamBefore, lpp.PaogDownstreamBefore, lpp.AogUpstreamBefore, lpp.AogDownstreamBefore,
                    lpp.PaogUpstreamPredicted, lpp.PaogDownstreamPredicted, lpp.AogUpstreamPredicted, lpp.AogDownstreamPredicted,
                    lpp.SignalApproach.Signal.SignalDescription, lpp.DownSignalApproach.Signal.SignalID, lpp.DownSignalApproach.DirectionType.Description,
                    lpp.SignalApproach.DirectionType.Description, lpp.ResultChartLocation, lpp.DownSignalApproach.Signal.SignalDescription, 
                    lpp.AogTotalBefore, lpp.PaogTotalBefore,
                    lpp.AogTotalPredicted, lpp.PaogTotalPredicted, lpp.LinkNumber, lpp.TotalVolumeDownstream,
                    lpp.TotalVolumeUpstream);
            }

            //Set the end row to have zero for the ajustments. No adjustment can be made because 
            //downstream is unknown. The end row is determined by the starting point seleceted by the user
            if (direction == "Upstream")
            {
                //End row for upstream is index 0
                Adjustment.AddLinkPivotAdjustmentRow(PairedApproaches[0].SignalApproach.SignalID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                    PairedApproaches[0].SignalApproach.Signal.SignalDescription,
                    "", "", "", "", "", 0, 0, 0, 0, 1, 0, 0);
            }
            else
            {
                //End row for downstream is last row in the detail table
                Adjustment.AddLinkPivotAdjustmentRow(PairedApproaches[PairedApproaches.Count - 1].SignalApproach.SignalID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                    PairedApproaches[PairedApproaches.Count - 1].SignalApproach.Signal.SignalDescription, "", "", "", "", "", 0, 0, 0, 0,
                    PairedApproaches.Count, 0, 0);
            }

            int cumulativeChange = 0;

            //Determine the adjustment by adding the previous rows adjustment to the current rows delta
            for (int i = Adjustment.Count - 1; i >= 0; i--)
            {
                //if the new adjustment is greater than the cycle time than the adjustment should subtract
                // the cycle time from the current adjustment and the result should be the new adjustment
                if (cumulativeChange + Adjustment[i].Delta > cycleTime)
                {
                    Adjustment[i].Adjustment = cumulativeChange + Adjustment[i].Delta - cycleTime;
                    cumulativeChange = cumulativeChange + Adjustment[i].Delta - cycleTime;
                }
                else
                {
                    Adjustment[i].Adjustment = cumulativeChange + Adjustment[i].Delta;
                    cumulativeChange = cumulativeChange + Adjustment[i].Delta;
                }
            }

        }

        private string GetOppositeDirection(Models.DirectionType direction)
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
