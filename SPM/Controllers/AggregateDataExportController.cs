//using Ionic.Zip;
using MOE.Common.Business.Bins;
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using SPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;

namespace SPM.Controllers
{
    public class AggregateDataExportController : Controller
    {
        MOE.Common.Models.Repositories.IMetricTypeRepository metricTyperepository =
            MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();

        // GET: DataExportViewModels
        public ActionResult GetRouteSignals(int id)
        {
            AggDataExportViewModel aggDataExportViewModel = new AggDataExportViewModel();
            var routeRepository = MOE.Common.Models.Repositories.RouteRepositoryFactory.Create();
            var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            Route route = routeRepository.GetRouteByID(id);
            foreach (var routeignal in route.RouteSignals)
            {
                var signal = signalRepository.GetLatestVersionOfSignalBySignalID(routeignal.SignalId);
                aggDataExportViewModel.FilterSignals.Add(GetFilterSignal(signal));
            }
            return PartialView(aggDataExportViewModel);
        }

        public ActionResult GetSignal(string signalId, int index)
        {
            var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            AggDataExportViewModel aggDataExportViewModel = new AggDataExportViewModel();
            var signal = signalRepository.GetLatestVersionOfSignalBySignalID(signalId);
            aggDataExportViewModel.FilterSignals.Add(GetFilterSignal(signal));
            return PartialView("GetRouteSignals", aggDataExportViewModel);
        }

        private static FilterSignal GetFilterSignal(MOE.Common.Models.Signal signal)
        {
            var filterSignal = new FilterSignal
            {
                Exclude = false,
                SignalId = signal.SignalID,
                VersionId = signal.VersionID,
                Description = signal.SignalDescription
            };
            foreach (var approach in signal.Approaches)
            {
                var filterApproach = new MOE.Common.Business.FilterExtensions.FilterApproach
                {
                    ApproachId = approach.ApproachID,
                    Exclude = false,
                    Description = approach.Description
                };
                foreach (var detector in approach.Detectors)
                {
                    var filterDetector = new MOE.Common.Business.FilterExtensions.FilterDetector
                    {
                        Id = detector.ID,
                        Exclude = false,
                        Description = detector.Description
                    };
                    filterApproach.FilterDetectors.Add(filterDetector);
                }
                filterSignal.FilterApproaches.Add(filterApproach);
            }

            return filterSignal;
        }

        // POST: AggDataExportViewModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMetric(AggDataExportViewModel aggDataExportViewModel)
        {
            switch (aggDataExportViewModel.SelectedMetricTypeId)
            {
                case 16:
                    return GetLaneByLaneChart(aggDataExportViewModel);
                case 25:
                    return GetApproachSpeedAggregationChart(aggDataExportViewModel);
                case 18:
                    return GetPCDChart(aggDataExportViewModel);
                case 19:
                    return GetCycleChart(aggDataExportViewModel);
                case 20:
                    return GetSplitFailChart(aggDataExportViewModel);
                case 26:
                    return GetYraChart(aggDataExportViewModel);
                case 22:
                    return GetPreemptionChart(aggDataExportViewModel);
                case 24:
                    return GetPriorityChart(aggDataExportViewModel);
                case 27:
                    return GetSignalEventCountChart(aggDataExportViewModel);
                //case 28:
                //    return GetApproachEventCountChart(aggDataExportViewModel);
                case 29:
                    return GetPhaseTerminationChart(aggDataExportViewModel);
                case 30:
                    return GetPhasePedChart(aggDataExportViewModel);
                case 34:
                    return GetLeftTurnGapChart(aggDataExportViewModel);
                case 35:
                    return GetSplitMonitorChart(aggDataExportViewModel);
                default:
                    return Content("<h1 class='text-danger'>Unkown Chart Type</h1>");
            }
        }


        private ActionResult GetCycleChart(AggDataExportViewModel aggDataExportViewModel)
        {
            PhaseCycleAggregationOptions options = new PhaseCycleAggregationOptions();
            return GetChart(aggDataExportViewModel, options);
        }

        private ActionResult GetApproachSpeedAggregationChart(AggDataExportViewModel aggDataExportViewModel)
        {
            ApproachSpeedAggregationOptions options = new ApproachSpeedAggregationOptions();
            return GetChart(aggDataExportViewModel, options);
        }

        private ActionResult GetLaneByLaneChart(AggDataExportViewModel aggDataExportViewModel)
        {
            DetectorVolumeAggregationOptions options = new DetectorVolumeAggregationOptions();
            return GetChart(aggDataExportViewModel, options);
        }

        private ActionResult GetChart(AggDataExportViewModel aggDataExportViewModel, SignalAggregationMetricOptions options)
        {
            Enum.TryParse(aggDataExportViewModel.SelectedChartType, out SeriesChartType tempSeriesChartType);
            options.SelectedChartType = tempSeriesChartType;
            if (TryValidateModel(aggDataExportViewModel) && aggDataExportViewModel.FilterSignals.Count > 0)
            {
                SetCommonValues(aggDataExportViewModel, options);
                return GetChartFromService(options);
            }
            else
            {
                return Content("<h1 class='text-danger'>Missing Parameters</h1>");
            }
        }
        private ActionResult GetPhasePedChart(AggDataExportViewModel aggDataExportViewModel)
        {
            PhasePedAggregationOptions options = new PhasePedAggregationOptions();
            return GetChart(aggDataExportViewModel, options);
        }
        private ActionResult GetSplitMonitorChart(AggDataExportViewModel aggDataExportViewModel)
        {
            PhaseSplitMonitorAggregationOptions options = new PhaseSplitMonitorAggregationOptions();
            return GetChart(aggDataExportViewModel, options);
        }
        private ActionResult GetLeftTurnGapChart(AggDataExportViewModel aggDataExportViewModel)
        {
            PhaseLeftTurnGapAggregationOptions options = new PhaseLeftTurnGapAggregationOptions();
            return GetChart(aggDataExportViewModel, options);
        }
        private ActionResult GetPhaseTerminationChart(AggDataExportViewModel aggDataExportViewModel)
        {
            PhaseTerminationAggregationOptions options = new PhaseTerminationAggregationOptions();
            return GetChart(aggDataExportViewModel, options);
        }

        private ActionResult GetPriorityChart(AggDataExportViewModel aggDataExportViewModel)
        {
            PriorityAggregationOptions options = new PriorityAggregationOptions();
            return GetChart(aggDataExportViewModel, options);
        }

        private ActionResult GetPreemptionChart(AggDataExportViewModel aggDataExportViewModel)
        {
            PreemptionAggregationOptions options = new PreemptionAggregationOptions();
            return GetChart(aggDataExportViewModel, options);
        }

        private ActionResult GetSplitFailChart(AggDataExportViewModel aggDataExportViewModel)
        {
            ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            return GetChart(aggDataExportViewModel, options);
        }
        private ActionResult GetSignalEventCountChart(AggDataExportViewModel aggDataExportViewModel)
        {
            SignalEventCountAggregationOptions options = new SignalEventCountAggregationOptions();
            return GetChart(aggDataExportViewModel, options);
        }
        //private ActionResult GetApproachEventCountChart(AggDataExportViewModel aggDataExportViewModel)
        //{
        //    ApproachEventCountAggregationOptions options = new ApproachEventCountAggregationOptions();
        //    return GetChart(aggDataExportViewModel, options);
        //}
        private ActionResult GetYraChart(AggDataExportViewModel aggDataExportViewModel)
        {
            ApproachYellowRedActivationsAggregationOptions options = new ApproachYellowRedActivationsAggregationOptions();
            return GetChart(aggDataExportViewModel, options);
        }
        private ActionResult GetPCDChart(AggDataExportViewModel aggDataExportViewModel)
        {
            ApproachPcdAggregationOptions options = new ApproachPcdAggregationOptions();
            return GetChart(aggDataExportViewModel, options);
        }

        private static void SetCommonValues(AggDataExportViewModel aggDataExportViewModel, SignalAggregationMetricOptions options)
        {
            //aggDataExportViewModel.EndDateDay =aggDataExportViewModel.EndDateDay.Value.AddDays(1);
            options.StartDate = aggDataExportViewModel.StartDateDay.Value;
            options.EndDate = aggDataExportViewModel.EndDateDay.Value;
            options.SelectedAggregationType = aggDataExportViewModel.SelectedAggregationType.Value;
            options.SelectedXAxisType = aggDataExportViewModel.SelectedXAxisType.Value;
            options.SeriesWidth = aggDataExportViewModel.SelectedSeriesWidth;
            options.SelectedSeries = aggDataExportViewModel.SelectedSeriesType.Value;
            options.SelectedDimension = aggDataExportViewModel.SelectedDimension;
            SetTimeOptionsFromViewModel(aggDataExportViewModel, options);
            options.FilterSignals = aggDataExportViewModel.FilterSignals;
            options.FilterMovements = aggDataExportViewModel.FilterMovements;
            options.FilterDirections = aggDataExportViewModel.FilterDirections;
            options.SelectedAggregatedDataType =
                options.AggregatedDataTypes.FirstOrDefault(a =>
                    a.Id == aggDataExportViewModel.SelectedAggregatedData);
            options.ShowEventCount = aggDataExportViewModel.ShowEventCount;
        }

        private ActionResult GetChartFromService(SignalAggregationMetricOptions options)
        {
            Models.MetricAndXmlResultViewModel result = new Models.MetricAndXmlResultViewModel();
            MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
            try
            {
                client.Open();
                var resultPaths = client.GetChartAndXmlFileLocations(options);
                client.Close();
                result.ChartPaths = resultPaths.ToList();
                return PartialView("~/Views/AggregateDataExport/MetricResult.cshtml", result);
            }
            catch (Exception ex)
            {
                client.Close();
                return Content("<h1>" + ex.Message + "</h1>");
            }
        }

        private static void SetTimeOptionsFromViewModel(AggDataExportViewModel aggDataExportViewModel, SignalAggregationMetricOptions options)
        {
            string[] startTime;
            string[] endTime;
            BinFactoryOptions.TimeOptions timeOptions = BinFactoryOptions.TimeOptions.StartToEnd;
            int? startHour;
            int? startMinute;
            int? endHour;
            int? endMinute;
            if (!string.IsNullOrEmpty(aggDataExportViewModel.StartTime) &&
                !string.IsNullOrEmpty(aggDataExportViewModel.EndTime))
            {
                startTime = aggDataExportViewModel.StartTime.Split(':');
                startHour = Convert.ToInt32(startTime[0]);
                if (startHour == 12 && aggDataExportViewModel.SelectedStartAMPM.Contains("AM"))
                {
                    startHour = 0;
                }
                if (aggDataExportViewModel.SelectedStartAMPM.Contains("PM"))
                {
                    startHour += 12;
                }
                startMinute = startTime.Length > 1 ? Convert.ToInt32(startTime[1]) : 0;
                endTime = aggDataExportViewModel.EndTime.Split(':');
                endHour = Convert.ToInt32(endTime[0]);
                if (endHour == 12 && aggDataExportViewModel.SelectedEndAMPM.Contains("AM"))
                {
                    endHour = 0;
                }
                if (aggDataExportViewModel.SelectedEndAMPM.Contains("PM") && endHour < 12)
                {
                    endHour += 12;
                }
                endMinute = endTime.Length > 1 ? Convert.ToInt32(endTime[1]) : 0;
                //timeOptions = BinFactoryOptions.TimeOptions.TimePeriod;
                aggDataExportViewModel.StartDateDay = aggDataExportViewModel.StartDateDay.Value
                    .AddHours(startHour.Value).AddMinutes(startMinute.Value);
                aggDataExportViewModel.EndDateDay = aggDataExportViewModel.EndDateDay.Value
                    .AddHours(endHour.Value).AddMinutes(endMinute.Value);
            }
            else
            {
                startHour = 0;
                startMinute = 0;
                endHour = 23;
                endMinute = 59;
                aggDataExportViewModel.EndDateDay = aggDataExportViewModel.EndDateDay.Value.AddDays(1);
                    //.AddHours(23).AddMinutes(59);
            }
            List<DayOfWeek> daysOfWeek = new List<DayOfWeek>();
            // three cases 1) Week Day, 2) Week End, 3) time or WeekDay and Weekend Andre
            var whichDaysAreIncluded = 0;
            if (aggDataExportViewModel.Weekends)
            {
                whichDaysAreIncluded = 2;
            }
            if (aggDataExportViewModel.Weekdays)
            {
                whichDaysAreIncluded++;
            }
            if (aggDataExportViewModel.SelectedXAxisType.Value == XAxisType.Time)
            {
                whichDaysAreIncluded = 3;
            }
            daysOfWeek = getRequiredDayNames(whichDaysAreIncluded, aggDataExportViewModel.StartDateDay.Value.Date,
                aggDataExportViewModel.EndDateDay.Value.Date);

            BinFactoryOptions.BinSize binSize = (BinFactoryOptions.BinSize)aggDataExportViewModel.SelectedBinSize;
            switch (aggDataExportViewModel.SelectedXAxisType)
            {
                case XAxisType.Time:
                    {
                        timeOptions = BinFactoryOptions.TimeOptions.StartToEnd;
                        break;
                    }
                case XAxisType.TimeOfDay:
                    {
                        timeOptions = BinFactoryOptions.TimeOptions.TimePeriod;
                        break;
                    }
                default:
                    {
                        timeOptions = BinFactoryOptions.TimeOptions.StartToEnd;
                        break;
                    }
            }

            options.TimeOptions = new BinFactoryOptions(
                aggDataExportViewModel.StartDateDay.Value,
                aggDataExportViewModel.EndDateDay.Value,
                startHour, startMinute, endHour, endMinute, daysOfWeek,
                binSize,
                timeOptions);
        }

        private static List<DayOfWeek> getRequiredDayNames(int whichDaysAreIncluded, DateTime? startDateDay, DateTime? endDateDay)
        {
            var numberOfDays = endDateDay - startDateDay;
            int days = numberOfDays.Value.Days + 1;
            List<DayOfWeek> correctDaysOfWeek = new List<DayOfWeek>();
            var dayNumber = "";
            switch (days)
            {
                case 0:
                    dayNumber = "One Day";
                    break;
                case 1:
                    dayNumber = "One Day";
                    break;
                case 2:
                    dayNumber = "Two Days";
                    break;
                case 3:
                    dayNumber = "Three Days";
                    break;
                case 4:
                    dayNumber = "Four Days";
                    break;
                case 5:
                    dayNumber = "Five Days";
                    break;
                case 6:
                    dayNumber = "Six Days";
                    break;
                default:
                    dayNumber = "Seven Days";
                    break;
            }
            var daysIncluded = "";
            switch (whichDaysAreIncluded)
            {
                case 1:
                    daysIncluded = "WeekDay";
                    break;
                case 2:
                    daysIncluded = "WeekEnd";
                    break;
                case 3:
                    daysIncluded = "AllDays";
                    break;
            }

            switch (dayNumber)
            {
                case "One Day":
                    switch (daysIncluded)
                    {
                        case "WeekDay":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Saturday:
                                    break;
                                case DayOfWeek.Sunday:
                                    break;
                            }
                            break;
                        case "WeekEnd":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    break;
                                case DayOfWeek.Tuesday:
                                    break;
                                case DayOfWeek.Wednesday:
                                    break;
                                case DayOfWeek.Thursday:
                                    break;
                                case DayOfWeek.Friday:
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday });
                                    break;
                            }
                            break;
                        case "AllDays":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday });
                                    break;
                            }
                            break;
                    }
                    break;
                case "Two Days":
                    switch (daysIncluded)
                    {
                        case "WeekDay":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday, DayOfWeek.Thursday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Saturday:
                                    break;
                                case DayOfWeek.Sunday:
                                    break;
                            }
                            break;
                        case "WeekEnd":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    break;
                                case DayOfWeek.Tuesday:
                                    break;
                                case DayOfWeek.Wednesday:
                                    break;
                                case DayOfWeek.Thursday:
                                    break;
                                case DayOfWeek.Friday:
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday });
                                    break;
                            }
                            break;
                        case "AllDays":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday, DayOfWeek.Thursday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday, DayOfWeek.Saturday });
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday });
                                    break;
                            }
                            break;
                    }
                    break;
                case "Three Days":
                    switch (daysIncluded)
                    {
                        case "WeekDay":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Saturday:
                                    break;
                                case DayOfWeek.Sunday:
                                    break;
                            }
                            break;
                        case "WeekEnd":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    break;
                                case DayOfWeek.Tuesday:
                                    break;
                                case DayOfWeek.Wednesday:
                                    break;
                                case DayOfWeek.Thursday:
                                    break;
                                case DayOfWeek.Friday:
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday });
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday });
                                    break;
                            }
                            break;
                        case "AllDays":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday });
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday });
                                    break;
                            }
                            break;
                    }
                    break;
                case "Four Days":
                    switch (daysIncluded)
                    {
                        case "WeekDay":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Saturday:
                                    break;
                                case DayOfWeek.Sunday:
                                    break;
                            }
                            break;
                        case "WeekEnd":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    break;
                                case DayOfWeek.Tuesday:
                                    break;
                                case DayOfWeek.Wednesday:
                                    break;
                                case DayOfWeek.Thursday:
                                    break;
                                case DayOfWeek.Friday:
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday });
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday });
                                    break;
                            }
                            break;
                        case "AllDays":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday });
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday });
                                    break;
                            }
                            break;
                    }
                    break;
                case "Five Days":
                    switch (daysIncluded)
                    {
                        case "WeekDay":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Saturday:
                                    break;
                                case DayOfWeek.Sunday:
                                    break;
                            }
                            break;
                        case "WeekEnd":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    break;
                                case DayOfWeek.Tuesday:
                                    break;
                                case DayOfWeek.Wednesday:
                                    break;
                                case DayOfWeek.Thursday:
                                    break;
                                case DayOfWeek.Friday:
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday });
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday });
                                    break;
                            }
                            break;
                        case "AllDays":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday });
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });
                                    break;
                            }
                            break;
                    }
                    break;
                case "Six Days":
                    switch (daysIncluded)
                    {
                        case "WeekDay":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Monday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Monday, DayOfWeek.Tuesday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday });
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                            }
                            break;
                        case "WeekEnd":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    break;
                                case DayOfWeek.Tuesday:
                                    break;
                                case DayOfWeek.Wednesday:
                                    break;
                                case DayOfWeek.Thursday:
                                    break;
                                case DayOfWeek.Friday:
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday });
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday });
                                    break;
                            }
                            break;
                        case "AllDays":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday });
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                            }
                            break;
                    }
                    break;
                case "Seven Days":
                    switch (daysIncluded)
                    {
                        case "WeekDay":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Monday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Monday, DayOfWeek.Tuesday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                            }
                            break;
                        case "WeekEnd":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    break;
                                case DayOfWeek.Tuesday:
                                    break;
                                case DayOfWeek.Wednesday:
                                    break;
                                case DayOfWeek.Thursday:
                                    break;
                                case DayOfWeek.Friday:
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday });
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Saturday });
                                    break;
                            }
                            break;
                        case "AllDays":
                            switch (startDateDay.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday });
                                    break;
                                case DayOfWeek.Tuesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday });
                                    break;
                                case DayOfWeek.Wednesday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday });
                                    break;
                                case DayOfWeek.Thursday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday });
                                    break;
                                case DayOfWeek.Friday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });
                                    break;
                                case DayOfWeek.Saturday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                                    break;
                                case DayOfWeek.Sunday:
                                    correctDaysOfWeek.AddRange(new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday });
                                    break;
                            }
                            break;
                    }
                    break;
            }
            return correctDaysOfWeek;
        }

        // GET: DataExportViewModels
        public ActionResult Index()
        {
            AggDataExportViewModel viewModel = new AggDataExportViewModel();
            viewModel.SelectedMetricTypeId = 20;
            SetAggregateDataViewModelLists(viewModel);
            viewModel.SelectedChartType = SeriesChartType.StackedColumn.ToString();
            viewModel.SelectedBinSize = 0;
            viewModel.StartDateDay = Convert.ToDateTime("10/17/2017");
            viewModel.EndDateDay = Convert.ToDateTime("10/18/2017");
            viewModel.Weekdays = true;
            viewModel.Weekends = true;
            return View(viewModel);
        }

        private void SetAggregateDataViewModelLists(AggDataExportViewModel viewModel)
        {
            var routeRepository = MOE.Common.Models.Repositories.RouteRepositoryFactory.Create();
            viewModel.SetSeriesTypes();
            viewModel.SetAggregationTypes();
            viewModel.SetBinSizeList();
            viewModel.SetChartTypes();
            viewModel.SetDefaultDates();
            viewModel.SetDimensions();
            viewModel.SetSeriesWidth();
            viewModel.SetXAxisTypes();
            viewModel.SetDirectionTypes();
            viewModel.SetMovementTypes();
            viewModel.SetAggregateData();
            viewModel.Routes = routeRepository.GetAllRoutes();
            viewModel.MetricTypes = metricTyperepository.GetAllToAggregateMetrics();
        }

        private int[] SplitHourMinute(String timeFromFrontEnd)
        {
            int[] HourMinute = new int[] { 0, 0 };
            string[] splitted = timeFromFrontEnd.Split(':');
            int.TryParse(splitted[0], out HourMinute[0]);
            int.TryParse(splitted[1], out HourMinute[1]);
            return HourMinute;
        }


        public static List<int> StringToIntList(string str)
        {
            return str.Split(',').Select(int.Parse).ToList();
        }

        public ActionResult GetAggregateDataTypes(int id)
        {
            List<AggregatedDataType> AggregatedDataTypes;
            switch (id)
            {
                case 16:
                    AggregatedDataTypes = new DetectorVolumeAggregationOptions().AggregatedDataTypes;
                    break;
                case 25:
                    AggregatedDataTypes = new ApproachSpeedAggregationOptions().AggregatedDataTypes;
                    break;
                case 18:
                    AggregatedDataTypes = new ApproachPcdAggregationOptions().AggregatedDataTypes;
                    break;
                case 19:
                    AggregatedDataTypes = new PhaseCycleAggregationOptions().AggregatedDataTypes;
                    break;
                case 20:
                    AggregatedDataTypes = new ApproachSplitFailAggregationOptions().AggregatedDataTypes;
                    break;
                case 26:
                    AggregatedDataTypes = new ApproachYellowRedActivationsAggregationOptions().AggregatedDataTypes;
                    break;
                case 22:
                    AggregatedDataTypes = new PreemptionAggregationOptions().AggregatedDataTypes;
                    break;
                case 24:
                    AggregatedDataTypes = new PriorityAggregationOptions().AggregatedDataTypes;
                    break;
                case 27:
                    AggregatedDataTypes = new SignalEventCountAggregationOptions().AggregatedDataTypes;
                    break;
                //case 28:
                //    AggregatedDataTypes = new ApproachEventCountAggregationOptions().AggregatedDataTypes;
                //    break;
                case 29:
                    AggregatedDataTypes = new PhaseTerminationAggregationOptions().AggregatedDataTypes;
                    break;
                case 30:
                    AggregatedDataTypes = new PhasePedAggregationOptions().AggregatedDataTypes;
                    break;
                case 34:
                    AggregatedDataTypes = new PhaseLeftTurnGapAggregationOptions().AggregatedDataTypes;
                    break;
                case 35:
                    AggregatedDataTypes = new PhaseSplitMonitorAggregationOptions().AggregatedDataTypes;
                    break;
                default:
                    throw new Exception("Invalid Metric Type");
            }
            return PartialView(AggregatedDataTypes);
        }
    }
}
