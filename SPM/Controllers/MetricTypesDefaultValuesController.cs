using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Models;
using SPM.Models;

namespace SPM.Controllers
{
    public class MetricTypesDefaultValuesController : Controller
    {
        MOE.Common.Models.Repositories.IMetricTypesDefaultValuesRepository metricTypesDefaultValuesRepository =
        MOE.Common.Models.Repositories.MetricTypesDefaultValuesRepositoryFactory.Create();
        // GET: MetricTypesDefaultValues
        public ActionResult Index()
        {
            var viewModel = new MetricTypesDefaultValuesViewModel();

            viewModel.AoR = new AoRDefaultValuesViewModel();
            AddValuesToViewModel(viewModel.AoR, "AoR");

            viewModel.ApproachDelay = new ApproachDelayDefaultValuesViewModel();
            AddValuesToViewModel(viewModel.ApproachDelay, "ApproachDelay");

            viewModel.ApproachSpeed = new ApproachSpeedDefaultValuesViewModel();
            AddValuesToViewModel(viewModel.ApproachSpeed, "ApproachSpeed");

            viewModel.ApproachVolume = new ApproachVolumeDefaultValuesViewModel();
            AddValuesToViewModel(viewModel.ApproachVolume, "ApproachVolume");

            viewModel.LeftTurnGapAnalysis = new LeftTurnGapAnalysisDefaulValuesViewModel();
            AddValuesToViewModel(viewModel.LeftTurnGapAnalysis, "LeftTurnGapAnalysis");

            viewModel.PCD = new PCDefaulValuesViewModel();
            AddValuesToViewModel(viewModel.PCD, "PCD");

            viewModel.PedDelay = new PedDelayDefaulValuesViewModel();
            AddValuesToViewModel(viewModel.PedDelay, "PedDelay");

            viewModel.PhaseTermination = new PhaseTerminationDefaultValuesViewModel();
            AddValuesToViewModel(viewModel.PhaseTermination, "PhaseTermination");

            viewModel.SplitFail = new SplitFailDefaultValuesViewModel();
            AddValuesToViewModel(viewModel.SplitFail, "SplitFail");

            viewModel.SplitMonitor = new SplitMonitorDefaultValuesViewModel();
            AddValuesToViewModel(viewModel.SplitMonitor, "SplitMonitor");

            viewModel.TimingAndActuations = new TimingAndActuationsDefaulValuesViewModel();
            AddValuesToViewModel(viewModel.TimingAndActuations, "TimingAndActuations");

            viewModel.TMC = new TMCDefaultValuesViewModel();
            AddValuesToViewModel(viewModel.TMC, "TMC");

            viewModel.WaitTime = new WaitTimeDefaultValuesViewModel();
            AddValuesToViewModel(viewModel.WaitTime, "WaitTime");

            viewModel.YellowAndRed = new YellowAndRedDefaultValuesViewModel();
            AddValuesToViewModel(viewModel.YellowAndRed, "YellowAndRed");

            var list = new List<int>() { 5, 15 };
            ViewBag.BinSize = new SelectList(list);

            return View(viewModel);
        }

        public void AddValuesToViewModel<T>(T viewModel, string chart)
        {
            var charts = metricTypesDefaultValuesRepository.GetChartDefaultsAsDictionary(chart);
            foreach (var option in charts)
            {
                if (option.Value != null)
                {
                    if (viewModel.GetType().GetProperty(option.Key) != null)
                    {
                        var type = viewModel.GetType().GetProperty(option.Key).PropertyType;
                        if (type != null && !type.IsEnum)
                        {
                            var converted = Convert.ChangeType(option.Value, type);
                            viewModel.GetType().GetProperty(option.Key).SetValue(viewModel, converted);
                        }
                    }
                }
            }

        }
    }
}
