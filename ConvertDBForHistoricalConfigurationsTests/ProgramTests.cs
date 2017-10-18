using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConvertDBForHistoricalConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Data;
using MOE.Common.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace ConvertDBForHistoricalConfigurations.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void AddDummyRecordsToTheDatabaseTest()
        {
            AddDummyRecordsToTheDatabase();
        }

        public void AddDummyRecordsToTheDatabase()
        {
            MOE.Common.Models.SPM db = new SPM();

            PopulateSignal(db);
            PopulateSignalsWithApproaches(db);
            PopulateApproachesWithDetectors(db);

        }
        public void PopulateSignal(MOE.Common.Models.SPM db)
        {
            for (int i = 1; i < 6; i++)
            {
                MOE.Common.Models.Signal s = new Signal();

                s.SignalID = "10" + i.ToString();
              
                s.End = Convert.ToDateTime("1/1/9999");
                s.PrimaryName = "Primary: " + i.ToString();
                s.SecondaryName = "Secondary: " + i.ToString();
                s.Note = "Create Dummy";
                s.IPAddress = "10.10.10.10";
                s.Latitude = "0.01";
                s.Longitude = "0.01";
                s.ControllerTypeID = 1;
                s.RegionID = 1;

                s.VersionActionId = 10;

                db.Signals.Add(s);

            }
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }

        }

        public void PopulateSignalsWithApproaches(MOE.Common.Models.SPM db)
        {
            int i = 1;
            foreach (var s in db.Signals)
            {
                Approach a = new Approach
                {
                    ApproachID = (s.VersionID * 1020) + 1,
                    Description = "NB Approach for Signal " + s.SignalID,
                    DirectionType = (from r in db.DirectionTypes
                                     where r.Abbreviation == "NB"
                                     select r).FirstOrDefault(),



                    ProtectedPhaseNumber = 2,
                    SignalID = s.SignalID,
                    MPH = 40,
                    VersionID = s.VersionID,
                    Signal = s
                };

                db.Approaches.Add(a);

                Approach b = new Approach
                {
                    ApproachID = (s.VersionID * 1040) + 1,
                    Description = "SB Approach for Signal " + s.SignalID,
                    DirectionType = (from r in db.DirectionTypes
                                     where r.Abbreviation == "SB"
                                     select r).FirstOrDefault(),

                    ProtectedPhaseNumber = 4,
                    SignalID = s.SignalID,
                    MPH = 40,
                    VersionID = s.VersionID,
                    Signal = s
                };

                db.Approaches.Add(b);

                Approach c = new Approach
                {
                    ApproachID = (s.VersionID * 1060) + 1,
                    Description = "SB Approach for Signal " + s.SignalID,
                    DirectionType = (from r in db.DirectionTypes
                                     where r.Abbreviation == "EB"
                                     select r).FirstOrDefault(),

                    ProtectedPhaseNumber = 6,
                    SignalID = s.SignalID,
                    MPH = 40,
                    VersionID = s.VersionID,
                    Signal = s
                };

                db.Approaches.Add(c);

                Approach d = new Approach
                {
                   
                    Description = "SB Approach for Signal " + s.SignalID,
                    DirectionType = (from r in db.DirectionTypes
                                     where r.Abbreviation == "WB"
                                     select r).FirstOrDefault(),

                    ProtectedPhaseNumber = 8,
                    SignalID = s.SignalID,
                    MPH = 40,
                    VersionID = s.VersionID,
                    Signal = s
                };

                db.Approaches.Add(d);

                i++;
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }

        }

        public static void PopulateApproachesWithDetectors(MOE.Common.Models.SPM db)
        {
            foreach (var signal in db.Signals)
            {

                signal.Approaches = (from r in db.Approaches
                                     where r.VersionID == signal.VersionID
                                     select r).ToList();

                int i = 1;
                foreach (var appr in signal.Approaches)
                {

                    MOE.Common.Models.Detector a = new Detector()
                    {
                        ApproachID = appr.ApproachID,
                 
                        DetChannel = appr.ProtectedPhaseNumber,
                        DetectionHardwareID = 1,
                        DateAdded = DateTime.Today,
                        LaneNumber = 1,
                        MovementTypeID = 1,
                        DetectorID = appr.SignalID + "00" + appr.ProtectedPhaseNumber.ToString(),
                    };

                    db.Detectors.Add(a);


                    i++;
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
        }
    }
}
