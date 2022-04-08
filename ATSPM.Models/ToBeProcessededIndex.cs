using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class ToBeProcessededIndex
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public int IndexId { get; set; }
        public string ClusterText { get; set; }
        public string TextForIndex { get; set; }
        public string IndexName { get; set; }
    }
}
