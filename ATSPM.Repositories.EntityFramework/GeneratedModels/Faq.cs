using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class Faq
    {
        public int Faqid { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public int OrderNumber { get; set; }
    }
}
