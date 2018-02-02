using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SPM.Models;

namespace SPM.Controllers
{
    public class ArchiveSettingsController : Controller
    {
        // GET: ArchiveSettings
        public ActionResult Index()
        {
            ArchiveSettingsViewModel viewModel = new ArchiveSettingsViewModel();
            return View(viewModel);
        }


        // GET: ArchiveSettings/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ArchiveSettings/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
