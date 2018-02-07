using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models
{
    //public enum UseArchive
    //{
    //    Yes,
    //    No
    //}
    public enum TablePartition
    {
        PartitionTables,
        NonPartitionTables
    }
    public enum DeleteOrMove
    {
        Delete,
        Move
    }
    public class DatabaseArchiveSettings : ApplicationSettings
    {
        [Display(Name = "Archive Or Not")]
        //[NotMapped]
        //public List<UseArchive> UseArchiveList { get; set; }
        public bool SelectedUseArchive { get; set; }
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
        [Display(Name = "Number Of Rows To Delete/Move At A Time")]
        public int NumberOfRows { get; set; }
        [Display(Name = "Start Hour (0-23)")]
        public int StartTime { get; set; }
        [Display(Name = "Hour Duration(1-24)")]
        public int TimeDuration { get; set; }

    }
}
