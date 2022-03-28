using Xunit;
using MOE.Common.Business.Bins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.Bins.Tests
{
    public class BinFactoryTests
    {
        [Fact()]
        public void GetBinsForRange15MinuteStartToEndTest()
        {
            BinFactoryOptions timeOptions = new BinFactoryOptions(
                DateTime.MinValue,
                DateTime.MinValue.AddDays(1),
                null,
                null,
                null,
                null,
                new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday },
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd
            );
            var binSize = 15;
            var bins = BinFactory.GetBinsForRange(timeOptions, binSize);
            Assert.Single(bins);
            Assert.Equal(timeOptions.Start, bins[0].Start);
            Assert.Equal(timeOptions.End, bins[0].End);
            Assert.Equal(96, bins[0].Bins.Count);
            int index = 0;
            for (var dt = timeOptions.Start; dt < timeOptions.End; dt = dt.AddMinutes(binSize))
            {
                Assert.Equal(dt, bins[0].Bins[index].Start);
                Assert.Equal(dt.AddMinutes(binSize), bins[0].Bins[index].End);
                index++;
            }
        }

        [Fact()]
        public void GetBinsForRange15MinuteTimePeriod()
        {
            BinFactoryOptions timeOptions = new BinFactoryOptions(
                DateTime.MinValue,
                DateTime.MinValue.AddDays(2),
                6,
                0,
                9,
                0,
                new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday },
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.TimePeriod
            );
            var binSize = 15;
            var bins = BinFactory.GetBinsForRange(timeOptions, binSize);
            Assert.Single(bins);
            Assert.Equal(timeOptions.Start, bins[0].Start);
            Assert.Equal(timeOptions.End, bins[0].End);
            Assert.Equal(24, bins[0].Bins.Count);
            var startTime = new TimeSpan(timeOptions.TimeOfDayStartHour.Value, timeOptions.TimeOfDayStartMinute.Value, 0);
            var endTime = new TimeSpan(timeOptions.TimeOfDayEndHour.Value, timeOptions.TimeOfDayEndMinute.Value, 0);

            foreach (var bin in bins[0].Bins)
            {
                Assert.True(bin.Start.TimeOfDay >= startTime && bin.Start.TimeOfDay < endTime);
            }
        }

        [Fact()]
        public void GetMonthBinsForRangeTest()
        {
            BinFactoryOptions timeOptions = new BinFactoryOptions(
                DateTime.MinValue,
                DateTime.MinValue.AddYears(1),
                null,
                null,
                null,
                null,
                new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday },
                BinFactoryOptions.BinSize.FifteenMinute,
                BinFactoryOptions.TimeOptions.StartToEnd
            );
            var bins = BinFactory.GetMonthBinsForRange(timeOptions);
            Assert.Single(bins);
            Assert.Equal(12, bins[0].Bins.Count);
            Assert.Equal(timeOptions.Start, bins[0].Start);
            Assert.Equal(timeOptions.End, bins[0].End);
            int index = 0;
            for (var dt = timeOptions.Start; dt < timeOptions.End; dt = dt.AddMonths(1))
            {
                Assert.Equal(dt, bins[0].Bins[index].Start);
                Assert.Equal(dt.AddMonths(1), bins[0].Bins[index].End);
                index++;
            }
        }
    }
}