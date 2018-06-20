using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using MOE.Common.Models;
using Owin;

namespace MOE.Common.Business.SiteSecurity
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
                LoginPath = new PathString("/Home/Login")
            });
        }
    }
}