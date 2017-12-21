using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MOE.Common.Models.ViewModel.Chart;

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
        public SignalSearchViewModel SignalSearch { get; set; }

        public DataExportViewModel()
        {
            SignalSearch = new SignalSearchViewModel();
        }
    }

}