namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Archived_Metrics
    {
        [Key]
        [Column(Order = 0)]
        public DateTime Timestamp { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string DetectorID { get; set; }

        public int? Volume { get; set; }

        public int? speed { get; set; }

        public int? delay { get; set; }

        public int? AoR { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BinSize { get; set; }

        public int? SpeedHits { get; set; }

        public int? BinGreenTime { get; set; }
    }
}
