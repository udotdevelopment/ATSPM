using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.DataAggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.DataAggregation.Tests
{
    [TestClass()]
    public class DataAggregationTests
    {
        private string dateOne = "11/12/2017";
        private string dateTwo = "01/02/2018";

        private string[] dates;
        

        [TestMethod()]
        public void StartAggregationTest()
        {
            DataAggregation dag = new DataAggregation();


        }

        [TestMethod()]
        public void SetStartEndDateTest()
        {
            dates = new[] {dateOne, dateTwo};

            DataAggregation dag = new DataAggregation();

            dag.SetStartEndDate(dates, null);
        }
    }
}