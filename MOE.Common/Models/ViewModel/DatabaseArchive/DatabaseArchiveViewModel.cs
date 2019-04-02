using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models.ViewModel.Chart;

namespace MOE.Common.Models.ViewModel.DatabaseArchive
{
    public class ArchiveSettingsViewModel
    {
        public MOE.Common.Models.DatabaseArchiveSettings DatabaseArchiveSettings { get; set; }

        public List<DatabaseArchiveExcludedSignal> ExcludedSignals { get; set; } = new List<DatabaseArchiveExcludedSignal>();

        [Required]
        public SignalSearchViewModel SignalSearch { get; set; } = new SignalSearchViewModel();

        public ArchiveSettingsViewModel()
        {

        }

    }
}
