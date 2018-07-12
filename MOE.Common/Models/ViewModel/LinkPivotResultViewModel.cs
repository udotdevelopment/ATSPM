using System;
using System.Collections.Generic;

namespace MOE.Common.Models.ViewModel
{
    public class LinkPivotResultViewModel
    {
        public LinkPivotResultViewModel()
        {
            Adjustments = new List<LinkPivotAdjustment>();
            ApproachLinks = new List<LinkPivotApproachLink>();
        }

        public List<LinkPivotAdjustment> Adjustments { get; set; }
        public List<LinkPivotApproachLink> ApproachLinks { get; set; }

        //Summary Info
        public double TotalAogDownstreamBefore { get; set; }

        public double TotalPaogDownstreamBefore { get; set; }
        public double TotalAogDownstreamPredicted { get; set; }
        public double TotalPaogDownstreamPredicted { get; set; }
        public double TotalAogUpstreamBefore { get; set; }
        public double TotalPaogUpstreamBefore { get; set; }
        public double TotalAogUpstreamPredicted { get; set; }
        public double TotalPaogUpstreamPredicted { get; set; }
        public double TotalAogBefore { get; set; }
        public double TotalPaogBefore { get; set; }
        public double TotalAogPredicted { get; set; }
        public double TotalPaogPredicted { get; set; }

        //Total change chart
        public double TotalChartExisting { get; set; }

        public double TotalChartPositiveChange { get; set; }
        public double TotalChartNegativeChange { get; set; }
        public double TotalChartRemaining { get; set; }

        //Total upstream change chart
        public double TotalUpstreamChartExisting { get; set; }

        public double TotalUpstreamChartPositiveChange { get; set; }
        public double TotalUpstreamChartNegativeChange { get; set; }
        public double TotalUpstreamChartRemaining { get; set; }

        //Total downstream change chart
        public double TotalDownstreamChartExisting { get; set; }

        public double TotalDownstreamChartPositiveChange { get; set; }
        public double TotalDownstreamChartNegativeChange { get; set; }
        public double TotalDownstreamChartRemaining { get; set; }

        public void SetSummary()
        {
            //Get the Total Summary Chart Settings
            var tempChange = TotalPaogPredicted - TotalPaogBefore;
            if (tempChange < 0)
            {
                TotalChartPositiveChange = 0;
                TotalChartNegativeChange = Math.Abs(tempChange);
                TotalChartExisting = TotalPaogBefore - TotalChartNegativeChange;
            }
            else
            {
                TotalChartNegativeChange = 0;
                TotalChartPositiveChange = tempChange;
                TotalChartExisting = TotalPaogBefore;
            }
            TotalChartRemaining = 100 - (TotalChartExisting + TotalChartPositiveChange + TotalChartNegativeChange);

            //Get the Upstream Summary Chart Settings
            tempChange = TotalPaogUpstreamPredicted - TotalPaogUpstreamBefore;
            if (tempChange < 0)
            {
                TotalUpstreamChartPositiveChange = 0;
                TotalUpstreamChartNegativeChange = Math.Abs(tempChange);
                TotalUpstreamChartExisting = TotalPaogUpstreamBefore - TotalUpstreamChartNegativeChange;
            }
            else
            {
                TotalUpstreamChartNegativeChange = 0;
                TotalUpstreamChartPositiveChange = tempChange;
                TotalUpstreamChartExisting = TotalPaogUpstreamBefore;
            }
            TotalUpstreamChartRemaining = 100 - (TotalUpstreamChartExisting + TotalUpstreamChartPositiveChange +
                                                 TotalUpstreamChartNegativeChange);

            //Get the Downstream Summary Chart Settings
            tempChange = TotalPaogDownstreamPredicted - TotalPaogDownstreamBefore;
            if (tempChange < 0)
            {
                TotalDownstreamChartPositiveChange = 0;
                TotalDownstreamChartNegativeChange = Math.Abs(tempChange);
                TotalDownstreamChartExisting = TotalPaogDownstreamBefore - TotalDownstreamChartNegativeChange;
            }
            else
            {
                TotalDownstreamChartNegativeChange = 0;
                TotalDownstreamChartPositiveChange = tempChange;
                TotalDownstreamChartExisting = TotalPaogDownstreamBefore;
            }
            TotalDownstreamChartRemaining = 100 - (TotalDownstreamChartExisting + TotalDownstreamChartPositiveChange +
                                                   TotalDownstreamChartNegativeChange);
        }
    }
}