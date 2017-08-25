using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace MOE.Common.Business.SiteSecurity
{
    public class IdentityConfig
    {
        public void Configuration(Owin.IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new Models.SPM());
            app.CreatePerOwinContext<SPMUserManager>(SPMUserManager.Create);
            app.CreatePerOwinContext<RoleManager<SPMRole>>((options, context) =>
                new RoleManager<SPMRole>(
                    new RoleStore<SPMRole>(context.Get<MOE.Common.Models.SPM>())));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login"),
            });
        }
    }
}
