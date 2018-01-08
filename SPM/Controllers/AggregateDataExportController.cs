using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Business.WCFServiceLibrary;
using SPM.Models;
using MOE.Common.Models;

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

        // GET: DataExportViewModels
        public ActionResult Index(AggDataExportViewModel vm)
        {
            //MOE.Common.Models.Repositories.ISignalsRepository signalsRepository =
            //    MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            //Signal signal = signalsRepository.GetSignalVersionByVersionId(Convert.ToInt32(versionId));
            //mc.Signal = signal;
            List<MetricType> allMetricTypes = metricTyperepository.GetAllToAggregateMetrics();
            List<DirectionType> allDirectionTypes = directionTypeRepository.GetAllDirections();
            List<MovementType> allMovementTypes = movementTypeRepository.GetAllMovementTypes();
            List<LaneType> allLaneTypes = laneTypeRepository.GetAllLaneTypes();
            vm.AllMetricTypes = allMetricTypes;
            vm.AllApproachTypes = allDirectionTypes;
            vm.AllMovementTypes = allMovementTypes;
            vm.AllLaneTypes = allLaneTypes;
            List<string> WeekdayWeekends = new List<string>();
            WeekdayWeekends.Add("Weekdays");
            WeekdayWeekends.Add("Weekends");
            MOE.Common.Models.Repositories.IRouteRepository routeRepository =
                MOE.Common.Models.Repositories.RouteRepositoryFactory.Create();
            vm.Routes = routeRepository.GetAllRoutes();

            MOE.Common.Business.WCFServiceLibrary.AggregationMetricOptions options
                = SetOptionFromVm(vm);
            if (Request.Form["Create"] != null)
            {
                //Create agg data export report
                MOE.Common.Business.ChartFactory.CreateLaneByLaneAggregationChart(options);
            }
            return View(vm);
        }

        private AggregationMetricOptions SetOptionFromVm(AggDataExportViewModel vm)
        {
            MOE.Common.Business.WCFServiceLibrary.AggregationMetricOptions options
                = new AggregationMetricOptions();
            //options.Approaches = vm.ApproachTypeIDs;
            //options.BinSize = vm.SelectedBinSize;
            //options.ChartType = vm.ChartType;
            //options.GroupBy = ;
            //options.Detectors = ;
            //options.Signals = ;
            options.EndDate = vm.EndDateDay;
            //options.MetricTypeID = ;
            //options.SignalID = ;
            options.StartDate = vm.StartDateDay;
            return options;

        }

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
