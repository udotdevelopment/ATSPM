using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.Export.Tests
{
    [TestClass()]
    public class ExporterTests
    {
        [TestMethod()]
        public void ExporterTest()
        {
            InMemoryMOEDatabase _db = new InMemoryMOEDatabase();
            _db.PopulateSignal();
            _db.PopulateSignalsWithApproaches();
            _db.PopulateApproachesWithDetectors();
            _db.PopulateApproachSplitFailAggregationsWithRandomRecords();

            var recordList = from r in _db.ApproachSplitFailAggregations select r;

            string filepath = @"c:\temp\recordsExport.csv";


            var exporter = new Exporter();

            Assert.IsTrue(File.Exists(filepath));

            
        }
    }
}