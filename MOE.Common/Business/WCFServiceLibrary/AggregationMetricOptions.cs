using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business;
using MOE.Common.Models;
using System.Runtime.Serialization;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MOE.Common.Business.WCFServiceLibrary
{
    public class AggregationMetricOptions : MetricOptions
    {
        public enum ChartTypes
        {
            Column,
            StackedColumn,
            Line,
            StackedLine

        };

        public enum AggregationOpperations
        {
            Unknown,
            Sum,
            Average

        };

        public enum AggregationSeriesOptions
        {
            Unknown,
            Movement,
            Approach,
            Signal,
            Route
        }

        public enum AggregationMetrics
        {
            LaneByLaneCounts,
            AdvancedCounts,
            ArrivalonGreen,
            PlatoonRatio,
            SplitFail,
            PedestrianActuations,
            Preemption,
            TSP,
            DataQuality
        }

        public enum AggregationGroups
        {
            Hour,
            Day,
            Month,
            Year,
            None,
            Signal
        }



       

        public AggregationMetricOptions()
        {

            



        }





        public ChartTypes ChartType { get; set; }

        public AggregationOpperations AggregationOpperation { get; set; }

        public AggregationGroups GroupBy { get; set; }

        public AggregationSeriesOptions AggregationSeries { get; set; }

        public List<Models.Signal> Signals = new List<Models.Signal>();

        public List<Models.Approach> Approaches = new List<Models.Approach>();

        public List<Models.Detector> Detectors = new List<Models.Detector>();

        public int BinSize { get; set; }
    }

    






    
}