using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IDatabaseArchiveExcludedSignalsRepository
    {
        List<DatabaseArchiveExcludedSignals> GetAllExcludedSignals();
        DatabaseArchiveExcludedSignals GetExcludedSignalBySignalId(string signalId);
        void DeleteFromExcludedList(string signalId);
        void AddToExcludedList(string signalId);
    }
}