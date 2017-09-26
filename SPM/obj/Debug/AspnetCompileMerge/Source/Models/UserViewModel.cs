using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM.Models
{
    public class UserViewModel
    {
        public MOE.Common.Business.SiteSecurity.SPMUser User { get; set; }
        public List<String> Roles { get; set; }
        public List<IdentityRole> RolesList { get; set; }

        public UserViewModel()
        {
            User = new MOE.Common.Business.SiteSecurity.SPMUser();
            Roles = new List<String>();
            RolesList = new List<IdentityRole>();
        }
    }
}