using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using NuGet;

namespace MOE.Common.Models.Repositories
{
    public class DatabaseArchiveExcludedSignalsRepository : IDatabaseArchiveExcludedSignalsRepository
    {
        private Models.SPM _db;
       
        public DatabaseArchiveExcludedSignalsRepository()
        {
          _db  = new SPM();
        }

        public DatabaseArchiveExcludedSignalsRepository(SPM context)
        {
            _db = context;
        }

        public List<Models.DatabaseArchiveExcludedSignals> GetAllExcludedSignals()
        {
            
            List<Models.DatabaseArchiveExcludedSignals> excludedSignals = _db.DatabaseArchiveExcludedSignals
                    .ToList();
            List<Models.DatabaseArchiveExcludedSignals> orderedSignals = excludedSignals.OrderBy(signal => signal.SignalId).ToList();

            return orderedSignals;
        }

        public DatabaseArchiveExcludedSignals GetExcludedSignalBySignalId(string signalId)
        {
            DatabaseArchiveExcludedSignals signalToFind = (from r in _db.DatabaseArchiveExcludedSignals
                where r.SignalId == signalId
                select r).FirstOrDefault();
            return signalToFind;
        }

        public void AddToExcludedList(string signalId)
        {
            List<Models.DatabaseArchiveExcludedSignals> excludedSignals = _db.DatabaseArchiveExcludedSignals
                .ToList();
            if (GetExcludedSignalBySignalId(signalId) == null)
            {
                DatabaseArchiveExcludedSignals newSignal = new DatabaseArchiveExcludedSignals();
                newSignal.SignalId = signalId;
                excludedSignals.Add(newSignal);
            }
        }

        public void DeleteFromExcludedList(string signalId)
        {
            List<Models.DatabaseArchiveExcludedSignals> excludedSignals = _db.DatabaseArchiveExcludedSignals
                .ToList();
            DatabaseArchiveExcludedSignals signalToDelete = GetExcludedSignalBySignalId(signalId);
            if (signalToDelete != null)
            {
                excludedSignals.Remove(signalToDelete);
            }
        }


    }
}