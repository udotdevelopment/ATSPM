using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Business.SiteSecurity;
using MOE.Common.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using SPM.Filters;

namespace SPM.Controllers
{

    [Authorize(Roles = "Admin")]
    public class SPMUsersController : Controller
    {
        // GET: SPMUsers
        public ActionResult Index()
        {
            List<Models.UserViewModel> users = new List<Models.UserViewModel>();
            using (var context = new MOE.Common.Models.SPM())
            {
                var userStore = new UserStore<SPMUser>(context);
                var userManager = new UserManager<SPMUser>(userStore);
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                foreach (SPMUser user in userStore.Users.OrderBy(u=> u.Email).ToList())
                {
                    Models.UserViewModel userViewModel = new Models.UserViewModel();
                    userViewModel.User = user;
                    foreach(IdentityUserRole role in user.Roles)
                    {
                        userViewModel.Roles.Add(roleManager.Roles.Where(r => r.Id == role.RoleId).First().Name);
                    }
                    users.Add(userViewModel);
                }
            }
            return View(users);
        }

        //// GET: SPMUsers/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SPMUser sPMUser = db.SPMUsers.Find(id);
        //    if (sPMUser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(sPMUser);
        //}


        // GET: SPMUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.UserViewModel user = new Models.UserViewModel();
            
            using (var context = new MOE.Common.Models.SPM())
            {
                List<string> roles;
                var userStore = new UserStore<SPMUser>(context);
                var userManager = new UserManager<SPMUser>(userStore);
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                user.User = userManager.Users.Where(u => u.Id == id).FirstOrDefault();
                foreach (IdentityUserRole role in user.User.Roles)
                {
                    user.Roles.Add(roleManager.Roles.Where(r => r.Id == role.RoleId).First().Name);
                }
                roles = (from r in roleManager.Roles select r.Name).ToList();
                ViewBag.Roles = new SelectList(roles.OrderBy(r=>r));
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: SPMUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.UserViewModel sPMUser)
        {
            if (ModelState.IsValid)
            {
                SPMUser user;
                using (var context = new MOE.Common.Models.SPM())
                {
                    try
                    {
                        var userStore = new UserStore<SPMUser>(context);
                        var userManager = new UserManager<SPMUser>(userStore);
                        user = userManager.Users.Where(u => u.Id == sPMUser.User.Id).FirstOrDefault();
                        user.ReceiveAlerts = sPMUser.User.ReceiveAlerts;
                        user.Email = sPMUser.User.Email;
                        user.UserName = sPMUser.User.Email;
                        context.SaveChanges();
                    }
                    catch(Exception ex)
                    {
                        return Content("<h1>" + ex.Message + "</h1>");
                    }
                }
                return RedirectToAction("Index");
            }
            return View(sPMUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult RemoveRoleFromUser(string roleName, string userName)
        {
            using (var context = new MOE.Common.Models.SPM())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var userStore = new UserStore<SPMUser>(context);
                var userManager = new UserManager<SPMUser>(userStore);
                var user = userManager.FindByName(userName);
                if (user == null)
                {
                    return Content("User not found");
                }
                var role = roleManager.FindByName(roleName);
                if (role == null)
                {
                    return Content("Role not found");
                }
                if (!userManager.IsInRole(user.Id, role.Name))
                {
                    return Content("This user does not have this role assigned");
                }
                userManager.RemoveFromRole(user.Id, role.Name);
                context.SaveChanges();
                return Content("Role Removed");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult AddRoleToUser(string roleName, string userName)
        {
            using (var context = new MOE.Common.Models.SPM())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var userStore = new UserStore<SPMUser>(context);
                var userManager = new UserManager<SPMUser>(userStore);
                var user = userManager.FindByName(userName);
                if (user == null)
                {
                    return Content("User not found");
                }
                var role = roleManager.FindByName(roleName);
                if (role == null)
                {
                    return Content("Role not found");
                }
                if (userManager.IsInRole(user.Id, role.Name))
                {
                    return Content("This user already has this role assigned");
                }
                userManager.AddToRole(user.Id, role.Name);
                context.SaveChanges();
                return Content("Role Added");
            }
        }

        // GET: SPMUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPMUser user;
            using (var context = new MOE.Common.Models.SPM())
            {
                var userStore = new UserStore<SPMUser>(context);
                var userManager = new UserManager<SPMUser>(userStore);
                user = userManager.Users.Where(u => u.Id == id).FirstOrDefault();
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: SPMUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SPMUser user;
            using (var context = new MOE.Common.Models.SPM())
            {
                var userStore = new UserStore<SPMUser>(context);
                var userManager = new UserManager<SPMUser>(userStore);
                user = userManager.Users.Where(u => u.Id == id).FirstOrDefault();
                userManager.Delete(user);
            }
            return RedirectToAction("Index");
        }
        
    }
}
