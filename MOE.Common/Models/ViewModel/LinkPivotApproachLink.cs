using System;

namespace MOE.Common.Models.ViewModel
{
    public class LinkPivotApproachLink
    {
        public LinkPivotApproachLink(string signalId, string location,
            string upstreamApproachDirection, string downSignalID,
            string downLocation, string downstreamApproachDirection,
            double pAOGUpstreamBefore, double pAOGUpstreamPredicted,
            double pAOGDownstreamBefore, double pAOGDownstreamPredicted,
            double aOGUpstreamBefore, double aOGUpstreamPredicted,
            double aOGDownstreamBefore, double aOGDownstreamPredicted,
            double delta, string resultChartLocation, double aogTotalBefore,
            double pAogTotalBefore, double aogTotalPredicted, double pAogTotalPredicted,
            int linkNumber)
        {
            SignalId = signalId;
            Location = location;
            UpstreamApproachDirection = upstreamApproachDirection;
            DownSignalID = downSignalID;
            DownLocation = downLocation;
            DownstreamApproachDirection = downstreamApproachDirection;
            PAOGUpstreamBefore = pAOGUpstreamBefore;
            PAOGUpstreamPredicted = pAOGUpstreamPredicted;
            PAOGDownstreamBefore = pAOGDownstreamBefore;
            PAOGDownstreamPredicted = pAOGDownstreamPredicted;
            AOGUpstreamBefore = aOGUpstreamBefore;
            AOGUpstreamPredicted = aOGUpstreamPredicted;
            AOGDownstreamBefore = aOGDownstreamBefore;
            AOGDownstreamPredicted = aOGDownstreamPredicted;
            Delta = delta;
            ResultChartLocation = resultChartLocation;
            AogTotalBefore = aogTotalBefore;
            PAogTotalBefore = pAogTotalBefore;
            AogTotalPredicted = aogTotalPredicted;
            PAogTotalPredicted = pAogTotalPredicted;
            LinkNumber = linkNumber;

            //Get the Total Chart Settings
            var tempChange = PAogTotalPredicted - PAogTotalBefore;
            if (tempChange < 0)
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
            UpstreamChartRemaining =
                100 - (UpstreamChartExisting + UpstreamChartPositiveChange + UpstreamChartNegativeChange);

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
            DownstreamChartRemaining =
                100 - (DownstreamChartExisting + DownstreamChartPositiveChange + DownstreamChartNegativeChange);
        }

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
        public string UpstreamCombinedLocation => SignalId + "\n" + UpstreamApproachDirection;
        public string DownstreamCombinedLocation => DownSignalID + "\n" + DownstreamApproachDirection;
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

        public string TotalChartName => "Total" + SignalId + "Chart";
        public string UpstreamChartName => "Up" + SignalId + "Chart";
        public string DownstreamChartName => "Down" + SignalId + "Chart";

        public int LinkNumber { get; set; }
    }
}