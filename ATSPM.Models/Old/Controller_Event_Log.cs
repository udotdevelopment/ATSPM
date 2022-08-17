using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ATSPM.Models
{
    public class Controller_Event_Log
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Controller_Event_Log()
        {
        }

        [Column(Order = 1)]
        [StringLength(10)]
        public string SignalID { get; set; }

        //[Column(Order = 0, TypeName = "datetime2")]
        [Column(Order = 0)]
        public DateTime Timestamp { get; set; }

        [Column(Order = 2)]
        public int EventCode { get; set; }

        [Column(Order = 3)]
        public int EventParam { get; set; }


    }
}