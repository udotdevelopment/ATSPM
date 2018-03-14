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

        public List<DatabaseArchiveExcludedSignal> GetAllExcludedSignals()
        {
            var excludedSignals = _db.DatabaseArchiveExcludedSignals
                .ToList();
            var orderedSignals = excludedSignals.OrderBy(signal => signal.SignalId).ToList();
            var signalRepository = SignalsRepositoryFactory.Create();
            foreach (var databaseArchiveExcludedSignal in orderedSignals)
            {
                databaseArchiveExcludedSignal.SignalDescription =
                    signalRepository.GetSignalDescription(databaseArchiveExcludedSignal.SignalId);
            }

            return orderedSignals;
        }

        public DatabaseArchiveExcludedSignal GetExcludedSignalBySignalId(string signalId)
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
                var newSignal = new DatabaseArchiveExcludedSignal();
                newSignal.SignalId = signalId;
                _db.DatabaseArchiveExcludedSignals.Add(newSignal);
                _db.SaveChanges();
            }
        }

        public bool Exists(string signalId)
        {
            return _db.DatabaseArchiveExcludedSignals.Any(s => s.SignalId == signalId);
        }

        public void DeleteFromExcludedList(string signalId)
        {
            var signalToDelete = GetExcludedSignalBySignalId(signalId);
            if (signalToDelete != null)
            {
                _db.DatabaseArchiveExcludedSignals.Remove(signalToDelete);
                _db.SaveChanges();
            }
        }
    }
}