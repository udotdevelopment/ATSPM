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

       
        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            Controller_Event_Log y = (Controller_Event_Log)obj;
            return this != null && y != null && this.SignalID == y.SignalID && this.Timestamp == y.Timestamp
                   && this.EventCode == y.EventCode && this.EventParam == y.EventParam
                ;
        }


  
        public override int GetHashCode() => this == null ? 0 : (this.SignalID.GetHashCode() ^ this.Timestamp.GetHashCode() ^ this.EventCode.GetHashCode() ^ this.EventParam.GetHashCode());


    }
}
