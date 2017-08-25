namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Program_Message
    {
        public int ID { get; set; }

        [StringLength(10)]
        public string Priority { get; set; }

        [StringLength(50)]
        public string Program { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        public DateTime? Timestamp { get; set; }
    }
}
