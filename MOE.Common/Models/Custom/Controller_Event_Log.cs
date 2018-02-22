using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MOE.Common.Models
{
    public class Controller_Event_Log
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Controller_Event_Log()
        {
        }

        [Column(Order = 1)]
        [Key]
        [StringLength(10)]
        public string SignalID { get; set; }

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