using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Models;

namespace SPM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GeneralSettingsController : Controller
    {
        private MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();

        // GET: GeneralSettings/Edit/5
        public ActionResult Edit()
        {
            MOE.Common.Models.Repositories.IApplicationSettingsRepository repository =
                    MOE.Common.Models.Repositories.ApplicationSettingsRepositoryFactory.Create();
            GeneralSettings generalSettings = repository.GetGeneralSettings();
            if (generalSettings == null)
            {
                return HttpNotFound();
            }
            return View(generalSettings);
        }

        // POST: GeneralSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GeneralSettings generalSettings)
        {
            if (ModelState.IsValid)
            {
                MOE.Common.Models.Repositories.IApplicationSettingsRepository repository =
                    MOE.Common.Models.Repositories.ApplicationSettingsRepositoryFactory.Create();
                repository.Save(generalSettings);
            }
            
            ViewBag.ApplicationID = new SelectList(db.Applications, "ID", "Name", generalSettings.ApplicationID);
            return View(generalSettings);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
