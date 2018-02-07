using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using MOE.Common.Models.Repositories;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.ViewModel.Chart;

namespace SPM.Models
{
    public class AggDataExportViewModel
    {
        public List<Route> Routes { get; set; }
        public int SelectedRouteId { get; set; }
        public List<Signal> Signals { get; set; } = new List<Signal>();
        [Required]
        public virtual List<int> MetricTypeIDs { get; set; }
        public Dictionary<int, string> MetricItems { get; set; }

        public virtual ICollection<MetricType> MetricTypes { get; set; }
        public virtual ICollection<MovementType> MovementTypes { get; set; }
        public virtual ICollection<AggregationMetricOptions.Dimension> Dimensions { get; set; }
        public virtual ICollection<AggregationMetricOptions.SeriesType> SeriesTypes { get; set; }
        public List<MOE.Common.Business.WCFServiceLibrary.AggregationMetricOptions.XAxisType> XAxisTypes { get; set; }
        public List<AggregationMetricOptions.AggregationType> AggregationTypes { get; set; }
        public List<string> ChartTypesList { get; set; } = new List<string>();
        public List<MOE.Common.Business.Bins.BinFactoryOptions.BinSize> BinSizes { get; set; }
        public List<int> SeriesWidths { get; set; } = new List<int>();
        public List<SelectListItem> StartAMPMList { get; set; }
        public List<SelectListItem> EndAMPMList { get; set; }


        [Required]
        [Display(Name = "Dimesion")]
        public AggregationMetricOptions.Dimension SelectedDimension { get; set; }
        [Required]
        [Display(Name = "Series Type")]
        public AggregationMetricOptions.SeriesType SelectedSeriesType { get; set; }

        [Required]
        [Display(Name = "X-Axis")]
        public  AggregationMetricOptions.XAxisType SelectedXAxisType { get; set; }

        [Required]
        [Display(Name = "Aggregation Type")]
        public AggregationMetricOptions.AggregationType SelectedAggregationType { get; set; }

        [Required]
        [Display(Name = "Metric Type")]
        public MetricType SelectedMetricType { get; set; }

        [Required]
        [Display(Name="Chart Type")]
        public string SelectedChartType  { get; set; }

        [Display(Name = "Bin Size")]
        public BinFactoryOptions.BinSize SelectedBinSize { get; set; }

        public bool Weekdays { get; set; }
        public bool Weekends { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDateDay { get; set; }

        [Display(Name = "Start Time")]
        public string StartTime { get; set; }

        [Display(Name = "Start AM/PM")]
        public string SelectedStartAMPM { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime EndDateDay { get; set; }

        [Display(Name = "End Time")]
        public string EndTime { get; set; }

        [Display(Name = "End AM/PM")]
        public string SelectedEndAMPM { get; set; }

        [Required]
        [Display(Name = "Series Width")]
        public int SelectedSeriesWidth { get; set; }

        private IMetricTypeRepository _metricRepository;
        public AggDataExportViewModel()
        {
            _metricRepository = MetricTypeRepositoryFactory.Create();
            var regionRepositry = MOE.Common.Models.Repositories.RegionsRepositoryFactory.Create();
            List<MetricType> allMetricTypes = _metricRepository.GetAllToAggregateMetrics();
            MetricItems = new Dictionary<int, string>();
            //SignalSearchViewModel = new MOE.Common.Models.ViewModel.Chart.SignalSearchViewModel(regionRepositry, _metricRepository);
            SetDefaultDates();
            SetBinSizeList();
            SetXAxisTypes();
            SetAggregationTypes();
            SetChartTypes();
            SetSeriesWidth();
            SetSeriesTypes();
            SetDimensions();
        }

        private void SetDimensions()
        {
            Dimensions = Enum.GetValues(typeof(AggregationMetricOptions.Dimension)).Cast<AggregationMetricOptions.Dimension>().ToList();
        }

        private void SetSeriesWidth()
        {
            for (int i = 1; i < 6; i++)
            {
                SeriesWidths.Add(i);
            }
        }

        private void SetChartTypes()
        {
            List<SeriesChartType> chartTypes = Enum.GetValues(typeof(SeriesChartType)).Cast<SeriesChartType>().ToList();
            var chartsToExclude = new List<SeriesChartType>
            {
                SeriesChartType.Kagi,
                SeriesChartType.Renko,
                SeriesChartType.Stock,
                SeriesChartType.Pyramid,
                SeriesChartType.PointAndFigure
            };
            foreach (var chartType in chartTypes)
            {
                if(!chartsToExclude.Contains(chartType) )
                    ChartTypesList.Add(chartType.ToString());
            }
            ChartTypesList.Sort();
        }

        private void SetSeriesTypes()
        {
            SeriesTypes = Enum.GetValues(typeof(AggregationMetricOptions.SeriesType)).Cast<AggregationMetricOptions.SeriesType>().ToList();
        }

        private void SetAggregationTypes()
        {
            AggregationTypes = new List<AggregationMetricOptions.AggregationType>{ AggregationMetricOptions.AggregationType.Sum, AggregationMetricOptions.AggregationType.Average};
        }

        private void SetXAxisTypes()
        {
            XAxisTypes = Enum.GetValues(typeof(AggregationMetricOptions.XAxisType)).Cast<AggregationMetricOptions.XAxisType>().ToList();
        }

        protected void SetBinSizeList()
        {
            BinSizes = Enum.GetValues(typeof(BinFactoryOptions.BinSize)).Cast<BinFactoryOptions.BinSize>().ToList();
        }

        protected void SetDefaultDates()
        {
            StartAMPMList = new List<SelectListItem>();
            StartAMPMList.Add(new SelectListItem { Value = "AM", Text = "AM", Selected = true });
            StartAMPMList.Add(new SelectListItem { Value = "PM", Text = "PM" });
            EndAMPMList = new List<SelectListItem>();
            EndAMPMList.Add(new SelectListItem { Value = "AM", Text = "AM" });
            EndAMPMList.Add(new SelectListItem { Value = "PM", Text = "PM", Selected = true });
        }

    }
}