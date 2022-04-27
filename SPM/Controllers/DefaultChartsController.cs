using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MOE.Common.Models.ViewModel.Chart;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Business.ApproachVolume;
using System.Data.Entity.Migrations;
using System.Text;
using MOE.Common.Models;

namespace SPM.Controllers
{
    public class DefaultChartsController : Controller
    {
        // GET: DefaultCharts
        public ActionResult Index()
        {
            InitializeDatabase();
            DefaultChartsViewModel viewModel = new DefaultChartsViewModel();
            return View(viewModel);
        }

        private void InitializeDatabase()
        {
            var config = new MOE.Common.Migrations.Configuration();
            var migrator = new DbMigrator(config);
            var _pendingMigrations = migrator.GetPendingMigrations();//.Any();
            if (_pendingMigrations.Count() > 0)
            {
                migrator.Update();
            }
        }

        public ActionResult SignalSearch(SignalSearchViewModel ssvm)
        {
            return PartialView("SignalSearch", ssvm);
        }

        public ActionResult SignalInfoBox(string signalID)
        {
            SignalInfoBoxViewModel viewModel = new SignalInfoBoxViewModel(signalID);
            return PartialView("SignalInfoBox", viewModel);
        }

        public ActionResult GetSignalLocation(string signalID)
        {
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            string signalLocation = repository.GetSignalLocation(signalID);
            if (signalLocation == string.Empty)
            {
                signalLocation = "Signal Not Found";
            }
            return Content(signalLocation);
        }

        public ActionResult FillSignals(int? page, int? filterType, string filterCriteria)
        {
            FillSignalsViewModel fsv;
            if (page == null)
            {
                fsv = new FillSignalsViewModel();
            }
            else
            {
                fsv = new FillSignalsViewModel(page??1, filterType, filterCriteria);
            }
            return PartialView("FillSignals", fsv);
        }

        public ActionResult GetMetricsList(int? selectedMetricID, string signalID)
        {
            MetricsListViewModel mlv = new MetricsListViewModel(signalID, selectedMetricID);
            return PartialView("GetMetricsList", mlv);
        }
        public ActionResult GetMap(DefaultChartsViewModel dcv)
        {
            return PartialView("Map", dcv);
        }

        public ActionResult MetricOptions(int id)
        {
            switch (id)
            {
                case 1:
                    PhaseTerminationOptions phaseTerminationOptions = new PhaseTerminationOptions();
                    return PartialView("PhaseTerminationOptions", phaseTerminationOptions);
                case 2:
                    SplitMonitorOptions SplitMonitorOptions = new SplitMonitorOptions();
                    return PartialView("SplitMonitorOptions", SplitMonitorOptions);
                case 3:
                    PedDelayOptions pedDelayOptions = new PedDelayOptions();
                    return PartialView("PedDelayOptions", pedDelayOptions);
                case 4:
                    MetricOptions preemptOptions = new MetricOptions();
                    preemptOptions.YAxisMax = 3;
                    preemptOptions.Y2AxisMax = 10;
                    return PartialView("PreemptOptions", preemptOptions);
                case 5:
                    TMCOptions tMCOptions = new TMCOptions();
                    return PartialView("TMCOptions", tMCOptions);
                case 6:
                    PCDOptions pcdOptions = new PCDOptions();
                    return PartialView("PCDOptions", pcdOptions);
                case 7:
                    ApproachVolumeOptions approachVolumeOptions = new ApproachVolumeOptions();
                    return PartialView("ApproachVolumeOptions", approachVolumeOptions);
                case 8:
                    ApproachDelayOptions approachDelayOptions = new ApproachDelayOptions();
                    return PartialView("ApproachDelayOptions", approachDelayOptions);
                case 9:
                    AoROptions aoROptions = new AoROptions();
                    return PartialView("AoROptions", aoROptions);
                case 10:
                    ApproachSpeedOptions approachSpeedOptions = new ApproachSpeedOptions();
                    return PartialView("ApproachSpeedOptions", approachSpeedOptions);
                case 11:
                    YellowAndRedOptions yellowAndRedOptions = new YellowAndRedOptions();
                    return PartialView("YellowAndRedOptions", yellowAndRedOptions);
                case 31:
                    LeftTurnGapAnalysisOptions leftTurnGapAnalysisOptions = new LeftTurnGapAnalysisOptions();
                    return PartialView("LeftTurnGapAnalysisOptions", leftTurnGapAnalysisOptions);
                case 32:
                    WaitTimeOptions waitTimeOptions = new WaitTimeOptions();
                    return PartialView("WaitTimeOptions", waitTimeOptions);
                case 17:
                    TimingAndActuationsOptions timingAndActuationsOptions = new TimingAndActuationsOptions();
                    return PartialView("TimingAndActuationsOptions", timingAndActuationsOptions);
                case 12: default:
                    SplitFailOptions splitFailOptions = new SplitFailOptions();
                    return PartialView("SplitFailOptions", splitFailOptions);
            }
        }

        public ActionResult GetPhaseTerminationMetricByUrl(PhaseTerminationOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 1); " +
                                                          "SetPhaseTerminationMetric(" + metricOptions.SelectedConsecutiveCount.ToString() + "," +
                                                          metricOptions.ShowPlanStripes.ToString().ToLower() + "," +
                                                          metricOptions.ShowPedActivity.ToString().ToLower() +                                                       
                                                          "); CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }
        public ActionResult TimingAndActionResultByUrl(TimingAndActuationsOptions  metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 17); " +
                                                          "SetTimingAndActuationsMetric(" +
                                                          metricOptions.ShowLegend.ToString().ToLower() + "," +
                                                          metricOptions.ShowHeaderForEachPhase.ToString().ToLower() + "," +
                                                          metricOptions.CombineLanesForEachGroup.ToString().ToLower() + "," +
                                                          metricOptions.DotAndBarSize + "," +
                                                          metricOptions.PhaseFilter + "," +
                                                          metricOptions.PhaseEventCodes + "," +
                                                          metricOptions.GlobalCustomEventCodes + "," +
                                                          metricOptions.GlobalCustomEventParams + "," +
                                                          metricOptions.ExtendVsdSearch + "," +
                                                          metricOptions.ShowVehicleSignalDisplay.ToString().ToLower() + "," +
                                                          metricOptions.ShowPedestrianIntervals.ToString().ToLower() + "," +
                                                          metricOptions.ShowPedestrianActuation.ToString().ToLower() + "," +
                                                          metricOptions.ExtendStartStopSearch + "," +
                                                          metricOptions.ShowStopBarPresence.ToString().ToLower() + "," +
                                                          metricOptions.ShowLaneByLaneCount.ToString().ToLower() + "," +
                                                          metricOptions.ShowAdvancedDilemmaZone.ToString().ToLower() + "," +
                                                          metricOptions.ShowAdvancedCount.ToString().ToLower() + "," +
                                                          metricOptions.AdvancedOffset + "," +
                                                          metricOptions.ShowAllLanesInfo.ToString().ToLower() + "," +
                                                          metricOptions.ShowLinesStartEnd.ToString().ToLower() + "," +
                                                          metricOptions.ShowEventPairs.ToString().ToLower() +
                                                          metricOptions.ShowRawEventData.ToString().ToLower() + "," +

                                                          "); CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        public ActionResult GetSplitMonitorMetricByUrl(SplitMonitorOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 2); " +
                                                          "SetSplitMonitorMetric(" + metricOptions.SelectedPercentileSplit.ToString() + "," +
                                                          metricOptions.ShowPlanStripes.ToString().ToLower() + "," +
                                                          metricOptions.ShowPedActivity.ToString().ToLower() + "," +
                                                          metricOptions.ShowAverageSplit.ToString().ToLower() + "," +
                                                          metricOptions.ShowPercentMaxOutForceOff.ToString().ToLower() + "," +
                                                          metricOptions.ShowPercentGapOuts.ToString().ToLower() + "," +
                                                          metricOptions.ShowPercentSkip.ToString().ToLower() +
                                                          "); CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        public ActionResult GetPedDelayMetricByUrl(PedDelayOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 3); " +
                                                          "CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        public ActionResult GetPreemptMetricByUrl(MetricOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 4); " +
                                                          "CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        public ActionResult GetTMCMetricByUrl(TMCOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 5); " +
                                                          "SetTMCMetric(" + metricOptions.SelectedBinSize.ToString() + "," +
                                                          metricOptions.ShowLaneVolumes.ToString().ToLower() + "," +
                                                          metricOptions.ShowTotalVolumes.ToString().ToLower() + "," +
                                                          metricOptions.ShowDataTable.ToString().ToLower() +
                                                          "); CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        public ActionResult GetPCDMetricByUrl(PCDOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 6); " +
                                                          "SetPCDMetric(" + metricOptions.SelectedBinSize.ToString() + "," +
                                                          metricOptions.SelectedDotSize.ToString().ToLower() + "," +
                                                          metricOptions.SelectedLineSize.ToString().ToLower() + "," +
                                                          metricOptions.ShowPlanStatistics.ToString().ToLower() + "," +
                                                          metricOptions.ShowVolumes.ToString().ToLower() + 
                                                          "); CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        public ActionResult GetApproachVolumeMetricByUrl(ApproachVolumeOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 7); " +
                                                          "SetApproachVolumeMetric(" + metricOptions.SelectedBinSize.ToString() + "," +
                                                          metricOptions.ShowDirectionalSplits.ToString().ToLower() + "," +
                                                          metricOptions.ShowTotalVolume.ToString().ToLower() + "," +
                                                          metricOptions.ShowSbWbVolume.ToString().ToLower() + "," +
                                                          metricOptions.ShowNbEbVolume.ToString().ToLower() + "," +
                                                          metricOptions.ShowTMCDetection.ToString().ToLower() + "," +
                                                          metricOptions.ShowAdvanceDetection.ToString().ToLower() +
                                                          "); CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        public ActionResult GetApproachDelayMetricByUrl(ApproachDelayOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 8); " +
                                                          "SetApproachDelayMetric(" + metricOptions.SelectedBinSize.ToString() + "," + 
                                                          metricOptions.ShowPlanStatistics.ToString().ToLower() + ","+
                                                          metricOptions.ShowTotalDelayPerHour.ToString().ToLower() + "," +
                                                          metricOptions.ShowDelayPerVehicle.ToString().ToLower() +
                                                          "); CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        public ActionResult GetAoRMetricByUrl(AoROptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 9); " +
                                                          "SetAoRMetric(" + metricOptions.SelectedBinSize.ToString() + "," + 
                                                          metricOptions.ShowPlanStatistics.ToString().ToLower()+"); " +
                                                          "CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        public ActionResult GetApproachSpeedMetricByUrl(ApproachSpeedOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 10); " +
                                                          "SetSpeedMetric(" + metricOptions.SelectedBinSize.ToString() + "," +
                                                          metricOptions.ShowPlanStatistics.ToString().ToLower() + "," +
                                                          metricOptions.ShowAverageSpeed.ToString().ToLower() + "," +
                                                          metricOptions.ShowPostedSpeed.ToString().ToLower() + ","+
                                                          metricOptions.Show85Percentile.ToString().ToLower() + "," +
                                                          metricOptions.Show15Percentile.ToString().ToLower() +
                                                          "); CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        public ActionResult GetYRAMetricByUrl(YellowAndRedOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 11); " +
                                                          "SetYRAMetric(" + metricOptions.SevereLevelSeconds.ToString() + "," +
                                                          metricOptions.ShowRedLightViolations.ToString().ToLower() + "," +
                                                          metricOptions.ShowSevereRedLightViolations.ToString().ToLower() + "," +
                                                          metricOptions.ShowPercentRedLightViolations.ToString().ToLower() + "," +
                                                          metricOptions.ShowPercentSevereRedLightViolations.ToString().ToLower() + "," +
                                                          metricOptions.ShowAverageTimeRedLightViolations.ToString().ToLower() + "," +
                                                          metricOptions.ShowYellowLightOccurrences.ToString().ToLower() + "," +
                                                          metricOptions.ShowPercentYellowLightOccurrences.ToString().ToLower() + "," +
                                                          metricOptions.ShowAverageTimeYellowOccurences.ToString().ToLower() +
                                                          "); CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        public ActionResult GetSplitFailMetricByUrl(SplitFailOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 12); " +
                                                          "SetSplitFailMetric(" + metricOptions.FirstSecondsOfRed.ToString() + "," + 
                                                          metricOptions.ShowFailLines.ToString().ToLower() + "," + 
                                                          metricOptions.ShowAvgLines.ToString().ToLower() + "," +
                                                          metricOptions.ShowPercentFailLines.ToString().ToLower() + "); CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        public ActionResult GetLeftTurnGapAnalysisMetricByUrl(LeftTurnGapAnalysisOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 31); " +
                                                          "CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        public ActionResult GetWaitTimeMetricByURL(WaitTimeOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 32);";

            return View("Index", defaultChartsViewModel);
        }


        private string GetCommonJavascriptProperties(MetricOptions metricOptions)
        {
            return @"SetCommonValues('" + metricOptions.SignalID + @"','" + metricOptions.StartDate.ToShortDateString() +
                   "','" + metricOptions.StartDate.ToString("hh") + ":" + metricOptions.StartDate.ToString("mm") + "','" +
                   metricOptions.StartDate.ToString("tt") + "','" + metricOptions.EndDate.ToShortDateString() +
                   "','" + metricOptions.EndDate.ToString("hh") + ":" + metricOptions.EndDate.ToString("mm") + "','" +
                   metricOptions.EndDate.ToString("tt") + "'," + "null, null); ";
            //endDateDay, endTime, endAmPmDdl, yAxisMax, y2AxisMax);
        }

        public ActionResult YellowAndRedOptions(int id)
        {
            YellowAndRedOptions yellowAndRedOptions = new YellowAndRedOptions();
            return PartialView("YellowAndRedOptions", yellowAndRedOptions);
        }

        public ActionResult GetYellowAndRedMetric(YellowAndRedOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID);
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result.ChartPaths = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/DefaultCharts/GetYRAMetricByUrl?");
            sb.Append("&SevereLevelSeconds=" + metricOptions.SevereLevelSeconds.ToString());
            sb.Append("&ShowRedLightViolations=" + metricOptions.ShowRedLightViolations.ToString().ToLower());
            sb.Append("&ShowPercentRedLightViolations=" +
                      metricOptions.ShowPercentRedLightViolations.ToString().ToLower());
            sb.Append("&ShowPercentSevereRedLightViolations=" +
                      metricOptions.ShowSevereRedLightViolations.ToString().ToLower());
            sb.Append("&ShowAverageTimeRedLightViolations=" +
                      metricOptions.ShowAverageTimeRedLightViolations.ToString().ToLower());
            sb.Append("&ShowYellowLightOccurrences=" +
                      metricOptions.ShowYellowLightOccurrences.ToString().ToLower());
            sb.Append("&ShowPercentYellowLightOccurrences=" +
                      metricOptions.ShowPercentYellowLightOccurrences.ToString().ToLower());
            sb.Append("&ShowAverageTimeYellowOccurences="+ metricOptions.ShowAverageTimeYellowOccurences.ToString().ToLower());
            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");
            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);
            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);
            result.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";
            return PartialView("MetricResult", result);
        }

        public ActionResult SplitFailOptions(int id)
        {
            SplitFailOptions splitFailOptions = new SplitFailOptions();
            return PartialView("SplitFailOptions", splitFailOptions);
        }

        public ActionResult GetSplitFailMetric(SplitFailOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID);
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client = new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result.ChartPaths = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/DefaultCharts/GetSplitFailMetricByUrl?");
            sb.Append("&FirstSecondsOfRed=" + metricOptions.FirstSecondsOfRed.ToString());
            sb.Append("&ShowFailLines="+metricOptions.ShowFailLines.ToString().ToLower());
            sb.Append("&ShowAvgLines=" +metricOptions.ShowAvgLines.ToString().ToLower());
            sb.Append("&ShowPercentFailLines=" + metricOptions.ShowPercentFailLines.ToString().ToLower());
            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");
            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);
            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);
            result.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";
            return PartialView("MetricResult", result);
        }

        public ActionResult ApproachSpeedOptions(int id)
        {
            ApproachSpeedOptions approachSpeedOptions =
                new ApproachSpeedOptions();
            return PartialView("ApproachSpeedOptions", approachSpeedOptions);
        }

        public ActionResult GetApproachSpeedMetric(ApproachSpeedOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID);
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client = new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result.ChartPaths = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/DefaultCharts/GetApproachSpeedMetricByUrl?");
            sb.Append("&SelectedBinSize=" + metricOptions.SelectedBinSize.ToString());
            sb.Append("&ShowPlanStatistics=" + metricOptions.ShowPlanStatistics.ToString().ToLower());
            sb.Append("&ShowAverageSpeed=" + metricOptions.ShowAverageSpeed.ToString().ToLower());
            sb.Append("&ShowPostedSpeed=" + metricOptions.ShowPostedSpeed.ToString().ToLower());
            sb.Append("&Show85Percentile=" + metricOptions.Show85Percentile.ToString().ToLower());
            sb.Append("&Show15Percentile=" + metricOptions.Show15Percentile.ToString().ToLower());
            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");
            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);
            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);
            result.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";
            return PartialView("MetricResult", result);
        }

        public ActionResult AoROptions(int id)
        {
            AoROptions aoROptions = new AoROptions();
            return PartialView("AoROptions", aoROptions);
        }

        public ActionResult GetAoRMetric(AoROptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID);
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result.ChartPaths = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/DefaultCharts/GetAoRMetricByUrl?");
            sb.Append("&SelectedBinSize=" + metricOptions.SelectedBinSize.ToString());
            sb.Append("&ShowPlanStatistics=" + metricOptions.ShowPlanStatistics.ToString().ToLower());
            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");
            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);
            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);
            result.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";
            return PartialView("MetricResult", result);
        }

        public ActionResult ApproachDelayOptions(int id)
        {
            ApproachDelayOptions approachDelayOptions = new ApproachDelayOptions();
            return PartialView("ApproachDelayOptions", approachDelayOptions);
        }

        public ActionResult GetApproachDelayMetric(ApproachDelayOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID);
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client = new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result.ChartPaths = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/DefaultCharts/GetApproachDelayMetricByUrl?");
            sb.Append("&SelectedBinSize=" + metricOptions.SelectedBinSize.ToString());
            sb.Append("&ShowPlanStatistics=" + metricOptions.ShowPlanStatistics.ToString().ToLower());
            sb.Append("&ShowTotalDelayPerHour=" + metricOptions.ShowTotalDelayPerHour.ToString().ToLower());
            sb.Append("&ShowDelayPerVehicle=" + metricOptions.ShowDelayPerVehicle.ToString().ToLower());
            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");
            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);
            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);
            result.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";
            return PartialView("MetricResult", result);
        }

        public ActionResult PhaseTerminationOptions(int id)
        {
            PhaseTerminationOptions phaseTerminationOptions = new PhaseTerminationOptions(); 
            return PartialView("PhaseTerminationOptions", phaseTerminationOptions);
        }

        public ActionResult GetPhaseTerminationMetric(PhaseTerminationOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID);
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result.ChartPaths = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/DefaultCharts/GetPhaseTerminationMetricByUrl?");
            sb.Append("&SelectedConsecutiveCount=" + metricOptions.SelectedConsecutiveCount.ToString());
            sb.Append("&ShowPlanStripes=" + metricOptions.ShowPlanStripes.ToString().ToLower());
            sb.Append("&ShowPedActivity=" + metricOptions.ShowPedActivity.ToString().ToLower());
            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");
            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);
            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);
            result.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" +hostname.Trim() + sb + "\");";
            return PartialView("MetricResult", result);
        }
        
        public ActionResult GetTimingAndActuations (TimingAndActuationsOptions  metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID);
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result.ChartPaths = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/DefaultCharts/TimingAndActuationsByUrl?");
            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");
            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);
            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);
            result.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";
            return PartialView("MetricResult", result);
        }

        public ActionResult GetPreemptMetric(MetricOptions metricOptions)
        {
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();

            if (ModelState.IsValid)
            {
                PreemptServiceRequestOptions requestOptions = new PreemptServiceRequestOptions(metricOptions.SignalID,
                        metricOptions.StartDate, metricOptions.EndDate);
                requestOptions.MetricTypeID = 14; 
                PreemptServiceMetricOptions serviceOptions = new PreemptServiceMetricOptions(metricOptions.SignalID,
                        metricOptions.StartDate, metricOptions.EndDate, metricOptions.YAxisMax??0);
                serviceOptions.MetricTypeID = 15; 
                PreemptDetailOptions detailOptions = new PreemptDetailOptions(metricOptions.SignalID,
                        metricOptions.StartDate, metricOptions.EndDate);
                detailOptions.MetricTypeID = metricOptions.MetricTypeID; 
                MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
                client.Open();
                string[] tempResult1 = client.CreateMetric(requestOptions);
                string[] tempResult2 = client.CreateMetric(serviceOptions);
                string[] tempResult3 = client.CreateMetric(detailOptions);
                client.Close();
                List<string> finalList = new List<string>();
                finalList.AddRange(tempResult1);
                finalList.AddRange(tempResult2);
                finalList.AddRange(tempResult3);
                result.ChartPaths = finalList.ToArray();
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/DefaultCharts/GetPreemptMetricByUrl?");
            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");
            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);
            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);
            result.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";
            return PartialView("MetricResult", result);
        }

        public ActionResult TMCOptions(int id)
        {
            TMCOptions tMCOptions = new TMCOptions();
            return PartialView("TMCOptions", tMCOptions);
        }

        public ActionResult GetTMCMetric(TMCOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID); 
            MOE.Common.Business.TMC.TMCInfo tmcInfo = new MOE.Common.Business.TMC.TMCInfo();
            MOE.Common.Business.TMC.TMCViewModel tmcvm = new MOE.Common.Business.TMC.TMCViewModel(
                metricOptions.ShowLaneVolumes, metricOptions.ShowDataTable);
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client = new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    tmcInfo = client.CreateTMCChart(metricOptions);
                    client.Close();
                    if (metricOptions.ShowDataTable)
                    {
                        tmcvm.PopulateViewModel(tmcInfo.tmcData, metricOptions.SelectedBinSize);
                    }
                    tmcvm.ImageLocations.AddRange(tmcInfo.ImageLocations);
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/DefaultCharts/GetTMCMetricByUrl?");
            sb.Append("&SelectedBinSize=" + metricOptions.SelectedBinSize.ToString());
            sb.Append("&ShowLaneVolumes=" + metricOptions.ShowLaneVolumes.ToString().ToLower());
            sb.Append("&ShowTotalVolumes=" + metricOptions.ShowTotalVolumes.ToString().ToLower());
            sb.Append("&ShowDataTable=" + metricOptions.ShowDataTable.ToString().ToLower());
            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");
            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);
            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);
            tmcvm.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";
            return PartialView("GetTMCMetric", tmcvm);
        }

        public ActionResult ApproachVolumeOptions(int id)
        {
            ApproachVolumeOptions approachVolumeOptions = new ApproachVolumeOptions();
            return PartialView("ApproachVolumeOptions", approachVolumeOptions);
        }

        public ActionResult GetApproachVolumeMetric(ApproachVolumeOptions metricOptions)
        {
            MOE.Common.Models.Repositories.IApplicationEventRepository logRepository = MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
            MOE.Common.Business.ApproachVolume.ApproachVolumeViewModel viewModel = new ApproachVolumeViewModel();
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client = new MetricGeneratorService.MetricGeneratorClient();
                try
                {                    
                    client.Open();
                    viewModel.InfoList = client.CreateMetricWithDataTable((ApproachVolumeOptions)metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                } 
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/DefaultCharts/GetApproachVolumeMetricByUrl?");
            sb.Append("&SelectedBinSize=" + metricOptions.SelectedBinSize.ToString());
            sb.Append("&ShowDirectionalSplits=" + metricOptions.ShowDirectionalSplits.ToString());
            sb.Append("&ShowTotalVolume=" + metricOptions.ShowTotalVolume.ToString().ToLower());
            sb.Append("&ShowSbWbVolume=" + metricOptions.ShowSbWbVolume.ToString().ToLower());
            sb.Append("&ShowNbEbVolume=" + metricOptions.ShowNbEbVolume.ToString().ToLower());
            sb.Append("&ShowTMCDetection=" + metricOptions.ShowTMCDetection.ToString().ToLower());
            sb.Append("&ShowAdvanceDetection=" + metricOptions.ShowAdvanceDetection.ToString().ToLower());
            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");
            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);
            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);
            viewModel.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";
            return PartialView("GetApproachVolumeMetric", viewModel);
        }

        public ActionResult GetSplitMonitorMetric(SplitMonitorOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID);
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client = new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result.ChartPaths = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/DefaultCharts/GetSplitMonitorMetricByUrl?");
            sb.Append("&SelectedPercentileSplit=" + metricOptions.SelectedPercentileSplit.ToString());
            sb.Append("&ShowPlanStripes=" + metricOptions.ShowPlanStripes.ToString().ToLower());
            sb.Append("&ShowPedActivity=" + metricOptions.ShowPedActivity.ToString().ToLower());
            sb.Append("&ShowAverageSplit=" + metricOptions.ShowAverageSplit.ToString().ToLower());
            sb.Append("&ShowPercentMaxOutForceOff=" + metricOptions.ShowPercentMaxOutForceOff.ToString().ToLower());
            sb.Append("&ShowPercentGapOuts=" + metricOptions.ShowPercentGapOuts.ToString().ToLower());
            sb.Append("&ShowPercentSkip=" + metricOptions.ShowPercentSkip.ToString().ToLower());
            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");
            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);
            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);
            result.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";
            return PartialView("MetricResult", result);
        }

        public ActionResult PedDelayOptions(int id)
        {
            PedDelayOptions pedDelayOptions = new PedDelayOptions();
            return PartialView("PedDelayOptions", pedDelayOptions);
        }

        public ActionResult GetPedDelayMetric(PedDelayOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID);
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client = new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result.ChartPaths = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/DefaultCharts/GetPedDelayMetricByUrl?");
            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");
            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);
            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);
            result.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";
            return PartialView("MetricResult", result);
        }

        public ActionResult GetLeftTurnGapAnalysisMetric(LeftTurnGapAnalysisOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID);
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                        new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result.ChartPaths = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("/DefaultCharts/GetLeftTurnGapAnalysisMetricByUrl?");

            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");

            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);

            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);

            result.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";

            return PartialView("MetricResult", result);
        }

        public ActionResult GetWaitTimeMetric(WaitTimeOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID);
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                        new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result.ChartPaths = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("/DefaultCharts/GetWaitTimeMetricByUrl?");

            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");

            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);

            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);

            result.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";

            return PartialView("MetricResult", result);
        }

        public ActionResult GetPCDMetric(PCDOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID);
            Models.MetricResultViewModel result = new Models.MetricResultViewModel();
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client = new MetricGeneratorService.MetricGeneratorClient();
                try
                {                    
                    client.Open();
                    result.ChartPaths = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                } 
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("/DefaultCharts/GetPCDMetricByUrl?");
            sb.Append("&SelectedBinSize=" + metricOptions.SelectedBinSize.ToString());
            sb.Append("&SelectedDotSize=" + metricOptions.SelectedDotSize.ToString().ToLower());
            sb.Append("&SelectedLineSize=" + metricOptions.SelectedLineSize.ToString().ToLower());
            sb.Append("&ShowPlanStatistics=" + metricOptions.ShowPlanStatistics.ToString().ToLower());
            sb.Append("&ShowVolumes=" + metricOptions.ShowVolumes.ToString().ToLower());
            sb.Append("&SignalID=" + metricOptions.SignalID);
            string _startDate = metricOptions.StartDate.ToString().Trim();
            _startDate = _startDate.Replace(" ", "%20");
            string _endDate = metricOptions.EndDate.ToString().Trim();
            _endDate = _endDate.Replace(" ", "%20");
            sb.Append("&StartDate=" + _startDate);
            sb.Append("&EndDate=" + _endDate);
            string fullUri = Request.Url.AbsoluteUri;
            int placeCounter = fullUri.IndexOf("/DefaultCharts/");
            string hostname = fullUri.Substring(0, placeCounter);
            result.ShowMetricUrlJavascript = "window.history.pushState(\"none\", \"none\", \"" + hostname.Trim() + sb + "\");";
            return PartialView("MetricResult", result);
        }

        public string GetChartComment(string SignalID, int MetricID)
        {
            MOE.Common.Models.Repositories.IMetricCommentRepository mcr = MOE.Common.Models.Repositories.MetricCommentRepositoryFactory.Create();
            MOE.Common.Models.Comment comment = mcr.GetLatestCommentForReport(SignalID, MetricID);
            if (comment != null)
            {
                return comment.CommentText;
            }
            return "";
        }

        private MOE.Common.Models.MetricType GetMetricType(int id)
        {
            MOE.Common.Models.Repositories.IMetricTypeRepository metricRepository = MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
            return  metricRepository.GetMetricsByID(id);
        }
    }
}