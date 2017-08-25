namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   


    public partial class LastUpdate
    {
        [Key]
        public int UpdateID { get; set; }       

        [Required, StringLength(10)]
        public string SignalID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public DateTime? LastErrorTime { get; set; }

        //public virtual Signal Signal { get; set; }
    }
}
