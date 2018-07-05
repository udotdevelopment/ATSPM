using System.Collections.Generic;
using System.Configuration;
using System.IO;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.ScriptGenerator
{
    public class GenerateAddData
    {
        private static List<Pin> GetPins()
        {
            var repository = SignalsRepositoryFactory.Create();
            return repository.GetPinInfo();
        }

        public static void CreateScript()
        {
            //string Locations = string.Empty;
            var script = @"function AddData() {var pinColor =new Microsoft.Maps.Color(255, 238, 118, 35);
            var iconURL = './images/orangePin.png';
            var regionDdl = $('#Regions')[0];
            var pins = [];
            var regionFilter = 0; if (regionDdl.options[regionDdl.selectedIndex].value != '') 
                {regionFilter = regionDdl.options[regionDdl.selectedIndex].value;}
            var reportType = $('#MetricTypes')[0]; 
            var reportTypeFilter = 0; if (reportType.options[reportType.selectedIndex].value != '')
                { reportTypeFilter = ','+reportType.options[reportType.selectedIndex].value;}";

            var pins = GetPins();


            foreach (var pin in pins)
            {
                var PinName = "pin" + pin.SignalID;

                //The script string is appended for every pin in the collection.
                script +=
                    " if((regionFilter == 0 && reportTypeFilter == 0) || (reportTypeFilter == 0 && regionFilter == " +
                    pin.Region + ") " +
                    "|| (regionFilter == 0 && '" + pin.MetricTypes + "'.indexOf(reportTypeFilter) > -1) " +
                    "|| ('" + pin.MetricTypes + "'.indexOf(reportTypeFilter) > -1 && regionFilter == " + pin.Region +
                    ") )" +
                    "{var " + PinName + " = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(" +
                    pin.Latitude + ", " + pin.Longitude +
                    "));" + PinName +
                    ".SignalID = '" + pin.SignalID + "';" + PinName + ".Region = '" + pin.Region + "';" + PinName +
                    ".Actions = '" + pin.MetricTypes + "';" +
                    "Microsoft.Maps.Events.addHandler(" + PinName + ", 'mouseup', ZoomIn);" +
                    "Microsoft.Maps.Events.addHandler(" + PinName + ", 'click', displayInfobox);pins.push(" +
                    PinName + ");}";
            }
            script += @" 
return pins;}";

            var appSettings = ConfigurationManager.AppSettings;
            for (var i = 0; i < appSettings.Count; i++)
                using (var sw = File.CreateText(appSettings[i] + "AddData.js"))
                {
                    sw.Write(script);
                }
        }

        public static void CreateRouteScript()
        {
            //string Locations = string.Empty;
            var script = @"function AddData() {
            var regionDdl = $('#Regions')[0];
            var pins = [];
            var regionFilter = 0; if (regionDdl.options[regionDdl.selectedIndex].value != '') 
                {regionFilter = regionDdl.options[regionDdl.selectedIndex].value;}
            var reportType = $('#MetricTypes')[0]; 
            var reportTypeFilter = 0; if (reportType.options[reportType.selectedIndex].value != '')
                { reportTypeFilter = ','+reportType.options[reportType.selectedIndex].value;}";
            var pins = GetPins();
            foreach (var pin in pins)
            {
                var PinName = "pin" + pin.SignalID;
                //The script string is appended for every pin in the collection.
                script +=
                    " if((regionFilter == 0 && reportTypeFilter == 0) || (reportTypeFilter == 0 && regionFilter == " +
                    pin.Region + ") || (regionFilter == 0 && '" + pin.MetricTypes +
                    "'.indexOf(reportTypeFilter) > -1) || ('" + pin.MetricTypes +
                    "'.indexOf(reportTypeFilter) > -1 && regionFilter == " + pin.Region + ") ){var " + PinName +
                    " = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(" +
                    pin.Latitude + ", " + pin.Longitude +
                    "));" +
                    PinName + ".SignalID = '" + pin.SignalID + "';" + PinName + ".Region = '" + pin.Region + "';" +
                    PinName + ".Actions = '" + pin.MetricTypes + "';Microsoft.Maps.Events.addHandler(" + PinName +
                    ", 'mouseup', ZoomIn);Microsoft.Maps.Events.addHandler(" + PinName +
                    ", 'mouseover', displayRouteInfobox);Microsoft.Maps.Events.addHandler(" + PinName +
                    ", 'mouseout', closeInfobox);Microsoft.Maps.Events.addHandler(" + PinName +
                    ", 'click', AddSignalFromPin);pins.push(" + PinName + ");}";
            }

            //The Locaitons string will be used ot create a literal that is inserted into the default.html

            script += @" 
return pins;}";

            var appSettings = ConfigurationManager.AppSettings;
            for (var i = 0; i < appSettings.Count; i++)
                using (var sw = File.CreateText(appSettings[i] + "AddRouteData.js"))
                {
                    sw.Write(script);
                }
        }
    }
}