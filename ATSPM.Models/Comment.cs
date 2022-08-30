using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class Comment
    {
        public long CommentId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Entity { get; set; }
        public int ChartType { get; set; }
        public string Comment1 { get; set; }
        public int? EntityType { get; set; }
    }
}
