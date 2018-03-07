using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.ViewModel.DatabaseArchive
{
    public class ArchiveSettingsViewModel
    {
        public MOE.Common.Models.DatabaseArchiveSettings DbArchiveSettings { get; set; }

        public List<DatabaseArchiveExcludedSignals> ExcludedSignals { get; set; }

        public ArchiveSettingsViewModel()
        {
            DbArchiveSettings = new DatabaseArchiveSettings();
            //SetTablePartitionList();
            //SetDeletOrMoveList();
        }

        //private void SetDeletOrMoveList()
        //{
        //    DbArchiveSettings.DeleteOrMoveOptionList = new List<DeleteOrMove>();
        //    DbArchiveSettings.DeleteOrMoveOptionList.AddRange(new List<DeleteOrMove> { DeleteOrMove.Delete, DeleteOrMove.Move });
        //}

        //private void SetTablePartitionList()
        //{
        //    DbArchiveSettings.TablePartitionsList = new List<TableScheme>();
        //    DbArchiveSettings.TablePartitionsList.AddRange(new List<TableScheme> { TableScheme.Partitioned, TableScheme.Standard });
        //}

    }
}
