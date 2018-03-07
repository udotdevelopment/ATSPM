using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models
{
    //public enum UseArchive
    //{
    //    Yes,
    //    No
    //}
    public enum TableScheme
    {
        Partitioned,
        Standard
    }

    public enum DeleteOrMove
    {
        Delete,
        Move
    }

    public class DatabaseArchiveSettings : ApplicationSettings
    {
        [Display(Name = "Enable Database Archive")]
        public bool EnableDatbaseArchive { get; set; }

        public TableScheme? SelectedTableScheme { get; set; }

        [Display(Name = "Remove Index after how many months:")]
        public int? MonthsToKeepIndex { get; set; }

        [Display(Name = "Move/Remove data after how many months:")]
        public int? MonthsToKeepData { get; set; }

        [Display(Name = "Move to path:")]
        public string ArchivePath { get; set; }

        public DeleteOrMove? SelectedDeleteOrMove { get; set; }

        [Display(Name = "Number Of Rows To Delete/Move At A Time")]
        public int? NumberOfRows { get; set; }

        [Display(Name = "Start Hour (0-23)")]
        public int? StartTime { get; set; }

        [Display(Name = "Hour Duration(1-24)")]
        public int? TimeDuration { get; set; }
    }
}