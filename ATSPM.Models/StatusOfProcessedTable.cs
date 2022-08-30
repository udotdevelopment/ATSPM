using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class StatusOfProcessedTable
    {
        public int Id { get; set; }
        public string PartitionedTableName { get; set; }
        public DateTime? TimeEntered { get; set; }
        public string PartitionName { get; set; }
        public int? PartitionYear { get; set; }
        public int? PartitionMonth { get; set; }
        public string FunctionOrProcedure { get; set; }
        public string SqlstatementOrMessage { get; set; }
        public string Notes { get; set; }
    }
}
