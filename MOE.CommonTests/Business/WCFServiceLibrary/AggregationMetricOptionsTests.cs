using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;
using SPM.Models;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class AggregationMetricOptionsTests
    {
        [TestMethod()]
        public void AggregationMetricOptionsTest()
        {
            AggregationMetricOptions metricOptions = new AggregationMetricOptions();

              MetricResultViewModel result = new MetricResultViewModel();

            SPM.MetricGeneratorService.MetricGeneratorClient client =
                new SPM.MetricGeneratorService.MetricGeneratorClient();

     
                client.Open();
                result.ChartPaths = client.CreateMetric(metricOptions);
                client.Close();
            

            
        }
    }
}