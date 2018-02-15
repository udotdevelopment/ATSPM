using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using MOE.Common.Models;
using MOE.Common.Models.ViewModel.Chart;
using SPM = MOE.Common.Models.SPM;

namespace SPM.Models
{
    public class DataExportViewModel
    {
        //[Key]
        //public int Id { get; set; }
        [Required]
        [Display(Name = "Signal Id")]
        public string SignalId { get; set; }
        [Display(Name = "Event Codes")]
        public string EventCodes { get; set; }
        [Display(Name = "Event Parameters")]
        public string EventParams { get; set; }
        public IEnumerable<MOE.Common.Models.Controller_Event_Log> ControllerEventLogs { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Count")]
        public int? Count { get; set; }
        public int? RecordCountLimit { get; set; }
        public string EnumerationsName { get; set; }
        public string EnumerationsUrl { get; set; }
        public SignalSearchViewModel SignalSearch { get; set; }

        public DataExportViewModel()
        {
            RecordCountLimit = Convert.ToInt32(ConfigurationManager.AppSettings["RawDataCountLimit"]);
            SignalSearch = new SignalSearchViewModel();
            MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();
            EnumerationsName = "";
            EnumerationsUrl = "";
            List <ExternalLink> ExternalLinks = new List<ExternalLink>();
            ExternalLinks = db.ExternalLinks.ToList();
            foreach (ExternalLink e in ExternalLinks)
            {
                if (e.Name.ToUpper().Contains("ENUMERATIONS"))
                {
                    EnumerationsName = e.Name;
                    EnumerationsUrl = e.Url;
                    break;
                }
            }
        }
    }

}