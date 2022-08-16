using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetUserRoles = new HashSet<AspNetUserRole>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Discriminator { get; set; }

        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}
