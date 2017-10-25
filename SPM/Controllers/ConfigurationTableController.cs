using MOE.Common.Business.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MOE.Common;

namespace SPM.Controllers
{
    public class ConfigurationTableController : Controller
    {
        // GET: ConfigurationTable
        public ActionResult Index(string SignalID)
        {
            MOE.Common.Models.Repositories.ISignalsRepository sr = 
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = sr.GetLatestVersionOfSignalBySignalID(SignalID);
            List<MOE.Common.Business.Helpers.ConfigurationRecord> records = 
                new List<MOE.Common.Business.Helpers.ConfigurationRecord>();
            foreach (MOE.Common.Models.Detector gd in signal.GetDetectorsForSignal())
            {
                MOE.Common.Business.Helpers.ConfigurationRecord r = new ConfigurationRecord(gd);
                records.Add(r);               
            }                        
            Models.SPMConfigurationTableViewModel model = new Models.SPMConfigurationTableViewModel();
            model.Records = records;
            return PartialView("ConfigurationTable",model);
        }

        public ActionResult IndexByVersion(int versionId)
        {
            MOE.Common.Models.Repositories.ISignalsRepository sr =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = sr.GetSignalVersionByVersionId(versionId);
            List<MOE.Common.Business.Helpers.ConfigurationRecord> records =
                new List<MOE.Common.Business.Helpers.ConfigurationRecord>();
            foreach (MOE.Common.Models.Detector gd in signal.GetDetectorsForSignal())
            {
                MOE.Common.Business.Helpers.ConfigurationRecord r = new ConfigurationRecord(gd);
                records.Add(r);
            }
            Models.SPMConfigurationTableViewModel model = new Models.SPMConfigurationTableViewModel();
            model.Records = records;
            return PartialView("ConfigurationTable", model);
        }
    }
}