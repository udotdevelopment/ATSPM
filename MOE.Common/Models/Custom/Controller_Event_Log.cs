namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Controller_Event_Log
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Controller_Event_Log()
        {
        }

        [Column(Order=1), Key]
        [StringLength(10)]
        public string SignalID { get; set; }

        [Column(Order = 0), Key]
        public DateTime Timestamp { get; set; }

        [Column(Order = 2), Key]
        public int EventCode { get; set; }

        [Column(Order = 3), Key]
        public int EventParam { get; set; }

       
    }
}
