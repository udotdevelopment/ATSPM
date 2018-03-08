using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IDatabaseArchiveExcludedSignalsRepository
    {
        List<DatabaseArchiveExcludedSignal> GetAllExcludedSignals();
        DatabaseArchiveExcludedSignal GetExcludedSignalBySignalId(string signalId);
        void DeleteFromExcludedList(string signalId);
        void AddToExcludedList(string signalId);
    }
}