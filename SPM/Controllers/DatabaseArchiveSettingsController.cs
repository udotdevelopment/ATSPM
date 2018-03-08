using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Models;
using MOE.Common.Models.ViewModel.DatabaseArchive;

namespace SPM.Controllers
{
    public class DatabaseArchiveSettingsController : Controller
    {

        // GET: DatabaseArchiveSettings/Edit/5
        public ActionResult Edit()
        {
            MOE.Common.Models.ViewModel.DatabaseArchive.ArchiveSettingsViewModel archiveSettingsViewModel = new ArchiveSettingsViewModel();
            var applicationSettingsRepository =
                MOE.Common.Models.Repositories.ApplicationSettingsRepositoryFactory.Create();
            archiveSettingsViewModel.DatabaseArchiveSettings =
                applicationSettingsRepository.GetDatabaseArchiveSettings();
            archiveSettingsViewModel.ExcludedSignals.Add(new DatabaseArchiveExcludedSignal{ Id = 1, SignalId = "7063"});
            archiveSettingsViewModel.ExcludedSignals.Add(new DatabaseArchiveExcludedSignal { Id = 1, SignalId = "7064" });
            if (archiveSettingsViewModel.DatabaseArchiveSettings == null)
            {
                return HttpNotFound();
            }
            return View(archiveSettingsViewModel);
        }

        // POST: DatabaseArchiveSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MOE.Common.Models.ViewModel.DatabaseArchive.ArchiveSettingsViewModel archiveSettingsViewModel)
        {
            if (ModelState.IsValid)
            {
                var applicationSettingsRepository =
                    MOE.Common.Models.Repositories.ApplicationSettingsRepositoryFactory.Create();
                applicationSettingsRepository.Save(archiveSettingsViewModel.DatabaseArchiveSettings);
                return View(archiveSettingsViewModel);
            }
            return Content("Unable to Save Database Archive Settings");
        }
    }
}
