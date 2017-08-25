using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute("Startup", typeof(SPMUserAndRoleManagement.Startup))]
namespace SPMUserAndRoleManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
