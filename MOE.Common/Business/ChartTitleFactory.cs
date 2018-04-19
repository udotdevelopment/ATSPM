using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business
{
    public static class ChartTitleFactory
    {
        private static readonly ISignalsRepository signalsRepository =
            SignalsRepositoryFactory.Create();

        private static readonly IMetricTypeRepository metricTypesRepository =
            MetricTypeRepositoryFactory.Create();

        internal static Title GetChartName(int metricId)
        {
            var metricType = metricTypesRepository.GetMetricsByID(metricId);
            var title = new Title(metricType.ChartName);
            title.Font = new Font(title.Font.FontFamily, title.Font.Size, FontStyle.Bold);
            return title;
        }

        internal static Title GetSignalLocationAndDateRange(string signalID, DateTime startDate, DateTime endDate)
        {
            var title = signalsRepository.GetSignalLocation(signalID);
            title += " - SIG#" + signalID + " \n" + startDate.ToString("f") + " - " + endDate.ToString("f");
            return new Title(title);
        }

        internal static Title GetPhase(int phase)
        {
            var title = new Title("Phase " + phase);
            title.Font = new Font(title.Font.FontFamily, title.Font.Size, FontStyle.Bold);
            return title;
        }

        internal static Title GetStatistics(Dictionary<string, string> statistics)
        {
            var title = string.Empty;
            foreach (var stat in statistics)
                if (stat.Key == statistics.Last().Key)
                    title += stat.Key + " = " + stat.Value;
                else
                    title += stat.Key + " = " + stat.Value + "; ";
            return new Title(title);
        }

        internal static Title GetBoldTitle(string customTitle)
        {
            var title = new Title(customTitle);
            title.Font = new Font(title.Font.FontFamily, title.Font.Size, FontStyle.Bold);
            return title;
        }

        internal static Title GetTitle(string customTitle)
        {
            var title = new Title(customTitle);
            return title;
        }

        internal static Title GetPhaseAndPhaseDescriptions(Models.Approach approach, bool getPermissivePhase)
        {
            Title title;
            int phaseNumber = getPermissivePhase ? approach.PermissivePhaseNumber.Value : approach.ProtectedPhaseNumber;
            if (approach.IsProtectedPhaseOverlap)
            {
                title = new Title("Overlap " + phaseNumber + ": " + approach.Description);
            }
            else
            {
                title = new Title("Phase " + phaseNumber + ": " + approach.Description);
            }
            
            title.Font = new Font(title.Font.FontFamily, title.Font.Size, FontStyle.Bold);
            return title;
        }

        internal static Title GetSignalLocationAndDateRangeAndMessage(string signalID, DateTime startDate,
            DateTime endDate, string message)
        {
            var title = signalsRepository.GetSignalLocation(signalID);
            title += " - SIG#" + signalID + " \n" + startDate.ToString("f") + " - " + endDate.ToString("f");
            title += message;
            return new Title(title);
        }
    }
}