using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business.WCFServiceLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business.Bins;
using MOE.Common.Business.FilterExtensions;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.WCFServiceLibrary.Tests
{
    [TestClass()]
    public class ApproachCycleAggregationApproachOptionsTests:ApproachAggregationCreateMetricTestsBase
    {
        




        protected override ApproachCycleAggregationOptions getOptionDefaults()
        {
            ApproachCycleAggregationOptions options = new ApproachCycleAggregationOptions();
            options.SeriesWidth = 3;
            options.SelectedXAxisType = XAxisType.Time;
            options.SelectedSeries = SeriesType.PhaseNumber;
            SetFilterSignal(options);
            return options;
        }
        


        [TestMethod()]
        public void CreateTimeMetricStartToFinishAllBinSizesAllAggregateDataTypesTest()
        {
            var options = getOptionDefaults();
            foreach (var xAxisType in Enum.GetValues(typeof(XAxisType)).Cast<XAxisType>().ToList())
            {
                options.SelectedXAxisType = xAxisType;
            foreach (BinFactoryOptions.BinSize tempBinSize in Enum.GetValues(typeof(BinFactoryOptions.BinSize)).Cast<BinFactoryOptions.BinSize>().ToList())
            {
                SetTimeOptionsBasedOnBinSize(options, tempBinSize);
                options.TimeOptions.SelectedBinSize = tempBinSize;
                foreach (var aggregatedDataType in options.AggregatedDataTypes)
                {
                    options.SelectedAggregatedDataType = aggregatedDataType;
                    try
                    {
                        CreateStackedColumnChart(options);
                        Assert.IsTrue(options.ReturnList.Count == 2);
                    }
                    catch (InvalidBinSizeException e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    options.ReturnList = new List<string>();
                }
                }
            }
        }

        private static void SetTimeOptionsBasedOnBinSize(ApproachCycleAggregationOptions options, BinFactoryOptions.BinSize binSize)
        {
            if (binSize == BinFactoryOptions.BinSize.Day)
            {
                options.StartDate = Convert.ToDateTime("10/1/2017");
                options.EndDate = Convert.ToDateTime("11/1/2017");
                options.TimeOptions = new BinFactoryOptions(
                    Convert.ToDateTime("10/1/2017"),
                    Convert.ToDateTime("11/1/2017"),
                    null, null, null, null,
                    new List<DayOfWeek>
                    {
                            DayOfWeek.Monday,
                            DayOfWeek.Tuesday,
                            DayOfWeek.Wednesday,
                            DayOfWeek.Thursday,
                            DayOfWeek.Friday,
                            DayOfWeek.Saturday,
                            DayOfWeek.Sunday
                    },
                    BinFactoryOptions.BinSize.FifteenMinute,
                    BinFactoryOptions.TimeOptions.StartToEnd);
            }
            else if (binSize == BinFactoryOptions.BinSize.Month)
            {
                options.StartDate = Convert.ToDateTime("1/1/2017");
                options.EndDate = Convert.ToDateTime("1/1/2018");
                options.TimeOptions = new BinFactoryOptions(
                    Convert.ToDateTime("1/1/2017"),
                    Convert.ToDateTime("1/1/2018"),
                    null, null, null, null,
                    new List<DayOfWeek>
                    {
                            DayOfWeek.Monday,
                            DayOfWeek.Tuesday,
                            DayOfWeek.Wednesday,
                            DayOfWeek.Thursday,
                            DayOfWeek.Friday,
                            DayOfWeek.Saturday,
                            DayOfWeek.Sunday
                    },
                    BinFactoryOptions.BinSize.FifteenMinute,
                    BinFactoryOptions.TimeOptions.StartToEnd);
            }
            else if (binSize == BinFactoryOptions.BinSize.Year)
            {
                options.StartDate = Convert.ToDateTime("1/1/2016");
                options.EndDate = Convert.ToDateTime("1/1/2018");
                options.TimeOptions = new BinFactoryOptions(
                    Convert.ToDateTime("1/1/2016"),
                    Convert.ToDateTime("1/1/2018"),
                    null, null, null, null,
                    new List<DayOfWeek>
                    {
                            DayOfWeek.Monday,
                            DayOfWeek.Tuesday,
                            DayOfWeek.Wednesday,
                            DayOfWeek.Thursday,
                            DayOfWeek.Friday,
                            DayOfWeek.Saturday,
                            DayOfWeek.Sunday
                    },
                    BinFactoryOptions.BinSize.FifteenMinute,
                    BinFactoryOptions.TimeOptions.StartToEnd);
            }
            else
            {
                options.StartDate = Convert.ToDateTime("10/17/2017");
                options.EndDate = Convert.ToDateTime("10/18/2017");
                options.TimeOptions = new BinFactoryOptions(
                    Convert.ToDateTime("10/17/2017"),
                    Convert.ToDateTime("10/18/2017"),
                    null, null, null, null,
                    new List<DayOfWeek>
                    {
                            DayOfWeek.Monday,
                            DayOfWeek.Tuesday,
                            DayOfWeek.Wednesday,
                            DayOfWeek.Thursday,
                            DayOfWeek.Friday,
                            DayOfWeek.Saturday,
                            DayOfWeek.Sunday
                    },
                    BinFactoryOptions.BinSize.FifteenMinute,
                    BinFactoryOptions.TimeOptions.StartToEnd);
            }
            if (options.SelectedXAxisType == XAxisType.TimeOfDay)
            {
                options.TimeOptions.TimeOfDayStartHour = 7;
                options.TimeOptions.TimeOfDayStartMinute = 0;
                options.TimeOptions.TimeOfDayEndHour = 10;
                options.TimeOptions.TimeOfDayStartHour = 0;
            }
        }

        protected override void SetSpecificAggregateRepositoriesForTest()
        {
            ApproachCycleAggregationRepositoryFactory.SetApplicationEventRepository(
                new InMemoryApproachCycleAggregationRepository(Db));
        }

        protected override void PopulateApproachData(Approach approach)
        {
            Db.PopulateApproachCycleAggregationsWithRandomRecords(Convert.ToDateTime("1/1/2016"),
                Convert.ToDateTime("1/1/2018"), approach);
        }
    }
}