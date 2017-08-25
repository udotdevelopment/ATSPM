using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using MOE.Common;
using MOE.Common.Models;
using MOE.Common.Business.SiteSecurity;

namespace SecurityConfiguration.App_Start
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new SPM());
            app.CreatePerOwinContext<SPMUserManager>(SPMUserManager.Create);
            app.CreatePerOwinContext<RoleManager<SPMRole>>((options, context) =>
                new RoleManager<SPMRole>(
                    new RoleStore<SPMRole>(context.Get<SPM>())));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login"),
            });
        }
    }
}