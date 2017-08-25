using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.ViewModel
{
    public class LinkPivotApproachLink
    {
        public string SignalId { get; set; }
        public string Location { get; set; }
        public string UpstreamApproachDirection { get; set; }
        public string DownSignalID { get; set; }
        public string DownLocation { get; set; }
        public string DownstreamApproachDirection { get; set; }
        public double PAOGUpstreamBefore { get; set; }
        public double PAOGUpstreamPredicted { get; set; }
        public double PAOGDownstreamBefore { get; set; }
        public double PAOGDownstreamPredicted { get; set; }
        public double AOGUpstreamBefore { get; set; }
        public double AOGUpstreamPredicted { get; set; }
        public double AOGDownstreamBefore { get; set; }
        public double AOGDownstreamPredicted { get; set; }
        public double Delta { get; set; }
        public string ResultChartLocation { get; set; }
        public string UpstreamCombinedLocation { get { return SignalId + "\n" + UpstreamApproachDirection; } }
        public string DownstreamCombinedLocation { get { return DownSignalID + "\n" + DownstreamApproachDirection; } }
        public double AogTotalBefore { get; set; }
        public double PAogTotalBefore { get; set; }
        public double AogTotalPredicted { get; set; }
        public double PAogTotalPredicted { get; set; }

        public double TotalChartExisting { get; set; }
        public double TotalChartPositiveChange { get; set; }
        public double TotalChartNegativeChange { get; set; }
        public double TotalChartRemaining { get; set; }

        public double UpstreamChartExisting { get; set; }
        public double UpstreamChartPositiveChange { get; set; }
        public double UpstreamChartNegativeChange { get; set; }
        public double UpstreamChartRemaining { get; set; }

        public double DownstreamChartExisting { get; set; }
        public double DownstreamChartPositiveChange { get; set; }
        public double DownstreamChartNegativeChange { get; set; }
        public double DownstreamChartRemaining { get; set; }

        public string TotalChartName { get { return "Total" + SignalId + "Chart"; }  }
        public string UpstreamChartName { get { return "Up" + SignalId + "Chart"; } }
        public string DownstreamChartName { get { return "Down" + SignalId + "Chart"; } }

        public int LinkNumber { get; set; }

        public LinkPivotApproachLink(string signalId, string location, 
            string upstreamApproachDirection, string downSignalID, 
            string downLocation, string downstreamApproachDirection, 
            double pAOGUpstreamBefore, double pAOGUpstreamPredicted, 
            double  pAOGDownstreamBefore, double pAOGDownstreamPredicted,
            double aOGUpstreamBefore, double aOGUpstreamPredicted,
            double aOGDownstreamBefore, double aOGDownstreamPredicted,
            double delta, string resultChartLocation, double aogTotalBefore,
            double pAogTotalBefore, double aogTotalPredicted, double pAogTotalPredicted,
            int linkNumber)
        {
            this.SignalId = signalId;
            this.Location = location;
            this.UpstreamApproachDirection = upstreamApproachDirection;
            this.DownSignalID = downSignalID;
            this.DownLocation = downLocation;
            this.DownstreamApproachDirection = downstreamApproachDirection;
            this.PAOGUpstreamBefore = pAOGUpstreamBefore;
            this.PAOGUpstreamPredicted = pAOGUpstreamPredicted;
            this.PAOGDownstreamBefore = pAOGDownstreamBefore;
            this.PAOGDownstreamPredicted = pAOGDownstreamPredicted;
            this.AOGUpstreamBefore = aOGUpstreamBefore;
            this.AOGUpstreamPredicted = aOGUpstreamPredicted;
            this.AOGDownstreamBefore = aOGDownstreamBefore;
            this.AOGDownstreamPredicted = aOGDownstreamPredicted;
            this.Delta = delta;
            this.ResultChartLocation = resultChartLocation;
            this.AogTotalBefore = aogTotalBefore;
            this.PAogTotalBefore = pAogTotalBefore;
            this.AogTotalPredicted = aogTotalPredicted;
            this.PAogTotalPredicted = pAogTotalPredicted;
            this.LinkNumber = linkNumber;

            //Get the Total Chart Settings
            double tempChange = PAogTotalPredicted - PAogTotalBefore;
            if(tempChange < 0)
            {
                TotalChartPositiveChange = 0;
                TotalChartNegativeChange = Math.Abs(tempChange);
                TotalChartExisting = PAogTotalBefore - TotalChartNegativeChange;
            }
            else
            {
                TotalChartNegativeChange = 0;
                TotalChartPositiveChange = tempChange;
                TotalChartExisting = PAogTotalBefore;
            }
            TotalChartRemaining = 100 - (TotalChartExisting + TotalChartPositiveChange + TotalChartNegativeChange);

            //Get the Upstream Chart Settings
            tempChange = PAOGUpstreamPredicted - PAOGUpstreamBefore;
            if (tempChange < 0)
            {
                UpstreamChartPositiveChange = 0;
                UpstreamChartNegativeChange = Math.Abs(tempChange);
                UpstreamChartExisting = PAOGUpstreamBefore - UpstreamChartNegativeChange;
            }
            else
            {
                UpstreamChartNegativeChange = 0;
                UpstreamChartPositiveChange = tempChange;
                UpstreamChartExisting = PAOGUpstreamBefore;
            }
            UpstreamChartRemaining = 100 - (UpstreamChartExisting + UpstreamChartPositiveChange + UpstreamChartNegativeChange);

            //Get the Downstream Chart Settings
            tempChange = PAOGDownstreamPredicted - PAOGDownstreamBefore;
            if (tempChange < 0)
            {
                DownstreamChartPositiveChange = 0;
                DownstreamChartNegativeChange = Math.Abs(tempChange);
                DownstreamChartExisting = PAOGDownstreamBefore - DownstreamChartNegativeChange;
            }
            else
            {
                DownstreamChartNegativeChange = 0;
                DownstreamChartPositiveChange = tempChange;
                DownstreamChartExisting = PAOGDownstreamBefore;
            }
            DownstreamChartRemaining = 100 - (DownstreamChartExisting + DownstreamChartPositiveChange + DownstreamChartNegativeChange);
        }
    }
}
