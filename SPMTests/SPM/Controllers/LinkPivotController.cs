using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MOE.Common;
using MOE.Common.Models;

namespace SPM.Controllers
{
    [AllowAnonymous]
    public class LinkPivotController : Controller
    {
        private MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();

    // GET: LinkPivot
        public ActionResult Analysis()
        {
            var lp = new MOE.Common.Models.ViewModel.LinkPivotViewModel();
            lp.Routes = (from a in db.Routes
                               orderby a.RouteName
                               select a).ToList();
            lp.Bias = 0;
            lp.BiasUpDownStream = "Downstream";
            lp.StartingPoint = "Downstream";
            lp.StartDate = DateTime.Today.AddDays(-1);
            lp.EndDate = DateTime.Today.AddDays(-1);
            lp.StartTime = "8:00";
            lp.EndTime = "9:00";
            lp.StartAMPM = "AM";
            lp.EndAMPM = "AM";
            lp.CycleLength = 90;
            return View(lp);
        }

        public ActionResult FillSignals(int id)
        {
            var routeRepository = MOE.Common.Models.Repositories.RouteRepositoryFactory.Create();
            var route = routeRepository.GetRouteByID(id);
            List <MOE.Common.Models.Signal> signals = new List<Signal>();
            var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            foreach (var routeSignal in route.RouteSignals)
            {
                var signal = signalRepository.GetLatestVersionOfSignalBySignalID(routeSignal.SignalId);
                signals.Add(signal);
            }
            return PartialView("FillSignals", signals);
        }

        public ActionResult LinkPivotResult(MOE.Common.Models.ViewModel.LinkPivotViewModel lpvm)
        {
            if (ModelState.IsValid)
            {
                DateTime startDate = Convert.ToDateTime(lpvm.StartDate.Value.ToShortDateString() + " " + lpvm.StartTime +
                    " " + lpvm.StartAMPM);
                DateTime endDate = Convert.ToDateTime(lpvm.EndDate.Value.ToShortDateString() + " " + lpvm.EndTime +
                    " " + lpvm.EndAMPM);

                LinkPivotServiceReference.LinkPivotServiceClient client =
                    new LinkPivotServiceReference.LinkPivotServiceClient();

                //TestLinkPivot.LinkPivotServiceClient client =
                //    new TestLinkPivot.LinkPivotServiceClient();


                LinkPivotServiceReference.AdjustmentObject[] adjustments;
                //MOEWcfServiceLibrary.AdjustmentObject[] adjustments;
                client.Open();

                //Based on the starting point selected by the user Create the link pivot object
                if (lpvm.StartingPoint == "Downstream")
                {
                    adjustments = client.GetLinkPivot(
                        lpvm.SelectedRouteId,
                        startDate,
                        endDate,
                        lpvm.CycleLength,
                        "Downstream",
                        lpvm.Bias,
                        lpvm.BiasUpDownStream,
                        lpvm.PostedDays.DayIDs.Contains("0"),//Sunday,
                        lpvm.PostedDays.DayIDs.Contains("1"),//Monday
                        lpvm.PostedDays.DayIDs.Contains("2"),//Tuesday
                        lpvm.PostedDays.DayIDs.Contains("3"),//Wednesday
                        lpvm.PostedDays.DayIDs.Contains("4"),//Thursday
                        lpvm.PostedDays.DayIDs.Contains("5"),//Friday
                        lpvm.PostedDays.DayIDs.Contains("6"));//Saturday

                }
                else
                {
                    adjustments = client.GetLinkPivot(
                        lpvm.SelectedRouteId,
                        startDate,
                        endDate,
                        lpvm.CycleLength,
                        "Upstream",
                        lpvm.Bias,
                        lpvm.BiasUpDownStream,
                        lpvm.PostedDays.DayIDs.Contains("0"),//Sunday,
                        lpvm.PostedDays.DayIDs.Contains("1"),//Monday
                        lpvm.PostedDays.DayIDs.Contains("2"),//Tuesday
                        lpvm.PostedDays.DayIDs.Contains("3"),//Wednesday
                        lpvm.PostedDays.DayIDs.Contains("4"),//Thursday
                        lpvm.PostedDays.DayIDs.Contains("5"),//Friday
                        lpvm.PostedDays.DayIDs.Contains("6"));//Saturday
                }
                client.Close();
                MOE.Common.Models.ViewModel.LinkPivotResultViewModel lprvm =
                    new MOE.Common.Models.ViewModel.LinkPivotResultViewModel();

                double totalVolume = 0;
                double totalDownstreamVolume = 0;
                double totalUpstreamVolume = 0;
                foreach (LinkPivotServiceReference.AdjustmentObject a in adjustments)
                {
                    lprvm.Adjustments.Add(new MOE.Common.Models.ViewModel.LinkPivotAdjustment(a.LinkNumber, a.SignalId, a.Location,
                        a.Delta, a.Adjustment));

                    lprvm.ApproachLinks.Add(new MOE.Common.Models.ViewModel.LinkPivotApproachLink(a.SignalId,
                        a.Location, a.UpstreamApproachDirection,
                        a.DownSignalId, a.DownstreamLocation, a.DownstreamApproachDirection, a.PAOGUpstreamBefore,
                        a.PAOGUpstreamPredicted, a.PAOGDownstreamBefore, a.PAOGDownstreamPredicted,
                        a.AOGUpstreamBefore, a.AOGUpstreamPredicted, a.AOGDownstreamBefore,
                        a.AOGDownstreamPredicted, a.Delta, a.ResultChartLocation, a.AogTotalBefore,
                        a.PAogTotalBefore, a.AogTotalPredicted, a.PAogTotalPredicted, a.LinkNumber
                        ));

                    totalVolume = totalVolume + a.DownstreamVolume + a.UpstreamVolume;
                    totalDownstreamVolume = totalDownstreamVolume + a.DownstreamVolume;
                    totalUpstreamVolume = totalUpstreamVolume + a.UpstreamVolume;
                }

                //Remove the last row from approch links because it will always be 0
                lprvm.ApproachLinks.RemoveAt(lprvm.ApproachLinks.Count - 1);

                //Get the totals
                lprvm.TotalAogDownstreamBefore = adjustments.Sum(a => a.AOGDownstreamBefore);
                lprvm.TotalPaogDownstreamBefore = Math.Round((adjustments.Sum(a => a.AOGDownstreamBefore)/totalDownstreamVolume)*100);
                lprvm.TotalAogDownstreamPredicted = adjustments.Sum(a => a.AOGDownstreamPredicted);
                lprvm.TotalPaogDownstreamPredicted = Math.Round((adjustments.Sum(a => a.AOGDownstreamPredicted) / totalDownstreamVolume)*100);

                lprvm.TotalAogUpstreamBefore = adjustments.Sum(a => a.AOGUpstreamBefore);
                lprvm.TotalPaogUpstreamBefore = Math.Round((adjustments.Sum(a => a.AOGUpstreamBefore) / totalUpstreamVolume)*100);
                lprvm.TotalAogUpstreamPredicted = adjustments.Sum(a => a.AOGUpstreamPredicted);
                lprvm.TotalPaogUpstreamPredicted = Math.Round((adjustments.Sum(a => a.AOGUpstreamPredicted) / totalUpstreamVolume)*100);

                lprvm.TotalAogBefore = lprvm.TotalAogUpstreamBefore + lprvm.TotalAogDownstreamBefore;
                lprvm.TotalPaogBefore = Math.Round((lprvm.TotalAogBefore / totalVolume) * 100);
                lprvm.TotalAogPredicted = lprvm.TotalAogUpstreamPredicted + lprvm.TotalAogDownstreamPredicted;
                lprvm.TotalPaogPredicted = Math.Round((lprvm.TotalAogPredicted / totalVolume) * 100);

                //once all the data has been set we get the summary info.
                lprvm.SetSummary();

                return PartialView("LinkPivotResult", lprvm);
            }
            return View(lpvm);
        }
        [HttpPost]
        public ActionResult LinkPivotPCDOptions(MOE.Common.Models.ViewModel.LinkPivotViewModel lpvm)
        {
            if (ModelState.IsValid)
            {
                MOE.Common.Models.ViewModel.LinkPivotPCDOptions options =
                    new MOE.Common.Models.ViewModel.LinkPivotPCDOptions();

                options.Dates = lpvm.GetDays();
                options.YAxis = 150;
                options.DownDirection = lpvm.SelectedDownstreamDirection;
                options.UpstreamDirection = lpvm.SelectedUpstreamDirection;
                options.SignalId = lpvm.SelectedSignalId;
                options.DownSignalId = lpvm.SelectedDownSignalId;
                options.Delta = lpvm.SelectedDelta;
                options.EndDate = Convert.ToDateTime(lpvm.EndDate.Value.ToShortDateString() + 
                    " " + lpvm.EndTime +
                    " " + lpvm.EndAMPM);

                return PartialView("LinkPivotPCDOptions", options);
            }
            return Content("Invalid Parameters");
        }

        public ActionResult PCDs(MOE.Common.Models.ViewModel.LinkPivotPCDOptions pcdOptions)
        {
            if (ModelState.IsValid)
            {
                LinkPivotServiceReference.LinkPivotServiceClient client = new LinkPivotServiceReference.LinkPivotServiceClient();
                LinkPivotServiceReference.DisplayObject display = new LinkPivotServiceReference.DisplayObject();                
                pcdOptions.SelectedEndDate = Convert.ToDateTime(pcdOptions.SelectedStartDate[0].ToShortDateString() + " " +
                    pcdOptions.EndDate.ToShortTimeString());
                DateTime pcdEndDate = Convert.ToDateTime(pcdOptions.SelectedStartDate[0].ToShortDateString()
                    + " " + pcdOptions.SelectedEndDate.Value.TimeOfDay.ToString());                
                client.Open();
                display = client.DisplayLinkPivotPCD(pcdOptions.SignalId, pcdOptions.UpstreamDirection,
                    pcdOptions.DownSignalId, pcdOptions.DownDirection, pcdOptions.Delta, pcdOptions.SelectedStartDate[0],
                    pcdEndDate, pcdOptions.YAxis);
                client.Close();
                MOE.Common.Models.ViewModel.LinkPivotPCDsViewModel pcdModel = new MOE.Common.Models.ViewModel.LinkPivotPCDsViewModel();

                var settingsRepository = MOE.Common.Models.Repositories.ApplicationSettingsRepositoryFactory.Create();
                GeneralSettings settings = settingsRepository.GetGeneralSettings();
                string imagePath = settings.ImagePath;

                pcdModel.ExistingChart = display.UpstreamBeforePCDPath;
                pcdModel.PredictedChart = display.UpstreamAfterPCDPath;
                pcdModel.ExistingDownChart = display.DownstreamBeforePCDPath;
                pcdModel.PredictedDownChart = display.DownstreamAfterPCDPath;
                pcdModel.ExistingAog = Convert.ToInt32(display.ExistingAOG);
                pcdModel.ExistingAogPercent = Math.Round(display.ExistingPAOG * 100);
                pcdModel.PredictedAog = Convert.ToInt32(display.PredictedAOG);
                pcdModel.PredictedAogPercent = Math.Round(display.PredictedPAOG * 100);
                pcdModel.DownstreamBeforeTitle = display.DownstreamBeforeTitle;
                pcdModel.UpstreamBeforeTitle = display.UpstreamBeforeTitle;
                pcdModel.DownstreamAfterTitle = display.DownstreamAfterTitle;
                pcdModel.UpstreamAfterTitle = display.UpstreamAfterTitle;
                return PartialView("PCDs", pcdModel);
            }
            return View();
        }

    }
}
