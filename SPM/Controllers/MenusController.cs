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

namespace SPM.Controllers
{
    [Authorize (Roles="Admin")]
    public class MenusController : Controller
    {
        //private MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();
        private MOE.Common.Models.Repositories.IMenuRepository menuRepository =
            MOE.Common.Models.Repositories.MenuRepositoryFactory.Create();

        // GET: Menus
        public ActionResult Index()
        {
            return View(menuRepository.GetAll("SignalPerformanceMetric"));
        }
        [AllowAnonymous]
        public PartialViewResult _MainMenuPartial()
        {
            var menuItems = menuRepository.GetTopLevelMenuItems("SignalPerformanceMetrics");
            List<MOE.Common.Models.ViewModel._MainMenu.MenuItem> items =
                new List<MOE.Common.Models.ViewModel._MainMenu.MenuItem>();
            foreach (var m in menuItems)
            {
                items.Add(new MOE.Common.Models.ViewModel._MainMenu.MenuItem(m));
            }
            return PartialView("_MainMenuPartial", items);
        }

        // GET: Menus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = menuRepository.GetMenuItembyID(id.Value);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: Menus/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "MenuId,MenuName,Controller,Action,MenuLocation,ParentId,Application,DisplayOrder")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                menuRepository.Add(menu);
                return RedirectToAction("Index");
            }

            return View(menu);
        }

        // GET: Menus/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = menuRepository.GetMenuItembyID(id.Value);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "MenuId,MenuName,Controller,Action,MenuLocation,ParentId,Application,DisplayOrder")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                menuRepository.Update(menu);
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        // GET: Menus/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = menuRepository.GetMenuItembyID(id.Value);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            menuRepository.Remove(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}
