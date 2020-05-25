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

        public  MOE.Common.Models.DatabaseArchiveSettings DbArchiveSettings { get; set; }

        public List<DatabaseArchiveExcludedSignal> ExcludedSignals { get; set; }

        public ArchiveSettingsViewModel()
        {
            DbArchiveSettings = new DatabaseArchiveSettings();
            //SetTablePartitionList();
            //SetDeletOrMoveList();
        }

        //private void SetDeletOrMoveList()
        //{
        //    DbArchiveSettings.DeleteOrMoveOptionList = new List<DeleteOrMove>();
        //    DbArchiveSettings.DeleteOrMoveOptionList.AddRange(new List<DeleteOrMove> { DeleteOrMove.Delete, DeleteOrMove.Move});
        //}

        //private void SetTablePartitionList()
        //{
        //    DbArchiveSettings.TablePartitionsList = new List<TableScheme>();
        //    DbArchiveSettings.TablePartitionsList.AddRange(new List<TableScheme> { TableScheme.Partitioned, TableScheme.Standard});
        //}

    }
}