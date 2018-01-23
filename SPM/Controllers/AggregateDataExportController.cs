using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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

        MOE.Common.Models.Repositories.IControllerEventLogRepository cr =
            MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
        //private ApplicationDbContext db = new ApplicationDbContext();

        //public ActionResult SignalSearch(SignalSearchViewModel ssvm)
        //{
        //    return PartialView("SignalSearch", ssvm);
        //}

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
            options.AggregationOpperation = AggregationMetricOptions.AggregationOpperations.Sum;
            options.XAxisAggregationSeriesOption = AggregationMetricOptions.XAxisAggregationSeriesOptions.Time;
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
                BinFactoryOptions.BinSizes.Hour,
                timeOptions);
            foreach (var signal in aggDataExportViewModel.Signals)
            {
                options.SignalIds.Add(signal.SignalID);
            }
            options.ChartType = AggregationMetricOptions.ChartTypes.Column;
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
                AggDataExportViewModel viewModel = new AggDataExportViewModel();
                var routeRepository = MOE.Common.Models.Repositories.RouteRepositoryFactory.Create();
                viewModel.Routes = routeRepository.GetAllRoutes();

            //MOE.Common.Models.Repositories.ISignalsRepository signalsRepository =
            //    MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            //Signal signal = signalsRepository.GetSignalVersionByVersionId(Convert.ToInt32(versionId));
            //mc.Signal = signal;
            List<MetricType> allMetricTypes = metricTyperepository.GetAllToAggregateMetrics();
            foreach (var metricType in allMetricTypes)
            {
                viewModel.MetricItems.Add(metricType.MetricID, metricType.ChartName);
            }
            List<DirectionType> allDirectionTypes = directionTypeRepository.GetAllDirections();
            List<MovementType> allMovementTypes = movementTypeRepository.GetAllMovementTypes();
            List<LaneType> allLaneTypes = laneTypeRepository.GetAllLaneTypes();
            viewModel.AllMetricTypes = allMetricTypes;
                viewModel.AllApproachTypes = allDirectionTypes;
                viewModel.AllMovementTypes = allMovementTypes;
                viewModel.AllLaneTypes = allLaneTypes;
            //List<string> WeekdayWeekends = new List<string>();
            //WeekdayWeekends.Add("Weekdays");
            //WeekdayWeekends.Add("Weekends");
            //MOE.Common.Models.Repositories.IRouteRepository routeRepository =
            //    MOE.Common.Models.Repositories.RouteRepositoryFactory.Create();
            //vm.Routes = routeRepository.GetAllRoutes();

            ////MOE.Common.Business.WCFServiceLibrary.AggregationMetricOptions options
            ////    = SetOptionFromVm(vm);
            //if (Request.Form["Create"] != null)
            //{
            //    //Create agg data export report
            //        switch (vm.SelectedMetric)
            //        {
            //            case 20:
            //            //    AoROptions options = new AoROptions();
            //            //CreateArrivalOnGreenAggregationChart()
            //                //BinFactoryOptions.BinSizes selectedBinSize = vm.SelectedBinSize;

            //                List<DayOfWeek> daysOfWeek = new List<DayOfWeek>();
            //                if (vm.Weekdays)
            //                {
            //                    daysOfWeek.Add(DayOfWeek.Monday);
            //                    daysOfWeek.Add(DayOfWeek.Tuesday);
            //                    daysOfWeek.Add(DayOfWeek.Wednesday);
            //                    daysOfWeek.Add(DayOfWeek.Thursday);
            //                    daysOfWeek.Add(DayOfWeek.Friday);
            //                }
            //                if (vm.Weekends)
            //                {
            //                    daysOfWeek.Add(DayOfWeek.Saturday);
            //                    daysOfWeek.Add(DayOfWeek.Sunday);
            //                }
            //                ApproachSplitFailAggregationOptions options = new ApproachSplitFailAggregationOptions();
            //                options.StartDate = vm.StartDateDay;
            //                options.EndDate = vm.EndDateDay;
            //                int startHour, startMinute, endHour, endMinute;
            //                BinFactoryOptions.TimeOptions timeOption = BinFactoryOptions.TimeOptions.StartToEnd;
            //                if (vm.StartTime != null)
            //                {
            //                    timeOption = BinFactoryOptions.TimeOptions.TimePeriod;
            //                    int[] hourMin = SplitHourMinute(vm.StartTime);
            //                    startHour = hourMin[0];
            //                    startMinute = hourMin[1];
            //                    if (vm.SelectedStartAMPM.ToUpper().Contains("PM") && startHour < 12)
            //                    {
            //                        startHour += 12;
            //                    }
            //                }
            //                if (vm.EndTime != null)
            //                {
            //                    timeOption = BinFactoryOptions.TimeOptions.TimePeriod;
            //                    int[] hourMin = SplitHourMinute(vm.EndTime);
            //                    endHour = hourMin[0];
            //                    endMinute = hourMin[1];
            //                    if (vm.SelectedEndAMPM.ToUpper().Contains("PM") && endHour < 12)
            //                    {
            //                        endHour += 12;
            //                    }
            //                }

            //            //    options.AggregationOpperation = vm.IsSum
            //            //    ? AggregationMetricOptions.AggregationOpperations.Sum
            //            //    : AggregationMetricOptions.AggregationOpperations.Average;
            //            //options.TimeOptions = new BinFactoryOptions(vm.StartDateDay, vm.EndDateDay,
            //            //    (vm.StartTime!=null) ? startHour : null, (vm.StartTime!=null) ? startMinute : null,
            //            //    (vm.EndTime!=null) ? endHour : null, (vm.EndTime!=null) ? endMinute : null,
            //            //    daysOfWeek, 
            //            //    )
            //            //    );


            //                break;
            //        }

            //}
            return View(viewModel);
        }

        private int[] SplitHourMinute(String timeFromFrontEnd)
        {
            int[] HourMinute = new int[]{0, 0};
            string[] splitted = timeFromFrontEnd.Split(':');
            int.TryParse(splitted[0], out HourMinute[0]);
            int.TryParse(splitted[1], out HourMinute[1]);
            return HourMinute;
        }

        //private AggregationMetricOptions SetOptionFromVm(AggDataExportViewModel vm)
        //{
        //    //MOE.Common.Business.WCFServiceLibrary.AggregationMetricOptions options
        //    //    = new AggregationMetricOptions();
        //    ////options.Approaches = vm.ApproachTypeIDs;
        //    ////options.BinSize = vm.SelectedBinSize;
        //    ////options.ChartType = vm.ChartType;
        //    ////options.GroupBy = ;
        //    ////options.Detectors = ;
        //    ////options.Signals = ;
        //    //options.EndDate = vm.EndDateDay;
        //    ////options.MetricTypeID = ;
        //    ////options.SignalID = ;
        //    //options.StartDate = vm.StartDateDay;
        //    //return options;

        //}

        public ActionResult AggregateDataExport()
        {
            return View();
        }

        public static List<int> StringToIntList(string str)
        {
            return str.Split(',').Select(int.Parse).ToList();
        }

        // POST: DataExportViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AggregateDataExport(AggDataExportViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //int Count = cr.GetEventCountByEventCodesParamDateTimeRange(vm.SignalId, vm.StartDateDate,
                //    vm.EndDateDate, StartHour, StartMinute, EndHour, EndMinute,
                //    inputEventCodes, inputParam);
                //vm.Count = Count;
                return RedirectToAction("AggregateDataExport");
            }

            return View(vm);
        }

        // POST: AggDataExportViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MetricTypeIDs,ApproachTypeIDs,AggSeriesOptions,LaneTypeIDs,Weekdays,Weekend,IsSum,SelectedRouteID,StartDateDay,EndDateDay,StartTime,SelectedStartAMPM,EndDateDay,SelectedEndAMPM")] AggDataExportViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //db.DataExportViewModels.Add(vm);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vm);
        }

        //// GET: DataExportViewModels/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DataExportViewModel dataExportViewModel = db.DataExportViewModels.Find(id);
        //    if (dataExportViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(dataExportViewModel);
        //}

        // POST: DataExportViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "SignalId,StartDateDate,EndDateDate,StartDateHour,StartDateMinute,EndDateHour,EndDateMinute,Count")] DataExportViewModel vm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        vm.Count = cr.GetEventCountByEventCodesParamDateTimeRange(vm.SignalId, vm.StartDateDate, vm.EndDateDate,
        //            vm.StartDateHour, vm.StartDateMinute, vm.EndDateHour, vm.EndDateMinute, eventCodes, param);
        //        return RedirectToAction("Index");
        //    }
        //    return View(dataExportViewModel);
        //}

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
