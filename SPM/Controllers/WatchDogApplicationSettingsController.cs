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
    public class WatchDogApplicationSettingsController : Controller
    {
        private MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();



        // GET: WatchDogApplicationSettings/Edit/5
        public ActionResult Edit()
        {
            MOE.Common.Models.Repositories.IApplicationSettingsRepository repository =
                    MOE.Common.Models.Repositories.ApplicationSettingsRepositoryFactory.Create();
            WatchDogApplicationSettings watchDogApplicationSettings = repository.GetWatchDogSettings();
            if (watchDogApplicationSettings == null)
            {
                return HttpNotFound();
            }
            return View(watchDogApplicationSettings);
        }

        // POST: WatchDogApplicationSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ApplicationID,ConsecutiveCount,MinPhaseTerminations,PercentThreshold,MaxDegreeOfParallelism,ScanDayStartHour,ScanDayEndHour,PreviousDayPMPeakStart,PreviousDayPMPeakEnd,MinimumRecords,WeekdayOnly,DefaultEmailAddress,FromEmailAddress,LowHitThreshold,EmailServer,MaximumPedestrianEvents,EmailAllErrors")] WatchDogApplicationSettings watchDogApplicationSettings)
        {


            if (ModelState.IsValid)
            {
                MOE.Common.Models.Repositories.IApplicationSettingsRepository repository =
                    MOE.Common.Models.Repositories.ApplicationSettingsRepositoryFactory.Create();
                repository.Save(watchDogApplicationSettings);
            }
            

            ViewBag.ApplicationID = new SelectList(db.Applications, "ID", "Name", watchDogApplicationSettings.ApplicationID);
            return View(watchDogApplicationSettings);
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
