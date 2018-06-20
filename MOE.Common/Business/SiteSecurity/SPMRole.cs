using Microsoft.AspNet.Identity.EntityFramework;

namespace MOE.Common.Business.SiteSecurity
{
    public class SPMRole : IdentityRole
    {
        public SPMRole()
        {
        }

        public SPMRole(string name) : base(name)
        {
        }
    }
}