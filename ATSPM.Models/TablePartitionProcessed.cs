using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class TablePartitionProcessed
    {
        public int Id { get; set; }
        public string SwapTableName { get; set; }
        public int PartitionNumber { get; set; }
        public int PartitionBeginYear { get; set; }
        public int PartitionBeginMonth { get; set; }
        public string FileGroupName { get; set; }
        public string PhysicalFileName { get; set; }
        public bool IndexRemoved { get; set; }
        public bool SwappedTableRemoved { get; set; }
        public DateTime TimeIndexdropped { get; set; }
        public DateTime TimeSwappedTableDropped { get; set; }
    }
}
