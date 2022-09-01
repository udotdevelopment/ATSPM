using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Extensions.Logging;
using MOE.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace SPM.Controllers
{
    [Authorize(Roles = "Configuration, Admin")]
    public class AreasController : Controller
    {
        MOE.Common.Models.Repositories.IAreaRepository areaRepository = MOE.Common.Models.Repositories.AreaRepositoryFactory.Create();
        MOE.Common.Models.Repositories.ISignalsRepository signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();

        // GET: Area
        public ActionResult Index()
        {
            return View(areaRepository.GetAllAreas());
        }

        [Authorize(Roles = "Configuration, Admin")]
        public ActionResult Create()
        {
            Area area = new Area();
            return View(area);
        }

        // POST: Area/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Configuration, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, AreaName")] Area area)
        {
            if (ModelState.IsValid)
            {
                areaRepository.Add(area);
                return RedirectToAction("Index");
            }

            return View(area);
        }

        // GET: Area/Edit/5
        [Authorize(Roles = "Configuration, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = areaRepository.GetAreaByID(id.Value);
            area.Signals = area.Signals.ToList();
            var ids = new List<int>();
            if (area.Signals != null && area.Signals.FirstOrDefault() != null)
            {
                foreach (var s in area.Signals)
                {
                    ids.Add(s.VersionID);
                }
            }
            ViewBag.SignalIds = ids;
            //ViewBag.Signals = new MultiSelectList(signalRepository.GetAllSignals(), "SignalId", "PrimaryName");
            ViewBag.Signals = new MultiSelectList(areaRepository.GetAllAreas(), "Id", "AreaName");


            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }
        // POST: Area/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Configuration, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, AreaName")] Area area)
        {
            if (ModelState.IsValid)
            {
                areaRepository.Update(area);
                return RedirectToAction("Index");
            }
            return View(area);
        }

        // GET: Area/Delete/5
        [Authorize(Roles = "Configuration, Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = areaRepository.GetAreaByID(id.Value);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // POST: Area/Delete/5
        [Authorize(Roles = "Configuration, Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            areaRepository.DeleteByID(id);
            return RedirectToAction("Index");
        }

    }
}
