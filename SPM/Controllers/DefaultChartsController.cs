using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Models.ViewModel.Chart;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Business.ApproachVolume;
using System.Data.Entity.Migrations;

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

        //public ActionResult GetAvailableMetricsForSignal(string signalID)
        //{
        //    return PartialView("MetricsList", dcv);
        //}

        public ActionResult GetMetricProperties(MetricOptions options)
        {
            return PartialView("MetricProperties", options);
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
                    PhaseTerminationOptions phaseTerminationOptions =
                        new PhaseTerminationOptions();
                    phaseTerminationOptions.SetDefaults();
                    return PartialView("PhaseTerminationOptions", phaseTerminationOptions);

                case 2:
                    SplitMonitorOptions SplitMonitorOptions =
                        new SplitMonitorOptions();
                    return PartialView("SplitMonitorOptions", SplitMonitorOptions);
                
                case 3:
                    PedDelayOptions pedDelayOptions =
                        new PedDelayOptions();
                    pedDelayOptions.SetDefaults();
                    return PartialView("PedDelayOptions", pedDelayOptions);

                case 5:
                    TMCOptions tMCOptions =
                        new TMCOptions();
                    tMCOptions.SetDefaults();
                    return PartialView("TMCOptions", tMCOptions);

                case 4:
                    MetricOptions preemptOptions =
                        new MetricOptions();
                    preemptOptions.YAxisMax = 3;
                    preemptOptions.Y2AxisMax = 10;
                    return PartialView("PreemptOptions", preemptOptions);
                case 6:
                    PCDOptions pcdOptions =
                        new PCDOptions();
                    return PartialView("PCDOptions", pcdOptions);

                case 7:
                    ApproachVolumeOptions approachVolumeOptions =
                        new ApproachVolumeOptions();
                    return PartialView("ApproachVolumeOptions", approachVolumeOptions);

                case 8:
                    ApproachDelayOptions approachDelayOptions =
                        new ApproachDelayOptions();
                    approachDelayOptions.SetDefaults();
                    return PartialView("ApproachDelayOptions", approachDelayOptions);

                case 9:
                    AoROptions aoROptions =
                        new AoROptions();
                    aoROptions.SetDefaults();
                    return PartialView("AoROptions", aoROptions);

                case 10:
                    ApproachSpeedOptions approachSpeedOptions =
                        new ApproachSpeedOptions();
                    approachSpeedOptions.SetDefaults();
                    return PartialView("ApproachSpeedOptions", approachSpeedOptions);

                case 11:
                    YellowAndRedOptions yellowAndRedOptions =
                        new YellowAndRedOptions();
                    yellowAndRedOptions.SetDefaults();
                    return PartialView("YellowAndRedOptions", yellowAndRedOptions);

                case 12: default:
                    SplitFailOptions splitFailOptions =
                        new SplitFailOptions();
                    splitFailOptions.SetDefaults();
                    return PartialView("SplitFailOptions", splitFailOptions);

            }

        }

        public ActionResult SplitFailOptions(int id)
        {
            SplitFailOptions splitFailOptions =
                new SplitFailOptions();
            splitFailOptions.SetDefaults();
            return PartialView("SplitFailOptions", splitFailOptions);
        }

        public ActionResult GetSplitFailMetric(SplitFailOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID); 
            string[] result = new string[1] { "" };
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            return PartialView("MetricResult", result);
        }

        public ActionResult GetSplitFailMetricByUrl(SplitFailOptions metricOptions)
        {
            DefaultChartsViewModel defaultChartsViewModel = new DefaultChartsViewModel();
            defaultChartsViewModel.RunMetricJavascript = GetCommonJavascriptProperties(metricOptions);
            defaultChartsViewModel.RunMetricJavascript += "GetMetricsList('" + metricOptions.SignalID + "', 12); " +
            "SetSplitFailMetric(" + metricOptions.FirstSecondsOfRed.ToString() +"," + metricOptions.ShowFailLines.ToString().ToLower() +
            "," + metricOptions.ShowAvgLines.ToString().ToLower() + "," + metricOptions.ShowPercentFailLines.ToString().ToLower() + "); CreateMetric();";
            return View("Index", defaultChartsViewModel);
        }

        private string GetCommonJavascriptProperties(MetricOptions metricOptions)
        {
            return @"SetCommonValues('" + metricOptions.SignalID + @"','" + metricOptions.StartDate.ToShortDateString() +
                   "','" + metricOptions.StartDate.ToString("hh") + ":" + metricOptions.StartDate.ToString("mm") + "','" +
                   metricOptions.StartDate.ToString("tt") + "','" + metricOptions.EndDate.ToShortDateString() +
                   "','" + metricOptions.EndDate.ToString("hh") + ":" + metricOptions.EndDate.ToString("mm") + "','" +
                   metricOptions.StartDate.ToString("tt") + "'," + "null, null); ";
            //endDateDay, endTime, endAmPmDdl, yAxisMax, y2AxisMax);
        }

        public ActionResult YellowAndRedOptions(int id)
        {
            YellowAndRedOptions yellowAndRedOptions =
                new YellowAndRedOptions();
            yellowAndRedOptions.SetDefaults();
            return PartialView("YellowAndRedOptions", yellowAndRedOptions);
        }

        public ActionResult GetYellowAndRedMetric(YellowAndRedOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID); 
            string[] result = new string[1] { "" };
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
                try
                {

                    client.Open();
                    result = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            return PartialView("MetricResult", result);
        }

        public ActionResult ApproachSpeedOptions(int id)
        {
            ApproachSpeedOptions approachSpeedOptions =
                new ApproachSpeedOptions();
            approachSpeedOptions.SetDefaults();
            return PartialView("ApproachSpeedOptions", approachSpeedOptions);
        }

        public ActionResult GetApproachSpeedMetric(ApproachSpeedOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID); 
            string[] result = new string[1] { "" };
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
                try
                {

                    client.Open();
                    result = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            return PartialView("MetricResult", result);
        }

        public ActionResult AoROptions(int id)
        {
            AoROptions aoROptions =
                new AoROptions();
            aoROptions.SetDefaults();
            return PartialView("AoROptions", aoROptions);
        }

        public ActionResult GetAoRMetric(AoROptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID); 
            string[] result = new string[1] { "" };
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
                try
                {

                    client.Open();
                    result = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            return PartialView("MetricResult", result);
        }

        public ActionResult ApproachDelayOptions(int id)
        {
            ApproachDelayOptions approachDelayOptions =
                new ApproachDelayOptions();
            approachDelayOptions.SetDefaults();
            return PartialView("ApproachDelayOptions", approachDelayOptions);
        }

        public ActionResult GetApproachDelayMetric(ApproachDelayOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID); 
            string[] result = new string[1] { "" };
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
                try
                {

                    client.Open();
                    result = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            return PartialView("MetricResult", result);
        }

        public ActionResult PhaseTerminationOptions(int id)
        {
            PhaseTerminationOptions phaseTerminationOptions =
                new PhaseTerminationOptions(); 
            phaseTerminationOptions.SetDefaults();
            return PartialView("PhaseTerminationOptions", phaseTerminationOptions);
        }

        public ActionResult GetPhaseTerminationMetric(PhaseTerminationOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID); 
            string[] result = new string[1] { "" };
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();

                try
                {

                    client.Open();
                    result = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            return PartialView("MetricResult", result);
        }
        public ActionResult TMCOptions(int id)
        {
            TMCOptions tMCOptions =
                new TMCOptions();
            tMCOptions.SetDefaults();
            return PartialView("TMCOptions", tMCOptions);
        }

        public ActionResult GetPreemptMetric(MetricOptions metricOptions)
        {
            
            string[] result = new string[1] { "" };
            if (ModelState.IsValid)
            {
                string[] tempResult1;
                string[] tempResult2;
                string[] tempResult3;
                PreemptServiceRequestOptions requestOptions =
                    new PreemptServiceRequestOptions(metricOptions.SignalID,
                        metricOptions.StartDate, metricOptions.EndDate);
                requestOptions.MetricTypeID = 14; 
                PreemptServiceMetricOptions serviceOptions =
                    new PreemptServiceMetricOptions(metricOptions.SignalID,
                        metricOptions.StartDate, metricOptions.EndDate, metricOptions.YAxisMax??0);
                serviceOptions.MetricTypeID = 15; 
                PreemptDetailOptions detailOptions =
                    new PreemptDetailOptions(metricOptions.SignalID,
                        metricOptions.StartDate, metricOptions.EndDate);
                detailOptions.MetricTypeID = metricOptions.MetricTypeID; 

                MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
                client.Open();
                tempResult1 = client.CreateMetric(requestOptions);
                tempResult2 = client.CreateMetric(serviceOptions);                
                tempResult3 = client.CreateMetric(detailOptions);
                client.Close();
                List<string> finalList = new List<string>();
                finalList.AddRange(tempResult1);
                finalList.AddRange(tempResult2);
                finalList.AddRange(tempResult3);
                result = finalList.ToArray();
            }
            return PartialView("MetricResult", result);
        }

        public ActionResult GetTMCMetric(TMCOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID); 
            //string[] result = new string[1] { "" };
            MOE.Common.Business.TMC.TMCInfo tmcInfo = new MOE.Common.Business.TMC.TMCInfo();
            MOE.Common.Business.TMC.TMCViewModel tmcvm = new MOE.Common.Business.TMC.TMCViewModel(
                metricOptions.ShowLaneVolumes, metricOptions.ShowDataTable);
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                    new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    tmcInfo = client.CreateTMCChart(metricOptions);
                    client.Close();
                    if (metricOptions.ShowDataTable)
                    {
                        tmcvm.PopulateViewModel(tmcInfo.tmcData, metricOptions.SelectedBinSize);
                    }
                    tmcvm.ImageLocations = tmcInfo.ImageLocations;
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            return PartialView("GetTMCMetric", tmcvm);
        }

        public ActionResult ApproachVolumeOptions(int id)
        {
            ApproachVolumeOptions approachVolumeOptions =
                new ApproachVolumeOptions();
            approachVolumeOptions.SetDefaults();
            return PartialView("ApproachVolumeOptions", approachVolumeOptions);
        }

        public ActionResult GetApproachVolumeMetric(ApproachVolumeOptions metricOptions)
        {
            MOE.Common.Models.Repositories.IApplicationEventRepository logRepository =
                MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
            MetricInfo[] metrics = null;
            //metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID); 
            //string[] result = new string[1] { "" };
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                        new MetricGeneratorService.MetricGeneratorClient();
                try
                {                    
                    client.Open();
                    metrics = client.CreateMetricWithDataTable((ApproachVolumeOptions)metricOptions);
                    client.Close();
                   
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                } 
            }
            return PartialView("GetApproachVolumeMetric", metrics);
        }


        public ActionResult GetSplitMonitorMetric(SplitMonitorOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID); 
            string[] result = new string[1] { "" };
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                        new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            return PartialView("MetricResult", result);
        }
        public ActionResult PedDelayOptions(int id)
        {
            PedDelayOptions pedDelayOptions =
                new PedDelayOptions();
            pedDelayOptions.SetDefaults();
            return PartialView("PedDelayOptions", pedDelayOptions);
        }

        public ActionResult GetPedDelayMetric(PedDelayOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID); 
            string[] result = new string[1] { "" };
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                        new MetricGeneratorService.MetricGeneratorClient();
                try
                {
                    client.Open();
                    result = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                }
            }
            return PartialView("MetricResult", result);
        }

        public ActionResult GetPCDMetric(PCDOptions metricOptions)
        {
            metricOptions.MetricType = GetMetricType(metricOptions.MetricTypeID); 
            string[] result = new string[1] { "" };
            if (ModelState.IsValid)
            {
                MetricGeneratorService.MetricGeneratorClient client =
                        new MetricGeneratorService.MetricGeneratorClient();
                try
                {                    
                    client.Open();
                    result = client.CreateMetric(metricOptions);
                    client.Close();
                }
                catch (Exception ex)
                {
                    client.Close();
                    return Content("<h1>" + ex.Message + "</h1>");
                } 
            }
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
            MOE.Common.Models.Repositories.IMetricTypeRepository metricRepository =
                MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
            return  metricRepository.GetMetricsByID(id);
        }

        

       

    }
}