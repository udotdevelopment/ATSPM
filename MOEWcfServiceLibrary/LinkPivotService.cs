using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Xml;
using MOE.Common;

namespace MOEWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PCDService" in both code and config file together.
    /// <summary>
    /// This class contains all functions available to the Link Pivot chart
    /// </summary>
    public class LinkPivotService : ILinkPivotService
    {
        /// <summary>
        /// Generates 4 PCD Charts and returns the path to the images create and summary info
        /// through the Display object.
        /// </summary>
        /// <param name="upstreamSignalID"></param>
        /// <param name="upstreamDirection"></param>
        /// <param name="downstreamSignalID"></param>
        /// <param name="downstreamDirection"></param>
        /// <param name="delta"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="maxYAxis"></param>
        /// <returns>DisplayObject</returns>
        public DisplayObject DisplayLinkPivotPCD(string upstreamSignalID, string upstreamDirection,
            string downstreamSignalID, string downstreamDirection, int delta, 
            DateTime startDate, DateTime endDate, int maxYAxis)
        {
            //Buid the Link Pivot PCD Object
            MOE.Common.Business.LinkPivotPCDDisplay display =
                new MOE.Common.Business.LinkPivotPCDDisplay(upstreamSignalID, upstreamDirection,
                    downstreamSignalID, downstreamDirection, delta, startDate, endDate,
                    maxYAxis);

            //Instantiate the object to be returned and set its values
            DisplayObject d = new DisplayObject();
            d.UpstreamBeforePCDPath = display.UpstreamBeforePCDPath;
            d.UpstreamAfterPCDPath = display.UpstreamAfterPCDPath;
            d.DownstreamBeforePCDPath = display.DownstreamBeforePCDPath;
            d.DownstreamAfterPCDPath = display.DownstreamAfterPCDPath;
            d.ExistingAOG = display.ExistingTotalAOG;
            d.ExistingPAOG = display.ExistingTotalAOG / display.ExistingVolume;
            d.PredictedAOG = display.PredictedTotalAOG;
            d.PredictedPAOG = display.PredictedTotalAOG / display.PredictedVolume;
            d.DownstreamBeforeTitle = display.DownstreamBeforeTitle;
            d.UpstreamBeforeTitle = display.UpstreamBeforeTitle;
            d.DownstreamAfterTitle = display.DownstreamAfterTitle;
            d.UpstreamAfterTitle = display.UpstreamAfterTitle;
            return d;
        }

        /// <summary>
        /// Runs the logic for a Link pivot analysis based on a pre-defined route
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="cycleTime"></param>
        /// <param name="chartLocation"></param>
        /// <param name="direction"></param>
        /// <param name="bias"></param>
        /// <param name="biasDirection"></param>
        /// <param name="sunday"></param>
        /// <param name="monday"></param>
        /// <param name="tuesday"></param>
        /// <param name="wednesday"></param>
        /// <param name="thursday"></param>
        /// <param name="friday"></param>
        /// <param name="saturday"></param>
        /// <returns>AdjustmentObject Array</returns>
        public AdjustmentObject[] GetLinkPivot(int routeId, DateTime startDate, DateTime endDate, int cycleTime, string chartLocation,
            string direction, double bias, string biasDirection, bool sunday, bool monday, bool tuesday, bool wednesday,
            bool thursday, bool friday, bool saturday)
        {
            //Create a list of days of the week selected to be included in the analysis
            List<DayOfWeek> daysList = new List<DayOfWeek>();
            if (monday)
            {
                daysList.Add(DayOfWeek.Monday);
            }
            if (tuesday)
            {
                daysList.Add(DayOfWeek.Tuesday);
            }
            if (wednesday)
            {
                daysList.Add(DayOfWeek.Wednesday);
            }
            if (thursday)
            {
                daysList.Add(DayOfWeek.Thursday);
            }
            if (friday)
            {
                daysList.Add(DayOfWeek.Friday);
            }
            if (saturday)
            {
                daysList.Add(DayOfWeek.Saturday);
            }
            if (sunday)
            {
                daysList.Add(DayOfWeek.Sunday);
            }

            //Generate a Link Pivot Object
            MOE.Common.Business.LinkPivot lp = new MOE.Common.Business.LinkPivot(routeId, startDate, endDate,
                cycleTime, chartLocation, direction, bias, biasDirection, daysList);

            //Instantiate the return object
            List<AdjustmentObject> adjustments = new List<AdjustmentObject>();

            //Add the data from the Link Pivot Object to the return object
            foreach(MOE.Common.Data.LinkPivot.LinkPivotAdjustmentRow row in lp.Adjustment)
            {
                AdjustmentObject a = new AdjustmentObject();
                a.SignalId = row.SignalId;
                a.Location = row.Location;
                a.DownstreamLocation = row.DownstreamLocation;
                a.Delta = row.Delta;
                a.Adjustment = row.Adjustment;
                a.PAOGDownstreamBefore = row.PAOGDownstreamBefore;
                a.PAOGDownstreamPredicted = row.PAOGDownstreamPredicted;
                a.PAOGUpstreamBefore = row.PAOGUpstreamBefore;
                a.PAOGUpstreamPredicted = row.PAOGUpstreamPredicted;
                a.AOGDownstreamBefore = row.AOGDownstreamBefore;
                a.AOGDownstreamPredicted = row.AOGDownstreamPredicted;
                a.AOGUpstreamBefore = row.AOGUpstreamBefore;
                a.AOGUpstreamPredicted = row.AOGUpstreamPredicted;
                a.DownSignalId = row.DownstreamSignalID;
                a.DownstreamApproachDirection = row.DownstreamApproachDirection;
                a.UpstreamApproachDirection = row.UpstreamApproachDirection;
                a.ResultChartLocation = row.ResultChartLocation;
                a.AogTotalBefore = row.AOGTotalBefore;
                a.PAogTotalBefore = row.PAOGToatalBefore;
                a.AogTotalPredicted = row.AOGTotalPredicted;
                a.PAogTotalPredicted = row.PAOGTotalPredicted;
                a.LinkNumber = row.LinkNumber;
                a.DownstreamVolume = row.DownstreamVolume;
                a.UpstreamVolume = row.UpstreamVolume;
                adjustments.Add(a);
            }

           return adjustments.ToArray();
        }

        /// <summary>
        /// Used to test if the service is functioning
        /// </summary>
        /// <returns>The number 1</returns>
        public int Test()
        {
            return 1;
        }
    }
}
