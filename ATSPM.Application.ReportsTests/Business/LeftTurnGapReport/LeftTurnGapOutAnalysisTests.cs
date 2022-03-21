using Xunit;
using ATSPM.Application.Reports.Business.LeftTurnGapReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSPM.IRepositories;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport.Tests
{
    public class LeftTurnGapOutAnalysisTests
    {



        [Fact()]
        public void GetCriticalGapTest()
        {
            Assert.True(LeftTurnGapOutAnalysis.GetCriticalGap(3) == 5.3);
            Assert.True(LeftTurnGapOutAnalysis.GetCriticalGap(2) == 4.1);
        }

        [Fact()]
        public void GetGapColumnTest()
        {
            Assert.True(LeftTurnGapOutAnalysis.GetGapColumn(4.1) == 12);
            Assert.True(LeftTurnGapOutAnalysis.GetGapColumn(5.3) == 13);
            Assert.True(LeftTurnGapOutAnalysis.GetGapColumn(1) == 12);
        }

        [Fact()]
        public void SumGapColumnsTest()
        {
            List<Models.PhaseLeftTurnGapAggregation> leftTurnGaps = new List<Models.PhaseLeftTurnGapAggregation>()
            {
                new Models.PhaseLeftTurnGapAggregation{ ApproachId=1,
                 BinStartTime = DateTime.Now,
                 GapCount1=1,
                 GapCount2=1,
                 GapCount3=1,
                 GapCount4=1,
                 GapCount5=1,
                 GapCount6=1,
                 GapCount7=1,
                 GapCount8=1,
                 GapCount9=1,
                 GapCount10=1,
                 GapCount11=1, }
            };
            Assert.True(LeftTurnGapOutAnalysis.SumGapColumns(1, leftTurnGaps) == 3);
            Assert.True(LeftTurnGapOutAnalysis.SumGapColumns(12, leftTurnGaps) == 4);
            Assert.True(LeftTurnGapOutAnalysis.SumGapColumns(13, leftTurnGaps) == 3);
        }

        [Fact()]
        public void CalculateGapDemandTest()
        {
            Assert.True(LeftTurnGapOutAnalysis.CalculateGapDemand(4.1, 10) == 41.0);
            Assert.True(LeftTurnGapOutAnalysis.CalculateGapDemand(5.3, 10) == 53.0);
        }
    }
}

namespace ATSPM.Application.ReportsTests.Business.LeftTurnGapReport
{
    class LeftTurnGapOutAnalysisTests
    {
    }
}
