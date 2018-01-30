using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.Bins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.Bins.Tests
{
    [TestClass()]
    public class BinFactoryTests
    {
        [TestMethod()]
        public void GetBins15MinutesTest()
        {
            BinFactoryOptions binFactoryOptions = new BinFactoryOptions(Convert.ToDateTime("10/17/2017 12:00 AM"), Convert.ToDateTime("10/17/2017 1:00 AM"), null, null, null, null, null, BinFactoryOptions.BinSizes.FifteenMinutes, BinFactoryOptions.TimeOptions.StartToEnd);
            var bins = BinFactory.GetBins(binFactoryOptions);
            Assert.IsTrue(bins.FirstOrDefault().Bins.Count == 4);
        }

        [TestMethod()]
        public void GetBins15MinutesTimePeriodMultipleDaysTest()
        {
            List<DayOfWeek> days = new List<DayOfWeek>();
            days.Add(DayOfWeek.Monday);
            days.Add(DayOfWeek.Tuesday);
            days.Add(DayOfWeek.Wednesday);
            days.Add(DayOfWeek.Thursday);
            days.Add(DayOfWeek.Friday);

            BinFactoryOptions binFactoryOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/19/2017 12:00 AM"),
                Convert.ToDateTime("10/24/2017 1:00 AM"), 
                0,0,1,0, days, 
                BinFactoryOptions.BinSizes.FifteenMinutes, 
                BinFactoryOptions.TimeOptions.TimePeriod);
            var bins = BinFactory.GetBins(binFactoryOptions);
            Assert.IsTrue(bins.First().Bins.Count == 16);
        }

        [TestMethod()]
        public void GetBins30MinutesTimePeriodMultipleDaysTest()
        {
            List<DayOfWeek> days = new List<DayOfWeek>();
            days.Add(DayOfWeek.Monday);
            days.Add(DayOfWeek.Tuesday);
            days.Add(DayOfWeek.Wednesday);
            days.Add(DayOfWeek.Thursday);
            days.Add(DayOfWeek.Friday);

            BinFactoryOptions binFactoryOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/19/2017 12:00 AM"),
                Convert.ToDateTime("10/24/2017 1:00 AM"),
                0, 0, 1, 0, days,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.TimePeriod);
            var bins = BinFactory.GetBins(binFactoryOptions);
            Assert.IsTrue(bins.First().Bins.Count == 8);
        }

        [TestMethod()]
        public void GetBinsHourTimePeriodMultipleDaysTest()
        {
            List<DayOfWeek> days = new List<DayOfWeek>();
            days.Add(DayOfWeek.Monday);
            days.Add(DayOfWeek.Tuesday);
            days.Add(DayOfWeek.Wednesday);
            days.Add(DayOfWeek.Thursday);
            days.Add(DayOfWeek.Friday);

            BinFactoryOptions binFactoryOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/19/2017 12:00 AM"),
                Convert.ToDateTime("10/24/2017 1:00 AM"),
                0, 0, 1, 0, days,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.TimePeriod);
            var bins = BinFactory.GetBins(binFactoryOptions);
            Assert.IsTrue(bins.First().Bins.Count == 4);
        }

        [TestMethod()]
        public void GetBins30MinutesTest()
        {
            BinFactoryOptions binFactoryOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017 12:00 AM"),
                Convert.ToDateTime("10/17/2017 1:00 AM"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.ThirtyMinutes,
                BinFactoryOptions.TimeOptions.StartToEnd);
            var bins = BinFactory.GetBins(binFactoryOptions);
            Assert.IsTrue(bins.First().Bins.Count == 2);
        }

        [TestMethod()]
        public void GetBinsHourTest()
        {
            BinFactoryOptions binFactoryOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017 12:00 AM"),
                Convert.ToDateTime("10/17/2017 5:00 AM"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Hour,
                BinFactoryOptions.TimeOptions.StartToEnd);
            var bins = BinFactory.GetBins(binFactoryOptions);
            Assert.IsTrue(bins.First().Bins.Count == 5);
        }

        [TestMethod()]
        public void GetBinsDayTest()
        {
            BinFactoryOptions binFactoryOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017 12:00 AM"),
                Convert.ToDateTime("10/27/2017 5:00 AM"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Day,
                BinFactoryOptions.TimeOptions.StartToEnd);
            var bins = BinFactory.GetBins(binFactoryOptions);
            Assert.IsTrue(bins.First().Bins.Count == 11);
        }

        [TestMethod()]
        public void GetBinsWeekTest()
        {
            BinFactoryOptions binFactoryOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/17/2017 12:00 AM"),
                Convert.ToDateTime("10/27/2017 5:00 AM"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Week,
                BinFactoryOptions.TimeOptions.StartToEnd);
            var bins = BinFactory.GetBins(binFactoryOptions);
            Assert.IsTrue(bins.First().Bins.Count == 2);
        }

        [TestMethod()]
        public void GetBinsMonthTest()
        {
            BinFactoryOptions binFactoryOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017 12:00 AM"),
                Convert.ToDateTime("12/27/2017 5:00 AM"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Month,
                BinFactoryOptions.TimeOptions.StartToEnd);
            var bins = BinFactory.GetBins(binFactoryOptions);
            Assert.IsTrue(bins.First().Bins.Count == 3);
        }

        [TestMethod()]
        public void GetBinsYearTest()
        {
            BinFactoryOptions binFactoryOptions = new BinFactoryOptions(
                Convert.ToDateTime("10/1/2017 12:00 AM"),
                Convert.ToDateTime("12/27/2018 5:00 AM"),
                null, null, null, null, null,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.StartToEnd);
            var bins = BinFactory.GetBins(binFactoryOptions);
            Assert.IsTrue(bins.Count == 2);
        }

        [TestMethod()]
        public void YearOfWednesdaysShouldReturn52Bins()
        {
            List<DayOfWeek>weekdays = new List<DayOfWeek>();
            weekdays.Add(DayOfWeek.Wednesday);

            BinFactoryOptions binFactoryOptions = new BinFactoryOptions(
                Convert.ToDateTime("1/1/2017 12:00 AM"),
                Convert.ToDateTime("12/31/2017 11:59 PM"),
                0, 1, 23, 59, weekdays,
                BinFactoryOptions.BinSizes.Year,
                BinFactoryOptions.TimeOptions.TimePeriod);
            var bins = BinFactory.GetBins(binFactoryOptions);
            Assert.IsTrue(bins.First().Bins.Count == 52);
        }
    }
}