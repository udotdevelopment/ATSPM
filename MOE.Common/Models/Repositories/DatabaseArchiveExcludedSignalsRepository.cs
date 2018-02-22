using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class DatabaseArchiveExcludedSignalsRepository : IDatabaseArchiveExcludedSignalsRepository
    {
        private readonly SPM _db;

        public DatabaseArchiveExcludedSignalsRepository()
        {
            _db = new SPM();
        }

        public DatabaseArchiveExcludedSignalsRepository(SPM context)
        {
            _db = context;
        }

        public List<DatabaseArchiveExcludedSignals> GetAllExcludedSignals()
        {
            var excludedSignals = _db.DatabaseArchiveExcludedSignals
                .ToList();
            var orderedSignals = excludedSignals.OrderBy(signal => signal.SignalId).ToList();

            return orderedSignals;
        }

        public DatabaseArchiveExcludedSignals GetExcludedSignalBySignalId(string signalId)
        {
            var signalToFind = (from r in _db.DatabaseArchiveExcludedSignals
                where r.SignalId == signalId
                select r).FirstOrDefault();
            return signalToFind;
        }

        public void AddToExcludedList(string signalId)
        {
            var excludedSignals = _db.DatabaseArchiveExcludedSignals
                .ToList();
            if (GetExcludedSignalBySignalId(signalId) == null)
            {
                var newSignal = new DatabaseArchiveExcludedSignals();
                newSignal.SignalId = signalId;
                excludedSignals.Add(newSignal);
            }
        }

        public void DeleteFromExcludedList(string signalId)
        {
            var excludedSignals = _db.DatabaseArchiveExcludedSignals
                .ToList();
            var signalToDelete = GetExcludedSignalBySignalId(signalId);
            if (signalToDelete != null)
                excludedSignals.Remove(signalToDelete);
        }
    }
}