using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.ViewModel.DatabaseArchive
{
    public class ArchiveSettingsViewModel
    {
        public MOE.Common.Models.DatabaseArchiveSettings DatabaseArchiveSettings { get; set; }

        public List<DatabaseArchiveExcludedSignal> ExcludedSignals { get; set; } = new List<DatabaseArchiveExcludedSignal>();

        public ArchiveSettingsViewModel()
        {
        }

    }
}
