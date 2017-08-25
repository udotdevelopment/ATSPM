namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Speed_Events
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string DetectorID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MPH { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KPH { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "datetime2")]
        public DateTime timestamp { get; set; }
    }
}
