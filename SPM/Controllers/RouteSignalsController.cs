using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Models;
using MOE.Common.Models.ViewModel.Chart;
using SPM.Filters;

namespace SPM.Controllers
{

    [Authorize(Roles = "Configuration, Admin, Technician")]
    public class RouteSignalsController : Controller
    {
        MOE.Common.Models.Repositories.IRouteSignalsRepository routeSignalsRepository = MOE.Common.Models.Repositories.RouteSignalsRepositoryFactory.Create();
        MOE.Common.Models.Repositories.IRouteRepository routeRepository = MOE.Common.Models.Repositories.RouteRepositoryFactory.Create();

        // GET: RouteSignals
        public ActionResult Index(int id)
        {
            MOE.Common.Models.ViewModel.RouteEdit.RouteCreateViewModel routeViewModel = new MOE.Common.Models.ViewModel.RouteEdit.RouteCreateViewModel();
            routeViewModel.Route = routeRepository.GetRouteByID(id);
            var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            foreach (var routeSignal in routeViewModel.Route.RouteSignals)
            {
                if (routeSignal.SignalId != null)
                {
                    Tuple<string, string> tuple = new Tuple<string, string>(routeSignal.Id.ToString(), signalRepository.GetSignalDescription(routeSignal.SignalId));
                    routeViewModel.SignalSelectList.Add(tuple);
                }
            }
            return View(routeViewModel);
        }

        // GET: RouteSignals
        public ActionResult SignalsList(int id)
        {
            var routeSignalRepository = MOE.Common.Models.Repositories.RouteSignalsRepositoryFactory.Create();
            List<RouteSignal> routeSignals = routeSignalRepository.GetByRouteID(id);
            List<Signal> signals = new List<Signal>();
            var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            foreach (var routeSignal in routeSignals)
            {
                signals.Add(signalRepository.GetLatestVersionOfSignalBySignalID(routeSignal.SignalId));
            }
            return PartialView(signals);
        }

        public ActionResult RouteInfoBox(string signalID)
        {
            SignalInfoBoxViewModel viewModel = new SignalInfoBoxViewModel(signalID);
            return PartialView("RouteInfoBox", viewModel);
        }

        public ActionResult RouteMap(MOE.Common.Models.ViewModel.Chart.SignalSearchViewModel ssvm)
        {
            return PartialView(ssvm);
        }


        // POST: RouteSignals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Configuration, Admin")]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RouteId,SignalId")] RouteSignal routeSignal)
        {
            MOE.Common.Models.Repositories.IRouteRepository routeRepository = MOE.Common.Models.Repositories.RouteRepositoryFactory.Create();
            var route = routeRepository.GetRouteByID(routeSignal.RouteId);
            if(route.RouteSignals == null)
            {
                route.RouteSignals = new List<RouteSignal>();
            }
            routeSignal.Order = route.RouteSignals.Count + 1;

            if (TryValidateModel(routeSignal))
            {
                routeSignalsRepository.Add(routeSignal);
            }
            return Content(routeSignal.Id.ToString());
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult ApproachList(int id)
        {
            var routeSignalRepository = MOE.Common.Models.Repositories.RouteSignalsRepositoryFactory.Create();
            var routeSignal = routeSignalRepository.GetByRouteSignalId(id);
            if (routeSignal == null)
            {
                return Content("Signal Not Found");
            }
            if (routeSignal.Signal.Approaches == null)
            {
                return Content("Approaches Not Found");
            }
            return PartialView(routeSignal);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult ApproachesForRoute(int id)
        {
            var route = routeRepository.GetRouteByID(id);
            MOE.Common.Models.ViewModel.RouteEdit.ApproachesForRouteViewModel viewModel = 
                new MOE.Common.Models.ViewModel.RouteEdit.ApproachesForRouteViewModel();
            var signals = route.RouteSignals.OrderBy(r => r.Order);

            foreach (var signal in signals)
            {
                if (signal.PhaseDirections != null)
                {
                    var primaryApproach = signal.PhaseDirections.Where(p => p.IsPrimaryApproach).DefaultIfEmpty(new RoutePhaseDirection()).FirstOrDefault();
                    var opposingApproach = signal.PhaseDirections.Where(p => p.IsPrimaryApproach == false).DefaultIfEmpty(new RoutePhaseDirection()).FirstOrDefault();
                    viewModel.PairedApproaches.Add(new Tuple<RoutePhaseDirection, RoutePhaseDirection>(primaryApproach, opposingApproach));
                }
            }
            return PartialView(viewModel);
        }

        // POST: RouteSignals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Configuration, Admin")]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public void UpdateApproach(RoutePhaseDirection routePhaseDirection)
        {
            if (ModelState.IsValid)
            {
                var routePhaseDirectionRepository = MOE.Common.Models.Repositories.RoutePhaseDirectionRepositoryFactory.Create();
                routePhaseDirectionRepository.Update(routePhaseDirection);
            }
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Configuration, Admin")]
        public void MoveSignalUp(int routeId, int signalId)
        {
            var routePhaseDirectionRepository = MOE.Common.Models.Repositories.RouteSignalsRepositoryFactory.Create();
                routePhaseDirectionRepository.MoveRouteSignalUp(routeId, signalId);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Authorize(Roles = "Configuration, Admin")]
        public void MoveSignalDown(int routeId, int signalId)
        {
            var routePhaseDirectionRepository = MOE.Common.Models.Repositories.RouteSignalsRepositoryFactory.Create();
            routePhaseDirectionRepository.MoveRouteSignalDown(routeId, signalId);
        }

        //// GET: RouteSignals/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RouteSignal routeSignal = db.RouteSignals.Find(id);
        //    if (routeSignal == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(routeSignal);
        //}

        //// GET: RouteSignals/Create
        //public ActionResult Create()
        //{
        //    ViewBag.RouteId = new SelectList(db.Routes, "Id", "RouteName");
        //    return View();
        //}


        //// GET: RouteSignals/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RouteSignal routeSignal = db.RouteSignals.Find(id);
        //    if (routeSignal == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.RouteId = new SelectList(db.Routes, "Id", "RouteName", routeSignal.RouteId);
        //    return View(routeSignal);
        //}

        //// POST: RouteSignals/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,RouteId,Order,SignalId")] RouteSignal routeSignal)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(routeSignal).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.RouteId = new SelectList(db.Routes, "Id", "RouteName", routeSignal.RouteId);
        //    return View(routeSignal);
        //}

        // GET: RouteSignals/Delete/5
        [Authorize(Roles = "Configuration, Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var routeSignalRepository = MOE.Common.Models.Repositories.RouteSignalsRepositoryFactory.Create();
            RouteSignal routeSignal = routeSignalRepository.GetByRouteSignalId(id.Value);
            if (routeSignal == null)
            {
                return HttpNotFound();
            }
            return PartialView(routeSignal);
        }

        // POST: RouteSignals/Delete/5
        [Authorize(Roles = "Configuration, Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateJsonAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var routeSignalRepository = MOE.Common.Models.Repositories.RouteSignalsRepositoryFactory.Create();
            routeSignalRepository.DeleteById(id);
            return Content("Delete Successful!");
        }
    }
}
