namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MOE.Common.Models;

    public partial class ApproachRoute
    {
        public List<Models.Approach> GetRouteNonMember()
        {
            MOE.Common.Models.Repositories.IApproachRepository sr = 
                MOE.Common.Models.Repositories.ApproachRepositoryFactory.Create();
            var members = from r in ApproachRouteDetails
                          select r.Approach;
            return sr.GetAllApproaches().Except(members).ToList();
        }

    }
}