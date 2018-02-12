using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business;
using MOE.Common.Business.Bins;
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.Common.Models.ViewModel.Chart;
using Signal = MOE.Common.Models.Signal;

namespace SPM.Models
{
    public class AggDataExportViewModel
    {
        public List<Route> Routes { get; set; }
        public int? SelectedRouteId { get; set; }
        [Required]
        public List<FilterSignal> FilterSignals { get; set; } = new List<FilterSignal>();

        public virtual ICollection<MetricType> MetricTypes { get; set; }
        public virtual ICollection<AggregationMetricOptions.Dimension> Dimensions { get; set; }
        public virtual ICollection<AggregationMetricOptions.SeriesType> SeriesTypes { get; set; }
        public List<AggregationMetricOptions.XAxisType> XAxisTypes { get; set; }
        public List<AggregationMetricOptions.AggregationType> AggregationTypes { get; set; }
        public List<string> ChartTypesList { get; set; } = new List<string>();
        public List<Tuple<int, String>> BinSizes { get; set; } = new List<Tuple<int, string>>();
        public List<int> SeriesWidths { get; set; } = new List<int>();
        public List<SelectListItem> StartAMPMList { get; set; }
        public List<SelectListItem> EndAMPMList { get; set; }
        public List<FilterDirection> FilterDirections { get; set; } = new List<FilterDirection>();



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
        public int SelectedMetricTypeId { get; set; }

        [Required]
        [Display(Name="Chart Type")]
        public string SelectedChartType  { get; set; }

        [Display(Name = "Bin Size")]
        public int SelectedBinSize { get; set; }

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


        public MOE.Common.Models.ViewModel.Chart.SignalSearchViewModel SignalSearch { get; set; } = new SignalSearchViewModel();

        private IMetricTypeRepository _metricRepository;
        public AggDataExportViewModel()
        {
            
        }

        public void SetDirectionTypes()
        {
            var directionTypeRepository = MOE.Common.Models.Repositories.DirectionTypeRepositoryFactory.Create();
            var directionTypes = directionTypeRepository.GetAllDirections();
            foreach (var direction in directionTypes)
            {
                FilterDirections.Add(new FilterDirection(direction.DirectionTypeID, direction.Description, true));
            }
        }

        public void SetDimensions()
        {
            Dimensions = Enum.GetValues(typeof(AggregationMetricOptions.Dimension)).Cast<AggregationMetricOptions.Dimension>().ToList();
        }

        public void SetSeriesWidth()
        {
            for (int i = 1; i < 6; i++)
            {
                SeriesWidths.Add(i);
            }
        }

        public void SetChartTypes()
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

        public void SetSeriesTypes()
        {
            SeriesTypes = Enum.GetValues(typeof(AggregationMetricOptions.SeriesType)).Cast<AggregationMetricOptions.SeriesType>().ToList();
        }

        public void SetAggregationTypes()
        {
            AggregationTypes = new List<AggregationMetricOptions.AggregationType>{ AggregationMetricOptions.AggregationType.Sum, AggregationMetricOptions.AggregationType.Average};
        }

        public void SetXAxisTypes()
        {
            XAxisTypes = Enum.GetValues(typeof(AggregationMetricOptions.XAxisType)).Cast<AggregationMetricOptions.XAxisType>().ToList();
        }

        public void SetBinSizeList()
        {
            List<BinFactoryOptions.BinSize> binSizes = Enum.GetValues(typeof(BinFactoryOptions.BinSize)).Cast<BinFactoryOptions.BinSize>().ToList();
            foreach (var binSize in binSizes)
            {
                switch (binSize)
                {
                    case BinFactoryOptions.BinSize.FifteenMinute:
                        BinSizes.Add(new Tuple<int, string>((int)binSize, "Fifteen Minutes"));
                        break;
                    case BinFactoryOptions.BinSize.ThirtyMinute:
                        BinSizes.Add(new Tuple<int, string>((int)binSize, "Thirty Minutes"));
                        break;
                    case BinFactoryOptions.BinSize.Hour:
                        BinSizes.Add(new Tuple<int, string>((int)binSize, "Hour"));
                        break;
                    case BinFactoryOptions.BinSize.Day:
                        BinSizes.Add(new Tuple<int, string>((int)binSize, "Day"));
                        break;
                    case BinFactoryOptions.BinSize.Month:
                        BinSizes.Add(new Tuple<int, string>((int)binSize, "Month"));
                        break;
                    case BinFactoryOptions.BinSize.Year:
                        BinSizes.Add(new Tuple<int, string>((int)binSize, "Year"));
                        break;
                }
            }
        }

        public void SetDefaultDates()
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