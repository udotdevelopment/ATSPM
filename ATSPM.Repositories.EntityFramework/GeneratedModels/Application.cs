using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class Application
    {
        public Application()
        {
            ApplicationSettings = new HashSet<ApplicationSetting>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApplicationSetting> ApplicationSettings { get; set; }
    }
}
