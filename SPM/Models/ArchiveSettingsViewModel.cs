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
        //public TablePartition  SelectedTablePartition { get; set; }
        public List<Signal> Signals { get; set; } = new List<Signal>();

         //public DeleteOrMove SelecteDeleteOrMove { get; set; }

        public  MOE.Common.Models.DatabaseArchiveSettings DbArchiveSettings { get; set; }
        public SignalSearchViewModel SignalSearch { get; set; }

        public List<DatabaseArchiveExcludedSignals> ExcludedSignals { get; set; }

        public ArchiveSettingsViewModel()
        {
            DbArchiveSettings = new DatabaseArchiveSettings();
            SetUseArchiveList();
            SetTablePartitionList();
            SetDeletOrMoveList();
            SignalSearch = new SignalSearchViewModel();
        }

        private void SetUseArchiveList()
        {
            //DbArchiveSettings.UseArchiveList = new List<UseArchive>();
            //DbArchiveSettings.UseArchiveList.AddRange(new List<UseArchive> { UseArchive.Yes, UseArchive.No});
        }

        private void SetDeletOrMoveList()
        {
            DbArchiveSettings.DeleteOrMoveOptionList = new List<DeleteOrMove>();
            DbArchiveSettings.DeleteOrMoveOptionList.AddRange(new List<DeleteOrMove> { DeleteOrMove.Delete, DeleteOrMove.Move});
        }

        private void SetTablePartitionList()
        {
            DbArchiveSettings.TablePartitionsList = new List<TablePartition>();
            DbArchiveSettings.TablePartitionsList.AddRange(new List<TablePartition> { TablePartition.PartitionTables, TablePartition.NonPartitionTables});
        }


        protected void SetDefaultTimes()
        {

        }

    }
}