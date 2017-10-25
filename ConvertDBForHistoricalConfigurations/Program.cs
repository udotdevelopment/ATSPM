using MOE.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;

namespace ConvertDBForHistoricalConfigurations
{
    public class Program
    {
        static void Main()
        {
            UpdateSignalRecordsWithStartDateAndVersion();
            UpdateApproachesWithVersionID();
            UpdateMetriCommentsWithVersionID();

        }

        private static void UpdateMetriCommentsWithVersionID()
        {
            MOE.Common.Models.SPM db = new SPM();

            var comments = (from r in db.MetricComments
                            select r).ToList();

            foreach (var c in comments)
            {
                var oldSig = c.SignalID;

                c.VersionID = (from r in db.Signals
                               where r.SignalID == oldSig
                               select r.VersionID).FirstOrDefault();
            }

            db.SaveChanges();
            db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[MetricComments] ADD CONSTRAINT [FK_dbo.MetricComments_dbo.Signals_VersionID] FOREIGN KEY ([VersionID]) REFERENCES [dbo].[Signals] ([VersionID])");

            db.Dispose();
        }

        private static void UpdateApproachesWithVersionID()
        {
            MOE.Common.Models.SPM db = new SPM();

            var approaches = (from r in db.Approaches
                              select r).ToList();

            foreach (var a in approaches)
            {
                a.Signal = (from r in db.Signals
                            where r.SignalID == a.SignalID
                            select r).FirstOrDefault();
            }

            db.SaveChanges();
            db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[Approaches] ADD CONSTRAINT [FK_dbo.Approaches_dbo.Signals_VersionID] FOREIGN KEY ([VersionID]) REFERENCES [dbo].[Signals] ([VersionID]) ");

            db.Dispose();
        }

        private static void UpdateSignalRecordsWithStartDateAndVersion()
        {
            MOE.Common.Models.SPM db = new SPM();

            var signals = (from r in db.Signals
                           select r).ToList();

            var version = (from r in db.VersionActions
                           where r.ID == 10
                           select r).FirstOrDefault();

            foreach (var s in signals)
            {
                s.Start = s.FirstDate;
                s.VersionAction = version;
            }

            db.SaveChanges();
            db.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[Signals] ADD CONSTRAINT [FK_dbo.Signals_dbo.VersionActions_VersionAction_ID] FOREIGN KEY ([VersionActionId]) REFERENCES [dbo].[VersionActions] ([ID])");



            db.Dispose();
        }


    }
}

