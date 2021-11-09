using Microsoft.Owin;
using Owin;
using SPM.Models;

[assembly: OwinStartupAttribute("Startup",typeof(SPM.Startup))]
namespace SPM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AutoMapperWebConfiguration.Configure();
            ConfigureAuth(app);
            app.CreatePerOwinContext<MOE.Common.Business.SiteSecurity.SPMUserManager>(MOE.Common.Business.SiteSecurity.SPMUserManager.Create);
            
            
        }
    }
}
