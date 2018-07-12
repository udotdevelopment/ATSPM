using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.Common.Models.ViewModel.DatabaseArchive;
using SPM.Filters;

namespace SPM.Controllers
{

    [Authorize(Roles = "Admin")]
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
            var databaseArchiveExcludedSignal =
                MOE.Common.Models.Repositories.DatabaseArchiveExcludedSignalsRepositoryFactory.Create();
            archiveSettingsViewModel.ExcludedSignals = databaseArchiveExcludedSignal.GetAllExcludedSignals();
            //archiveSettingsViewModel.DatabaseArchiveSettings.SelectedTableScheme = null;
            //archiveSettingsViewModel.DatabaseArchiveSettings.SelectedDeleteOrMove = null;
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

        // POST: DatabaseArchiveSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult RemoveSignal(string id)
        {
            var excludedSignalRepository = DatabaseArchiveExcludedSignalsRepositoryFactory.Create();
            if (!String.IsNullOrEmpty(id) && excludedSignalRepository.Exists(id))
            {
                excludedSignalRepository.DeleteFromExcludedList(id);
                return Content("Signal Removed");
            }
            return Content("Unable to Remove Signal");
        }

        public ActionResult AddSignal(string id)
        {
            var signalRepository = SignalsRepositoryFactory.Create();
            if (!String.IsNullOrEmpty(id) && !signalRepository.Exists(id))
            {
                DatabaseArchiveExcludedSignal databaseArchiveExcludedSignal = new DatabaseArchiveExcludedSignal();
                databaseArchiveExcludedSignal.SignalId = id;
                databaseArchiveExcludedSignal.SignalDescription = signalRepository.GetSignalDescription(id);
                var excludedSignalRepository = DatabaseArchiveExcludedSignalsRepositoryFactory.Create();
                excludedSignalRepository.AddToExcludedList(id);
                return PartialView("EditorTemplates/DatabaseArchiveExcludedSignal", databaseArchiveExcludedSignal);
            }
            return Content("Unable to load signal");
        }
    }
}
