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
    [Authorize(Roles = "Configuration, Admin")] // need to confirm if we need the Technician role as well
    public class JurisdictionsController : Controller
    {
        MOE.Common.Models.Repositories.IJurisdictionRepository jurisdictionRepository = MOE.Common.Models.Repositories.JurisdictionRepositoryFactory.Create();

        // GET: Jurisdictions
        public ActionResult Index()
        {
            return View(jurisdictionRepository.GetAllJurisdictions());
        }

 /*       // GET: Jurisdictions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var jurisdiction = jurisdictionRepository.GetJurisdictionByID(id.Value);
            if (jurisdiction == null)
            {
                return HttpNotFound();
            }
            return PartialView(jurisdiction);
        }
 */
        [Authorize(Roles = "Configuration, Admin")]
        public ActionResult Create()
        {
            Jurisdiction jurisdiction = new Jurisdiction();
            return View(jurisdiction);
        }

        // POST: Jurisdictions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Configuration, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,JurisdictionName, CountyParish, MPO, OtherPartners")] Jurisdiction jurisdiction)
        {
            if (ModelState.IsValid)
            {
                jurisdictionRepository.Add(jurisdiction);
                return RedirectToAction("Index");
            }

            return View(jurisdiction);
        }

        // GET: Jurisdictions/Edit/5
        [Authorize(Roles = "Configuration, Admin")]
        public ActionResult Edit(int? id)
        {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Jurisdiction jurisdiction = jurisdictionRepository.GetJurisdictionByID(id.Value);
                if (jurisdiction == null)
                {
                    return HttpNotFound();
                }
                return View(jurisdiction);
            }
        // POST: Jurisdictions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Configuration, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,JurisdictionName, CountyParish, MPO, OtherPartners")] Jurisdiction jurisdiction)
        {
            if (ModelState.IsValid)
            {
                jurisdictionRepository.Update(jurisdiction);
                return RedirectToAction("Index");
            }
            return View(jurisdiction);
        }

        // GET: Jurisdictions/Delete/5
        [Authorize(Roles = "Configuration, Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jurisdiction jurisdiction = jurisdictionRepository.GetJurisdictionByID(id.Value);
            if (jurisdiction == null)
            {
                return HttpNotFound();
            }
            return View(jurisdiction);
        }

        // POST: Jurisdictions/Delete/5
        [Authorize(Roles = "Configuration, Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            jurisdictionRepository.DeleteByID(id);
            return RedirectToAction("Index");
        }

    }
}
