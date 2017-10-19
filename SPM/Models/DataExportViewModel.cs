using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPM.Models
{
    public class DataExportViewModel
    {
        //[Key]
        //public int Id { get; set; }
        [Required]
        public string SignalId { get; set; }
        [Required]
        public string EventCodes { get; set; }
        public string EventParams { get; set; }
        public IEnumerable<MOE.Common.Models.Controller_Event_Log> ControllerEventLogs { get; set; }
        [Required]
        public DateTime StartDateDate { get; set; }
        [Required]
        public DateTime EndDateDate { get; set; }
        public int? StartDateHour { get; set; }
        public int? StartDateMinute { get; set; }
        public int? EndDateHour { get; set; }
        public int? EndDateMinute { get; set; }
        public int? Count { get; set; }
    }
}