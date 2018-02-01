using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using MOE.Common.Models.Repositories;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.ViewModel.Chart;

namespace SPM.Models
{
    public class ArchiveSettingsViewModel
    {
        public enum TablePartition
        {
            Off,
            PartitionTables,
            NonPartitionTables
        };
        [Display (Name = "Use Table Partition Or Not")]
        public List<TablePartition> TablePartitionsList { get; set; }
        public TablePartition  SelectedTablePartition { get; set; }
        public List<Signal> Signals { get; set; } = new List<Signal>();
        [Display (Name = "Remove Index after how many months:")]
        public int MonthsToRemoveIndex { get; set; }
        [Display(Name = "Move/Remove data after how many months:")]
        public int MonthsToRemoveData { get; set; }
        [Display(Name = "Move to path:")]
        public string  ArchivePath { get; set; }

        public enum DeleteOrMove
        {
            Delete,
            Move
        }
        public List<DeleteOrMove> DeleteOrMoveOptionList { get; set; }
        public DeleteOrMove SelecteDeleteOrMove { get; set; }

        [Display(Name = "Start Time")]
        public string StartTime { get; set; }
        [Display(Name = "End Time")]
        public string EndTime { get; set; }

        public ArchiveSettingsViewModel()
        {
            SetTablePartitionList();
            SetDeletOrMoveList();
        }

        private void SetDeletOrMoveList()
        {
            DeleteOrMoveOptionList = new List<DeleteOrMove>();
            DeleteOrMoveOptionList.AddRange(new List<DeleteOrMove>{DeleteOrMove.Delete, DeleteOrMove.Move});
        }

        private void SetTablePartitionList()
        {
            TablePartitionsList = new List<TablePartition>();
            TablePartitionsList.AddRange(new List<TablePartition>{TablePartition.Off, TablePartition.PartitionTables, TablePartition.NonPartitionTables});
        }


        protected void SetDefaultTimes()
        {

        }

    }
}