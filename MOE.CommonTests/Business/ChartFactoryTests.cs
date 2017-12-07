using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.WCFServiceLibrary;

namespace MOE.Common.Business.Tests
{
    [TestClass()]
    public class ChartFactoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestMethod()]
        public void CreateLineSeriesTest()
        {
            
            var s = ChartFactory.CreateLineSeries("TestSeries", Color.Green);

            Assert.IsTrue(s.ChartType == SeriesChartType.Line);
        }

        [TestMethod()]
        public void CreateStackedAreaSeriesTest()
        {

            var s = ChartFactory.CreateStackedAreaSeries("TestSeries", Color.Green);

            Assert.IsTrue(s.ChartType == SeriesChartType.StackedArea);
        }

        [TestMethod()]
        public void CreateColumnSeriesTest()
        {

            var s = ChartFactory.CreateColumnSeries("TestSeries", Color.Green);

            Assert.IsTrue(s.ChartType == SeriesChartType.Column);
        }

        [TestMethod()]
        public void CreateStackedColumnSeriesTest()
        {

            var s = ChartFactory.CreateStackedColumnSeries("TestSeries", Color.Green);

            Assert.IsTrue(s.ChartType == SeriesChartType.StackedColumn);
        }

  
    }
}