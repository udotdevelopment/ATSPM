using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AtspmApi.Models
{
    [Table("Controller_Event_Log")]
    public class Controller_Event_Log
    {
        [Column(Order = 1)]
        [Key]
        [StringLength(10)]
        public string SignalID { get; set; }

        //[Column(Order = 0, TypeName = "datetime2")]
        [Column(Order = 0)]
        [Key]
        public DateTime Timestamp { get; set; }

        [Column(Order = 2)]
        [Key]
        public int EventCode { get; set; }

        [Column(Order = 3)]
        [Key]
        public int EventParam { get; set; }

    }
}