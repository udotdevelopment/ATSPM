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
    public class DatabaseArchiveSettingsController : Controller
    {

        // GET: DatabaseArchiveSettings/Edit/5
        public ActionResult Edit()
        {
            var applicationSettingsRepository =
                MOE.Common.Models.Repositories.ApplicationSettingsRepositoryFactory.Create();
            DatabaseArchiveSettings databaseArchiveSettings =
                applicationSettingsRepository.GetDatabaseArchiveSettings();
            if (databaseArchiveSettings == null)
            {
                return HttpNotFound();
            }
            return View(databaseArchiveSettings);
        }

        // POST: DatabaseArchiveSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( DatabaseArchiveSettings databaseArchiveSettings)
        {
            if (ModelState.IsValid)
            {
                var applicationSettingsRepository =
                    MOE.Common.Models.Repositories.ApplicationSettingsRepositoryFactory.Create();
                applicationSettingsRepository.Save(databaseArchiveSettings);
                return View(databaseArchiveSettings);
            }
            return View(databaseArchiveSettings);
        }
    }
}
