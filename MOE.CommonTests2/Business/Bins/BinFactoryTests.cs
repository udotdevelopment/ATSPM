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
        public void GetBinsForRangeTest()
        {
            BinFactoryOptions timeOptions = new BinFactoryOptions(
                DateTime.MinValue,
                DateTime.MinValue.AddDays(1),
                null,
                null,
                null,
                null,
                new List<DayOfWeek>{DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday },
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
    }
}