using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.ScriptGenerator
{
    public class GenerateAddData
    {
        private static List<Business.Pin> GetPins()
        {
            ISignalsRepository repository = SignalsRepositoryFactory.Create();
            return repository.GetPinInfo();           
        } 

        public static void CreateScript()
        {
            //string Locations = string.Empty;
            string script = @"function AddData() {var regionDdl = $('#Regions')[0];var regionFilter = 0; if (regionDdl.options[regionDdl.selectedIndex].value != '') {regionFilter = regionDdl.options[regionDdl.selectedIndex].value;} var reportType = $('#MetricTypes')[0]; var reportTypeFilter = 0; if (reportType.options[reportType.selectedIndex].value != '') { reportTypeFilter = ','+reportType.options[reportType.selectedIndex].value;}";
            List<Pin> pins = GetPins();

          
            foreach (MOE.Common.Business.Pin pin in pins)
            {
                string PinName = "pin" + pin.SignalID.ToString();
                
                //The script string is appended for every pin in the collection.
                script += " if((regionFilter == 0 && reportTypeFilter == 0) || (reportTypeFilter == 0 && regionFilter == " + pin.Region + ") || (regionFilter == 0 && '" + pin.MetricTypes + "'.indexOf(reportTypeFilter) > -1) || ('" + pin.MetricTypes + "'.indexOf(reportTypeFilter) > -1 && regionFilter == " + pin.Region + ") ){var " + PinName + " = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(" +
                    pin.Latitude.ToString() + ", " + pin.Longitude.ToString() +
                    "));" + PinName + ".SignalID = '" + pin.SignalID + "';" + PinName + ".Region = '" + pin.Region + "';" + PinName + ".Actions = '" + pin.MetricTypes + "';Microsoft.Maps.Events.addHandler(" + PinName + ", 'mouseup', ZoomIn);Microsoft.Maps.Events.addHandler(" + PinName + ", 'click', displayInfobox);dataLayer.push(" + PinName + ");}";

            }

            //The Locaitons string will be used ot create a literal that is inserted into the default.html
            script += "}";

            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            for (int i = 0; i < appSettings.Count; i++)
            {
                using (StreamWriter sw = File.CreateText(appSettings[i]))
                {
                    sw.Write(script);
                }
            }


        }


        }
    }

