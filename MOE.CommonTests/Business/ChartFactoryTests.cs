using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business.Tests
{
    [TestClass()]
    public class ChartFactoryTests
    {
        [TestMethod()]
        public void CreateLineSeresTest()
        {
            
            var s = ChartFactory.CreateLineSeres("TestSeries", Color.Green);

            Assert.IsTrue(s.ChartType == SeriesChartType.Line);
        }

        [TestMethod()]
        public void CreateStackedAreaSeresTest()
        {

            var s = ChartFactory.CreateStackedAreaSeres("TestSeries", Color.Green);

            Assert.IsTrue(s.ChartType == SeriesChartType.StackedArea);
        }

        [TestMethod()]
        public void CreateColumnSeresTest()
        {

            var s = ChartFactory.CreateColumnSeres("TestSeries", Color.Green);

            Assert.IsTrue(s.ChartType == SeriesChartType.Column);
        }

        [TestMethod()]
        public void CreateStackedColumnSeresTest()
        {

            var s = ChartFactory.CreateStackedColumnSeres("TestSeries", Color.Green);

            Assert.IsTrue(s.ChartType == SeriesChartType.StackedColumn);
        }
    }
}