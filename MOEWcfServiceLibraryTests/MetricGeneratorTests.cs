using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using MOEWcfServiceLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOEWcfServiceLibrary.Tests
{
    [TestClass()]
    public class MetricGeneratorTests
    {
        [TestMethod()]
        public void CreateMetricTest()
        {
            string testDirPath = "E:\\Temp\\ATSPMImages";

            PhaseTerminationOptions options = NewOptionsForTest();

            options.MetricFileLocation = testDirPath;

            options.CreateMetric();
            Assert.IsTrue(options.ReturnList.Count > 1);

        }

        private PhaseTerminationOptions NewOptionsForTest()
        {

            var start = DateTime.Now.AddDays(-1);//Convert.ToDateTime("02/01/2018 00:01");
            var end = DateTime.Now;//Convert.ToDateTime("02/01/2018 23:59");
            PhaseTerminationOptions options = new PhaseTerminationOptions(start, 8, end, "10",false, 1, true);
            options.StartDate = start;
            options.EndDate = end;


            options.MetricFileLocation = "E:\\Temp\\ATSPMImages";
                

            return options;
        }

        }
    }
