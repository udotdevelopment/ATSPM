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

        public enum AggregationGroups
        {
            Unknown,
            Movement,
            Approach,
            Signal,
            Route
        }



        public List<KeyValuePair<string, int>> BinSizes = new List<KeyValuePair<string, int>>();

        public AggregationMetricOptions()
        {

            PopulateBinSizeList();



        }

        private void PopulateBinSizeList()
        {
            KeyValuePair<string, int> q = new KeyValuePair<string, int>("Fifteen Minutes", 15);
            KeyValuePair<string, int> h = new KeyValuePair<string, int>("Hour", 60);
            KeyValuePair<string, int> d = new KeyValuePair<string, int>("Day", 1440);
            KeyValuePair<string, int> w = new KeyValuePair<string, int>("Week", 10080);
            KeyValuePair<string, int> m = new KeyValuePair<string, int>("Month", 43800);
            KeyValuePair<string, int> y = new KeyValuePair<string, int>("Year", 525600);

            BinSizes.Add(q);
            BinSizes.Add(h);
            BinSizes.Add(d);
            BinSizes.Add(w);
            BinSizes.Add(m);
            BinSizes.Add(y);
        }



        public ChartTypes ChartType;

        public AggregationOpperations AggregationOpperation;

        public AggregationGroups GroupBy;

        public List<Models.Signal> Signals = new List<Models.Signal>();

        public List<Models.Approach> Approaches = new List<Models.Approach>();

        public List<Models.Detector> Detectors = new List<Models.Detector>();

        public int BinSize { get; set; }
    }

    






    
}