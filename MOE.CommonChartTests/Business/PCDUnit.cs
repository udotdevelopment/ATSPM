//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using MOE.Common.Business;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MOE.Common.Models.Repositories;
//using MOE.Common.Models ;
//using System.ComponentModel.DataAnnotations;
//using System.Drawing;
//using System.Runtime.Serialization;
//using System.Web.UI.DataVisualization.Charting;
//using MOE.Common.Business.WCFServiceLibrary;


//namespace PCDUnit.Tests
//{
//    public Module.Signal Signal { get; set; }
//   //private object Signal;

//    [TestClass()]
    
    
//    public class PcdUnitTest
//    {
//        private object Signal;

//        [TestMethod()]
//        public void PcdUnitTestOptions()
//        {
//            string SignalId = "7602";
//            int MetricTypeID = 6;
//            DateTime testStart = DateTime.Now;
//            Signal = signalRepository.GetVersionOfSignalByDate(SignalId, testStart);

//            var signalRepository =
//                SignalsRepositoryFactory.Create();
//            var metricApproaches = Signal.GetApproachesForSignalThatSupportMetric(MetricTypeID);
//            if (metricApproaches.Count > 0)
//                foreach (var approach in metricApproaches)
//                {
//                    var signalPhase = new SignalPhase(testStart, EndDate, approach,
//                        ShowVolumes, SelectedBinSize, MetricTypeID, false);
//                }








//            Assert.Fail();
//        }
//    }
//}




////base.CreateMetric();
////var signalRepository =
////SignalsRepositoryFactory.Create();
//Signal = signalRepository.GetVersionOfSignalByDate(SignalID, StartDate);
////MetricTypeID = 6;
////var chart = new Chart();
////var metricApproaches = Signal.GetApproachesForSignalThatSupportMetric(MetricTypeID);

////if (metricApproaches.Count > 0)
////foreach (var approach in metricApproaches)
////{
////var signalPhase = new SignalPhase(StartDate, EndDate, approach,
////    ShowVolumes, SelectedBinSize, MetricTypeID, false);
////chart = GetNewChart();
////AddDataToChart(chart, signalPhase);
////var chartName = CreateFileName();
////chart.ImageLocation = MetricFileLocation + chartName;
////chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);
////ReturnList.Add(MetricWebPath + chartName);
////}