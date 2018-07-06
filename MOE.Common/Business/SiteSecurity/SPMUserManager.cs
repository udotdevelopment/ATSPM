using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using MOE.Common.Models;

namespace MOE.Common.Business.SiteSecurity
{
    public class SPMUserManager : UserManager<SPMUser>
    {
        public SPMUserManager(IUserStore<SPMUser> store)
            : base(store)
        {
        }

        // this method is called by Owin therefore best place to configure your User Manager
        public static SPMUserManager Create(
            IdentityFactoryOptions<SPMUserManager> options, IOwinContext context)
        {
            var manager = new SPMUserManager(
                new UserStore<SPMUser>(context.Get<SPM>()));

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = false
            };

            return manager;
        }
    }
}