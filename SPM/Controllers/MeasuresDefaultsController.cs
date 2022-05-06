using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Models;
using SPM.Models;

namespace SPM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MeasuresDefaultsController : Controller
    {
        MOE.Common.Models.Repositories.IMeasuresDefaultsRepository measuresDefaultsRepository =
        MOE.Common.Models.Repositories.MeasuresDefaultsRepositoryFactory.Create();
        // GET: MeasuresDefaults
        public ActionResult Index()
        {
            var viewModel = new MeasuresDefaultsViewModel();

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

            AddDropDownValuesToViewBag();

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MeasuresDefaultsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            UpdateChartDefaults("PhaseTermination", viewModel.PhaseTermination);
            UpdateChartDefaults("PedDelay", viewModel.PedDelay);
            UpdateChartDefaults("SplitFail", viewModel.SplitFail);
            UpdateChartDefaults("SplitMonitor", viewModel.SplitMonitor);
            UpdateChartDefaults("ApproachDelay", viewModel.ApproachDelay);
            UpdateChartDefaults("ApproachSpeed", viewModel.ApproachSpeed);
            UpdateChartDefaults("ApproachVolume", viewModel.ApproachVolume);
            UpdateChartDefaults("LeftTurnGapAnalysis", viewModel.LeftTurnGapAnalysis);
            UpdateChartDefaults("YellowAndRed", viewModel.YellowAndRed);
            UpdateChartDefaults("TimingAndActuations", viewModel.TimingAndActuations);
            UpdateChartDefaults("WaitTime", viewModel.WaitTime);
            UpdateChartDefaults("AoR", viewModel.AoR);
            UpdateChartDefaults("TMC", viewModel.TMC);
            UpdateChartDefaults("PCD", viewModel.PCD);

            return RedirectToAction("Index");
        }

        public void AddDropDownValuesToViewBag()
        {
            var binSize = new List<int>() { 5, 15 };
            ViewBag.BinSize = new SelectList(binSize);

            var consecutiveCount = new List<int>() { 1, 2, 3, 4, 5 };
            ViewBag.ConsecutiveCount = new SelectList(consecutiveCount);

            var lineAndDotSize = new List<SelectListItem>()
                {
                    new SelectListItem() {Text = "Small", Value = "1"},
                    new SelectListItem() {Text = "Large", Value = "2"}
                };
            ViewBag.LineAndDotSize = new SelectList(lineAndDotSize, "Value", "Text");

            var percentileSplit = new List<int>() { 50, 75, 85, 90, 95 };
            ViewBag.PercentileSplit = new SelectList(percentileSplit);
        }

        public void UpdateChartDefaults<T>(string chartName, T viewModel)
        {
            foreach (var prop in viewModel.GetType().GetProperties())
            {
                var measuresDefaultsModel = new MeasuresDefaults();
                measuresDefaultsModel.Measure = chartName;
                measuresDefaultsModel.OptionName = prop.Name;
                measuresDefaultsModel.Value = prop.GetValue(viewModel, null)?.ToString();
                measuresDefaultsRepository.Update(measuresDefaultsModel);
            }
        }

        public void AddValuesToViewModel<T>(T viewModel, string measure)
        {
            var measures = measuresDefaultsRepository.GetMeasureDefaultsAsDictionary(measure);
            foreach (var option in measures)
            {
                var type = viewModel.GetType().GetProperty(option.Key)?.PropertyType;

                if (option.Value == null || option.Value == "null") continue;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                var converted = Convert.ChangeType(option.Value, type);
                viewModel.GetType().GetProperty(option.Key).SetValue(viewModel, converted);
            }

        }
    }
}
