using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class DatabaseArchiveExcludedSignalsRepository : IDatabaseArchiveExcludedSignalsRepository
    {
        //private readonly MOEContext _db;

        //public DatabaseArchiveExcludedSignalsRepository(MOEContext context)
        //{
        //    _db = context;
        //}

        //public List<DatabaseArchiveExcludedSignal> GetAllExcludedSignals(ISignalsRepository signalsRepository)
        //{
        //    var excludedSignals = _db.DatabaseArchiveExcludedSignals
        //        .ToList();
        //    var orderedSignals = excludedSignals.OrderBy(signal => signal.SignalId).ToList();
        //    foreach (var databaseArchiveExcludedSignal in orderedSignals)
        //    {
        //        databaseArchiveExcludedSignal.SignalDescription =
        //            signalsRepository.GetSignalDescription(databaseArchiveExcludedSignal.SignalId);
        //    }

        //    return orderedSignals;
        //}

        //public DatabaseArchiveExcludedSignal GetExcludedSignalBySignalId(string signalId)
        //{
        //    var signalToFind = (from r in _db.DatabaseArchiveExcludedSignals
        //                        where r.SignalId == signalId
        //                        select r).FirstOrDefault();
        //    return signalToFind;
        //}

        //public void AddToExcludedList(string signalId)
        //{
        //    var excludedSignals = _db.DatabaseArchiveExcludedSignals
        //        .ToList();
        //    if (GetExcludedSignalBySignalId(signalId) == null)
        //    {
        //        var newSignal = new DatabaseArchiveExcludedSignal();
        //        newSignal.SignalId = signalId;
        //        _db.DatabaseArchiveExcludedSignals.Add(newSignal);
        //        _db.SaveChanges();
        //    }
        //}

        //public bool Exists(string signalId)
        //{
        //    return _db.DatabaseArchiveExcludedSignals.Any(s => s.SignalId == signalId);
        //}

        //public void DeleteFromExcludedList(string signalId)
        //{
        //    var signalToDelete = GetExcludedSignalBySignalId(signalId);
        //    if (signalToDelete != null)
        //    {
        //        _db.DatabaseArchiveExcludedSignals.Remove(signalToDelete);
        //        _db.SaveChanges();
        //    }
        //}
    }
}