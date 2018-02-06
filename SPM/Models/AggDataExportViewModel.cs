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
        [Display(Name = "Metric Selection")]
        public int SelectedMetric { get; set; }
        public virtual ICollection<MetricType> AllMetricTypes { get; set; }
        public virtual List<int> ApproachTypeIDs { get; set; }
        public virtual ICollection<DirectionType> AllApproachTypes { get; set; }
        public virtual List<int> MovementTypeIDs { get; set; }
        public virtual ICollection<MovementType> AllMovementTypes { get; set; }
        public List<MOE.Common.Business.WCFServiceLibrary.AggregationMetricOptions.XAxisAggregationSeriesOptions> AggSeriesOptions { get; set; }
        [Display(Name = "X -Axis")]
        public  AggregationMetricOptions.XAxisAggregationSeriesOptions SelectedAggregationSeriesOptions { get; set; }
        public List<AggregationMetricOptions.AggregationOperations> AggregationOperationsList { get; set; }
        [Display(Name = "Aggregation Type")]
        public AggregationMetricOptions.AggregationOperations SelectedAggregationOperation { get; set; }

        public List<string> ChartTypesList { get; set; } = new List<string>();
        [Display(Name="Chart Types")]
        public string SelectedChartType  { get; set; }
        //public List<AggregationMetricOptions.ChartTypes> ChartTypesList { get; set; }
        //[Display(Name = "Chart Types")]
        //public AggregationMetricOptions.ChartTypes SelectedChartType { get; set; }
        public virtual List<int> LaneTypeIDs { get; set; }
        public virtual ICollection<LaneType> AllLaneTypes { get; set; }
        ////public List<SelectListItem> AggregateMetricsList { get; set; }

        public bool Weekdays { get; set; }
        public bool Weekends { get; set; }
        public bool IsSum { get; set; }
        //public List<string> WeekdayWeekendIDs { get; set; }
        //public ICollection<string> SelectedWeekdayWeekend { get; set; }
        //public string ColorSelection { get; set; }
        //public List<Route> Routes { get; set; }
        //public int SelectedRouteId { get; set; }

        //public SignalSearchViewModel SignalSearchViewModel { get; set; }
        //public String RunMetricJavascript { get; set; } = string.Empty;

        ////[Required]
        ////[Display(Name = "Signal ID")]
        ////public string SignalId { get; set; }
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
        public List<SelectListItem> StartAMPMList { get; set; }
        public List<SelectListItem> EndAMPMList { get; set; }
        public List<MOE.Common.Business.Bins.BinFactoryOptions.BinSize> BinSizeList { get; set; }
        [Display(Name = "Bin Size")]
        public BinFactoryOptions.BinSize SelectedBinSize { get; set; }

        public int SeriesWidth { get; set; }
        public List<int> SeriesWidthList { get; set; } = new List<int>();

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
            SetXAxisAggregationOptions();
            SetAggregationOperations();
            SetChartTypes();
            SetSeriesWidth();
        }

        private void SetSeriesWidth()
        {
            for (int i = 1; i < 6; i++)
            {
                SeriesWidthList.Add(i);
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

        private void SetAggregationOperations()
        {
            AggregationOperationsList = new List<AggregationMetricOptions.AggregationOperations>{ AggregationMetricOptions.AggregationOperations.Sum, AggregationMetricOptions.AggregationOperations.Average};
        }

        private void SetXAxisAggregationOptions()
        {
            AggSeriesOptions = Enum.GetValues(typeof(AggregationMetricOptions.XAxisAggregationSeriesOptions)).Cast<AggregationMetricOptions.XAxisAggregationSeriesOptions>().ToList();
        }

        protected void SetBinSizeList()
        {
            BinSizeList = Enum.GetValues(typeof(BinFactoryOptions.BinSize)).Cast<BinFactoryOptions.BinSize>().ToList();
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