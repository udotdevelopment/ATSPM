using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute("Startup",typeof(SPM.Startup))]
namespace SPM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.CreatePerOwinContext<MOE.Common.Business.SiteSecurity.SPMUserManager>(MOE.Common.Business.SiteSecurity.SPMUserManager.Create);
            
            
        }
    }
}
