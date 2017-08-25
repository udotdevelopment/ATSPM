using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.SiteSecurity
{
    public class SPMRole: IdentityRole
    {
    public SPMRole() : base() { }
    public SPMRole(string name) : base(name) { }
    }
}
