using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using System.Xml;
using System.Xml.Linq;

namespace MOE.Common.Business.Export.Tests
{
    [TestClass()]
    public class ChartToCSVExporterTests
    {
        Chart chart;
        [TestInitialize]
        public void Initialize()
        {
            chart = CreateATestChart();
        }

        [TestMethod()]
        public void ExportChartDataToCSVForDownloadTest()
        {


            var binarycsvs = ChartToCSVExporter.ExportChartDataToCSVForDownload(chart);

            List<string> csvs = new List<string>();

            string descriminator = "I";
            foreach (byte[] series in binarycsvs)
            {
                
                File.WriteAllBytes(@"C:\SPMImages\chartSeriesExport" + descriminator + ".csv", series);
                descriminator = descriminator + "I";
            }

            

            Assert.IsTrue(binarycsvs.Count == 2);



        }

        [TestMethod()]
        public void SerializeChartTest()
        {

            string JSON = ChartToCSVExporter.SerializeChart(chart);
            Assert.IsTrue(JSON.Length > 0);
        }

        [TestMethod()]
        public void ConvertChartToXMLTest()
        {


            XmlDocument doc = ChartToCSVExporter.ConvertChartToXML(chart);
            // XDocument doc = ChartToCSVExporter.ConvertChartToXML(chart);
            //  doc.Save(@"c:\spmimages\chart.xml");
            Assert.IsNotNull(doc.HasChildNodes);
        }

        private Chart CreateATestChart()
        {
            Chart chart = new Chart();
            ChartArea cha1 = new ChartArea();

            Series s1 = new Series();
            Series s2 = new Series();
            s1.Name = "FirstSeries";
            s1.ChartType = SeriesChartType.Line;

            s2.Name = "SecondSeries";
            s2.ChartType = SeriesChartType.Column;

            AddPointsToTestSeries(s1);
            AddPointsToTestSeries(s2);

            s1.XAxisType = AxisType.Primary;
            s2.XAxisType = AxisType.Secondary;

            chart.Series.Add(s1);
            chart.Series.Add(s2);
            chart.ChartAreas.Add(cha1);


            return chart;

        }

        private void AddPointsToTestSeries(Series s)
        {
            for (int i = 1; i < 7; i++)
            {
                var dp = new DataPoint(i, i);
                s.Points.Add(dp);

            }
        }

        [TestMethod()]
        public void GetSeriesListFromChartXMLTest()
        {
            var series = ChartToCSVExporter.GetSeriesListFromChartXML(ChartToCSVExporter.ConvertChartToXML(chart));

            Assert.IsTrue(series.Count == 2);
        }

        [TestMethod()]
        public void GetPointsListFromSeriesNodeTest()
        {
            XmlNodeList series = ChartToCSVExporter.GetSeriesListFromChartXML(ChartToCSVExporter.ConvertChartToXML(chart));

            var points = ChartToCSVExporter.GetPointsListFromSeriesNode(series.Item(0));

            Assert.IsTrue(points.Count == 6);
        }

        [TestMethod()]
        public void CreateByteArrayListBySeriesTest()
        {
            XmlNodeList series = ChartToCSVExporter.GetSeriesListFromChartXML(ChartToCSVExporter.ConvertChartToXML(chart));



            var arrayList = ChartToCSVExporter.GetByteArrayListBySeries(series);
            Assert.IsTrue(arrayList.Count == 2);
        }
    }
}