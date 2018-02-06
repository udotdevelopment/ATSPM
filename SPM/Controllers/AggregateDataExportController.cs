using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using SPM.Models;
using MOE.Common.Models;
using MOE.Common.Models.ViewModel.Chart;

namespace SPM.Controllers
{
    public class AggregateDataExportController : Controller
    {
        MOE.Common.Models.Repositories.IMetricTypeRepository metricTyperepository =
            MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();

        MOE.Common.Models.Repositories.IDirectionTypeRepository directionTypeRepository =
            MOE.Common.Models.Repositories.DirectionTypeRepositoryFactory.Create();

        MOE.Common.Models.Repositories.IMovementTypeRepository movementTypeRepository =
            MOE.Common.Models.Repositories.MovementTypeRepositoryFactory.Create();

        MOE.Common.Models.Repositories.ILaneTypeRepository laneTypeRepository =
            MOE.Common.Models.Repositories.LaneTypeRepositoryFactory.Create();


        // GET: DataExportViewModels
        public ActionResult CreateMetric(int id)
        {
            AggDataExportViewModel aggDataExportViewModel = new AggDataExportViewModel();
            var routeRepository = MOE.Common.Models.Repositories.RouteRepositoryFactory.Create();
            var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            Route route = routeRepository.GetRouteByID(id);
            foreach (var routeignal in route.RouteSignals)
            {
                aggDataExportViewModel.Signals.Add(signalRepository.GetLatestVersionOfSignalBySignalID(routeignal.SignalId));
            }
            //List<MetricType> allMetricTypes = metricTyperepository.GetAllToAggregateMetrics();
            //foreach (var metricType in allMetricTypes)
            //{
            //    aggDataExportViewModel.MetricItems.Add(metricType.MetricID, metricType.ChartName);
            //}
            //List<DirectionType> allDirectionTypes = directionTypeRepository.GetAllDirections();
            //List<MovementType> allMovementTypes = movementTypeRepository.GetAllMovementTypes();
            //List<LaneType> allLaneTypes = laneTypeRepository.GetAllLaneTypes();
            //aggDataExportViewModel.AllMetricTypes = allMetricTypes;
            //aggDataExportViewModel.AllApproachTypes = allDirectionTypes;
            //aggDataExportViewModel.AllMovementTypes = allMovementTypes;
            //aggDataExportViewModel.AllLaneTypes = allLaneTypes;
            return PartialView(aggDataExportViewModel);
        }

        // POST: AggDataExportViewModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMetric(AggDataExportViewModel aggDataExportViewModel)
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            options.StartDate = aggDataExportViewModel.StartDateDay;
            options.EndDate = aggDataExportViewModel.EndDateDay;
            options.AggregationOperation = aggDataExportViewModel.SelectedAggregationOperation;
            options.XAxisAggregationSeriesOption = aggDataExportViewModel.SelectedAggregationSeriesOptions;
            options.SeriesWidth = aggDataExportViewModel.SeriesWidth;
            string[] startTime;
            string[] endTime;
            int? startHour = null;
            int? startMinute = null;
            int? endHour= null;
            int? endMinute = null;
            BinFactoryOptions.TimeOptions timeOptions = BinFactoryOptions.TimeOptions.StartToEnd;
            if (!String.IsNullOrEmpty(aggDataExportViewModel.StartTime) &&
                !String.IsNullOrEmpty(aggDataExportViewModel.EndTime))
            {
                startTime = aggDataExportViewModel.StartTime.Split(':');
                startHour = Convert.ToInt32(startTime[0]);
                if (aggDataExportViewModel.SelectedStartAMPM.Contains("PM"))
                {
                    startHour += 12;
                }
                if (startTime.Length > 1)
                {
                    startMinute = Convert.ToInt32(startTime[1]);
                }
                else
                {
                    startMinute = 0;
                }
                endTime = aggDataExportViewModel.EndTime.Split(':');
                endHour = Convert.ToInt32(endTime[0]);
                if (aggDataExportViewModel.SelectedEndAMPM.Contains("PM"))
                {
                    endHour += 12;
                }
                if (endTime.Length > 1)
                {
                    endMinute = Convert.ToInt32(endTime[1]);
                }
                else
                {
                    endMinute = 0;
                }
                timeOptions = BinFactoryOptions.TimeOptions.TimePeriod;
            }
            List<DayOfWeek> daysOfWeek = new List<DayOfWeek>();
            if (aggDataExportViewModel.Weekends)
            {
                daysOfWeek.AddRange(new List<DayOfWeek>{DayOfWeek.Sunday, DayOfWeek.Saturday});
            }
            if (aggDataExportViewModel.Weekdays)
            {
                daysOfWeek.AddRange(new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
            }
            options.TimeOptions = new BinFactoryOptions(
                aggDataExportViewModel.StartDateDay,
                aggDataExportViewModel.EndDateDay,
                startHour, startMinute, endHour, endMinute, daysOfWeek,
                aggDataExportViewModel.SelectedBinSize,
                timeOptions);
            foreach (var signal in aggDataExportViewModel.Signals)
            {
                options.SignalIds.Add(signal.SignalID);
            }
            SeriesChartType tempSeriesChartType;
            Enum.TryParse(aggDataExportViewModel.SelectedChartType, out tempSeriesChartType);
            options.ChartType = tempSeriesChartType;
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();
            MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result.ChartPaths = client.CreateMetric(options);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            return PartialView("~/Views/DefaultCharts/MetricResult.cshtml", result);
        }

        // GET: DataExportViewModels
        public ActionResult Index()
        {
            var routeRepository = MOE.Common.Models.Repositories.RouteRepositoryFactory.Create();
            AggDataExportViewModel aggDataExportViewModel = new AggDataExportViewModel();
            aggDataExportViewModel.Routes = routeRepository.GetAllRoutes();
            List<MetricType> allMetricTypes = metricTyperepository.GetAllToAggregateMetrics();
            foreach (var metricType in allMetricTypes)
            {
                aggDataExportViewModel.MetricItems.Add(metricType.MetricID, metricType.ChartName);
            }
            List<DirectionType> allDirectionTypes = directionTypeRepository.GetAllDirections();
            List<MovementType> allMovementTypes = movementTypeRepository.GetAllMovementTypes();
            List<LaneType> allLaneTypes = laneTypeRepository.GetAllLaneTypes();
            aggDataExportViewModel.AllMetricTypes = allMetricTypes;
            aggDataExportViewModel.AllApproachTypes = allDirectionTypes;
            aggDataExportViewModel.AllMovementTypes = allMovementTypes;
            aggDataExportViewModel.AllLaneTypes = allLaneTypes;
            aggDataExportViewModel.SelectedMetric = 4;

            aggDataExportViewModel.StartDateDay = Convert.ToDateTime("10/17/2017");
            aggDataExportViewModel.EndDateDay = Convert.ToDateTime("10/18/2017");
            return View(aggDataExportViewModel);
        }

        private int[] SplitHourMinute(String timeFromFrontEnd)
        {
            int[] HourMinute = new int[]{0, 0};
            string[] splitted = timeFromFrontEnd.Split(':');
            int.TryParse(splitted[0], out HourMinute[0]);
            int.TryParse(splitted[1], out HourMinute[1]);
            return HourMinute;
        }

        public ActionResult AggregateDataExport()
        {
            return View();
        }

        public static List<int> StringToIntList(string str)
        {
            return str.Split(',').Select(int.Parse).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
