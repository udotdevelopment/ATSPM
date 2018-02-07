using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models
{
    public enum TablePartition
        {
            Off,
            PartitionTables,
            NonPartitionTables
        };
    public enum DeleteOrMove
    {
        Delete,
        Move
    }
    public class DatabaseArchiveSettings : ApplicationSettings
    {
       
        [Display(Name= "Use Table Partition Or Not")]
        [NotMapped ]
        public List<TablePartition> TablePartitionsList { get; set; }
        public TablePartition SelectedTablePartition { get; set; }
        [Display(Name = "Remove Index after how many months:")]
        public int MonthsToRemoveIndex { get; set; }
        [Display(Name = "Move/Remove data after how many months:")]
        public int MonthsToRemoveData { get; set; }
        [Display(Name = "Move to path:")]
        public string ArchivePath { get; set; }
        [NotMapped]
        public List<DeleteOrMove> DeleteOrMoveOptionList { get; set; }
        public DeleteOrMove SelectedDeleteOrMove { get; set; }
        [Display(Name = "Start Time")]
        public int StartTime { get; set; }
        [Display(Name = "End Time")]
        public int EndTime { get; set; }

    }
}
