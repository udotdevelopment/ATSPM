using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class ExternalLink
    {
        public int ExternalLinkId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int DisplayOrder { get; set; }
    }
}
