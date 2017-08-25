namespace MOE.Common.Models.Inrix
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Segment
    {
        [Key]
        public int Segment_ID { get; set; }

        [StringLength(50)]
        public string Segment_Name { get; set; }

        [StringLength(50)]
        public string Segment_Description { get; set; }
    }
}
