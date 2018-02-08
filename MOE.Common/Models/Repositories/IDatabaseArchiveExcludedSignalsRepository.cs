using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IDatabaseArchiveExcludedSignalsRepository
    {
        List<Models.DatabaseArchiveExcludedSignals> GetAllExcludedSignals();
        DatabaseArchiveExcludedSignals GetExcludedSignalBySignalId(string signalId);
        void DeleteFromExcludedList(string signalId);
        void AddToExcludedList(string signalId);

    }
}
