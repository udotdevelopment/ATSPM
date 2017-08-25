using MOE.Common.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;

namespace MOE.Common.Business
{
    public static class ChartTitleFactory
    {
        private static MOE.Common.Models.Repositories.ISignalsRepository signalsRepository =
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
        private static MOE.Common.Models.Repositories.IMetricTypeRepository metricTypesRepository =
            MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();

        internal static Title GetChartName(int metricId)
        {
            MetricType metricType = metricTypesRepository.GetMetricsByID(metricId);
            Title title = new Title(metricType.ChartName);
            title.Font = new Font(title.Font.FontFamily, title.Font.Size, FontStyle.Bold);
            return title;
        }

        internal static Title GetSignalLocationAndDateRange(string signalID, DateTime startDate, DateTime endDate)
        {
            string title = signalsRepository.GetSignalLocation(signalID);
            title += " - SIG#" + signalID + " \n" + startDate.ToString("f") + " - " + endDate.ToString("f");
            return new Title(title);
        }

        internal static Title GetPhase(int phase)
        {
            Title title = new Title("Phase " + phase.ToString());
            title.Font = new Font(title.Font.FontFamily, title.Font.Size, FontStyle.Bold);
            return title;
        }

        internal static Title GetStatistics(Dictionary<string, string> statistics)
        {
            string title = string.Empty;
            foreach(var stat in statistics)
            {
                if (stat.Key == statistics.Last().Key)
                {
                    title += stat.Key + " = " + stat.Value;
                }
                else
                {
                    title += stat.Key + " = " + stat.Value + "; ";
                }
            }
            return new Title(title);
        }

        internal static Title GetBoldTitle(string customTitle)
        {
            Title title = new Title(customTitle);
            title.Font = new Font(title.Font.FontFamily, title.Font.Size, FontStyle.Bold);
            return title;
        }

        internal static Title GetTitle(string customTitle)
        {
            Title title = new Title(customTitle);
            return title;
        }

        internal static Title GetPhaseAndPhaseDescriptions(int phaseNumber, string description)
        {
            Title title = new Title("Phase " + phaseNumber.ToString() + ": " + description);
            title.Font = new Font(title.Font.FontFamily, title.Font.Size, FontStyle.Bold);
            return title;
        }

        internal static Title GetSignalLocationAndDateRangeAndMessage(string signalID, DateTime startDate, DateTime endDate, string message)
        {
            string title = signalsRepository.GetSignalLocation(signalID);
            title += " - SIG#" + signalID + " \n" + startDate.ToString("f") + " - " + endDate.ToString("f");
            title += message;
            return new Title(title);
        }
    }
}
