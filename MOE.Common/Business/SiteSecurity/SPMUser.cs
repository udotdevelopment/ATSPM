using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MOE.Common.Business.SiteSecurity
{
    public class SPMUser : IdentityUser
    {
        [Display(Name = "Receive Alerts")]
        public bool ReceiveAlerts { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SPMUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    //public class AggregationStausUser : IdentityUser
    //{
    //    [Display(Name = "Receive Alerts")]
    //    public bool ReceiveAlerts { get; set; }

    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SPMUser> manager)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }
    //}




}